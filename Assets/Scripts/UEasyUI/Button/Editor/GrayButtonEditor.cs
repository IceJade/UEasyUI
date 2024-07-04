using UnityEditor;
using UnityEditor.UI;

namespace UEasyUI
{
    [CustomEditor(typeof(GrayButton), true)]
    [CanEditMultipleObjects]
    public class GrayButtonEditor : SelectableEditor
    {
        SerializedProperty m_GrayMaterialProperty;
        SerializedProperty m_DelayEnabledTimeProperty;
        SerializedProperty m_IsShowGrayProperty;
        SerializedProperty m_IsShowCDProperty;
        SerializedProperty m_CDEventIdProperty;
        SerializedProperty m_ButtonTextProperty;
        SerializedProperty m_OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_GrayMaterialProperty = serializedObject.FindProperty("GrayMaterial");
            m_DelayEnabledTimeProperty = serializedObject.FindProperty("DelayEnabled");
            m_IsShowGrayProperty = serializedObject.FindProperty("IsShowGray");
            m_IsShowCDProperty = serializedObject.FindProperty("IsShowCD");
            m_CDEventIdProperty = serializedObject.FindProperty("CDEventId");
            m_ButtonTextProperty = serializedObject.FindProperty("ButtonText");
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_GrayMaterialProperty);
            EditorGUILayout.PropertyField(m_DelayEnabledTimeProperty);
            EditorGUILayout.PropertyField(m_IsShowGrayProperty);
            EditorGUILayout.PropertyField(m_IsShowCDProperty);
            EditorGUILayout.PropertyField(m_CDEventIdProperty);
            EditorGUILayout.PropertyField(m_ButtonTextProperty);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
