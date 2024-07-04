using System;

namespace UEasyUI
{
    public class TimeUtility
    {
        private static readonly DateTime time_1970 = new DateTime(1970, 1, 1);

        //获取从1970.1.1日0:0到现在为止的总秒数
        public static UInt64 SecondsFromBegin()
        {
            //DateTime beginTime = new DateTime(1970, 1, 1);
            return (UInt64)(DateTime.Now - time_1970).TotalSeconds;
        }

        public static DateTime TimeFromSeconds(UInt64 seconds)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            DateTime dt = new DateTime(1970, 1, 1);
            return dt.AddSeconds(seconds);
        }

        public static DateTime TimeFromSeconds(int seconds)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            DateTime dt = new DateTime(1970, 1, 1);
            return dt.AddSeconds(seconds);
        }

        public static DateTime TimeFromSeconds(long seconds)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            DateTime dt = new DateTime(1970, 1, 1);
            return dt.AddSeconds(seconds);
        }

        //获取从1970.1.1日0:0到指定日期的总秒数
        public static UInt64 SecondsFromBegin(DateTime dateTime)
        {
            //DateTime beginTime = new DateTime(1970, 1, 1);
            return (UInt64)(dateTime - time_1970).TotalSeconds;
        }

        //本周的第一秒（从1970.1.1日0:0开始计时）
        public static UInt64 FirstSecondOfWeek()
        {
            const UInt64 FOURDAY_SECONDS = 4 * 24 * 3600;
            const UInt64 WEEK_SECONDS = 7 * 24 * 3600;
            if (SecondsFromBegin() < FOURDAY_SECONDS)
            {
                return 0;
            }
            else
            {
                return (SecondsFromBegin() - FOURDAY_SECONDS) / WEEK_SECONDS * WEEK_SECONDS + FOURDAY_SECONDS;
            }
        }

        //返回：判断现在与参数是否在同周
        //参数iSeconds：任意的秒数
        public static bool IsSameWeek(UInt64 iSeconds)
        {
            const UInt64 THREEDAY_SECONDS = 3 * 24 * 3600;
            const UInt64 WEEK_SECONDS = 7 * 24 * 3600;
            iSeconds += THREEDAY_SECONDS;
            UInt64 iSecondsOther = SecondsFromBegin() + THREEDAY_SECONDS;
            return iSeconds / WEEK_SECONDS == iSecondsOther / WEEK_SECONDS;
        }

        //判断两个时间点是否在同周  added by haoshubin at 2012-11-17
        public static bool IsSameWeek(UInt64 iSeconds1, UInt64 iSeconds2)
        {
            const UInt64 THREEDAY_SECONDS = 3 * 24 * 3600;
            const UInt64 WEEK_SECONDS = 7 * 24 * 3600;
            iSeconds1 += THREEDAY_SECONDS;
            iSeconds2 += THREEDAY_SECONDS;
            return iSeconds1 / WEEK_SECONDS == iSeconds2 / WEEK_SECONDS;
        }

        //返回：判断现在与参数是否在同天
        //参数iSeconds：任意的秒数
        public static bool IsSameDay(UInt64 iSeconds)
        {
            const UInt64 DAY_SECONDS = 24 * 3600;
            return SecondsFromBegin() / DAY_SECONDS == iSeconds / DAY_SECONDS;
        }

        //以两天为一个时间间隔，现在所在时间间隔的第一秒（从1970.1.1日0:0开始计时）
        public static UInt64 FirstSecondOfTwoDays()
        {
            const UInt64 TWODAY_SECONDS = 2 * 24 * 3600;
            return (SecondsFromBegin()) / TWODAY_SECONDS * TWODAY_SECONDS;
        }

        //以两天为一个时间间隔
        //返回：判断现在与参数是否在同一个时间间隔
        //参数iSeconds：任意的秒数
        public static bool IsSameTwoDays(UInt64 iSeconds)
        {
            const UInt64 TWODAY_SECONDS = 2 * 24 * 3600;
            return iSeconds / TWODAY_SECONDS == SecondsFromBegin() / TWODAY_SECONDS;
        }

        //以两天为一个时间间隔
        //返回：判断现在是否在一个时间间隔的前五分钟
        public static bool IsFiveMinuteBeforeOneDay()
        {
            const UInt64 TWODAY_SECONDS = 2 * 24 * 3600;
            const UInt64 FIVEMINUTE_SECONDS = 5 * 60;
            return SecondsFromBegin() / TWODAY_SECONDS != (SecondsFromBegin() + FIVEMINUTE_SECONDS) / TWODAY_SECONDS;
        }

        //返回现在距今天零点已过去多少秒
        public static int SecondsFromToday()
        {
            DateTime now = DateTime.Now;
            return now.Hour * 60 * 60 + now.Minute * 60 + now.Second;
        }


        //返回指定时间距当天零点已过去多少秒
        public static int SecondsFromToday(DateTime dateTime)
        {
            return dateTime.Hour * 60 * 60 + dateTime.Minute * 60 + dateTime.Second;
        }

        //返回是否在两天内
        public static bool IsBetweenTwoDays(UInt64 iSeconds)
        {
            const int TWODAY_SECONDS = 2 * 24 * 3600;
            return Math.Abs((int)(iSeconds - SecondsFromBegin())) <= TWODAY_SECONDS;
        }

        //返回是否在连续的两天内
        public static bool IsInContinuousTwoDays(UInt64 iSeconds)
        {
            const int ONEDAY_SECONDS = 24 * 3600;
            return Math.Abs((int)(iSeconds / ONEDAY_SECONDS - SecondsFromBegin() / ONEDAY_SECONDS)) == 1;
        }

        /// <summary>
        /// 返回00 分秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string FormatSecond(float seconds)
        {
            int minute = (int)((seconds) / 60);
            int second = (int)(seconds) % 60;
            //return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);

            var sb = StringHelper.PoolNew();
            if (minute >= 10)
            {
                sb.Append(minute);
            }
            else
            {
                sb.Append("0");
                sb.Append(minute);
            }
            sb.Append(":");
            if (second >= 10)
            {
                sb.Append(second);
            }
            else
            {
                sb.Append("0");
                sb.Append(second);
            }
            //sb.AppendFormat("{0:D2}:{1:D2}", minute, second);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}:{1:D2}", minute, second);
        }

        /// <summary>
        /// 返回0 分秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string FormatSecondNormal(float seconds)
        {
            int hour = (int)(seconds) / 3600;
            int minute = (int)((seconds) / 60) % 60;
            int second = (int)(seconds) % 60;
            //return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);

            var sb = StringHelper.PoolNew();
            sb.Append(minute);
            sb.Append(":");
            if (second >= 10)
            {
                sb.Append(second);
            }
            else
            {
                sb.Append("0");
                sb.Append(second);
            }
            //sb.AppendFormat("{0:G}:{1:D2}", minute, second);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:G}:{1:D2}", minute, second);
        }

        /// <summary>
        /// 返回00：00 分秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string FormatSecondHour(float seconds)
        {
            int hour = (int)(seconds) / 3600;
            int minute = (int)((seconds) / 60) % 60;
            int second = (int)(seconds) % 60;

            var sb = StringHelper.PoolNew();
            if (hour >= 10)
            {
                sb.Append(hour);
            }
            else
            {
                sb.Append("0");
                sb.Append(hour);
            }
            sb.Append(":");
            if (minute >= 10)
            {
                sb.Append(minute);
            }
            else
            {
                sb.Append("0");
                sb.Append(minute);
            }
            sb.Append(":");
            if (second >= 10)
            {
                sb.Append(second);
            }
            else
            {
                sb.Append("0");
                sb.Append(second);
            }
            //sb.AppendFormat("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }

        public static string FormatPirateSecondHour(float seconds)
        {
            int hour = (int)(seconds) / 3600;
            int minute = (int)((seconds) / 60) % 60;
            int second = (int)(seconds) % 60;

            var sb = StringHelper.PoolNew();
            if (hour >= 10)
            {
                sb.Append(hour);
            }
            else
            {
                sb.Append("0");
                sb.Append(hour);
            }
            sb.Append("小时");
            if (minute >= 10)
            {
                sb.Append(minute);
            }
            else
            {
                sb.Append("0");
                sb.Append(minute);
            }
            sb.Append("分钟");         
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
        }


        public static string FormatMinuteHour(float seconds)
        {
            int hour = (int)(seconds) / 3600;
            int minute = (int)((seconds) / 60) % 60;
            int second = (int)(seconds) % 60;

            var sb = StringHelper.PoolNew();
            if (hour >= 10)
            {
                sb.Append(hour);
            }
            else
            {
                sb.Append("0");
                sb.Append(hour);
            }
            sb.Append(":");
            if (minute >= 10)
            {
                sb.Append(minute);
            }
            else
            {
                sb.Append("0");
                sb.Append(minute);
            }
            //sb.AppendFormat("{0:D2}:{1:D2}", hour, minute);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}:{1:D2}", hour, minute);
        }

        public static string FormatBySecond(float seconds)
        {
            int hour = (int)(seconds) / 3600;
            int minute = (int)((seconds) / 60) % 60;
            int second = (int)(seconds) % 60;
            //return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            
            var sb = StringHelper.PoolNew();
            if (second >= 10)
            {
                sb.Append(second);
            }
            else
            {
                sb.Append("0");
                sb.Append(second);
            }
            //sb.AppendFormat("{0:D2}", second);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}",second);
        }

        public static string FormatMillionSecond(float seconds)
        {
            int elapseSecond = (int)seconds;
            int elapseMS = (int)((seconds - elapseSecond) * 100);

            var sb = StringHelper.PoolNew();
            if (elapseSecond >= 10)
            {
                sb.Append(elapseSecond);
            }
            else
            {
                sb.Append("0");
                sb.Append(elapseSecond);
            }
            sb.Append("''");
            if (elapseMS >= 10)
            {
                sb.Append(elapseMS);
            }
            else
            {
                sb.Append("0");
                sb.Append(elapseMS);
            }
            //sb.AppendFormat("{0:D2}''{1:D2}", elapseSecond, elapseMS);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}''{1:D2}", elapseSecond, elapseMS);
        }

        /// <summary>
        /// 转换马拉松最长时间格式
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string FormatEndlessMaxTime(float seconds)
        {
            int elapseSecond = (int) seconds;
            int finalMinutes = elapseSecond / 60;
            int finalSeconds = elapseSecond % 60;

            var sb = StringHelper.PoolNew();
            if (finalMinutes >= 10)
            {
                sb.Append(finalMinutes);
            }
            else
            {
                sb.Append("0");
                sb.Append(finalMinutes);
            }
            sb.Append("分'");
            if (finalSeconds >= 10)
            {
                sb.Append(finalSeconds);
            }
            else
            {
                sb.Append("0");
                sb.Append(finalSeconds);
            }
            sb.Append("秒");
            //sb.AppendFormat("{0:D2}''{1:D2}", elapseSecond, elapseMS);
            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
            //return string.Format("{0:D2}''{1:D2}", elapseSecond, elapseMS);
        }

        /// <summary>
        /// 从标准时间多少秒后的时间转换
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnitxTimeStamp2LocalTime(int timeStamp)
        {
            var time = new DateTime(1970, 1, 1);
            return time.AddSeconds(timeStamp).ToLocalTime();
        }

        /// <summary>
        /// 当前时间的秒数
        /// </summary>
        /// <returns></returns>
        public static int CurrentTimeSecond()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (int)timeSpan.TotalSeconds;
        }

        public static long CurrentTimeMillSecond()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }

        public static int LocalTime2ServerTimeStamp()
        {
            var time = UnitxTimeStamp2LocalTime(CurrentTimeSecond());
            return (time.Second + time.Minute * 60 + time.Hour * 3600);
        }

        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        //public static string TimeSpanFormat(TimeSpan timeSpan)
        //{
        //    string hours = (timeSpan.Hours / 10) > 0 ? timeSpan.Hours.ToString() : string.Format("0{0}", timeSpan.Hours);
        //    string minutes = (timeSpan.Minutes / 10) > 0 ? timeSpan.Minutes.ToString() : string.Format("0{0}", timeSpan.Minutes);
        //    string seconds = (timeSpan.Seconds / 10) > 0 ? timeSpan.Seconds.ToString() : string.Format("0{0}", timeSpan.Seconds);

        //    if (timeSpan.Days > 0)
        //    {
        //        return string.Format("{0} day {1}:{2}:{3}", timeSpan.Days, hours, minutes, seconds);
        //    }
        //    else
        //    {
        //        return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
        //    }
        //}
        public static string TimeSpanFormat(TimeSpan timeSpan)
        {
            var sb = StringHelper.PoolNew();
            if (timeSpan.Days > 0)
            {
                sb.AppendFormatNoGC_Int("{0} day ", timeSpan.Days);
            }

            sb.Append(timeSpan.Hours / 10 > 0 ? "{0}:" : "0{0}:");
            sb = sb.ReplaceNoGC("{0}", StringHelper.IntToStr(timeSpan.Hours));

            sb.Append(timeSpan.Minutes / 10 > 0 ? "{0}:" : "0{0}:");
            sb = sb.ReplaceNoGC("{0}", StringHelper.IntToStr(timeSpan.Minutes));

            sb.Append(timeSpan.Seconds / 10 > 0 ? "{0}" : "0{0}");
            sb = sb.ReplaceNoGC("{0}", StringHelper.IntToStr(timeSpan.Seconds));

            string tempString = sb.ToString();
            StringHelper.PoolDel(ref sb);
            return tempString;
        }

        /// <summary>
        /// 当前服务器时间戳
        /// </summary>
        /// <returns></returns>
        public static long CurrentServerMillSecond()
        {
            // TODO:返回服务器时间戳;
            //return NetManager.Instance.GetServerTimestamp();
            return 0;
        }

        public static string TimeConvert(int seconds)
        {
            if (seconds <= 0)
                return "";

            int days = (seconds) / (3600 * 24);
            int hours = ((seconds) / 3600) % 24;
            int minute = (seconds - days * 24 * 3600 - hours * 3600) / 60;
            int second = (seconds - days * 24 * 3600 - hours * 3600) % 60;

            var dayStr = StringHelper.PoolNew();
            if (days < 10)
            {
                dayStr.Append("0");
                dayStr.Append(days);
            }
            else
                dayStr.Append(days);

            var hourStr = StringHelper.PoolNew();
            if (hours < 10)
            {
                hourStr.Append("0");
                hourStr.Append(hours);
            }
            else
                hourStr.Append(hours);

            var minuteStr = StringHelper.PoolNew();
            if (minute < 10)
            {
                minuteStr.Append("0");
                minuteStr.Append(minute);
            }
            else
                minuteStr.Append(minute);

            var secondStr = StringHelper.PoolNew();
            if (second < 10)
            {
                secondStr.Append("0");
                secondStr.Append(second);
            }
            else
                secondStr.Append(second);

            string tempString = "";
            if (days > 0)
            {
                tempString = string.Format("{0}天{1}小时", dayStr, hourStr);
            }
            else
            {
                tempString = string.Format("{0}:{1}:{2}", hourStr, minuteStr, secondStr);
            }

            StringHelper.PoolDel(ref dayStr);
            StringHelper.PoolDel(ref hourStr);
            StringHelper.PoolDel(ref minuteStr);
            StringHelper.PoolDel(ref secondStr);

            return tempString;
        }

        /// <summary>
        /// 获取客户端的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetClientTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间 
        /// </summary>
        /// <param name="timeStamp">毫秒数</param>
        /// <returns></returns>
        public static DateTime StampToDateTime(long timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = 0;
            if(long.TryParse(timeStamp.ToString() + "0000", out lTime))
            {
                TimeSpan toNow = new TimeSpan(lTime);
                return dateTimeStart.Add(toNow);
            }

            return DateTime.Now;
        }

        public static long DateTimeToStamp(DateTime dateTime)
        {
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);   
        }

        //获取服务器的时间第二天的零点(国际标准时间不是北京时间)
        public static long ZeroTime()
        {
           DateTime time =  StampToDateTime(CurrentServerMillSecond()).AddDays(1);
           DateTime time2 = time.Date;
            return (long)SecondsFromBegin(time2);
        }

        /// <summary>
        /// 设置聊天时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampStringToDateTime(string timeStamp)
        {
            long lTime = 0;
            if (long.TryParse(timeStamp + "0000000", out lTime))
            {
                TimeSpan toNow = new TimeSpan(lTime);

                DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return dateTimeStart.Add(toNow);
            }

            return DateTime.Now;
        }

        /// <summary>
        /// 服务器时间转为C#格式时间 
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerDateTime()
        {
            return UnitxTimeStamp2LocalTime((int)(TimeUtility.CurrentServerMillSecond() / 1000));
        }
    }
}