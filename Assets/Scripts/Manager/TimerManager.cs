using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class TimerManager : BaseSingleton<TimerManager>
    {
        private static int s_TimerId;
        
        /// <summary>
        /// Timer字典
        /// </summary>
        private Dictionary<int, Timer> m_TimerDict = new Dictionary<int, Timer>();

        /// <summary>
        /// 将要删除的Timer Id列表
        /// </summary>
        private List<int> m_RemovingTimerIdList = new List<int>();

        private Dictionary<int, MonoBehaviour> m_TimerBehaviourDict = new Dictionary<int, MonoBehaviour>();

        private int TimerId => s_TimerId++;

        /// <summary>
        /// 倒计时
        /// </summary>
        /// <param name="duration">需要倒计时的时间</param>
        /// <param name="completed">倒计时完成时的回调</param>
        /// <returns></returns>
        public int CountDown(float duration, Action completed)
        {
            Timer timer = Timer.Create(duration, completed);

            return Register(timer);
        }

        /// <summary>
        /// 倒计时，每次步进均有回调
        /// </summary>
        /// <param name="duration">需要倒计时的时间</param>
        /// <param name="completed">倒计时完成时的回调</param>
        /// <param name="process">每次步进的回调</param>
        /// <returns></returns>
        public int Clock(float duration, Action completed, Action<float, float> process)
        {
            Timer timer = Timer.Create(duration, false, completed, process);

            return Register(timer);
        }

        /// <summary>
        /// 无限循环的倒计时
        /// </summary>
        /// <param name="duration">每隔这么时间步进一次</param>
        /// <param name="process">每次步进的回调</param>
        /// <returns></returns>
        public int Loop(float duration, Action<float, float> process)
        {
            Timer timer = Timer.Create(duration, true, process, duration);

            return Register(timer);
        }

        private int Register(Timer timer)
        {
            int id = TimerId;

            m_TimerDict.Add(id, timer);

            return id;
        }

        public void Update()
        {
            m_RemovingTimerIdList.Clear();

            foreach(KeyValuePair<int, Timer> vk in m_TimerDict)
            {
                if (vk.Value.IsCompleted)
                {
                    m_RemovingTimerIdList.Add(vk.Key);
                }
                else if (vk.Value.IsPaused)
                {
                    // 被暂停了
                }
                else
                {
                    vk.Value.Update();
                }
            }

            foreach(int id in m_RemovingTimerIdList)
            {
                Remove(id);
            }
        }

        public void Pause(int id)
        {
            if (m_TimerDict.TryGetValue(id, out Timer timer))
            {
                timer.Pause();
            }
        }

        public void Resume(int id)
        {
            if (m_TimerDict.TryGetValue(id, out Timer timer))
            {
                timer.Resume();
            }
        }

        /// <summary>
        /// 移除一个倒计时
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            m_TimerDict.Remove(id);

            m_TimerBehaviourDict.Remove(id);
        }

        /// <summary>
        /// 把定时器与一个脚本绑定在一起
        /// </summary>
        /// <param name="id"></param>
        /// <param name="behavior"></param>
        public void Bind(int id, MonoBehaviour behavior)
        {
            m_TimerBehaviourDict.Add(id, behavior);
        }

        public void Adjust()
        {

        }
    }

}
