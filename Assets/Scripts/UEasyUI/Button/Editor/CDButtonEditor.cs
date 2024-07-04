using UnityEditor;
using UnityEditor.UI;

namespace UEasyUI
{
    [CustomEditor(typeof(CDButton))]
    [CanEditMultipleObjects]
    public class CDButtonEditor : SelectableEditor
    {
        SerializedProperty m_ClickCDProperty;
        SerializedProperty m_PopTextKeyProperty;
        SerializedProperty m_OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ClickCDProperty = serializedObject.FindProperty("ClickCD");
            m_PopTextKeyProperty = serializedObject.FindProperty("PopTextKey");
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_ClickCDProperty);
            EditorGUILayout.PropertyField(m_PopTextKeyProperty);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
