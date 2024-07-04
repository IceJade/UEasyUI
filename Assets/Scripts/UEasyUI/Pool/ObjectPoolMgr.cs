using System;
using System.Collections.Generic;

namespace UEasyUI
{
    public sealed class ObjectPoolMgr
    {
        private static Dictionary<int, Queue<Object>> m_ObjectDic = new Dictionary<int, Queue<Object>>();

        public static T Dequeue<T>() where T : class, new()
        {
            lock (m_ObjectDic)
            {
                int key = typeof(T).GetHashCode();

                Queue<Object> queue = null;
                m_ObjectDic.TryGetValue(key, out queue);

                if (queue == null)
                {
                    queue = new Queue<Object>();
                    m_ObjectDic[key] = queue;
                }

                int maxCount = GameEventArgs.GetEventCount(key);

                //UnityEngine.Debug.Log("maxCount="+ maxCount);

                if (queue.Count > maxCount)
                {
                    //UnityEngine.Debug.Log("从池中获取");
                    return (T)queue.Dequeue();
                }
                else
                {
                    //UnityEngine.Debug.Log("创建新对象");
                    return new T();
                }
            }
        }

        /// <summary>
        /// 对象回池
        /// </summary>
        /// <param name="obj"></param>
        public static void Enqueue(Object obj)
        {
            if (null == obj)
                return;

            int key = obj.GetType().GetHashCode();

            Queue<Object> queue = null;
            m_ObjectDic.TryGetValue(key, out queue);

            if (!queue.Contains(obj))
                queue.Enqueue(obj);
        }
    }
}