using System.Collections.Generic;
using UnityEngine;

namespace UEasyUI
{
    // 红点通知操作：显隐Object
    public class UINotificationOPRedDot
    {
        public int InstanceId;
        public string Path;
        public int NodeHash;
        private GameObject m_GameObject;

        private List<UINotificationOPRedDot> m_Brothers = new List<UINotificationOPRedDot>();
        public UINotificationOPRedDot(int nodeHash, GameObject gameObject)
        {
            NodeHash = nodeHash;
            m_GameObject = gameObject;
            if (gameObject != null)
            {
                InstanceId = gameObject.GetInstanceID();
            }
        }

        public void OnNotification(string path, int nodeHash, bool show)
        {
            if (m_GameObject != null)
            {
                m_GameObject.SetActive(show);
            }
        }

        public UINotificationOPRedDot AddNode(UINotificationOPRedDot node)
        {
            m_Brothers.Add(node);

            return node;
        }

        public bool RemoveNode(UINotificationOPRedDot node)
        {
            return m_Brothers.Remove(node);
        }

        public UINotificationOPRedDot Peek()
        {
            return m_Brothers.Count > 0 ? m_Brothers[0] : null;
        }

        public UINotificationOPRedDot RemoveFirst()
        {
            m_Brothers.RemoveAt(0);
            return Peek();
        }

        public void NotifyOthers(string path, int nodeHash, bool show)
        {
            foreach (var item in m_Brothers)
            {
                item.OnNotification(path, nodeHash, show);
            }
        }

        public override string ToString()
        {
            return Path + "     " + base.ToString();
        }
    }
}
