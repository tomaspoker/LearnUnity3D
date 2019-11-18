using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class Timer
    {
        /// <summary>
        /// 启动时的时间戳
        /// </summary>
        private float m_TimestampLaunch;

        /// <summary>
        /// 总时间
        /// </summary>
        private float m_Duration;

        /// <summary>
        /// 步长-默认1s
        /// </summary>
        private float m_Stride;

        /// <summary>
        /// 当前流失时间
        /// </summary>
        private float m_CurrentElapsedTime;

        /// <summary>
        /// 总流失时间
        /// </summary>
        private float m_TotalElapsedTime;

        /// <summary>
        /// 剩余的时间
        /// </summary>
        private float m_RemainingTime;

        /// <summary>
        /// 是否循环
        /// </summary>
        private bool m_IsLoop;

        /// <summary>
        /// 是否被暂停了
        /// </summary>
        private bool m_IsPaused;

        private bool m_IsFirstTikTok;

        /// <summary>
        /// 倒计时完成的回调
        /// </summary>
        private Action m_CallbackCompleted;

        /// <summary>
        /// 指定时间间隔的回调
        /// </summary>
        private Action<float, float> m_CallbackProcess;

        /// <summary>
        /// 倒计时是否完成
        /// </summary>
        public bool IsCompleted { get; set; }

        public bool IsPaused { get; set; }


        public Timer(float duration, bool isLoop, Action completed, Action<float, float> process, float stride)
        {
            m_Duration = duration;
            m_IsLoop = isLoop;

            m_CallbackCompleted = completed;
            m_CallbackProcess = process;

            m_Stride = stride;

            m_IsPaused = false;

            m_IsFirstTikTok = false;

            m_RemainingTime = duration;
        }

        public static Timer Create(float duration, Action completed)
        {
            return new Timer(duration, false, completed, null, 1f);
        }

        public static Timer Create(float duration, Action completed, Action<float, float> process)
        {
            return new Timer(duration, false, completed, process, 1f);
        }

        public static Timer Create(float duration, bool isLoop, Action<float, float> process)
        {
            return new Timer(duration, isLoop, null, process, 1f);
        }

        public static Timer Create(float duration, bool isLoop, Action completed, Action<float, float> process)
        {
            return new Timer(duration, isLoop, completed, process, 1f);
        }

        public static Timer Create(float duration, bool isLoop, Action<float, float> process, float stride)
        {
            return new Timer(duration, isLoop, null, process, stride);
        }

        public static Timer Create(float duration, bool isLoop, Action completed, Action<float, float> process, float stride)
        {
            return new Timer(duration, isLoop, completed, process, stride);
        }

        public void Update()
        {
            if (m_IsFirstTikTok == false)
            {
                m_IsFirstTikTok = true;
                m_CurrentElapsedTime = 0f;
            }
            else
            {
                m_CurrentElapsedTime += Time.deltaTime;

                if (m_CurrentElapsedTime >= m_Stride)
                {
                    m_RemainingTime -= m_Stride;

                    m_CurrentElapsedTime -= m_Stride;

                    m_TotalElapsedTime += m_Stride;

                    m_CallbackProcess?.Invoke(m_RemainingTime, m_TotalElapsedTime);
                }
                
                if (m_RemainingTime <= 0.005f)
                {
                    if (m_IsLoop)
                    {
                        m_RemainingTime = m_Duration;
                    }
                    else
                    {
                        IsCompleted = true;
                    }

                    m_CallbackCompleted?.Invoke();
                }
            }
        }

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }
    }

}
