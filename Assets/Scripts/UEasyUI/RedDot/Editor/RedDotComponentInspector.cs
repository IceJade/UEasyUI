using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UEasyUI
{
    [CustomEditor(typeof(RedDotComponent))]
    public class RedDotComponentInspector : Editor
    {
        Dictionary<int, Dot> m_Dots = new Dictionary<int, Dot>();
        Vector2 m_Position = Vector2.zero;
        int m_Level = 0;
        public override void OnInspectorGUI()
        {
            RedDotComponent instance = target as RedDotComponent;
            var type = instance.GetType();
            FieldInfo fieldInfo = type.GetField("m_Nodes", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                object value = fieldInfo.GetValue(instance);
                Dictionary<int, UINotificationNode> nodes = value as Dictionary<int, UINotificationNode>;
                Dictionary<int, Dot> dots = new Dictionary<int, Dot>();
                foreach (var kvp in nodes)
                {
                    var node = kvp.Value;
                    if (node.IsRoot())
                    {
                        if (m_Dots.ContainsKey(node.NodeHash))
                        {
                            Dot dot = m_Dots[node.NodeHash];
                            dot.ModifyData(node, dot.IsFold);
                        }
                        else
                        {
                            m_Dots.Add(node.NodeHash, new Dot(node));
                        }
                    }
                    dots.Add(node.NodeHash, new Dot(node));
                }

                List<int> remove = new List<int>();
                foreach (var kvp in m_Dots)
                {
                    if (!dots.ContainsKey(kvp.Key))
                    {
                        remove.Add(kvp.Key);
                    }
                }
                foreach (var key in remove)
                {
                    m_Dots.Remove(key);
                }
                m_Position = GUILayout.BeginScrollView(m_Position);
                {
                    m_Level = 0;
                    //Draw Dot Tree
                    foreach (var kvp in m_Dots)
                    {
                        DrawFold(kvp.Value, 0);
                    }
                }
                GUILayout.EndScrollView();
            }
        }

        void DrawFold(Dot dot, int depth)
        {
            EditorGUILayout.BeginVertical();
            {
                var rect = EditorGUILayout.BeginHorizontal();
                {
                    rect.x += (depth * 6 + 10);
                    rect.y += m_Level * 20;
                    m_Level++;
                    rect.width = 300;
                    rect.height = 20;
                    if (dot.Children.Count > 0)
                    {
                        var style = EditorStyles.foldout;
                        style.richText = true;
                        dot.IsFold = EditorGUI.Foldout(rect, dot.IsFold, dot.ToString(), true, style);
                        AddButton(dot, rect, depth);
                        EditorGUILayout.EndHorizontal();
                        if (dot.IsFold)
                        {
                            foreach (var kvp in dot.Children)
                            {
                                DrawFold(kvp.Value, depth + 1);
                            }
                        }
                    }
                    else
                    {
                        var style = EditorStyles.label;
                        style.richText = true;
                        GUI.Button(rect, dot.ToString(), style);
                        AddButton(dot, rect, depth);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void AddButton(Dot dot, Rect rect,int depth)
        {
            foreach (var kvp in dot.Children)
            {
                if(kvp.Value.Visible)
                {
                    return;
                }
            }
            rect.x += (350 - (depth * 6 + 10));
            rect.width = 80;
            rect.height = 18;
            if (dot.Visible)
            {
                if (GUI.Button(rect, "隐藏", EditorStyles.miniButton))
                {
                    GameEntry.RedDot.Set(dot.Path, false, "Editor");
                }
            }
            else
            {
                if (GUI.Button(rect, "显示", EditorStyles.miniButton))
                {
                    GameEntry.RedDot.Set(dot.Path, true, "Editor");
                }
            }
        }

        private class Dot
        {
            public Dot(UINotificationNode node)
            {
                Init(node);
            }
            private Dot(UINotificationNode node, Dot parent)
            {
                Parent = parent;
                Init(node);
            }

            private void Init(UINotificationNode node)
            {
                if (node.Children != null && node.Children.Count > 0)
                {
                    foreach (var dot in node.Children)
                    {
                        Children.Add(dot.NodeHash, new Dot(dot, this));
                    }
                }
                Path = node.Path;
                Visible = node.IsRedDotVisible;
                Count = node.NotificationCount;
            }
            public string Path { get; private set; }
            public bool IsFold { get; set; } = false;
            public bool Visible { get; private set; }
            public Dot Parent { get; private set; }

            private int Count { get; set; }
            public Dictionary<int, Dot> Children = new Dictionary<int, Dot>();

            public void ModifyData(UINotificationNode node, bool isFold)
            {
                if (node.Children != null && node.Children.Count > 0)
                {
                    HashSet<int> childrenHash = new HashSet<int>();
                    foreach (var child in node.Children)
                    {
                        if (Children.TryGetValue(child.NodeHash, out Dot dot))
                        {
                            dot.ModifyData(child, dot.IsFold);
                        }
                        else
                        {
                            Children.Add(child.NodeHash, new Dot(child, this));
                        }
                        childrenHash.Add(child.NodeHash);
                    }
                    List<int> remove = new List<int>();
                    foreach (var kvp in Children)
                    {
                        if (!childrenHash.Contains(kvp.Key))
                        {
                            remove.Add(kvp.Key);
                        }
                    }
                    foreach (var key in remove)
                    {
                        Children.Remove(key);
                    }
                }

                Visible = node.IsRedDotVisible;
                Count = node.NotificationCount;
                IsFold = isFold;
            }
            public override string ToString()
            {
                if (Visible)
                {
                    return $"<color=#888888FF>{Path}     Notice Count:{Count}</color>";
                }
                else
                {
                    return $"<color=#616161FF>{Path}     Notice Count:{Count}</color>";
                }
            }
        }
    }
}
