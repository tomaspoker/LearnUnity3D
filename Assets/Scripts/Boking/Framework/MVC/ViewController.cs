using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class ViewController
    {
        public View SelfView { get; set; }

        /// <summary>
        /// 视图是否需要常驻内存，如果指定，则打开后就不在释放，除非显式指定
        /// </summary>
        public bool IsResident { get; set; }

        public ViewController()
        {
            SelfView = null;

            IsResident = false;

            InitEvent();
        }

        /// <summary>
        /// 初始化模块事件监听
        /// </summary>
        public void InitEvent()
        {

        }

        /// <summary>
        /// 初始化View组件
        /// </summary>
        /// <param name="obj">初始化参数</param>
        public virtual async void InitView(object args)
        {
            // 加载Prefab
            // GameObject gameObject;
            // await ResouceManager.LoadPrefab(this.m_PrefabPath, out gameObject);
            // gameObject.AddComponent(gameObject);
            // this.m_View = gameObject.GetComponent();
            // this.m_View.SetController(this);
            // this.m_View.InitData(args);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        /// <param name="args"></param>
        public void Show(object args)
        {
            if (SelfView == null)
            {
                InitView(args);

                SelfView.Transition();
            }

            SelfView.SetActive(true);
        }

        /// <summary>
        /// 显示视图，此前视图已经存在
        /// </summary>
        /// <param name="args"></param>
        public void Display(object args)
        {
            if (SelfView != null)
            {
                SelfView.Reinit(args);

                SelfView.Transition();

                SelfView.SetActive(true);
            }
        }

        public void Hide()
        {
            if (SelfView)
            {
                SelfView.SetActive(false);
            }
        }

        public void Remove()
        {
            if (SelfView)
            {
                SelfView.Remove();
            }
        }
    }
}