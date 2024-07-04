using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace UEasyUI.Tools
{
    [DisallowMultipleComponent]
    [CustomEditor(typeof(TextMeshProUEUI), true), CanEditMultipleObjects]
    public class TextMeshProUEUIEditor : TMP_EditorPanelUI
    {
        SerializedProperty wordUpperProperty;

        #region 英文

        SerializedProperty fontENProperty;
        SerializedProperty fontSizeENProperty;
        SerializedProperty styleENProperty;
        SerializedProperty lineSpaceENProperty;
        SerializedProperty autoFontSizeENProperty;
        SerializedProperty minSizeENProperty;
        SerializedProperty maxSizeENProperty;
        SerializedProperty isNonBreakingSpaceENProperty;

        #endregion

        #region 中文

        SerializedProperty fontCNProperty;
        SerializedProperty fontSizeCNProperty;
        SerializedProperty styleCNProperty;
        SerializedProperty lineSpaceCNProperty;
        SerializedProperty autoFontSizeCNProperty;
        SerializedProperty minSizeCNProperty;
        SerializedProperty maxSizeCNProperty;
        SerializedProperty isNonBreakingSpaceCNProperty;

        #endregion

        #region 日语

        SerializedProperty fontJAProperty;
        SerializedProperty fontSizeJAProperty;
        SerializedProperty styleJAProperty;
        SerializedProperty lineSpaceJAProperty;
        SerializedProperty autoFontSizeJAProperty;
        SerializedProperty minSizeJAProperty;
        SerializedProperty maxSizeJAProperty;
        SerializedProperty isNonBreakingSpaceJAProperty;

        #endregion

        #region 韩语

        SerializedProperty fontKOProperty;
        SerializedProperty fontSizeKOProperty;
        SerializedProperty styleKOProperty;
        SerializedProperty lineSpaceKOProperty;
        SerializedProperty autoFontSizeKOProperty;
        SerializedProperty minSizeKOProperty;
        SerializedProperty maxSizeKOProperty;
        SerializedProperty isNonBreakingSpaceKOProperty;

        #endregion

        #region 泰语

        SerializedProperty fontTHProperty;
        SerializedProperty fontSizeTHProperty;
        SerializedProperty styleTHProperty;
        SerializedProperty lineSpaceTHProperty;
        SerializedProperty autoFontSizeTHProperty;
        SerializedProperty minSizeTHProperty;
        SerializedProperty maxSizeTHProperty;
        SerializedProperty isNonBreakingSpaceTHProperty;

        #endregion

        #region 阿拉伯语

        SerializedProperty fontARProperty;
        SerializedProperty fontSizeARProperty;
        SerializedProperty styleARProperty;
        SerializedProperty lineSpaceARProperty;
        SerializedProperty autoFontSizeARProperty;
        SerializedProperty minSizeARProperty;
        SerializedProperty maxSizeARProperty;
        SerializedProperty isNonBreakingSpaceARProperty;
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

        SerializedProperty isUseOutlineProperty;
        SerializedProperty outLineColorProperty;
        SerializedProperty outLineSizeProperty;

        //设置阴影
        SerializedProperty isUseShadowProperty;
        SerializedProperty shadowColorProperty;
        SerializedProperty offsetXProperty;
        SerializedProperty offsetYProperty;

        #endregion 设置语言ID

        private static bool _isFontSetting = false;
        private bool _isSetLanguage = false;
        private bool _isSetOutLine = false;
        private bool _isSetShadow = false;

        static readonly GUIContent k_BoldLabel = new GUIContent("B", "Bold");
        static readonly GUIContent k_ItalicLabel = new GUIContent("I", "Italic");
        static readonly GUIContent k_UnderlineLabel = new GUIContent("U", "Underline");
        static readonly GUIContent k_StrikethroughLabel = new GUIContent("S", "Strikethrough");
        static readonly GUIContent k_LowercaseLabel = new GUIContent("ab", "Lowercase");
        static readonly GUIContent k_UppercaseLabel = new GUIContent("AB", "Uppercase");
        static readonly GUIContent k_SmallcapsLabel = new GUIContent("SC", "Smallcaps");

        protected override void OnEnable()
        {
            base.OnEnable();

            wordUpperProperty = serializedObject.FindProperty("wordUpper");

            #region 英文

            fontENProperty = serializedObject.FindProperty("fontEN");
            fontSizeENProperty = serializedObject.FindProperty("fontSizeEN");
            styleENProperty = serializedObject.FindProperty("styleEN");
            lineSpaceENProperty = serializedObject.FindProperty("lineSpaceEN");
            autoFontSizeENProperty = serializedObject.FindProperty("autoFontSizeEN");
            minSizeENProperty = serializedObject.FindProperty("minSizeEN");
            maxSizeENProperty = serializedObject.FindProperty("maxSizeEN");
            isNonBreakingSpaceENProperty = serializedObject.FindProperty("isNonBreakingSpaceEN");

            #endregion

            #region 中文

            fontCNProperty = serializedObject.FindProperty("fontCN");
            fontSizeCNProperty = serializedObject.FindProperty("fontSizeCN");
            styleCNProperty = serializedObject.FindProperty("styleCN");
            lineSpaceCNProperty = serializedObject.FindProperty("lineSpaceCN");
            autoFontSizeCNProperty = serializedObject.FindProperty("autoFontSizeCN");
            minSizeCNProperty = serializedObject.FindProperty("minSizeCN");
            maxSizeCNProperty = serializedObject.FindProperty("maxSizeCN");
            isNonBreakingSpaceCNProperty = serializedObject.FindProperty("isNonBreakingSpaceCN");

            #endregion

            #region 日语

            fontJAProperty = serializedObject.FindProperty("fontJA");
            fontSizeJAProperty = serializedObject.FindProperty("fontSizeJA");
            styleJAProperty = serializedObject.FindProperty("styleJA");
            lineSpaceJAProperty = serializedObject.FindProperty("lineSpaceJA");
            autoFontSizeJAProperty = serializedObject.FindProperty("autoFontSizeJA");
            minSizeJAProperty = serializedObject.FindProperty("minSizeJA");
            maxSizeJAProperty = serializedObject.FindProperty("maxSizeJA");
            isNonBreakingSpaceJAProperty = serializedObject.FindProperty("isNonBreakingSpaceJA");

            #endregion

            #region 韩语

            fontKOProperty = serializedObject.FindProperty("fontKO");
            fontSizeKOProperty = serializedObject.FindProperty("fontSizeKO");
            styleKOProperty = serializedObject.FindProperty("styleKO");
            lineSpaceKOProperty = serializedObject.FindProperty("lineSpaceKO");
            autoFontSizeKOProperty = serializedObject.FindProperty("autoFontSizeKO");
            minSizeKOProperty = serializedObject.FindProperty("minSizeKO");
            maxSizeKOProperty = serializedObject.FindProperty("maxSizeKO");
            isNonBreakingSpaceKOProperty = serializedObject.FindProperty("isNonBreakingSpaceKO");

            #endregion

            #region 泰语

            fontTHProperty = serializedObject.FindProperty("fontTH");
            fontSizeTHProperty = serializedObject.FindProperty("fontSizeTH");
            styleTHProperty = serializedObject.FindProperty("styleTH");
            lineSpaceTHProperty = serializedObject.FindProperty("lineSpaceTH");
            autoFontSizeTHProperty = serializedObject.FindProperty("autoFontSizeTH");
            minSizeTHProperty = serializedObject.FindProperty("minSizeTH");
            maxSizeTHProperty = serializedObject.FindProperty("maxSizeTH");
            isNonBreakingSpaceTHProperty = serializedObject.FindProperty("isNonBreakingSpaceTH");

            #endregion

            #region 阿拉伯语

            fontARProperty = serializedObject.FindProperty("fontAR");
            fontSizeARProperty = serializedObject.FindProperty("fontSizeAR");
            styleARProperty = serializedObject.FindProperty("styleAR");
            lineSpaceARProperty = serializedObject.FindProperty("lineSpaceAR");
            autoFontSizeARProperty = serializedObject.FindProperty("autoFontSizeAR");
            minSizeARProperty = serializedObject.FindProperty("minSizeAR");
            maxSizeARProperty = serializedObject.FindProperty("maxSizeAR");
            isNonBreakingSpaceARProperty = serializedObject.FindProperty("isNonBreakingSpaceAR");
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
                EditorGUILayout.PropertyField(wordUpperProperty);

                #region 英文

                EditorGUILayout.PropertyField(fontENProperty);
                //EditorGUILayout.PropertyField(styleENProperty);
                this.DrawFontStyle(ref rect, ref styleENProperty, "Font Style EN");
                EditorGUILayout.PropertyField(fontSizeENProperty);
                EditorGUILayout.PropertyField(lineSpaceENProperty);
                EditorGUILayout.PropertyField(autoFontSizeENProperty);
                if (autoFontSizeENProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeENProperty);
                    EditorGUILayout.PropertyField(maxSizeENProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceENProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看英文字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.English);
                }

                #endregion 英文

                #region 中文

                EditorGUILayout.PropertyField(fontCNProperty);
                //EditorGUILayout.PropertyField(styleCNProperty);
                this.DrawFontStyle(ref rect, ref styleCNProperty, "Font Style CN");
                EditorGUILayout.PropertyField(fontSizeCNProperty);
                EditorGUILayout.PropertyField(lineSpaceCNProperty);
                EditorGUILayout.PropertyField(autoFontSizeCNProperty);
                if (autoFontSizeCNProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeCNProperty);
                    EditorGUILayout.PropertyField(maxSizeCNProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceCNProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看中文字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.ChineseSimplified);
                }

                #endregion

                #region 日语

                EditorGUILayout.PropertyField(fontJAProperty);
                //EditorGUILayout.PropertyField(styleJAProperty);
                this.DrawFontStyle(ref rect, ref styleJAProperty, "Font Style JA");
                EditorGUILayout.PropertyField(fontSizeJAProperty);
                EditorGUILayout.PropertyField(lineSpaceJAProperty);
                EditorGUILayout.PropertyField(autoFontSizeJAProperty);
                if (autoFontSizeJAProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeJAProperty);
                    EditorGUILayout.PropertyField(maxSizeJAProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceJAProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看日语字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.Japanese);
                }

                #endregion

                #region 韩语

                EditorGUILayout.PropertyField(fontKOProperty);
                //EditorGUILayout.PropertyField(styleKOProperty);
                this.DrawFontStyle(ref rect, ref styleKOProperty, "Font Style KO");
                EditorGUILayout.PropertyField(fontSizeKOProperty);
                EditorGUILayout.PropertyField(lineSpaceKOProperty);
                EditorGUILayout.PropertyField(autoFontSizeKOProperty);
                if (autoFontSizeKOProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeKOProperty);
                    EditorGUILayout.PropertyField(maxSizeKOProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceKOProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看韩语字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.Korean);
                }

                #endregion

                #region 泰语

                EditorGUILayout.PropertyField(fontTHProperty);
                //EditorGUILayout.PropertyField(styleTHProperty);
                this.DrawFontStyle(ref rect, ref styleTHProperty, "Font Style TH");
                EditorGUILayout.PropertyField(fontSizeTHProperty);
                EditorGUILayout.PropertyField(lineSpaceTHProperty);
                EditorGUILayout.PropertyField(autoFontSizeTHProperty);
                if (autoFontSizeTHProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeTHProperty);
                    EditorGUILayout.PropertyField(maxSizeTHProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceTHProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看泰语字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.Thai);
                }

                #endregion

                #region 阿拉伯语

                EditorGUILayout.PropertyField(fontARProperty);
                //EditorGUILayout.PropertyField(styleARProperty);
                this.DrawFontStyle(ref rect, ref styleARProperty, "Font Style AR");
                EditorGUILayout.PropertyField(fontSizeARProperty);
                EditorGUILayout.PropertyField(lineSpaceARProperty);
                EditorGUILayout.PropertyField(autoFontSizeARProperty);
                if (autoFontSizeARProperty.boolValue == true)
                {
                    EditorGUILayout.PropertyField(minSizeARProperty);
                    EditorGUILayout.PropertyField(maxSizeARProperty);
                }
                EditorGUILayout.PropertyField(isNonBreakingSpaceARProperty);
                EditorGUILayout.PropertyField(isAlignmentRightProperty);

                rect = EditorGUILayout.GetControlRect(false, 24);
                if (GUI.Button(rect, "查看阿拉伯语字体效果"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.SetLanguageFont(Language.Arabic);
                }

                #endregion

                rect = EditorGUILayout.GetControlRect(false, 24);

                if (GUI.Button(rect, "自动填充字体"))
                {
                    var tmp = (TextMeshProUEUI)target;
                    if (null != tmp)
                        tmp.AutoFillFont();
                }
            }

            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFontStyle(ref Rect rect, ref SerializedProperty styleProperty, string fontStyleName)
        {
            // FONT STYLE
            EditorGUI.BeginChangeCheck();

            int v1, v2, v3, v4, v5, v6, v7;
            rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 2f);

            GUIContent k_FontStyleLabel = new GUIContent(fontStyleName, "Styles to apply to the text such as Bold or Italic.");
            EditorGUI.BeginProperty(rect, k_FontStyleLabel, styleProperty);

            EditorGUI.PrefixLabel(rect, k_FontStyleLabel);

            int styleValue = styleProperty.intValue;

            rect.x += EditorGUIUtility.labelWidth;
            rect.width -= EditorGUIUtility.labelWidth;

            rect.width = Mathf.Max(25f, rect.width / 7f);

            v1 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 1) == 1, k_BoldLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 1 : 0; // Bold
            rect.x += rect.width;
            v2 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 2) == 2, k_ItalicLabel, TMP_UIStyleManager.alignmentButtonMid) ? 2 : 0; // Italics
            rect.x += rect.width;
            v3 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 4) == 4, k_UnderlineLabel, TMP_UIStyleManager.alignmentButtonMid) ? 4 : 0; // Underline
            rect.x += rect.width;
            v7 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 64) == 64, k_StrikethroughLabel, TMP_UIStyleManager.alignmentButtonRight) ? 64 : 0; // Strikethrough
            rect.x += rect.width;

            int selected = 0;

            EditorGUI.BeginChangeCheck();
            v4 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 8) == 8, k_LowercaseLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 8 : 0; // Lowercase
            if (EditorGUI.EndChangeCheck() && v4 > 0)
            {
                selected = v4;
            }
            rect.x += rect.width;
            EditorGUI.BeginChangeCheck();
            v5 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 16) == 16, k_UppercaseLabel, TMP_UIStyleManager.alignmentButtonMid) ? 16 : 0; // Uppercase
            if (EditorGUI.EndChangeCheck() && v5 > 0)
            {
                selected = v5;
            }
            rect.x += rect.width;
            EditorGUI.BeginChangeCheck();
            v6 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 32) == 32, k_SmallcapsLabel, TMP_UIStyleManager.alignmentButtonRight) ? 32 : 0; // Smallcaps
            if (EditorGUI.EndChangeCheck() && v6 > 0)
            {
                selected = v6;
            }

            if (selected > 0)
            {
                v4 = selected == 8 ? 8 : 0;
                v5 = selected == 16 ? 16 : 0;
                v6 = selected == 32 ? 32 : 0;
            }

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                styleProperty.intValue = v1 + v2 + v3 + v4 + v5 + v6 + v7;
                //m_HavePropertiesChanged = true;
            }
        }
    }
}