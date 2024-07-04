using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace UEasyUI.Tools
{
    [DisallowMultipleComponent]
    [CustomEditor(typeof(TextMeshProUE), true), CanEditMultipleObjects]
    public class TextMeshProUEEditor : TMP_EditorPanel
    {
        SerializedProperty useLocalFontProperty;

        #region 英文

        SerializedProperty fontENProperty;
        SerializedProperty fontSizeENProperty;
        SerializedProperty styleENProperty;
        SerializedProperty lineSpaceENProperty;
        SerializedProperty autoFontSizeENProperty;
        SerializedProperty minSizeENProperty;
        SerializedProperty maxSizeENProperty;

        #endregion

        #region 中文

        SerializedProperty fontCNProperty;
        SerializedProperty fontSizeCNProperty;
        SerializedProperty styleCNProperty;
        SerializedProperty lineSpaceCNProperty;
        SerializedProperty autoFontSizeCNProperty;
        SerializedProperty minSizeCNProperty;
        SerializedProperty maxSizeCNProperty;

        #endregion

        #region 日语

        SerializedProperty fontJAProperty;
        SerializedProperty fontSizeJAProperty;
        SerializedProperty styleJAProperty;
        SerializedProperty lineSpaceJAProperty;
        SerializedProperty autoFontSizeJAProperty;
        SerializedProperty minSizeJAProperty;
        SerializedProperty maxSizeJAProperty;

        #endregion

        #region 韩语

        SerializedProperty fontKOProperty;
        SerializedProperty fontSizeKOProperty;
        SerializedProperty styleKOProperty;
        SerializedProperty lineSpaceKOProperty;
        SerializedProperty autoFontSizeKOProperty;
        SerializedProperty minSizeKOProperty;
        SerializedProperty maxSizeKOProperty;

        #endregion

        #region 泰语

        SerializedProperty fontTHProperty;
        SerializedProperty fontSizeTHProperty;
        SerializedProperty styleTHProperty;
        SerializedProperty lineSpaceTHProperty;
        SerializedProperty autoFontSizeTHProperty;
        SerializedProperty minSizeTHProperty;
        SerializedProperty maxSizeTHProperty;

        #endregion

        #region 阿拉伯语

        SerializedProperty fontARProperty;
        SerializedProperty fontSizeARProperty;
        SerializedProperty styleARProperty;
        SerializedProperty lineSpaceARProperty;
        SerializedProperty autoFontSizeARProperty;
        SerializedProperty minSizeARProperty;
        SerializedProperty maxSizeARProperty;
        SerializedProperty isAlignmentRightProperty;

        #endregion

        #region 设置语言ID

        SerializedProperty languageIdProperty;

        SerializedProperty setPara1Property;
        SerializedProperty para1Property;

        SerializedProperty setPara2Property;
        SerializedProperty para2Property;

        SerializedProperty setPara3Property;
        SerializedProperty para3Property;

        #endregion 设置语言ID

        SerializedProperty isUseOutlineProperty;
        SerializedProperty outLineColorProperty;
        SerializedProperty outLineSizeProperty;

        //设置阴影
        SerializedProperty isUseShadowProperty;
        SerializedProperty shadowColorProperty;
        SerializedProperty offsetXProperty;
        SerializedProperty offsetYProperty;

        private bool _isFontSetting = false;
        private bool _isSetLanguage = false;
        private bool _isSetOutLine = false;
        private bool _isSetShadow = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            useLocalFontProperty = serializedObject.FindProperty("useLocalFont");

            #region 英文

            fontENProperty = serializedObject.FindProperty("fontEN");
            fontSizeENProperty = serializedObject.FindProperty("fontSizeEN");
            styleENProperty = serializedObject.FindProperty("styleEN");
            lineSpaceENProperty = serializedObject.FindProperty("lineSpaceEN");
            autoFontSizeENProperty = serializedObject.FindProperty("autoFontSizeEN");
            minSizeENProperty = serializedObject.FindProperty("minSizeEN");
            maxSizeENProperty = serializedObject.FindProperty("maxSizeEN");

            #endregion

            #region 中文

            fontCNProperty = serializedObject.FindProperty("fontCN");
            fontSizeCNProperty = serializedObject.FindProperty("fontSizeCN");
            styleCNProperty = serializedObject.FindProperty("styleCN");
            lineSpaceCNProperty = serializedObject.FindProperty("lineSpaceCN");
            autoFontSizeCNProperty = serializedObject.FindProperty("autoFontSizeCN");
            minSizeCNProperty = serializedObject.FindProperty("minSizeCN");
            maxSizeCNProperty = serializedObject.FindProperty("maxSizeCN");

            #endregion

            #region 日语

            fontJAProperty = serializedObject.FindProperty("fontJA");
            fontSizeJAProperty = serializedObject.FindProperty("fontSizeJA");
            styleJAProperty = serializedObject.FindProperty("styleJA");
            lineSpaceJAProperty = serializedObject.FindProperty("lineSpaceJA");
            autoFontSizeJAProperty = serializedObject.FindProperty("autoFontSizeJA");
            minSizeJAProperty = serializedObject.FindProperty("minSizeJA");
            maxSizeJAProperty = serializedObject.FindProperty("maxSizeJA");

            #endregion

            #region 韩语

            fontKOProperty = serializedObject.FindProperty("fontKO");
            fontSizeKOProperty = serializedObject.FindProperty("fontSizeKO");
            styleKOProperty = serializedObject.FindProperty("styleKO");
            lineSpaceKOProperty = serializedObject.FindProperty("lineSpaceKO");
            autoFontSizeKOProperty = serializedObject.FindProperty("autoFontSizeKO");
            minSizeKOProperty = serializedObject.FindProperty("minSizeKO");
            maxSizeKOProperty = serializedObject.FindProperty("maxSizeKO");

            #endregion

            #region 泰语

            fontTHProperty = serializedObject.FindProperty("fontTH");
            fontSizeTHProperty = serializedObject.FindProperty("fontSizeTH");
            styleTHProperty = serializedObject.FindProperty("styleTH");
            lineSpaceTHProperty = serializedObject.FindProperty("lineSpaceTH");
            autoFontSizeTHProperty = serializedObject.FindProperty("autoFontSizeTH");
            minSizeTHProperty = serializedObject.FindProperty("minSizeTH");
            maxSizeTHProperty = serializedObject.FindProperty("maxSizeTH");

            #endregion

            #region 阿拉伯语

            fontARProperty = serializedObject.FindProperty("fontAR");
            fontSizeARProperty = serializedObject.FindProperty("fontSizeAR");
            styleARProperty = serializedObject.FindProperty("styleAR");
            lineSpaceARProperty = serializedObject.FindProperty("lineSpaceAR");
            autoFontSizeARProperty = serializedObject.FindProperty("autoFontSizeAR");
            minSizeARProperty = serializedObject.FindProperty("minSizeAR");
            maxSizeARProperty = serializedObject.FindProperty("maxSizeAR");
            isAlignmentRightProperty = serializedObject.FindProperty("isAlignmentRight");

            #endregion

            #region 设置语言ID

            languageIdProperty = serializedObject.FindProperty("languageId");

            setPara1Property = serializedObject.FindProperty("setPara1");
            para1Property = serializedObject.FindProperty("para1");

            setPara2Property = serializedObject.FindProperty("setPara2");
            para2Property = serializedObject.FindProperty("para2");

            setPara3Property = serializedObject.FindProperty("setPara3");
            para3Property = serializedObject.FindProperty("para3");

            #endregion 设置语言ID

            isUseOutlineProperty = serializedObject.FindProperty("isUseOutline");
            outLineColorProperty = serializedObject.FindProperty("outLineColor");
            outLineSizeProperty = serializedObject.FindProperty("outLineSize");

            isUseShadowProperty = serializedObject.FindProperty("isUseShadow");
            shadowColorProperty = serializedObject.FindProperty("shadowColor");
            offsetXProperty = serializedObject.FindProperty("offsetX");
            offsetYProperty = serializedObject.FindProperty("offsetY");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            #region 设置描边

            var rect = EditorGUILayout.GetControlRect(false, 24);

            if (GUI.Button(rect, new GUIContent("设置描边"), TMP_UIStyleManager.sectionHeader))
                _isSetOutLine = !_isSetOutLine;

            GUI.Label(rect, (_isSetOutLine ? "点击收缩" : "点击展开"), TMP_UIStyleManager.rightLabel);

            if (_isSetOutLine)
            {
                EditorGUILayout.LabelField("描边");

                EditorGUILayout.PropertyField(isUseOutlineProperty);
                EditorGUILayout.PropertyField(outLineColorProperty);

                EditorGUILayout.PropertyField(outLineSizeProperty);
            }

            #endregion

            #region 设置阴影

            rect = EditorGUILayout.GetControlRect(false, 24);

            if (GUI.Button(rect, new GUIContent("设置阴影"), TMP_UIStyleManager.sectionHeader))
                _isSetShadow = !_isSetShadow;

            GUI.Label(rect, (_isSetShadow ? "点击收缩" : "点击展开"), TMP_UIStyleManager.rightLabel);

            if (_isSetShadow)
            {

                EditorGUILayout.PropertyField(isUseShadowProperty);

                EditorGUILayout.PropertyField(shadowColorProperty);

                EditorGUILayout.PropertyField(offsetXProperty);
                EditorGUILayout.PropertyField(offsetYProperty);

            }

            #endregion

            #region 设置语言

            rect = EditorGUILayout.GetControlRect(false, 24);

            if (GUI.Button(rect, new GUIContent("设置语言"), TMP_UIStyleManager.sectionHeader))
                _isSetLanguage = !_isSetLanguage;

            GUI.Label(rect, (_isSetLanguage ? "点击收缩" : "点击展开"), TMP_UIStyleManager.rightLabel);

            if (_isSetLanguage)
            {
                EditorGUILayout.LabelField("语言ID");
                EditorGUILayout.PropertyField(languageIdProperty);

                EditorGUILayout.PropertyField(setPara1Property);
                if (setPara1Property.boolValue)
                    EditorGUILayout.PropertyField(para1Property);

                EditorGUILayout.PropertyField(setPara2Property);
                if (setPara2Property.boolValue)
                    EditorGUILayout.PropertyField(para2Property);

                EditorGUILayout.PropertyField(setPara3Property);
                if (setPara3Property.boolValue)
                    EditorGUILayout.PropertyField(para3Property);
            }

            #endregion

            #region 设置字体

            rect = EditorGUILayout.GetControlRect(false, 24);

            if (GUI.Button(rect, new GUIContent("设置字体"), TMP_UIStyleManager.sectionHeader))
                _isFontSetting = !_isFontSetting;

            GUI.Label(rect, (_isFontSetting ? "点击收缩" : "点击展开"), TMP_UIStyleManager.rightLabel);

            if (_isFontSetting)
            {
                EditorGUILayout.PropertyField(useLocalFontProperty);
                if (useLocalFontProperty.boolValue == true)
                {
                    #region 英文

                    EditorGUILayout.PropertyField(fontENProperty);
                    EditorGUILayout.PropertyField(styleENProperty);
                    EditorGUILayout.PropertyField(fontSizeENProperty);
                    EditorGUILayout.PropertyField(lineSpaceENProperty);
                    EditorGUILayout.PropertyField(autoFontSizeENProperty);
                    if (autoFontSizeENProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeENProperty);
                        EditorGUILayout.PropertyField(maxSizeENProperty);
                    }

                    #endregion 英文

                    #region 中文

                    EditorGUILayout.PropertyField(fontCNProperty);
                    EditorGUILayout.PropertyField(styleCNProperty);
                    EditorGUILayout.PropertyField(fontSizeCNProperty);
                    EditorGUILayout.PropertyField(lineSpaceCNProperty);
                    EditorGUILayout.PropertyField(autoFontSizeCNProperty);
                    if (autoFontSizeCNProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeCNProperty);
                        EditorGUILayout.PropertyField(maxSizeCNProperty);
                    }

                    #endregion

                    #region 日语

                    EditorGUILayout.PropertyField(fontJAProperty);
                    EditorGUILayout.PropertyField(styleJAProperty);
                    EditorGUILayout.PropertyField(fontSizeJAProperty);
                    EditorGUILayout.PropertyField(lineSpaceJAProperty);
                    EditorGUILayout.PropertyField(autoFontSizeJAProperty);
                    if (autoFontSizeJAProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeJAProperty);
                        EditorGUILayout.PropertyField(maxSizeJAProperty);
                    }

                    #endregion

                    #region 韩语

                    EditorGUILayout.PropertyField(fontKOProperty);
                    EditorGUILayout.PropertyField(styleKOProperty);
                    EditorGUILayout.PropertyField(fontSizeKOProperty);
                    EditorGUILayout.PropertyField(lineSpaceKOProperty);
                    EditorGUILayout.PropertyField(autoFontSizeKOProperty);
                    if (autoFontSizeKOProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeKOProperty);
                        EditorGUILayout.PropertyField(maxSizeKOProperty);
                    }

                    #endregion

                    #region 泰语

                    EditorGUILayout.PropertyField(fontTHProperty);
                    EditorGUILayout.PropertyField(styleTHProperty);
                    EditorGUILayout.PropertyField(fontSizeTHProperty);
                    EditorGUILayout.PropertyField(lineSpaceTHProperty);
                    EditorGUILayout.PropertyField(autoFontSizeTHProperty);
                    if (autoFontSizeTHProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeTHProperty);
                        EditorGUILayout.PropertyField(maxSizeTHProperty);
                    }

                    #endregion

                    #region 阿拉伯语

                    EditorGUILayout.PropertyField(fontARProperty);
                    EditorGUILayout.PropertyField(styleARProperty);
                    EditorGUILayout.PropertyField(fontSizeARProperty);
                    EditorGUILayout.PropertyField(lineSpaceARProperty);
                    EditorGUILayout.PropertyField(autoFontSizeARProperty);
                    if (autoFontSizeARProperty.boolValue == true)
                    {
                        EditorGUILayout.PropertyField(minSizeARProperty);
                        EditorGUILayout.PropertyField(maxSizeARProperty);
                    }
                    EditorGUILayout.PropertyField(isAlignmentRightProperty);

                    #endregion

                    rect = EditorGUILayout.GetControlRect(false, 24);

                    if (GUI.Button(rect, "自动填充字体"))
                    {
                        var tmp = (TextMeshProUE)target;
                        if (null != tmp)
                            tmp.AutoFillFont();
                    }
                }
            }

            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }
}