using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{

    public class LifeCycleManager : Singleton<LifeCycleManager>
    {
        private LifeCycleManager()
        {

        }

        public override void Init()
        {

        }


        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Destory()
        {

        }

        /// <summary>
        /// 游戏切到后台
        /// </summary>
        public void OnBackground()
        {

        }

        /// <summary>
        /// 游戏切到前台
        /// </summary>
        public void OnForeground()
        {

        }
    }

}