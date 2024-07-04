//----------------------------------------------------------------------------------
// @说    明: 组按钮
// @作    者: zhoumingfeng
// @版 本 号: V1.00
// @创建时间: 2020.9.5
// @使用说明: 相同组ID的按钮在规定帧数以内只响应最先点击的按钮点击事件
//----------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UEasyUI
{
    [AddComponentMenu("UI/PointerButton", 32)]
    public class GroupButton : Button
    {
        // 按钮组ID(注意要使用界面ID+数字,防止组ID重复);
        public int GroupId;

        // 间隔的帧数;
        public int FrameInterval = 10;

        public static Dictionary<int, int> GroupFrameDic = null;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(GroupId > 0 && FrameInterval > 0)
            {
                if(IsCanClick(this.GroupId, this.FrameInterval))
                    base.OnPointerClick(eventData);
            }
            else
            {
                base.OnPointerClick(eventData);
            }
        }

        /// <summary>
        /// 按钮添加监听事件
        /// </summary>
        /// <param name="callback">点击回调事件</param>
        /// <param name="groupId">组ID(建议使用界面ID或者界面ID+数字,防止组ID重复)</param>
        /// <param name="frameInterval">帧数</param>
        /// <returns>void</returns>
        public void AddListenerWithCD(UnityAction callback, int groupId, int frameInterval = 10)
        {
            this.GroupId = groupId;
            this.FrameInterval = frameInterval;

            this.onClick.RemoveAllListeners();
            this.onClick.AddListener(callback);
        }

        public static bool IsCanClick(int groupId, int frameCount)
        {
            bool isCanClick = true;

            if (null == GroupFrameDic)
                GroupFrameDic = new Dictionary<int, int>();

            if (GroupFrameDic.ContainsKey(groupId))
            {
                isCanClick = (Time.frameCount - GroupFrameDic[groupId] > frameCount);

                if (isCanClick)
                    GroupFrameDic[groupId] = Time.frameCount;
            }
            else
            {
                GroupFrameDic.Add(groupId, Time.frameCount);
            }

            return true;
        }
    }
}
