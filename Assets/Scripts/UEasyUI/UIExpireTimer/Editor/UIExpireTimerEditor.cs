using UnityEngine;
using UnityEditor;

namespace UEasyUI.Tools
{
    [CustomEditor(typeof(UIExpireTimer))]
    public class UIExpireTimerEditor : UITimerBaseEditor
    {
        protected static string[] m_unitMaskNames = new string[] { "Seconds", "Minute", "Hour", "Day" };

        protected override void DrawTimerSettings()
        {
            UIExpireTimer timer = target as UIExpireTimer;
            UGUIEditorTools.DrawProperty("Text", serializedObject, "text");
            UGUIEditorTools.DrawProperty("Carry Time", serializedObject, "carryTime");
            UGUIEditorTools.DrawProperty("Carry Last Bit", serializedObject, "carryLastBit");
            UGUIEditorTools.DrawProperty("Text Expired", serializedObject, "textExpired");

            DrawUnitField("Text Day", serializedObject.FindProperty("unitDay"), serializedObject.FindProperty("textDay"));
            DrawUnitField("Text Hour", serializedObject.FindProperty("unitHour"), serializedObject.FindProperty("textHour"));
            DrawUnitField("Text Minute", serializedObject.FindProperty("unitMinute"), serializedObject.FindProperty("textMinute"));
            DrawUnitField("Text Seconds", serializedObject.FindProperty("unitSeconds"), serializedObject.FindProperty("textSeconds"));
        }

        protected virtual void DrawUnitField(string propName, SerializedProperty flagProp, SerializedProperty textProp)
        {
            EditorGUILayout.BeginHorizontal();
            int flag = flagProp.intValue;
            int newFlag = EditorGUILayout.MaskField(propName, flag, m_unitMaskNames);
            if (flag != newFlag)
            {
                flagProp.intValue = newFlag;
            }
            string text = textProp.stringValue;
            string newText = EditorGUILayout.TextField(text);
            if (text != newText)
            {
                textProp.stringValue = newText;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}