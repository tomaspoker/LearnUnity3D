using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class ViewController
    {
        private View m_View;
        public View View { get => m_View; set => m_View = value; }

        public ViewController()
        {
            this.m_View = null;

            this.InitEvent();
        }

        /// <summary>
        /// 初始化模块事件监听
        /// </summary>
        public void InitEvent()
        {

        }

        public void Show(object args)
        {
            if (m_View == null)
            {
                this.InitView(args);
            }
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

        public void Hide()
        {
            if (m_View)
            {
                m_View.SetActive(false);
            }
        }

        public void Remove()
        {
            if (m_View)
            {
                m_View.Remove();
            }
        }
    }
}