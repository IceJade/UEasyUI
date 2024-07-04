//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace UEasyUI
{
    /// <summary>
    /// 游戏逻辑事件基类。
    /// </summary>
    public abstract class GameEventArgs : EventArgs
    {
        public static Dictionary<int, int> m_EventCountDic = new Dictionary<int, int>();

        public static void SetEventCount(int eventId, int count)
        {
            m_EventCountDic[eventId] = count;
        }

        public static int GetEventCount(int eventId)
        {
            int count = 0;

            m_EventCountDic.TryGetValue(eventId, out count);

            if (count <= 5)
                return 5;

            return count;
        }
    }
}
