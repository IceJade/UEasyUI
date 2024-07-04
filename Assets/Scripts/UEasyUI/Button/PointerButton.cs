//----------------------------------------------------------------------------------
// @说    明: 支持点击、长按、按下、抬起、离开焦点的按钮
// @作    者: zhoumingfeng
// @版 本 号: V1.00
// @创建时间: 2019.6.5
//----------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UEasyUI
{
    [AddComponentMenu("UI/PointerButton", 31)]
    public class PointerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        // 长按时长(多久算长按);
        public float LongPressInterval = 0.1f;

        // 滑动距离(多大距离范围内不算移动,单位为像素);
        public float SlidingDistance = 10.0f;

        public delegate void OnLongPressHandler(GameObject obj, int param);

        private OnLongPressHandler m_OnLongPress = null;

        // 长按委托;
        public OnLongPressHandler OnLongPress
        {
            get { return m_OnLongPress; }
            set { m_OnLongPress = value; }
        }

        public delegate void OnLongPressExHandler(GameObject obj, object param);
        private OnLongPressExHandler m_OnLongPressEx = null;
        // 长按委托;
        public OnLongPressExHandler OnLongPressEx
        {
            get { return m_OnLongPressEx; }
            set { m_OnLongPressEx = value; }
        }

        // 是否按下;
        private bool IsPressDown = false;

        // 是否抬起;
        public bool IsPressUp { get; private set; }

        // 是否长按;
        private bool IsLongPressDown = false;

        // 按下开始时间;
        private float PressDownStartTime = -1.0f;

        // 参数;
        private int Param = -1;

        // 参数2
        private object _userData = null;

        // 按下时的坐标;
        private Vector2 PressPosition = default(Vector2);

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onLongPressEnd")]
        [SerializeField]
        private ButtonClickedEvent m_OnLongPressEnd = new ButtonClickedEvent();

        public ButtonClickedEvent onLongPressEnd
        {
            get { return m_OnLongPressEnd; }
            set { m_OnLongPressEnd = value; }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.IsPressDown && !this.IsLongPressDown 
                && Time.time - PressDownStartTime > LongPressInterval)
            {
                // 长按标记;
                IsLongPressDown = true;

                if (null != OnLongPress)
                {
                    OnLongPress.Invoke(this.gameObject, Param);
                }
                if (null != OnLongPressEx)
                {
                    OnLongPressEx.Invoke(this.gameObject, _userData);
                }

                //Debug.LogWarning("长按");
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressDown = true;
            IsPressUp = false;
            IsLongPressDown = false;

            PressPosition = eventData.position;
            PressDownStartTime = Time.time;

            //Debug.LogWarning("鼠标按下");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressDown = false;
            IsPressUp = true;
            IsLongPressDown = false;

            if (null != onLongPressEnd)
                onLongPressEnd.Invoke();

            //Debug.LogWarning("鼠标抬起");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            /*
            Debug.LogWarningFormat("OnPointerExit position=({0},{1}), pressPosition=({2},{3}, delta=({4},{5})", 
                eventData.position.x, eventData.position.y, 
                eventData.pressPosition.x, eventData.pressPosition.y,
                eventData.delta.x, eventData.delta.y);

            if (IsPressDown && !IsPressUp 
                && eventData.delta.x > -SlidingDistance && eventData.delta.x < SlidingDistance
                && eventData.delta.y > -SlidingDistance && eventData.delta.y < SlidingDistance)
                return;
                */

            IsPressDown = false;
            IsLongPressDown = false;

            if (null != onLongPressEnd)
                onLongPressEnd.Invoke();

            //Debug.LogWarning("鼠标退出");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(null != onClick 
                && Time.time - PressDownStartTime <= LongPressInterval)
                onClick.Invoke();

            //Debug.LogWarning("点击事件");
        }

        public void SetParam(int param)
        {
            this.Param = param;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="userData"></param>
        public void SetUserData(object userData)
        {
            _userData = userData;
        }
    }
}
