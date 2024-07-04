using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/********************************************************************************
 * 说    明: 切换按钮,点击后自动切换
 * 版    本: 1.0
 * 创建时间: 2021.06.09
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace UEasyUI
{
    public class ToggleButton : Button
    {
        // 选中时显示的节点
        public GameObject CheckedNode;

        // 不选中时显示的节点
        public GameObject UncheckNode;

        // Toggle按钮组
        public ToggleButtonGroup group;

        // 当前是否选中
        public bool IsOn = false;

        protected override void Awake()
        {
            base.Awake();

            this.Set(IsOn, false);

            if (null != group)
                group.Register(this);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.Checked();

            base.OnPointerClick(eventData);
        }

        /// <summary>
        /// 选中(其它按钮状态会自动刷新)
        /// </summary>
        public void Checked()
        {
            if (this.IsOn)
                return;

            this.Set(true, true);
        }

        /// <summary>
        /// 设置开关
        /// </summary>
        /// <param name="on">true:开 false:关</param>
        /// <param name="isNotify">是否通知刷新组内其它按钮的状态</param>
        public void Set(bool on, bool isNotify = true)
        {
            if (null != CheckedNode)
                CheckedNode.SetActive(on);

            if (null != UncheckNode)
                UncheckNode.SetActive(!on);

            if (null != group && on && isNotify)
                group.NotifyToggleOn(this);
        }
    }
}
