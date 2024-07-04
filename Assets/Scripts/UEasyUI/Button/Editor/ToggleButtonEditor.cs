using UEasyUI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ToggleButton), true)]
    [CanEditMultipleObjects]
    public class ToggleButtonEditor : SelectableEditor
    {
        SerializedProperty IsOnProperty;
        SerializedProperty GroupProperty;
        SerializedProperty CheckedNodeProperty;
        SerializedProperty UncheckedNodeProperty;
        SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            IsOnProperty = serializedObject.FindProperty("IsOn");
            GroupProperty = serializedObject.FindProperty("group");
            CheckedNodeProperty = serializedObject.FindProperty("CheckedNode");
            UncheckedNodeProperty = serializedObject.FindProperty("UncheckNode");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.LabelField("是否默认选中");
            EditorGUILayout.PropertyField(IsOnProperty);

            EditorGUILayout.LabelField("按钮组");
            EditorGUILayout.PropertyField(GroupProperty);

            EditorGUILayout.LabelField("选中状态");
            EditorGUILayout.PropertyField(CheckedNodeProperty);

            EditorGUILayout.LabelField("未选中状态");
            EditorGUILayout.PropertyField(UncheckedNodeProperty);

            EditorGUILayout.PropertyField(OnClickProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
