//----------------------------------------------------------------------------------
// @说    明: CD按钮
// @作    者: zhoumingfeng
// @版 本 号: V1.00
// @创建时间: 2019.6.13
//----------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UEasyUI
{
    [AddComponentMenu("UI/CDButton", 33)]
    public class CDButton : Button
    {
        // 点击CD时间间隔(秒);
        public float ClickCD = 1.0f;

        // 最后一次的点击时间;
        private float LastClickTime = 0.0f;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(ClickCD > 0.0f)
            {
                if(Time.time - LastClickTime > ClickCD)
                {
                    LastClickTime = Time.time;

                    base.OnPointerClick(eventData);
                }
            }
            else
            {
                base.OnPointerClick(eventData);
            }
        }
    }
}
