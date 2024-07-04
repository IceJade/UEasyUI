//using Sirenix.OdinInspector;
using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

/************************************************************************************
* @说    明: TextMeshProUGUI扩展,可根据不同语言设置字体属性,也可支持自动设置语言文本
* @作    者: zhoumingfeng
* @版 本 号: V1.00
* @创建时间: 2023.03.31
*************************************************************************************/

namespace UEasyUI
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class TextMeshProUEUI : TextMeshProUGUI
    {
        [Tooltip("全部转大写")]
        public bool wordUpper = false;

        #region 英文

        [Header("英文系字体")]
        public TMP_FontAsset fontEN;

        public float fontSizeEN = 20;

        public FontStyles styleEN;

        public float lineSpaceEN = -25.0f;

        public bool autoFontSizeEN = false;

        public int minSizeEN = 10;

        public int maxSizeEN = 40;

        [Tooltip("是否不换行")]
        public bool isNonBreakingSpaceEN = false;

        #endregion

        #region 中文

        [Header("中文系字体")]
        public TMP_FontAsset fontCN;

        public float fontSizeCN = 18;

        public FontStyles styleCN;

        public float lineSpaceCN = -25.0f;

        public bool autoFontSizeCN = false;

        public int minSizeCN = 10;

        public int maxSizeCN = 40;

        [Tooltip("是否不换行")]
        public bool isNonBreakingSpaceCN = false;

        #endregion 中文

        #region 日语

        [Header("日语字体")]
        public TMP_FontAsset fontJA;

        public float fontSizeJA = 18;

        public FontStyles styleJA;

        public float lineSpaceJA = 0.0f;

        public bool autoFontSizeJA = false;

        public int minSizeJA = 10;

        public int maxSizeJA = 40;

        public bool isNonBreakingSpaceJA = false;

        #endregion 日语

        #region 韩语

        [Header("韩语字体")]
        public TMP_FontAsset fontKO;

        public float fontSizeKO = 16;

        public FontStyles styleKO;

        public float lineSpaceKO = -25.0f;

        public bool autoFontSizeKO = false;

        public int minSizeKO = 10;

        public int maxSizeKO = 40;

        [Tooltip("是否不换行")]
        public bool isNonBreakingSpaceKO = false;

        #endregion 韩语

        #region 泰语

        [Header("泰语字体")]
        public TMP_FontAsset fontTH;

        public float fontSizeTH = 18;

        public FontStyles styleTH;

        public float lineSpaceTH = -25.0f;

        public bool autoFontSizeTH = false;

        public int minSizeTH = 10;

        public int maxSizeTH = 40;

        public bool isNonBreakingSpaceTH = false;

        #endregion 泰语

        #region 阿拉伯语

        [Header("阿拉伯语字体")]
        public TMP_FontAsset fontAR;

        public float fontSizeAR = 20;

        public FontStyles styleAR;

        public float lineSpaceAR = -25.0f;

        public bool autoFontSizeAR = false;

        public int minSizeAR = 10;

        public int maxSizeAR = 40;

        [Tooltip("是否不换行")]
        public bool isNonBreakingSpaceAR = false;

        public bool isAlignmentRight = false;

        #endregion 阿拉伯语

        #region 设置语言ID

        // 语言ID
        public int languageId = 0;

        public bool setPara1 = false;
        public string para1 = "";

        public bool setPara2 = false;
        public string para2 = "";

        public bool setPara3 = false;
        public string para3 = "";

        #endregion 设置语言ID

        //设置描边
        public bool isUseOutline = false;
        public Color outLineColor = Color.black;
        [Range(0, 1)] public float outLineSize = 0.1f;

        //设置阴影
        public bool isUseShadow = false;
        public Color shadowColor = Color.black;
        [Range(-1, 1)] public float offsetX = 0.5f;
        [Range(-1, 1)] public float offsetY = -0.5f;

        private Material _material;
        private TMP_FontAsset _font;

        //private static readonly int s_DiffuseMaskTex_ID = Shader.PropertyToID("_DiffuseMaskTex");
        //private static readonly int s_Softness_ID = Shader.PropertyToID("_OutlineSoftness");
        //private static readonly int s_Dilate_ID = Shader.PropertyToID("_FaceDilate");
        //private static readonly int s_FaceTex_ID = Shader.PropertyToID("_FaceTex");

        private static readonly int s_OutlineColor_ID = Shader.PropertyToID("_OutlineColor");
        private static readonly int s_OutlineThickness_ID = Shader.PropertyToID("_OutlineWidth");

        private static readonly int s_UnderlayColor_ID = Shader.PropertyToID("_UnderlayColor");
        private static readonly int s_UnderlayOffsetX_ID = Shader.PropertyToID("_UnderlayOffsetX");
        private static readonly int s_UnderlayOffsetY_ID = Shader.PropertyToID("_UnderlayOffsetY");
        //private static readonly int s_UnderlayDilate_ID = Shader.PropertyToID("_UnderlayDilate");
        //private static readonly int s_UnderlaySoftness_ID = Shader.PropertyToID("_UnderlaySoftness");

        private string nobr = "<nobr>{0}</nobr>";

        #region 框架接口

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!Application.isPlaying)
            {
                this.CheckFontChange();
            }
            else
            {
                this.SetFont();

                this.CheckFontChange();

                this.AutoFillText();
            }




        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ReleaseMaterial();
        }

        #endregion

        #region 公有接口

        public void SetTextId(int textId)
        {
            this.SetText(GameEntry.Localization.GetString(textId));
        }

        public void SetText(int value)
        {
            this.SetTextEx(value);
        }

        public void SetTextEx(int value)
        {
            this.SetBaseText(value.ToString());
        }

        public void SetTextEx(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.SetBaseText("");
                return;
            }
            
            this.SetBaseText(text);
        }

        public void SetAutoFontSizeRange(float minSize, float maxSize)
        {
            this.fontSizeMin = minSize;
            this.fontSizeMax = maxSize;
        }

        public void SetNonBreakingSpace(bool isNonBreakingSpace)
        {
            this.m_isNonBreakingSpace = isNonBreakingSpace;
        }

        public void Refresh()
        {
            OnDisable();
            OnEnable();
        }

        #endregion

        #region 私有接口

        private void SetBaseText(string content)
        {
            string target = content;

            // 是否换行
            if (!string.IsNullOrEmpty(content) && IsNonBreakingSpace())
                target = string.Format(nobr, content);

            //if (wordUpper)
            //    target = target.ToUpperInvariant();

            base.SetText(target);
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        private void SetFont()
        {
            if (null == GameEntry.Localization)
                return;

            switch (GameEntry.Localization.Language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                case Language.Turkish:
                    {
                        if (null == this.fontCN)
                            return;

                        if (this.font != this.fontCN)
                            this.font = this.fontCN;

                        this.fontStyle = this.styleCN;
                        this.fontSize = this.fontSizeCN;
                        this.lineSpacing = this.lineSpaceCN;

                        if (this.autoFontSizeCN)
                        {
                            this.enableAutoSizing = this.autoFontSizeCN;
                            this.SetAutoFontSizeRange(this.minSizeCN, this.maxSizeCN);
                        }

                        break;
                    }
                case Language.Japanese:
                    {
                        if (null == this.fontJA)
                            return;

                        if (this.font != this.fontJA)
                            this.font = this.fontJA;

                        this.fontStyle = this.styleJA;
                        this.fontSize = this.fontSizeJA;
                        this.lineSpacing = this.lineSpaceJA;

                        if (this.autoFontSizeJA)
                        {
                            this.enableAutoSizing = this.autoFontSizeJA;
                            this.SetAutoFontSizeRange(this.minSizeJA, this.maxSizeJA);
                        }

                        break;
                    }
                case Language.Korean:
                    {
                        if (null == this.fontKO)
                            return;

                        if (this.font != this.fontKO)
                            this.font = this.fontKO;

                        this.fontStyle = this.styleKO;
                        this.fontSize = this.fontSizeKO;
                        this.lineSpacing = this.lineSpaceKO;

                        if (this.autoFontSizeKO)
                        {
                            this.enableAutoSizing = this.autoFontSizeKO;
                            this.SetAutoFontSizeRange(this.minSizeKO, this.maxSizeKO);
                        }

                        break;
                    }
                case Language.Thai:
                    {
                        if (null == this.fontTH)
                            return;

                        if (this.font != this.fontTH)
                            this.font = this.fontTH;

                        this.fontStyle = this.styleTH;
                        this.fontSize = this.fontSizeTH;
                        this.lineSpacing = this.lineSpaceTH;

                        if (this.autoFontSizeTH)
                        {
                            this.enableAutoSizing = this.autoFontSizeTH;
                            this.SetAutoFontSizeRange(this.minSizeTH, this.maxSizeTH);
                        }

                        break;
                    }
                case Language.Arabic:
                    {
                        if (null == this.fontAR)
                            return;

                        if (this.font != this.fontAR)
                            this.font = this.fontAR;

                        if (this.isAlignmentRight)
                            this.SetAlignmentForArabic();

                        this.fontStyle = this.styleAR;
                        this.fontSize = this.fontSizeAR;
                        this.lineSpacing = this.lineSpaceAR;

                        if (this.autoFontSizeAR)
                        {
                            this.enableAutoSizing = this.autoFontSizeAR;
                            this.SetAutoFontSizeRange(this.minSizeAR, this.maxSizeAR);
                        }

                        break;
                    }
                default:
                    {
                        if (null == this.fontEN)
                            return;

                        if (this.font != this.fontEN)
                            this.font = this.fontEN;

                        this.fontStyle = this.styleEN;
                        this.fontSize = this.fontSizeEN;
                        this.lineSpacing = this.lineSpaceEN;

                        if (this.autoFontSizeEN)
                        {
                            this.enableAutoSizing = this.autoFontSizeEN;
                            this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);
                        }

                        break;
                    }
            }

            if (wordUpper)
            {
                if ((this.fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase)
                    this.fontStyle |= FontStyles.UpperCase;
            }
        }

        /// <summary>
        /// 为阿拉伯语设置文字对齐方式
        /// 由于阿拉伯语是从右往左阅读, 左对齐需要改成右对齐
        /// </summary>
        private void SetAlignmentForArabic()
        {
            if (GameEntry.Localization.Language != Language.Arabic)
                return;

            if (this.alignment == TextAlignmentOptions.MidlineLeft)
                this.alignment = TextAlignmentOptions.MidlineRight;
            else if (this.alignment == TextAlignmentOptions.BottomLeft)
                this.alignment = TextAlignmentOptions.BottomRight;
            else if (this.alignment == TextAlignmentOptions.TopLeft)
                this.alignment = TextAlignmentOptions.TopRight;
            else if (this.alignment == TextAlignmentOptions.Left)
                this.alignment = TextAlignmentOptions.Right;
        }

        /// <summary>
        /// 自动填充文本
        /// </summary>
        private void AutoFillText()
        {
            if (this.languageId <= 0)
                return;

            string text = GameEntry.Localization.GetString(languageId);

            this.SetTextEx(text);
        }

        private bool IsNonBreakingSpace()
        {
            bool isNonBreakingSpace = false;

            if (null == GameEntry.Localization)
                return isNonBreakingSpace;

            switch (GameEntry.Localization.Language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                case Language.Turkish:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceCN;

                        break;
                    }
                case Language.Japanese:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceJA;

                        break;
                    }
                case Language.Korean:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceKO;

                        break;
                    }
                case Language.Thai:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceTH;

                        break;
                    }
                case Language.Arabic:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceAR;

                        break;
                    }
                default:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceEN;

                        break;
                    }
            }

            return isNonBreakingSpace;
        }

        private void RefreshText()
        {
            if (!IsNonBreakingSpace())
                return;

            string content = this.text;
            if (content.StartsWith("<nobr>") && content.EndsWith("</nobr>"))
                return;

            this.SetBaseText(content);
        }

        private void CheckFontChange()
        {
            bool changed = false;
            if (_font != font)
            {
                _font = font;
                changed = true;
            }

            if (changed)
                ReleaseMaterial();

            if (_material == null && font.material != null)
            {
                _material = new Material(font.material) { hideFlags = HideFlags.HideAndDontSave };
                fontMaterial = _material;
            }

            RefreshProperty();
        }

        private void RefreshProperty()
        {
            if (_material == null)
                return;


            SetKeyword(_material, "OUTLINE_ON", isUseOutline);
            if (isUseOutline)
            {
                _material.SetColor(s_OutlineColor_ID, outLineColor);
                _material.SetFloat(s_OutlineThickness_ID, outLineSize);
            }
            else
            {
                _material.SetFloat(s_OutlineThickness_ID, 0);
            }

            if (isUseShadow)
            {

                SetKeyword(_material, "UNDERLAY_ON", true);
                SetKeyword(_material, "UNDERLAY_INNER", true);

                _material.SetColor(s_UnderlayColor_ID, shadowColor);
                _material.SetFloat(s_UnderlayOffsetX_ID, offsetX);
                _material.SetFloat(s_UnderlayOffsetY_ID, offsetY);
            }
            else
            {
                SetKeyword(_material, "UNDERLAY_ON", false);
                SetKeyword(_material, "UNDERLAY_INNER", false);
            }

            UpdateMeshPadding();
            fontMaterial = _material;
        }

        private void SetKeyword(Material mat, string key, bool enable)
        {
            if (enable)
                mat.EnableKeyword(key);
            else
                mat.DisableKeyword(key);
        }

        private void ReleaseMaterial()
        {
            if (_material != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    UnityEngine.Object.DestroyImmediate(_material, false);
                else
#endif
                    UnityEngine.Object.Destroy(_material);
                _material = null;
            }
        }

        #endregion

        #region Editor处理逻辑

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            RefreshProperty();
        }

        public void AutoFillFont()
        {
            this.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/ARIAL SDF.asset");
            this.fontEN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/ARIAL SDF.asset");
            this.fontCN = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/SOURCEHANSANSSC-BOLD SDF.asset");
            this.fontKO = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/SourceHanSansKR-Bold SDF.asset");
            this.fontTH = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/tahoma SDF.asset");
            this.fontAR = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/times_new_roman SDF.asset");
            this.fontJA = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Shelter/Font/yzgothic SDF.asset");

            this.fontSize = this.fontSizeEN;
            this.fontStyle = this.styleEN;
            this.lineSpacing = this.lineSpaceEN;
            this.enableAutoSizing = this.autoFontSizeEN;
            if (this.enableAutoSizing)
                this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        public void SetLanguageFont(Language language)
        {
            switch (language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                case Language.Turkish:
                    {
                        if (null == this.fontCN)
                            return;

                        if (this.font != this.fontCN)
                            this.font = this.fontCN;

                        this.fontStyle = this.styleCN;
                        this.fontSize = this.fontSizeCN;
                        this.lineSpacing = this.lineSpaceCN;
                        this.enableAutoSizing = this.autoFontSizeCN;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeCN, this.maxSizeCN);

                        break;
                    }
                case Language.Japanese:
                    {
                        if (null == this.fontJA)
                            return;

                        if (this.font != this.fontJA)
                            this.font = this.fontJA;

                        this.fontStyle = this.styleJA;
                        this.fontSize = this.fontSizeJA;
                        this.lineSpacing = this.lineSpaceJA;
                        this.enableAutoSizing = this.autoFontSizeJA;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeJA, this.maxSizeJA);

                        break;
                    }
                case Language.Korean:
                    {
                        if (null == this.fontKO)
                            return;

                        if (this.font != this.fontKO)
                            this.font = this.fontKO;

                        this.fontStyle = this.styleKO;
                        this.fontSize = this.fontSizeKO;
                        this.lineSpacing = this.lineSpaceKO;
                        this.enableAutoSizing = this.autoFontSizeKO;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeKO, this.maxSizeKO);

                        break;
                    }
                case Language.Thai:
                    {
                        if (null == this.fontTH)
                            return;

                        if (this.font != this.fontTH)
                            this.font = this.fontTH;

                        this.fontStyle = this.styleTH;
                        this.fontSize = this.fontSizeTH;
                        this.lineSpacing = this.lineSpaceTH;
                        this.enableAutoSizing = this.autoFontSizeTH;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeTH, this.maxSizeTH);

                        break;
                    }
                case Language.Arabic:
                    {
                        if (null == this.fontAR)
                            return;

                        if (this.font != this.fontAR)
                            this.font = this.fontAR;

                        if (this.isAlignmentRight)
                            this.SetAlignmentForArabic();

                        this.fontStyle = this.styleAR;
                        this.fontSize = this.fontSizeAR;
                        this.lineSpacing = this.lineSpaceAR;
                        this.enableAutoSizing = this.autoFontSizeAR;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeAR, this.maxSizeAR);

                        break;
                    }
                default:
                    {
                        if (null == this.fontEN)
                            return;

                        if (this.font != this.fontEN)
                            this.font = this.fontEN;

                        this.fontStyle = this.styleEN;
                        this.fontSize = this.fontSizeEN;
                        this.lineSpacing = this.lineSpaceEN;
                        this.enableAutoSizing = this.autoFontSizeEN;

                        if (this.enableAutoSizing)
                            this.SetAutoFontSizeRange(this.minSizeEN, this.maxSizeEN);

                        break;
                    }
            }

            if (wordUpper)
            {
                if ((this.fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase)
                    this.fontStyle |= FontStyles.UpperCase;
            }

            this.RefreshText(language);

#if UNITY_EDITOR
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
#endif

        }

        private void RefreshText(Language language)
        {
            string content = this.text;
            if (string.IsNullOrEmpty(content))
                return;

            string target = content;

            if (!IsNonBreakingSpace(language))
            {
                if (content.StartsWith("<nobr>") || content.EndsWith("</nobr>"))
                {
                    target = target.Replace("<nobr>", "");
                    target = target.Replace("</nobr>", "");
                }
            }
            else
            {
                if (content.StartsWith("<nobr>") && content.EndsWith("</nobr>"))
                    return;

                // 是否换行
                if (!string.IsNullOrEmpty(content))
                    target = string.Format(nobr, content);
            }

            base.SetText(target);
        }

        private bool IsNonBreakingSpace(Language language)
        {
            bool isNonBreakingSpace = false;

            switch (language)
            {
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                case Language.Turkish:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceCN;

                        break;
                    }
                case Language.Japanese:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceJA;

                        break;
                    }
                case Language.Korean:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceKO;

                        break;
                    }
                case Language.Thai:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceTH;

                        break;
                    }
                case Language.Arabic:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceAR;

                        break;
                    }
                default:
                    {
                        isNonBreakingSpace = this.isNonBreakingSpaceEN;

                        break;
                    }
            }

            return isNonBreakingSpace;
        }

#endif

        #endregion
    }
}