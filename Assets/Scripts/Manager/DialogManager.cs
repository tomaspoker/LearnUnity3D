using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boking
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        private Dictionary<string, DialogController> m_ControllerDict = new Dictionary<string, DialogController>();

        private List<DialogParams> m_WaittingList = new List<DialogParams>();

        private List<Dialog> m_ShowingList = new List<Dialog>();

        private DialogStatus m_DialogStatus;

        private bool m_IsDelayingPrepare;

        public bool IsUpwardEnabled { get; set; }  // 是否可以执行上拉操作
        
        public override void Init()
        {

        }

        private void Register<T>(string controllerName) where T : DialogController, new()
        {
            if (!m_ControllerDict.ContainsKey(controllerName))
            {
                m_ControllerDict.Add(controllerName, new T());
            }
        }

        public bool GetDialog(int id, out Dialog dialog)
        {
            foreach(Dialog d in m_ShowingList)
            {
                if (d.Id == id)
                {
                    dialog = d;
                    return true;
                }
            }

            dialog = null;
            return false;
        }

        public bool GetDialog(string alias, out Dialog dialog)
        {
            foreach (Dialog d in m_ShowingList)
            {
                if (d.Alias == alias)
                {
                    dialog = d;
                    return true;
                }
            }

            dialog = null;
            return false;
        }

        /// <summary>
        /// 把需要打开的弹窗的数据放入m_WaittingList中
        /// </summary>
        /// <param name="args">弹窗数据</param>
        /// <param name="index">插入的编号，默认插入最前面</param>
        /// <returns></returns>
        public int Push(ref DialogParams args, int index = 1)
        {
            // 判断数据是否有误
            if (args.DialogClassName == "" || args.DialogPrefabPath == "")
            {
                return -1;
            }

            if (index >= 0 && index < m_WaittingList.Count)
            {
                m_WaittingList.Insert(index, args);
            }
            else
            {
                m_WaittingList.Add(args);
            }

            return args.Id;
        }

        /// <summary>
        /// 检查是否可以打开下一个弹窗
        /// </summary>
        /// <param name="stay"></param>
        /// <returns></returns>
        public bool CheckCanPop(bool stay)
        {
            if (m_WaittingList.Count <= 0)
            {
                return false;
            }
            else if (m_ShowingList.Count > 0)
            {
                Dialog dialog = m_ShowingList[0];
                if (stay)
                {
                    if (dialog.IsActived)
                    {
                        return false;
                    }
                }

                if (dialog.IsActived && dialog.IsKeepTop)
                {
                    if (!m_WaittingList[0].IsKeepTop)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 打开m_WaittingList中第一个弹窗并打开
        /// </summary>
        /// <param name="stay">是否需要延迟打开</param>
        public void Pop(bool stay = false)
        {
            if (CheckCanPop(stay))
            {
                DialogParams dialogParams = m_WaittingList[0];

                m_WaittingList.RemoveAt(0);

                if (dialogParams.ShowType == DialogShowType.NORMAL)
                {
                    Hide(ref dialogParams);

                    Unique(ref dialogParams);

                    Show(ref dialogParams);

                }
                else if (dialogParams.ShowType == DialogShowType.EVENT)
                {
                    // 派发事件通知可以打开此弹窗了
                }
            }
        }

        private void Hide(ref DialogParams dialogParams)
        {
            if (m_ShowingList.Count > 0)
            {
                Dialog dialog = m_ShowingList[m_ShowingList.Count - 1];
                if (!dialog.IsKeepShow)
                {
                    dialog.Hide();
                }
            }
        }

        private void Unique(ref DialogParams dialogParams)
        {
            if (m_ShowingList.Count > 0)
            {
                for (int i = 0; i < m_ShowingList.Count; i++)
                {
                    if (m_ShowingList[i].DialogClassName == dialogParams.DialogClassName && dialogParams.IsKeepUnique)
                    {
                        // 移除此弹窗，序号i

                        m_ShowingList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void Show(ref DialogParams dialogParams)
        {
            GameObject gameObject = GameObject.Instantiate(new GameObject());

            Type type = Type.GetType("Boking." + dialogParams.DialogClassName);

            Dialog dialog = gameObject.GetComponent(type) as Dialog;
            if (dialog == null)
            {
                dialog = gameObject.AddComponent(type) as Dialog;
            }

            m_ShowingList.Add(dialog);

            dialog.RequestServerData();

            dialog.Show();
        }

        /// <summary>
        /// 移除一个弹窗
        /// </summary>
        /// <param name="id"></param>
        private void RemoveOne(int id)
        {
            for (int i = 0; i < m_ShowingList.Count; i++)
            {
                if (m_ShowingList[i].Id == id)
                {
                    m_ShowingList[i].Remove();

                    m_ShowingList.RemoveAt(i);

                    return;
                }
            }
        }

        /// <summary>
        /// 移除一组弹窗
        /// </summary>
        /// <param name="idList"></param>
        public void Remove(params int[] idList)
        {
            for (int i = 0; i < idList.Length; i++)
            {
                RemoveOne(idList[i]);
            }

            Check();
        }

        /// <summary>
        /// 移除所有弹窗，除了id
        /// </summary>
        public void Clear(int id = -1)
        {
            m_WaittingList.Clear();

            for (int i = 0; i < m_ShowingList.Count; i++)
            {
                if (id == -1 || id != m_ShowingList[i].Id)
                {
                    m_ShowingList[i].Remove();

                    m_ShowingList.RemoveAt(i);

                    i--;
                }
            }

            m_ShowingList.Clear();

            CheckAndRemoveBackground();
        }

        private void Check()
        {
            if (m_ShowingList.Count > 0 && m_ShowingList[m_ShowingList.Count - 1].IsKeepTop)
            {
                m_ShowingList[m_ShowingList.Count - 1].Display();
            }
            else if (m_WaittingList.Count > 0 &&  m_WaittingList[0].IsPriority)
            {
                Prepare();
            }
            else if (m_ShowingList.Count > 0)
            {
                m_ShowingList[m_ShowingList.Count - 1].Display();
            }
            else if (m_WaittingList.Count > 0)
            {
                Prepare();
            }
            else
            {
                CheckAndRemoveBackground();
            }
        }

        private void Prepare()
        {
            if (m_IsDelayingPrepare)
            {
                return;
            }

            StartCoroutine(DelayPrepare());
        }

        private IEnumerator DelayPrepare()
        {
            m_IsDelayingPrepare = true;

            yield return new WaitForEndOfFrame();

            m_IsDelayingPrepare = false;

            Pop();
        }

        private void CheckAndRemoveBackground()
        {

        }
    }

}