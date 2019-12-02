using System;
using System.Collections.Generic;

namespace Boking
{
    public delegate void EventListener(params object[] args);

    public interface IEventProtocol
    {
        void AddEvent(int key, EventListener listener);

        void RemoveEvent(int key);

        void RemoveEvent(int key, EventListener listener);

        void DispatchEvent(int key, params object[] args);
    }

    public class EventProtocol : IEventProtocol
    {
        private class ListenerWrapper
        {
            private List<EventListener> m_ListenerList = new List<EventListener>();

            private int Key { get; }

            public ListenerWrapper(int key)
            {
                Key = key;
            }

            public void Add(EventListener listener)
            {
                m_ListenerList.Add(listener);
            }

            public void Remove(EventListener listener)
            {
                m_ListenerList.Remove(listener);
            }

            public void Dispatch(params object[] args)
            {
                foreach(EventListener listener in m_ListenerList)
                {
                    listener(args);
                }
            }
        }

        private Dictionary<int, ListenerWrapper> m_EventDict = new Dictionary<int, ListenerWrapper>();

        public void AddEvent(int key, EventListener listener)
        {
            if (!m_EventDict.TryGetValue(key, out ListenerWrapper wrapper))
            {
                wrapper = new ListenerWrapper(key);

                m_EventDict.Add(key, wrapper);
            }

            wrapper.Add(listener);
        }

        public void RemoveEvent(int key)
        {
            m_EventDict.Remove(key);
        }

        public void RemoveEvent(int key, EventListener listener)
        {
            if (m_EventDict.TryGetValue(key, out ListenerWrapper wrapper))
            {
                wrapper.Remove(listener);
            }
        }

        public void DispatchEvent(int key, params object[] args)
        {
            if (m_EventDict.TryGetValue(key, out ListenerWrapper wrapper))
            {
                wrapper.Dispatch(args);
            }
        }
    }
}
