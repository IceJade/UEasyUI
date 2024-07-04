using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace UEasyUI.Tools
{
    [CustomEditor(typeof(UITimerBase), true)]
    public class UITimerBaseEditor : Editor
    {
        protected SerializedProperty m_currentProp = null;
        protected long m_testValue = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawArea("Base Settings", DrawBaseSettings);
            DrawArea("Timer Settings", DrawTimerSettings);
            DrawArea("Test Area", DrawTestArea);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawBaseSettings()
        {
            m_currentProp = serializedObject.GetIterator();
            Type slotBaseType = typeof(UITimerBase);
            if (m_currentProp.NextVisible(true))
            {
                m_currentProp.NextVisible(false); // 跳过Script
                do
                {
                    if (HasField(slotBaseType, m_currentProp.name))
                    {
                        EditorGUILayout.PropertyField(m_currentProp);
                    }
                    else
                    {
                        break;
                    }
                } while (m_currentProp.NextVisible(false));
            }

        }

        protected virtual void DrawTimerSettings()
        {
            if (m_currentProp != null)
            {
                do
                {
                    EditorGUILayout.PropertyField(m_currentProp);
                } while (m_currentProp.NextVisible(false));
            }
        }

        protected virtual void DrawTestArea()
        {
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            UITimerBase timer = target as UITimerBase;
            m_testValue = EditorGUILayout.LongField("Milliseconds", m_testValue);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Start"))
            {
                timer.StartCountdown(m_testValue);
            }
            if (GUILayout.Button("Stop"))
            {
                timer.StopTimer(false, false, false);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawArea(string areaName, Action drawAction)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(areaName);
            drawAction();
            EditorGUILayout.EndVertical();
        }

        protected bool HasField(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) != null;
        }
    }
}