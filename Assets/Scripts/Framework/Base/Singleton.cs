using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public interface ISingleton
    {
        void Init();
    }

    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T s_Instance;

        private static readonly object s_Lock = new object();

        public static T Instance
        {
            get
            {
                lock(s_Lock)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = CreateSingleton<T>();
                    }
                }
                return s_Instance;
            }
        }

        private static X CreateSingleton<X>() where X : class, ISingleton
        {
            var ctors = typeof(X).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor() not found! in " + typeof(X));
            }

            var instance = ctor.Invoke(null) as X;
            instance.Init();

            return instance;
        }

        public virtual void Dispose()
        {
            s_Instance = null;
        }

        public virtual void Init()
        {

        }
    }

}
