//------------------------------------------------------------
// UEasyUI v1.0
// Copyright © 2013-2017 Zhou Ming Feng. All rights reserved.
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UEasyUI
{
    /// <summary>
    /// 日志辅助器。
    /// </summary>
    internal class LogHelper
    {
        private bool m_isActiveLog = false;

        /// <summary>
        /// 是否显示（打印） 日志
        /// </summary>
        /// <param name="isShow"></param>
        public void ActiveLog(bool isShow)
        {
            m_isActiveLog = isShow;
        }

        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(LogLevel level, object message)
        {
            if (m_isActiveLog)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        Debug.Log(string.Format("<color=#888888>_tetris_ {0}</color>", message.ToString()));
                        break;

                    case LogLevel.Info:
                        Debug.Log("_tetris_ " + message.ToString());
                        break;

                    case LogLevel.Warning:
                        Debug.LogWarning("_tetris_ " + message.ToString());
                        break;

                    case LogLevel.Error:
                        Debug.LogError("_tetris_ " + message.ToString());
                        break;

                    default:
                        throw new Exception(message.ToString());
                }
            }
        }
    }
}