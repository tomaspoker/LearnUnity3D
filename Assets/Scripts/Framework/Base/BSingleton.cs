using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSingleton<T> where T : new()
{
    public static T s_Instance;

    /// <summary>
    /// 获取实例对象
    /// </summary>
    public static T Instance {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new T();
            }
            return s_Instance;
        }
    }
}
