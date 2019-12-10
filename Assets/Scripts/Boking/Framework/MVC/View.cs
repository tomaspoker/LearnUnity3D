using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class View : MonoBehaviour
    {

        private bool m_NeedRefreshUI;           // 标记是否需要刷新UI
        private bool m_RefreshedUI;             // 标记UI是否已经刷新过
        private bool m_IsTransitionCompleted;   // 过渡动画是否已经完成

        public ViewController m_Controller;

        public string ClassName { get => "View"; }

        public void Remove()
        {
            Destroy(gameObject);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetController(ViewController controller)
        {
            m_Controller = controller;
        }

        public void Awake()
        {
            m_RefreshedUI = false;
            m_NeedRefreshUI = false;
            m_IsTransitionCompleted = false;
        }

        public void Start()
        {
            InitUI();

            InitInternationalLanguageUI();

            InitClick();

            InitChildListen();
        }

        /// <summary>
        /// 初始化视图
        /// </summary>
        /// <param name="args"></param>
        public void Init(object args)
        {

        }

        /// <summary>
        /// 重新对此视图进行初始化
        /// </summary>
        /// <param name="args"></param>
        public void Reinit(object args)
        {
            m_RefreshedUI = false;
            m_NeedRefreshUI = false;
            m_IsTransitionCompleted = false;
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        public void InitUI()
        {

        }

        /// <summary>
        /// 初始化国际化语言
        /// </summary>
        public void InitInternationalLanguageUI()
        {

        }

        /// <summary>
        /// 初始化事件点击
        /// </summary>
        public void InitClick()
        {

        }

        /// <summary>
        /// 初始化对子节点的事件监听
        /// </summary>
        public void InitChildListen()
        {

        }

        /// <summary>
        /// 执行过渡动画
        /// </summary>
        public void Transition()
        {
            OnTransitionCompleted();
        }

        /// <summary>
        /// 过渡动画完成之后调用，如果完成后，服务器已经回包了，则直接刷新
        /// </summary>
        public void OnTransitionCompleted()
        {
            m_IsTransitionCompleted = true;

            if (m_NeedRefreshUI)
            {
                RefreshView();
            }
        }

        public bool CheckTransitionCompleted()
        {
            return m_IsTransitionCompleted;
        }

        /// <summary>
        /// 服务器回包，Controller中调用刷新View
        /// 如果过渡动画还没结束，则不刷新UI，在调用之前需要检查过渡动画是否完成
        /// </summary>
        public void RefreshView()
        {
            m_NeedRefreshUI = true;

            if (!CheckTransitionCompleted())
            {
                return;
            }

            RefreshUI();

            m_NeedRefreshUI = false;

            m_RefreshedUI = true;
        }

        /// <summary>
        /// 刷新界面UI
        /// </summary>
        public void RefreshUI()
        {

        }


    }
}