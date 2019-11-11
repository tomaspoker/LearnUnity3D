using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poking
{

    public interface IController
    {
        /// <summary>
        /// 初始化模块事件监听
        /// </summary>
        void InitEvent();

        /// <summary>
        /// 显示与此Controller绑定的View视图
        /// </summary>
        /// <param name="obj">传入的参数</param>
        void Show(object obj);

        /// <summary>
        /// 关闭与此Controller绑定的View视图
        /// </summary>
        void Remove();

    }

}
