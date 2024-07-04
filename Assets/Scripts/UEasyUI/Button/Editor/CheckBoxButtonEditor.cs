using UEasyUI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(CheckBoxButton), true)]
    [CanEditMultipleObjects]
    public class CheckBoxButtonEditor : SelectableEditor
    {
        SerializedProperty IsOnProperty;
        SerializedProperty CheckedNodeProperty;
        SerializedProperty OnClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            IsOnProperty = serializedObject.FindProperty("IsOn");
            CheckedNodeProperty = serializedObject.FindProperty("CheckedNode");
            OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.LabelField("是否默认勾选");
            EditorGUILayout.PropertyField(IsOnProperty);

            EditorGUILayout.LabelField("勾选时显示的节点");
            EditorGUILayout.PropertyField(CheckedNodeProperty);

            EditorGUILayout.PropertyField(OnClickProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
