using UnityEditor;
using UnityEditor.UI;

namespace UEasyUI
{
    [CustomEditor(typeof(GroupButton), true)]
    [CanEditMultipleObjects]
    public class GroupButtonEditor : SelectableEditor
    {
        SerializedProperty m_GroupIdProperty;
        SerializedProperty m_FrameIntervalProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_GroupIdProperty = serializedObject.FindProperty("GroupId");
            m_FrameIntervalProperty = serializedObject.FindProperty("FrameInterval");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.LabelField("组ID", "此值为界面ID(+数字),防止组ID重复");
            EditorGUILayout.PropertyField(m_GroupIdProperty);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("帧数", "指定帧数以内只有一个按钮点击事件生效");
            EditorGUILayout.PropertyField(m_FrameIntervalProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
