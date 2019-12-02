using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class CacheManager : Singleton<CacheManager>
    {
        private readonly Dictionary<int, object> m_Cache = new Dictionary<int, object>();

        private CacheManager()
        {

        }

        public T Get<T>(int key)
        {
            m_Cache.TryGetValue(key, out object value);

            return (T)value;
        }

        public void Set<T>(int key, T value)
        {
            m_Cache.Add(key, value);
        }

    }

}
