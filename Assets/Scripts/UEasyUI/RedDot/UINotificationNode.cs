using System.Collections.Generic;
using System.Linq;

namespace UEasyUI
{
    // 界面通知节点
    public class UINotificationNode
    {
        //----------------------------------------------------------------------
        // 常量

        protected const int NOTIFY_PARENT_COUNT = 1;
        protected const int NOTIFY_PARENT_ERASE = 2;

        //----------------------------------------------------------------------
        // 成员变量

        protected RedDotComponent m_Manager = null;
        protected HashSet<int> m_SelfCallerSet = new HashSet<int>();
        protected int m_SelfNotificationCount = 0;
        protected int m_ChildNotificationCount = 0;
        protected int m_LastSetCount = 0;
        protected bool m_AlwaysHide = false;
        protected bool m_RedDotVisible = false;
        protected long m_CurrentSerialNum = 0;
        protected long m_DisplaySerialNum = 0;

#if UNITY_EDITOR
        public HashSet<string> m_DebugCallerSet = new HashSet<string>();
#endif
        //----------------------------------------------------------------------
        // 属性

        public UINotificationNode Parent { get; set; }
        public List<UINotificationNode> Children { get; set; }

        public string Path { get; private set; }
        public int NodeHash { get; private set; }
        public int NotifyParent = NOTIFY_PARENT_COUNT;
        public int NotificationCount { get { return m_SelfNotificationCount + m_ChildNotificationCount; } }
        public bool IsRedDotVisible { get { return NotificationCount > 0 && m_CurrentSerialNum < m_DisplaySerialNum && !m_AlwaysHide; } }

        //----------------------------------------------------------------------
        // 构造函数
        public UINotificationNode(RedDotComponent manager, string path)
        {
            m_Manager = manager;
            Path = path;
            NodeHash = RedDotComponent.GetNodeHash(Path);
        }

        //----------------------------------------------------------------------
        // 公用函数

        public void Reset()
        {
            m_RedDotVisible = false;
            m_CurrentSerialNum = 0;
            m_DisplaySerialNum = 0;
        }

        public bool IsRoot()
        {
            return Parent == null;
        }

        public void AddChild(UINotificationNode child)
        {
            if (child == null)
            {
                return;
            }

            child.Parent = this;
            if (Children != null)
            {
                Children.Add(child);
            }
            else
            {
                Children = new List<UINotificationNode>();
                Children.Add(child);
            }
        }

        public void IncreaseNotificationCount(string caller, bool sendEvent = true)
        {
            UpdateDisplaySerialNum();
            int oldCount = NotificationCount;
            int callerId = RedDotComponent.GetNodeHash(caller);
            if (m_SelfCallerSet.Contains(callerId))
            {
                return;
            }
            m_SelfCallerSet.Add(callerId);
#if UNITY_EDITOR
            m_DebugCallerSet.Add(caller);
#endif
            m_SelfNotificationCount = m_SelfCallerSet.Count;
            if (NotifyParent > 0)
            {
                UpdateParentNotificationCount(sendEvent);
            }
            if (sendEvent)
            {
                UpdateRedDot();
            }
        }

        public void DecreaseNotificationCount(string caller, bool sendEvent = true)
        {
            int callerId = RedDotComponent.GetNodeHash(caller);
            if (m_SelfCallerSet.Remove(callerId))
            {
#if UNITY_EDITOR
                m_DebugCallerSet.Remove(caller);
#endif
                m_SelfNotificationCount = m_SelfCallerSet.Count;
                UpdateParentNotificationCount(sendEvent);
                if (sendEvent)
                {
                    UpdateRedDot();
                }
            }
        }

        public void ClearNotificationCount(bool clearChildren, bool sendEvent = true)
        {
            m_SelfCallerSet.Clear();
#if UNITY_EDITOR
            m_DebugCallerSet.Clear();
#endif
            m_SelfNotificationCount = 0;
            int childNotifyParent = 0;
            if (clearChildren && Children != null)
            {
                foreach (var child in Children)
                {
                    childNotifyParent |= child.NotifyParent;
                    child.ClearNotificationCount(true, sendEvent);
                }
            }
            // 如果子节点没有配置通知父级，则由此节点通知父级
            if (childNotifyParent <= 0)
            {
                UpdateParentNotificationCount(sendEvent);
            }
            if (sendEvent)
            {
                UpdateRedDot();
            }
        }

        public bool IsAlwaysHide()
        {
            return m_AlwaysHide;
        }

        public void SetAlwaysHide(bool value)
        {
            if (m_AlwaysHide != value)
            {
                m_AlwaysHide = value;
                UpdateRedDot();
            }
        }

        public void EraseDisplay()
        {
            if (m_CurrentSerialNum < m_DisplaySerialNum)
            {
                m_CurrentSerialNum = m_DisplaySerialNum;
                UpdateRedDot();
            }
            if (Parent != null && NotifyParent > 0)
            {
                UpdateParentNotificationCount();
                if (NotifyParent == NOTIFY_PARENT_ERASE)
                {
                    UINotificationNode parentNode = (Parent as UINotificationNode);
                    parentNode.EraseDisplay();
                }
            }
        }

        public void OnModuleEnableInited()
        {
            UpdateParentNotificationCount();
            UpdateRedDot();
        }

        public void OnModuleSwitchUpdate()
        {
            OnModuleEnableInited();
        }

        //----------------------------------------------------------------------
        // 内部函数

        protected void ChangeChildNotificationCount(int count, bool sendEvent = true)
        {
            m_ChildNotificationCount += count;
            if (m_ChildNotificationCount < 0)
            {
                m_ChildNotificationCount = 0;
            }
            if (sendEvent)
            {
                UpdateRedDot();
            }
        }

        protected void UpdateParentNotificationCount(bool sendEvent = true)
        {
            if (Parent != null && NotifyParent > 0)
            {
                UINotificationNode parentNode = (Parent as UINotificationNode);
                int count = IsRedDotVisible ? NotificationCount : 0;
                int diff = count - m_LastSetCount;
                m_LastSetCount = count;
                if (diff != 0)
                {
                    parentNode.ChangeChildNotificationCount(diff, sendEvent);
                }
                parentNode.UpdateParentNotificationCount(sendEvent);
            }
        }

        protected void UpdateDisplaySerialNum()
        {
            m_DisplaySerialNum = RedDotComponent.GetDisplaySerialNum();
            if (Parent != null && NotifyParent > 0)
            {
                UINotificationNode parentNode = (Parent as UINotificationNode);
                parentNode.UpdateDisplaySerialNum();
            }
        }

        protected void UpdateRedDot()
        {
            bool visible = IsRedDotVisible;
            if (m_RedDotVisible != visible)
            {
                m_RedDotVisible = visible;
                m_Manager.OnNotification(this, m_RedDotVisible);
            }
        }

#if UNITY_EDITOR
        public List<string> DebugGetCallerNames()
        {
            return m_DebugCallerSet.ToList();
        }
#endif
    }
}
