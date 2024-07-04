//------------------------------------------------------------
// UEasyUI v1.0
// Copyright © 2013-2017 Zhou Ming Feng. All rights reserved.
//------------------------------------------------------------

using System;
using System.Diagnostics;

namespace UEasyUI
{
    /// <summary>
    /// 日志类。
    /// </summary>
    public static partial class Log
    {
        private static LogHelper s_LogHelper = new LogHelper();

        /// <summary>
        /// 是否显示（打印）日志
        /// </summary>
        /// <param name="isShow"></param>
        public static void ActiveLog(bool isShow)
        {
            if (s_LogHelper != null)
            {
                s_LogHelper.ActiveLog(isShow);
            }
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="message">日志内容。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(object message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="message">日志内容。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(string message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, string.Format(format, arg0));
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Debug(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Debug, string.Format(format, args));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(object message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="message">日志内容</param>
        /// 
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(string message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, string.Format(format, arg0));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Info(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Info, string.Format(format, args));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(object message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(string message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, string.Format(format, arg0));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>

        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        /// 
        [Conditional("_DEBUG"), Conditional("UNITY_EDITOR")]
        public static void Warning(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Warning, string.Format(format, args));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        
        public static void Error(object message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        
        public static void Error(string message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        
        public static void Error(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, string.Format(format, arg0));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        
        public static void Error(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        
        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        
        public static void Error(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Error, string.Format(format, args));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Fatal(object message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Fatal(string message)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Fatal(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, string.Format(format, arg0));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Fatal(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Fatal(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        public static void Fatal(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                throw new Exception("Log helper is invalid.");
            }

            s_LogHelper.Log(LogLevel.Fatal, string.Format(format, args));
        }
    }
}