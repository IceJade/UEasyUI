
using System.Collections.Generic;
using UnityEngine;

namespace UEasyUI
{
    public class EventComponent : MonoBehaviour
    {
        public delegate void OnEventHandler(object userData);

        public Dictionary<short, LinkedList<OnEventHandler>> dic = new Dictionary<short, LinkedList<OnEventHandler>>();

        #region Subscribe 添加监听
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public void Subscribe(short key, OnEventHandler handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].AddLast(handler);
            }
            else
            {
                LinkedList<OnEventHandler> lstHandler = new LinkedList<OnEventHandler>();
                lstHandler.AddLast(handler);
                dic[key] = lstHandler;
            }
        }
        #endregion

        #region Unsubscribe 移除监听
        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(short key, OnEventHandler handler)
        {
            if (dic.ContainsKey(key))
            {
                LinkedList<OnEventHandler> lstHandler = dic[key];
                lstHandler.Remove(handler);
                if (lstHandler.Count == 0)
                {
                    dic.Remove(key);
                }
            }

        }

        #endregion

        #region Check 检查监听

        /// <summary>
        /// 判断事件是否已经注册上了
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(short key, OnEventHandler handler)
        {
            if (dic.ContainsKey(key))
            {
                LinkedList<OnEventHandler> lstHandler = dic[key];
                for (var item = lstHandler.First; item != null;)
                {
                    if (item.Value != null && item.Value == handler)
                    {
                        return true;
                    }

                    var next = item.Next;
                    item = next;
                }
            }

            return false;
        }

        #endregion

        #region Fire 派发
        /// <summary>
        /// 派发
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userData"></param>
        public void Fire(short key, object userData)
        {
            if (dic.ContainsKey(key))
            {
                LinkedList<OnEventHandler> lstHandler = dic[key];
                if (lstHandler != null && lstHandler.Count > 0)
                {
                    for (var item = lstHandler.First; item != null;)
                    {
                        var next = item.Next;
                        if (item.Value != null)
                        {
                            item.Value(userData);
                        }
                        item = next;
                    }
                }
            }
        }

        public void Fire(short key)
        {
            Fire(key, null);
        }
        #endregion
    }
}