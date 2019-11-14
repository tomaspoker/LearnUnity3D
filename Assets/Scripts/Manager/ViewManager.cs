using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class ModuleManager : BSingleton<ModuleManager>
    {
        private Dictionary<string, ViewController> m_ControllerDict = new Dictionary<string, ViewController>();

        private List<string> m_ShowingList = new List<string>();

        private List<string> m_HidingList = new List<string>();

        public bool GetController(string viewName, out ViewController controller)
        {
            return m_ControllerDict.TryGetValue(viewName, out controller);
        }

        private void RemoveFromShowingList(string viewName)
        {
            if (m_ShowingList.Contains(viewName))
            {
                m_ShowingList.Remove(viewName);
            }
        }

        /// <summary>
        /// 显示某一视图
        /// </summary>
        /// <param name="viewName">模块名称</param>
        /// <param name="cleanly">是否关闭其他已打开的模块</param>
        /// <param name="args">传入的参数</param>
        public void Show(string viewName, bool cleanly, object args)
        {
            if (GetController(viewName, out ViewController controller))
            {
                if (m_HidingList.Contains(viewName))
                {
                    controller.Display(args);

                    m_HidingList.Remove(viewName);
                }
                else
                {
                    controller.Show(args);
                }

                m_ShowingList.Add(viewName);

                if (cleanly)
                {
                    Clean(true);
                }
            }
        }

        public void Remove(string viewName)
        {
            if (GetController(viewName, out ViewController controller))
            {
                if (!controller.IsResident)
                {
                    controller.Remove();

                    RemoveFromShowingList(viewName);
                }
                else
                {
                    Hide(viewName);

                    RemoveFromShowingList(viewName);
                }
            }
        }

        /// <summary>
        /// 隐藏某一视图，内部方法，禁止外部调用
        /// </summary>
        /// <param name="viewName"></param>
        private void Hide(string viewName)
        {
            if (GetController(viewName, out ViewController controller))
            {
                controller.Hide();

                m_HidingList.Add(viewName);
            }
        }

        /// <summary>
        /// 清空所有已打开的视图
        /// </summary>
        /// <param name="isRetainTop">是否保留栈顶的视图</param>
        public void Clean(bool isRetainTop)
        {
            while (m_ShowingList.Count > 0)
            {
                Remove(m_ShowingList[0]);

                m_ShowingList.RemoveAt(0);
            }

            if (!isRetainTop)
            {
                Remove(m_ShowingList[0]);

                m_ShowingList.RemoveAt(0);
            }
        }
    }

}
