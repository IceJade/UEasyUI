/*
* ==============================================================================
*
* File name: UITimerBase.cs
* Description: UI倒计时组件基类
* Version: 1.0
* Created: 2019.5.24
* Author: zhoumingfeng
* Company: cyou-inc.com
* Modify Record:
* 
* ==============================================================================
*/

using System;
using UnityEngine;

namespace UEasyUI
{
    // 界面用的计时器基类
    public abstract class UITimerBase : MonoBehaviour
    {
        //----------------------------------------------------------------
        // 常量

        public const int UNIT_SECOND = 1;
        public const int UNIT_MINUTE = 1 << 1;
        public const int UNIT_HOUR = 1 << 2;
        public const int UNIT_DAY = 1 << 3;
        public const int UNIT_ALL = UNIT_SECOND | UNIT_MINUTE | UNIT_HOUR | UNIT_DAY;
        public readonly static int[] UNIT_ARRAY = new int[] { UNIT_SECOND, UNIT_MINUTE, UNIT_HOUR, UNIT_DAY };
        public readonly static long[] UNIT_TOTAL_TICKS = new long[] { 10000000, 600000000, 36000000000, 864000000000 };
        public const int UNIT_COUNT = 4;

        protected const float SLOW_UPDATE_INTERVAL = 60f;
        protected const float FAST_UPDATE_INTERVAL = 0.3f;

        //----------------------------------------------------------------
        // 编辑器参数

        // 是否使用服务器时间
        public bool useServerTime = false;

        // 是否使用TimerCtrl中的当前时间
        public bool useLocalTime = false;

        // 是否忽略时间缩放
        public bool ignoreTimeScale = true;

        // 倒计时结束时发送的事件
        public short eventId = 0;

        // 事件触发延迟（单位：秒）
        public float triggerDelay = 0;

        //倒计时结束时的委托
        public Action EndTimeCallBack;

        //----------------------------------------------------------------
        // 成员变量

        protected DateTime m_startTime = DateTime.MinValue;         // 开始时间
        protected DateTime m_targetTime = DateTime.MinValue;        // 结束时间
        protected DateTime m_finalTargetTime = DateTime.MinValue;   // 实际结束时间
        protected TimeSpan m_leftTime = new TimeSpan();             // 剩余时间
        protected long m_totalTicks = 0;                            // 总Tick数
        protected float m_elapse = 0f;                              // 当前间隔内流逝时间
        protected float m_updateInterval = 1f;                      // 更新间隔
        protected bool m_timerStarted = false;                      // 是否已经开始计时
        protected int m_displayUnit = UNIT_ALL;                     // 显示的单位

        //----------------------------------------------------------------
        // 公用函数

        /// <summary>
        /// 开始倒计时（单位:毫秒）
        /// </summary>
        /// <param name="milliseconds"></param>
        public virtual void StartCountdown(long milliseconds, Action callBack = null)
        {
            if (milliseconds <= 0)
                return;
            EndTimeCallBack = callBack;
            StopTimer(false, false, false);
            SetStartTimeNow();
            SetCountdown(milliseconds);
        }

        /// <summary>
        /// 设置开始时间
        /// </summary>
        /// <param name="startTime"></param>
        public virtual void SetStartTime(DateTime startTime)
        {
            m_startTime = startTime;
        }

        /// <summary>
        /// 设置开始时间为当前时间
        /// </summary>
        public virtual void SetStartTimeNow()
        {
            m_startTime = GetCurrentTime();
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetStartTime()
        {
            return m_startTime;
        }

        /// <summary>
        /// 设置目标时间（结束时间）
        /// </summary>
        /// <param name="targetTime"></param>
        public virtual void SetTargetTime(DateTime targetTime)
        {
            DateTime currentTime = GetCurrentTime();
            m_targetTime = targetTime;
            m_timerStarted = true;
            enabled = m_targetTime > currentTime;
            if (enabled)
            {
                try
                {
                    m_finalTargetTime = (m_targetTime != DateTime.MaxValue && triggerDelay > 0) ? m_targetTime.AddSeconds(triggerDelay) : m_targetTime;
                }
                catch
                {
                    Log.Error("[UITimerBase] Invalid target time: ", m_targetTime);
                    m_finalTargetTime = m_targetTime;
                }
                m_elapse = 0f;
                Reaccurate(currentTime);
                UpdateDisplayUnit();
                UpdateInterval();
                UpdateDisplay();
            }
            else
            {
                StopTimer(true, true, true);
            }
        }

        /// <summary>
        /// 获取目标时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetTargetTime()
        {
            return m_targetTime;
        }

        /// <summary>
        /// 设置倒计时时间（开始时间+倒计时为结束时间）
        /// </summary>
        /// <param name="milliseconds"></param>
        public virtual void SetCountdown(long milliseconds)
        {
            SetTargetTime(new DateTime(GetCurrentTime().Ticks + milliseconds * 10000));
        }

        public virtual DateTime GetCurrentTime()
        {
            if (useServerTime)
            {
                return new DateTime(TimeUtility.CurrentServerMillSecond() * 10000);
            }
            else if (useLocalTime)
            {
                return new DateTime(DateTime.Now.Ticks);
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 获取剩余时间，单位：毫秒
        /// </summary>
        /// <returns></returns>
        public virtual long GetLeftTime()
        {
            DateTime currentTime = GetCurrentTime();
            return m_targetTime > currentTime ? (long)((m_targetTime - currentTime).TotalMilliseconds) : 0;
        }

        /// <summary>
        /// 获取int型的剩余时间，lua用，注意时间太长可能截断
        /// </summary>
        /// <returns></returns>
        public virtual int GetLeftTimeInt()
        {
            return (int)GetLeftTime();
        }

        /// <summary>
        /// 设置显示的单位，关系到更新频率（内部用）
        /// </summary>
        /// <param name="displayUnit"></param>
        public virtual void SetDisplayUnit(int displayUnit)
        {
            m_displayUnit = displayUnit;
        }

        /// <summary>
        /// 重置计时器，即停止计时器
        /// </summary>
        /// <param name="sendEvent"></param>
        public virtual void StopTimer(bool notify = true, bool resetTime = false, bool updateDisplay = true)
        {
            enabled = false;
            if (resetTime)
            {
                SetStartTimeNow();
                m_finalTargetTime = m_targetTime = m_startTime;
                m_leftTime = m_targetTime - m_startTime;
                m_elapse = 0;
            }
            if (updateDisplay)
            {
                UpdateDisplayUnit();
                UpdateDisplay();
            }
            if (m_timerStarted)
            {
                m_timerStarted = false;
                if (notify)
                {
                    SendEvent();
                }
            }
        }

        //----------------------------------------------------------------
        // MonoBehaviour

        protected virtual void OnEnable()
        {
            if (m_timerStarted)
            {
                SetTargetTime(m_targetTime);
            }
        }

        protected virtual void OnApplicationFocus(bool focus)
        {
            if (m_timerStarted)
            {
                SetTargetTime(m_targetTime);
            }
        }

        protected virtual void Update()
        {
            if (m_elapse < m_updateInterval)
            {
                m_elapse += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                return;
            }

            DateTime currentTime = GetCurrentTime();
            if (m_updateInterval >= 59f)
            {
                Reaccurate(currentTime);
            }
            else
            {
                m_leftTime = m_finalTargetTime - currentTime;
                m_elapse %= m_updateInterval;
            }

            UpdateDisplayUnit();
            UpdateInterval();
            UpdateDisplay();
            if (m_leftTime.Ticks <= 0)
            {
                StopTimer(true, false, false);
            }
        }

        //----------------------------------------------------------------
        // 子类重载

        protected abstract void UpdateDisplay();

        //----------------------------------------------------------------
        // 内部函数

        protected virtual void UpdateDisplayUnit()
        {
            m_displayUnit = UNIT_ALL;
        }

        protected virtual void UpdateInterval()
        {
            if ((m_displayUnit & UNIT_SECOND) != 0)
            {
                m_updateInterval = FAST_UPDATE_INTERVAL;
            }
            else
            {
                m_updateInterval = SLOW_UPDATE_INTERVAL;
            }
        }

        protected void Reaccurate(DateTime currentTime)
        {
            m_totalTicks = (m_finalTargetTime - m_startTime).Ticks;
            m_leftTime = m_finalTargetTime - currentTime;
            long restTicks = m_leftTime.Ticks;
            m_elapse = m_updateInterval - restTicks % (long)(10000000L * m_updateInterval) / 10000000f;
        }

        protected void SendEvent()
        {
            if (eventId > 0)
                GameEntry.Event.Fire(eventId, gameObject);
        }
    }
}