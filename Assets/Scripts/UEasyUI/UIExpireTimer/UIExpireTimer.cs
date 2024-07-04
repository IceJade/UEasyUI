//----------------------------------------------------------------------------
// @说    明: UI倒计时组件
// @版 本 号: 1.0
// @创建时间: 2019.5.24
// @作    者: zhoumingfeng
// @使用说明: 
//----------------------------------------------------------------------------
using UnityEngine.UI;

namespace UEasyUI
{
    // 到期时间计时器
    public class UIExpireTimer : UITimerBase
    {
        //----------------------------------------------------------------
        // 编辑器参数

        // 文本框;
        public TextMeshProUEUI text;

        // 显示时是否进一位，比如：当前时间为10:10:10，如果只显示小时和分钟的话，勾选carryTime显示为10:11，不勾显示10:10
        public bool carryTime = true;

        // 最低显示位不显示0, 以1代替;
        public bool carryLastBit = false;

        // 显示的文本
        public string textExpired = "已经过期";
        public string textDay = "{0}天{1}小时";
        public string textHour = "{0}小时{1}分";
        public string textMinute = "{0}分{1}秒";
        public string textSeconds = "{0}秒";

        // 时间单位标识
        public int unitDay = UNIT_DAY | UNIT_HOUR;
        public int unitHour = UNIT_HOUR | UNIT_MINUTE;
        public int unitMinute = UNIT_MINUTE | UNIT_SECOND;
        public int unitSeconds = UNIT_SECOND;

        //----------------------------------------------------------------
        // 成员变量

        protected int m_textType = 0;
        protected long[] m_timeValues = null;
        protected string m_lastFormat = null;
        protected long[] m_lastTimeValues = null;

        //----------------------------------------------------------------
        // 公用函数

        public virtual void SetText(string strText)
        {
            if (null != text)
            {
                text.SetText(strText);
            }
            else
            {
                text = this.GetComponent<TextMeshProUEUI>();
                if (null != text)
                    text.SetText(strText);
                else
                    Log.Error("请挂载TextMeshProUEUI脚本再使用!");
            }
        }

        /// <summary>
        /// 定时器是否已经开始;
        /// </summary>
        public virtual bool IsTimerStarted()
        {
            return this.m_timerStarted;
        }

        public override void StopTimer(bool notify = true, bool resetTime = false, bool updateDisplay = true)
        {
            m_lastFormat = null;
            if (m_lastTimeValues != null)
            {
                for (int i = 0, imax = m_lastTimeValues.Length; i < imax; ++i)
                {
                    m_lastTimeValues[i] = 0;
                }
            }
            base.StopTimer(notify, resetTime, updateDisplay);
        }

        //----------------------------------------------------------------
        // 内部函数

        protected override void UpdateDisplay()
        {
            if (!this.IsTimerStarted())
                return;

            long ticks = m_leftTime.Ticks;
            if (ticks < 0) ticks = 0;
            if (ticks == 0 && !string.IsNullOrEmpty(textExpired))
            {
                SetText(textExpired);
                if (EndTimeCallBack != null)
                {
                    EndTimeCallBack();
                }
            }
            else
            {
                bool result = false;
                string text = "";
                switch (m_textType)
                {
                    case UNIT_DAY:
                        result = FormatText(out text, ticks, textDay, m_displayUnit);
                        break;
                    case UNIT_HOUR:
                        result = FormatText(out text, ticks, textHour, m_displayUnit);
                        break;
                    case UNIT_MINUTE:
                        result = FormatText(out text, ticks, textMinute, m_displayUnit);
                        break;
                    case UNIT_SECOND:
                        result = FormatText(out text, ticks, textSeconds, m_displayUnit);
                        break;
                    default:
                        text = textExpired;
                        break;
                }
                if (result)
                {
                    SetText(text);
                }
            }
        }

        protected override void UpdateDisplayUnit()
        {
            double leftSeconds = m_leftTime.TotalSeconds;
            if (leftSeconds >= 86400.0d)
            {
                m_textType = UNIT_DAY;
                SetDisplayUnit(unitDay);
            }
            else if (leftSeconds >= 3600.0d)
            {
                m_textType = UNIT_HOUR;
                SetDisplayUnit(unitHour);
            }
            else if (leftSeconds >= 60.0d)
            {
                m_textType = UNIT_MINUTE;
                SetDisplayUnit(unitMinute);
            }
            else
            {
                m_textType = UNIT_SECOND;
                SetDisplayUnit(unitSeconds);
            }
        }

        protected virtual bool FormatText(out string text, long ticks, string format, int displayUnit)
        {
            if (m_timeValues == null)
            {
                m_timeValues = new long[UNIT_COUNT];
            }

            long mod = 0;
            long unitTotalTicks = 0;
            if (carryTime)
            {
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    if ((displayUnit & UNIT_ARRAY[i]) != 0)
                    {
                        unitTotalTicks = UNIT_TOTAL_TICKS[i];
                        mod = ticks % unitTotalTicks;
                        if (mod > 0)
                        {
                            ticks += unitTotalTicks - mod;
                        }
                        break;
                    }
                }
            }

            int valueCount = 0;
            long timeValue = 0;
            long sum = 0;
            for (int i = UNIT_COUNT - 1; i >= 0; --i)
            {
                if ((displayUnit & UNIT_ARRAY[i]) != 0)
                {
                    unitTotalTicks = UNIT_TOTAL_TICKS[i];
                    timeValue = (ticks - sum) / unitTotalTicks;
                    sum += timeValue * unitTotalTicks;
                    m_timeValues[valueCount++] = timeValue;
                }
                else
                {
                    if (carryLastBit && valueCount > 0 && m_timeValues[valueCount - 1] == 0)
                        m_timeValues[valueCount - 1] = 1;
                }
            }

            bool sameTimeValue = false;
            if (m_lastTimeValues == null)
            {
                m_lastTimeValues = new long[UNIT_COUNT];
            }
            else
            {
                sameTimeValue = true;
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    if (m_lastTimeValues[i] != m_timeValues[i])
                    {
                        sameTimeValue = false;
                        break;
                    }
                }
            }

            if (sameTimeValue && m_lastFormat == format)
            {
                text = "";
                return false;
            }
            else
            {
                m_lastFormat = format;
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    m_lastTimeValues[i] = m_timeValues[i];
                }
            }

            switch (valueCount)
            {
                case 4:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1], m_timeValues[2], m_timeValues[3]);
                    break;
                case 3:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1], m_timeValues[2]);
                    break;
                case 2:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1]);
                    break;
                case 1:
                    text = string.Format(format, m_timeValues[0]);
                    break;
                default:
                    text = format;
                    break;
            }

            return true;
        }
    }
}