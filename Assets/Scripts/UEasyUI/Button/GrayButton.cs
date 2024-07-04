using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//----------------------------------------------------------------------------------
// @说    明: 置灰按钮,可将按钮下的图片、文字、阴影等都置灰
// @作    者: zhoumingfeng
// @版 本 号: V1.00
// @创建时间: 2019.4.12
//----------------------------------------------------------------------------------
namespace UEasyUI
{
    [AddComponentMenu("UI/GrayButton", 34)]
    public class GrayButton : Button
    {
        // 置灰材质;
        public Material GrayMaterial;

        // 可用的延迟时间(秒);
        public float DelayEnabled = 0.0f;

        // 不可用时是否置灰;
        public bool IsShowGray = true;

        // 是否显示倒计时;
        public bool IsShowCD = false;

        // CD结束时触发的事件ID;
        public short CDEventId = 0;

        // 显示文本;
        public Text ButtonText;

        private Shader grayShader = null;
        private Material grayMaterial = null;
        private Color[] originalTextColor = null;

        // 灰色;
        private Color grayTextColor = new Color(76.0f / 255f, 80.0f / 255f, 84.0f / 255f, 1.0f);
        private bool isEnabled = true;

        // 记录倒计时;
        private int countDown = 0;

        // 按钮文本框;
        private string strButtonText;

        protected override void Awake()
        {
            base.Awake();

            this.GetOriginalTextColor();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            this.ButtonText = null;
            this.grayShader = null;
            this.grayMaterial = null;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.IsEnabled = false;
            
            base.OnPointerClick(eventData);
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if(isEnabled != value)
                {
                    if (!value)
                        this.SetGray();
                    else
                        this.ClearGray();

                    isEnabled = value;
                    this.enabled = value;
                }
            }
        }

        private void GetOriginalTextColor()
        {
            if(originalTextColor == null)
            {
                Text[] text = this.GetComponentsInChildren<Text>(true);
                if (null != text)
                {
                    originalTextColor = new Color[text.Length];

                    for (int i = 0; i < text.Length; i++)
                        originalTextColor[i] = text[i].color;
                }
            }
        }

        private void SetGray()
        {
            if (this.IsShowGray)
            {
                //置灰之前获取原始字体颜色;
                this.GetOriginalTextColor();

                // 将图片置灰;
                this.SetImageGray(this.GetGrayMaterial());

                // 将文字置灰;
                this.SetTextGray(false);
            }

            // 使按钮可用;
            if(this.DelayEnabled > 0.0f)
            {
                if(this.IsShowCD && null != this.ButtonText)
                {
                    this.strButtonText = this.ButtonText.text;
                    this.countDown = Mathf.RoundToInt(this.DelayEnabled);
                    
                    InvokeRepeating("RefreshButton", 1.0f, 1.0f);
                }
                else
                {
                    Invoke("ButtonDelayEnabled", this.DelayEnabled);
                }
            }
        }

        /// <summary>
        /// 延迟将按钮置为可用;
        /// </summary>
        private void ButtonDelayEnabled()
        {
            this.IsEnabled = true;

            this.CancelInvoke();
        }

        /// <summary>
        /// 刷新CD;
        /// </summary>
        private void RefreshButton()
        {
            this.countDown--;

            if (countDown <= 0)
            {
                this.ButtonText.text = this.strButtonText;

                this.ButtonDelayEnabled();

                // 倒计时结束;
                if (this.CDEventId > 0)
                    GameEntry.Event.Fire(this.CDEventId, 1);
            }
            else
            {
                string countDownText = string.Format("{0}({1})", this.strButtonText, this.countDown);
                this.ButtonText.text = countDownText;

                // 倒计时开始;
                if (this.countDown == this.DelayEnabled - 1 && this.CDEventId > 0)
                    GameEntry.Event.Fire(this.CDEventId, 0);
            }
        }

        private void ClearGray()
        {
            if (!this.IsShowGray)
                return;

            // 恢复图片;
            this.SetImageGray(null);

            // 恢复文字;
            this.SetTextGray(true);
        }

        private void SetImageGray(Material material)
        {
            Image[] images = this.GetComponentsInChildren<Image>(true);
            if(null != images && images.Length > 0)
            {
                for(int i = 0; i < images.Length; i++)
                    images[i].material = material;
            }
        }

        private void SetTextGray(bool enabled)
        {
            Text[] texts = this.GetComponentsInChildren<Text>(true);
            if (null != texts && texts.Length > 0 && 
                null != originalTextColor && originalTextColor.Length >= texts.Length)
            {
                for (int i = 0; i < texts.Length; i++)
                    texts[i].color = enabled ? originalTextColor[i] : grayTextColor;
            }

            TextOutline[] textOutline = GetComponentsInChildren<TextOutline>(true);
            if(null != textOutline && textOutline.Length > 0)
            {
                for (int i = 0; i < textOutline.Length; i++)
                    textOutline[i].enabled = enabled;
            }

            Shadow[] shadow = GetComponentsInChildren<Shadow>(true);
            if (null != shadow && shadow.Length > 0)
            {
                for (int i = 0; i < shadow.Length; i++)
                    shadow[i].enabled = enabled;
            }
        }

        private Material GetGrayMaterial()
        {
            //if (null == grayMaterial)
            //    grayMaterial = new Material(GetGrayShader());

            //return grayMaterial;
            return this.GrayMaterial;
        }

        private Shader GetGrayShader()
        {
            if (null == grayShader)
                grayShader = Shader.Find("Custom/Gray");

            return grayShader;
        }
    }
}
