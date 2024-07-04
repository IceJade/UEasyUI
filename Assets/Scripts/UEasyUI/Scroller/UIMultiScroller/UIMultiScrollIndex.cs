using UnityEngine;

namespace UEasyUI
{
    public class UIMultiScrollIndex : MonoBehaviour
    {
        [SerializeField]
        public GameObject nodeCheckedFrame;

        private UIMultiScroller _scroller;
        private int _index;

        private void Start()
        {

        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                transform.localPosition = _scroller.GetPosition(_index);
                gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
            }
        }

        public UIMultiScroller Scroller
        {
            set { _scroller = value; }
        }

        /// <summary>
        /// 是否选中Item,选中时通知其他Item刷新状态
        /// </summary>
        /// <param name="check">true:显示选中框 false:隐藏选中框</param>
        /// <param name="notifyRefresh">true:通知其他Item刷新状态 false:不通知</param>
        public void Check(bool check, bool notifyRefresh = true)
        {
            if (check)
            {
                if (notifyRefresh)
                    this.CheckedAndNotify();
                else
                    this.Checked();
            }
            else
            {
                this.Unchecked();
            }
        }

        /// <summary>
        /// 选中(不会通知其他项刷新状态)
        /// </summary>
        public void Checked()
        {
            if (null == this.nodeCheckedFrame)
                return;

            this.nodeCheckedFrame.SetActiveEx(true);
        }

        /// <summary>
        /// 选中并通知列表刷新其他项
        /// </summary>
        public void CheckedAndNotify()
        {
            if (null == this.nodeCheckedFrame)
                return;

            this.nodeCheckedFrame.SetActiveEx(true);

            if (null != this._scroller)
                this._scroller.NotifyRefreshCheckState(this);
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        public void Unchecked()
        {
            if (null == this.nodeCheckedFrame)
                return;

            this.nodeCheckedFrame.SetActiveEx(false);
        }

        /// <summary>
        /// 是否为选中状态
        /// </summary>
        /// <returns></returns>
        public bool IsChecked()
        {
            if (null != this.nodeCheckedFrame)
                return this.nodeCheckedFrame.activeSelf;

            return false;
        }

        /// <summary>
        /// 选中状态是否生效
        /// </summary>
        /// <returns></returns>
        public bool IsCheckEnable()
        {
            return null != this.nodeCheckedFrame;
        }
    }
}