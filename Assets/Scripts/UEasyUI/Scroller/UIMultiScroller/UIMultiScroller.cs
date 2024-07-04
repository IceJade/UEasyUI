// -------------------------------------------------------------------------------------------
// @说       明: ScrollerView组件,内部使用缓存池,简单易用,性能比原生的好很多;
// @作       者: zhoumingfeng
// @版  本  号: V1.00
// @创建时间: 2020.11.18
// @修改记录:
//  2022.02.18 增加功能
//    1.自动计算一行或者一列显示的数量,可解决屏幕不同分辨率的适配问题;
//    2.自动计算可创建的行数或者列数;
//    3.变量添加注释说明;
//  2022.11.17 增加功能
//    1.内部自动绑定onValueChanged事件,防止有人忘了绑定onValueChanged事件;
//    2.增加自动跳转功能(支持跳转到某项、某行或者某列);
//    3.增加选中功能;
//    4.支持跳转并选中;
//  2022.12.30
//    1.支持最外框根据Content内容自适应高度或者宽度;
//    2.左侧和上方增加边距的设置;
// @使用方法: 
//    1.定义变量
//       public UIMultiScroller scroller;
//    2.赋值
//       scroller.DataCount = 10;                   // 创建Item的数量
//       scroller.OnItemCreate = OnItemCreate;      // 创建每个Item项处理回调
//       scroller.ResetScroller();                  // 创建ScrollerView
//    3.实现创建Item的回调
//       private void OnItemCreate(int index, GameObject obj)
//       {
//       }
//      回调函数参数说明:  
//       index: 创建的第几项(第一项为0)
//       obj:  item对象, 可通过其获取到每一个item对象上挂的脚本
// --------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UEasyUI
{
    public class UIMultiScroller : MonoBehaviour
    {
        #region Inspector

        public enum Arrangement { Horizontal, Vertical, }
        public Arrangement _movement = Arrangement.Horizontal;

        /// <summary>
        /// 单行或单列的Item数量
        /// </summary>
        [Range(0, 20)]
        [Header("单行或单列显示的Item数量,值为0时自动计算数量")]
        public int maxPerLine = 5;

        [Range(0, 100)]
        [Header("X坐标间距")]
        public int spacingX = 5;

        [Range(0, 100)]
        [Header("Y坐标间距")]
        public int spacingY = 5;

        //[Tooltip("初始化间隔(以帧计算)")]
        [Header("创建Item的间隔帧数")]
        public int intervalFrame = 0;
        private int tempIntervalFrame = 0;
        private List<int> m_CreateList = new List<int>();

        //[XLua.CSharpCallLua]
        public delegate void OnFuncCreateOverHandler();

        // 分帧创建Item完毕时的回调函数
        public OnFuncCreateOverHandler OnFuncCreateOver;

        [Header("Item的宽度")]
        public int cellWidth = 100;
        [Header("Item的高度")]
        public int cellHeight = 100;

        [Range(0, 200)]
        [Header("默认创建的行或列数,一般比可见的数量大2~3个,为0时自动计算")]
        public int viewCount = 6;

        [SerializeField]
        [Header("是否自适应高度或宽度")]
        private bool IsAdaptiveSize = false;

        [Header("Item模板")]
        public GameObject itemPrefab;

        public RectTransform _content;

        [SerializeField]
        private ScrollRect _ScrollRect;

        [SerializeField]
        private RectOffset _Padding;

        #endregion

        #region 公有属性

        /// <summary>
        /// _itemList数量
        /// </summary>
        public int Count
        {
            get
            {
                if (_itemList == null) return 0;
                else return _itemList.Count;
            }
        }

        /// <summary>
        /// Item创建回调
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="obj">创建的物体</param>
        //[XLua.CSharpCallLua]
        public delegate void OnItemCreateHandler(int index, GameObject obj);

        public OnItemCreateHandler OnItemCreate = null;

        /// <summary>
        /// 总数量
        /// </summary>
        public int DataCount
        {
            get { return _dataCount; }
            set
            {
                _dataCount = value;

                if (this.maxPerLine <= 0)
                    GetBestPerLine();

                if (this.viewCount <= 0)
                    GetBestViewCount();

                UpdateTotalSize();
            }
        }

        #endregion

        #region 私有变量

        private int _index = -1;
        private int _dataCount;
        private int _checked_index = -1;
        private float _default_size = 0;
        private RectTransform _rectTransform;

        private List<UIMultiScrollIndex> _itemList = new List<UIMultiScrollIndex>();

        //将未显示出来的Item存入未使用队列里面，等待需要使用的时候直接取出
        private Queue<UIMultiScrollIndex> _unUsedQueue = new Queue<UIMultiScrollIndex>();

        private Dictionary<int, Queue<GameObject>> m_Pool = new Dictionary<int, Queue<GameObject>>();
        private Dictionary<GameObject, int> m_GoTag = new Dictionary<GameObject, int>();

        #endregion

        #region 框架接口

        void Start()
        {
            tempIntervalFrame = intervalFrame;

            if (this.maxPerLine <= 0)
                GetBestPerLine();

            if (this.viewCount <= 0)
                GetBestViewCount();

            RegisterEvent();

            OnValueChange(Vector2.zero);
        }

        void LateUpdate()
        {
            if (m_CreateList.Count == 0) return;

            tempIntervalFrame--;

            if (tempIntervalFrame > 0) return;

            for (int i = 0; i < m_CreateList.Count; i++)
            {
                CreateItem(m_CreateList[i]);
                m_CreateList.RemoveAt(i);
                tempIntervalFrame = intervalFrame;

                break;
            }

            if (m_CreateList.Count == 0)
            {
                if (OnFuncCreateOver != null)
                    OnFuncCreateOver.Invoke();
            }
        }

        private void OnDestroy()
        {
            itemPrefab = null;
            _content = null;
            _itemList = null;
            _unUsedQueue = null;
            OnItemCreate = null;
            m_CreateList = null;
            DestoryAll();
            m_Pool = null;
            m_GoTag = null;
        }

        #endregion

        #region 公共接口

        /// <summary>
        /// 重置列表
        /// </summary>
        /// <param name="moveToIndex">跳转到第N项(小于0时表示以当前状态刷新, 否则为跳转到指定项, 0表示第一项, 依次类推)</param>
        /// <param name="check">跳转后是否选中(true:选中 false:不选中)</param>
        public void ResetScroller(int moveToIndex = 0, bool check = false)
        {
            this.ResetChilds();

            _index = -1;
            _checked_index = -1;

            UIMultiScrollIndex[] arr = _content.GetComponentsInChildren<UIMultiScrollIndex>();
            for (int i = 0; i < arr.Length; i++)
                DestroyItem(arr[i].gameObject);

            arr = null;

            _itemList?.Clear();
            _unUsedQueue?.Clear();

            if (moveToIndex < 0)
                this.OnValueChange(Vector2.zero);
            else
                this.MoveToIndex(moveToIndex, check);

            this.CheckAdaptiveSize();
        }

        /// <summary>
        /// 移动到指定项
        /// </summary>
        /// <param name="index">第index项,从0开始</param>
        /// <param name="check">移动到指定项后是否选中(true:选中 false:不选中)</param>
        /// <param name="smooth">true:平滑移动 false:瞬间移动</param>
        public void MoveToIndex(int index, bool check = false, bool smooth = false)
        {
            this.MoveToLine(index / this.maxPerLine, smooth);

            if (check)
                this.CheckedItem(index);
        }

        /// <summary>
        /// 移动到指定行
        /// </summary>
        /// <param name="lineIndex">第lineIndex行,从0开始</param>
        /// <param name="smooth">true:平滑移动 false:瞬间移动</param>
        public void MoveToLine(int lineIndex, bool smooth = false)
        {
            if (lineIndex <= 0)
            {
                this.MoveToTop();
            }
            else if (lineIndex > 0 && lineIndex >= this._dataCount - 1 / this.maxPerLine)
            {
                this.MoveToBottom();
            }
            else
            {
                if (null == _ScrollRect.viewport)
                {
                    Log.Error("列表的ScrollRect的viewport没有挂载节点...");
                    return;
                }

                // 列表显示区域大小;
                var rect = _ScrollRect.viewport.rect;

                switch (_movement)
                {
                    case Arrangement.Horizontal:
                        {
                            if (_content.sizeDelta.x < rect.width)
                                return;

                            float targetX = this._Padding.left + (this.cellWidth + this.spacingX) * lineIndex;
                            if (targetX > _content.sizeDelta.x - rect.width)
                                targetX = _content.sizeDelta.x - rect.width;

                            //if (smooth)
                            //    _content.DOAnchorPosX(-targetX, 0.2f);
                            //else
                                _content.anchoredPosition = new Vector2(-targetX, _content.anchoredPosition.y);

                            break;
                        }
                    case Arrangement.Vertical:
                        {
                            if (_content.sizeDelta.y < rect.height)
                                return;

                            float targetY = this._Padding.top + (this.cellHeight + this.spacingY) * lineIndex;
                            if (targetY > _content.sizeDelta.y - rect.height)
                                targetY = _content.sizeDelta.y - rect.height;

                            //if (smooth)
                            //    _content.DOAnchorPosY(targetY, 0.2f);
                            //else
                                _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, targetY);

                            break;
                        }
                    default:
                        break;
                }

                ForceUpdateItems();
            }
        }

        /// <summary>
        /// 移动到顶部
        /// </summary>
        public void MoveToTop()
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        _content.anchoredPosition = new Vector2(0, _content.anchoredPosition.y);

                        break;
                    }
                case Arrangement.Vertical:
                    {
                        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, 0);

                        break;
                    }
                default:
                    break;
            }

            ForceUpdateItems();
        }

        /// <summary>
        /// 移动到底部
        /// </summary>
        public void MoveToBottom()
        {
            if (null == _ScrollRect.viewport)
            {
                Log.Error("列表的ScrollRect的viewport没有挂载节点...");
                return;
            }

            // 列表显示区域大小;
            var rect = _ScrollRect.viewport.rect;

            switch (_movement)
            {
                case Arrangement.Horizontal:
                    if (_content.sizeDelta.x > rect.width)
                        _content.anchoredPosition = new Vector2(rect.width - _content.sizeDelta.x, _content.anchoredPosition.y);
                    break;
                case Arrangement.Vertical:
                    if (_content.sizeDelta.y > rect.height)
                        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, _content.sizeDelta.y - rect.height);
                    break;
            }

            ForceUpdateItems();
        }

        /// <summary>
        /// 强制刷新
        /// </summary>
        public void ForceUpdateItems()
        {
            int index = GetPosIndex();

            if (_index == index && index > -1)
            {
                for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
                {
                    if (i < 0)
                        continue;
                    if (i > _dataCount - 1)
                        continue;
                    bool isOk = false;
                    foreach (UIMultiScrollIndex item in _itemList)
                    {
                        if (item.Index == i)
                        {
                            item.Check(item.Index == this._checked_index, false);

                            OnItemCreate?.Invoke(item.Index, item.gameObject);

                            isOk = true;
                        }
                    }
                    if (isOk)
                        continue;
                    if (intervalFrame > 0 && _itemList.Count <= 0)
                        m_CreateList.Add(i);
                    else
                        CreateItem(i);
                }
            }
            else if (_index != index && index > -1)
            {
                _index = index;
                for (int i = _itemList.Count; i > 0; i--)
                {
                    UIMultiScrollIndex item = _itemList[i - 1];
                    if (item.Index < index * maxPerLine || (item.Index >= (index + viewCount) * maxPerLine))
                    {
                        _itemList.Remove(item);
                        _unUsedQueue.Enqueue(item);
                    }
                }
                for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
                {
                    if (i < 0) continue;
                    if (i > _dataCount - 1) continue;
                    bool isOk = false;
                    foreach (UIMultiScrollIndex item in _itemList)
                    {
                        if (item.Index == i)
                        {
                            isOk = true;
                        }
                    }
                    if (isOk) continue;
                    if (intervalFrame > 0 && _itemList.Count <= 0)
                    {
                        m_CreateList.Add(i);
                    }
                    else
                        CreateItem(i);
                }
            }
        }

        /// <summary>
        /// 更新指定项
        /// </summary>
        /// <param name="index"></param>
        public void UpdateItem(int index)
        {
            if (_itemList == null) return;

            for (int i = 0; i < _itemList.Count; i++)
            {
                if (_itemList[i].Index == index)
                {
                    _itemList[i].Check(index == this._checked_index, false);

                    OnItemCreate?.Invoke(index, _itemList[i].gameObject);
                }
            }
        }

        /// <summary>
        /// 更新当前的所有项
        /// </summary>
        public void UpdateAllItem()
        {
            if (_itemList == null) return;

            for (int i = 0; i < _itemList.Count; i++)
            {
                _itemList[i].Check(_itemList[i].Index == this._checked_index, false);

                OnItemCreate?.Invoke(_itemList[i].Index, _itemList[i].gameObject);
            }
        }

        /// <summary>
        /// 获得当前显示的所有Item
        /// </summary>
        /// <returns></returns>
        public List<UIMultiScrollIndex> GetItemList()
        {
            return this._itemList;
        }

        /// <summary>
        /// 根据索引号 获取当前item的位置
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vector3 GetPosition(int i)
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        return new Vector3(this._Padding.left + (cellWidth + spacingX) * (i / maxPerLine), 0 - this._Padding.top - (cellHeight + spacingY) * (i % maxPerLine), 0f);
                    }
                case Arrangement.Vertical:
                    {
                        return new Vector3(this._Padding.left + (cellWidth + spacingX) * (i % maxPerLine), 0 - this._Padding.top - (cellHeight + spacingY) * (i / maxPerLine), 0f);
                    }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 获得指定项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GameObject GetItemByIndex(int index)
        {
            if (null == _itemList || _itemList.Count <= 0)
                return null;

            foreach (UIMultiScrollIndex item in _itemList)
            {
                if (item.Index == index)
                    return item.gameObject;
            }

            return null;
        }

        /// <summary>
        /// 提供给外部的方法，添加指定位置的Item
        /// </summary>
        public void AddItem(int index)
        {
            DataCount += 1;
            AddItemIntoPanel(index);
        }

        /// <summary>
        /// 选中Item
        /// </summary>
        /// <param name="index">Item的索引,从0开始</param>
        public void CheckedItem(int index)
        {
            this._checked_index = index;

            if (null == _itemList || _itemList.Count <= 0)
                return;

            foreach (UIMultiScrollIndex item in _itemList)
                item.Check(item.Index == index, false);
        }

        /// <summary>
        /// 通知刷新选中的状态
        /// </summary>
        /// <param name="item">item对象</param>
        public void NotifyRefreshCheckState(UIMultiScrollIndex item)
        {
            this._checked_index = item.Index;

            if (null == _itemList || _itemList.Count <= 0)
                return;

            foreach (UIMultiScrollIndex per in _itemList)
            {
                if (per.Index != this._checked_index)
                    per.Unchecked();
            }
        }

        public void OnValueChange(Vector2 pos)
        {
            if (_itemList == null) return;

            if (pos.y < 0) return;
            int index = GetPosIndex();

            if (_index != index && index > -1)
            {
                _index = index;
                for (int i = _itemList.Count; i > 0; i--)
                {
                    UIMultiScrollIndex item = _itemList[i - 1];
                    if (item.Index < index * maxPerLine || (item.Index >= (index + viewCount) * maxPerLine))
                    {
                        _itemList.Remove(item);
                        _unUsedQueue.Enqueue(item);
                    }
                }
                for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
                {
                    if (i < 0) continue;
                    if (i > _dataCount - 1) continue;
                    bool isOk = false;
                    foreach (UIMultiScrollIndex item in _itemList)
                    {
                        if (item.Index == i)
                        {
                            isOk = true;
                        }
                    }
                    if (isOk) continue;
                    if (intervalFrame > 0 && _itemList.Count <= 0)
                    {
                        m_CreateList.Add(i);
                    }
                    else
                        CreateItem(i);
                }
            }
        }

        #endregion

        #region 私有接口

        /// <summary>
        /// 重置子列表,处理列表套列表的情况
        /// </summary>
        private void ResetChilds()
        {
            UIMultiScrollIndex[] arr = _content.GetComponentsInChildren<UIMultiScrollIndex>();
            if (null == arr || arr.Length <= 0)
                return;

            for (int i = 0; i < arr.Length; i++)
            {
                UIMultiScroller scroller = arr[i].GetComponentInChildren<UIMultiScroller>();
                if (null != scroller)
                {
                    scroller._index = -1;
                    UIMultiScrollIndex[] childArr = scroller._content.GetComponentsInChildren<UIMultiScrollIndex>();
                    for (int j = 0; j < childArr.Length; j++)
                    {
                        scroller.DestroyItem(childArr[j].gameObject);
                    }
                    childArr = null;

                    if (scroller._itemList != null)
                    {
                        scroller._itemList.Clear();
                    }

                    if (scroller._unUsedQueue != null)
                    {
                        scroller._unUsedQueue.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 创建Item
        /// </summary>
        /// <param name="index"></param>
        private void CreateItem(int index)
        {
            UIMultiScrollIndex itemBase;

            if (_unUsedQueue.Count > 0)
            {
                itemBase = _unUsedQueue.Dequeue();
                itemBase.Scroller = this;
            }
            else
            {
                if (itemPrefab == null)
                {
                    Log.Error("itemPrefab为空！！ 请检查资源");
                    return;
                }
                //GameObject obj = Instantiate(itemPrefab);
                GameObject obj = RequestGameObejct(itemPrefab);
                obj.SetActiveEx(true);
                //obj.transform.SetParent(_content);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                itemBase = obj.GetOrAddComponent<UIMultiScrollIndex>();
                itemBase.Scroller = this;
            }

            _itemList.Add(itemBase);

            itemBase.Check(index == this._checked_index, false);

            OnItemCreate?.Invoke(index, itemBase.gameObject);

            itemBase.Index = index;
        }

        /// <summary>
        /// 获取最上位置的索引
        /// </summary>
        /// <returns></returns>
        private int GetPosIndex()
        {
            int retValue = 0;

            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        retValue = Mathf.FloorToInt((_content.anchoredPosition.x + this._Padding.left) / -(cellWidth + spacingX));
                        break;
                    }
                case Arrangement.Vertical:
                    {
                        retValue = Mathf.FloorToInt((_content.anchoredPosition.y - this._Padding.top) / (cellHeight + spacingY));
                        break;
                    }
            }

            if (retValue < 0) retValue = 0;

            return retValue;
        }

        /// <summary>
        /// 获得指定项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private UIMultiScrollIndex GetScrollIndex(int index)
        {
            if (null == _itemList || _itemList.Count <= 0)
                return null;

            foreach (UIMultiScrollIndex item in _itemList)
            {
                if (item.Index == index)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// 这个方法的目的 就是根据总数量 行列 来计算content的真正宽度或者高度
        /// </summary>
        private void UpdateTotalSize()
        {
            if (null == _content)
                return;

            int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        _content.sizeDelta = new Vector2(this._Padding.left + cellWidth * lineCount + spacingX * (Mathf.Max(lineCount, 1) - 1), _content.sizeDelta.y);
                        break;
                    }
                case Arrangement.Vertical:
                    {
                        _content.sizeDelta = new Vector2(_content.sizeDelta.x, this._Padding.top + cellHeight * lineCount + spacingY * (Mathf.Max(lineCount, 1) - 1));
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 提供给外部的方法，删除指定位置的Item
        /// </summary>
        public void DelItem(int index)
        {
            if (index < 0 || index > _dataCount - 1)
            {
                Log.Error("删除错误:" + index);
                return;
            }
            DelItemFromPanel(index);
            DataCount -= 1;
        }

        private void AddItemIntoPanel(int index)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                UIMultiScrollIndex item = _itemList[i];
                if (item.Index >= index) item.Index += 1;
            }
            CreateItem(index);
        }

        private void DelItemFromPanel(int index)
        {
            int maxIndex = -1;
            int minIndex = int.MaxValue;
            for (int i = _itemList.Count; i > 0; i--)
            {
                UIMultiScrollIndex item = _itemList[i - 1];
                if (item.Index == index)
                {
                    DestroyItem(item.gameObject);
                    _itemList.Remove(item);
                }
                if (item.Index > maxIndex)
                {
                    maxIndex = item.Index;
                }
                if (item.Index < minIndex)
                {
                    minIndex = item.Index;
                }
                if (item.Index > index)
                {
                    item.Index -= 1;
                }
            }
            if (maxIndex < DataCount - 1)
            {
                CreateItem(maxIndex);
            }
        }

        private void DestroyItem(GameObject obj)
        {
            //GameObject.Destroy(obj);
            ReturnGameObejct(obj);
        }

        private void ReturnGameObejct(GameObject go)
        {
            go.SetActiveEx(false);

            if (!m_GoTag.ContainsKey(go))
                return;

            var tag = m_GoTag[go];
            RemoveOutMark(go);
            if (m_Pool.ContainsKey(tag))
                m_Pool[tag].Enqueue(go);
            else
            {
                var array = new Queue<GameObject>();
                array.Enqueue(go);
                m_Pool.Add(tag, array);
            }
        }

        private GameObject RequestGameObejct(GameObject prefab)
        {
            var tag = prefab.GetInstanceID();
            GameObject go = GetFromPool(tag);
            if (null == go)
                go = GameObject.Instantiate<GameObject>(prefab, _content);

            MarkAsOut(go, tag);

            return go;
        }

        private GameObject GetFromPool(int tag)
        {
            if (m_Pool.ContainsKey(tag) && m_Pool[tag].Count > 0)
            {
                GameObject obj = m_Pool[tag].Dequeue();
                obj.SetActiveEx(true);

                return obj;
            }
            else
                return null;
        }

        private void MarkAsOut(GameObject go, int tag)
        {
            m_GoTag.Add(go, tag);
        }

        private void RemoveOutMark(GameObject go)
        {
            if (m_GoTag.ContainsKey(go))
            {
                m_GoTag.Remove(go);
            }
            else
            {
                Log.Error("remove out mark error, gameObject has not been marked");
            }
        }

        private void ForceRebuildLayoutImmediate(RectTransform rect)
        {
            RectTransform childrect = null;
            for (int i = 0; i < rect.childCount; i++)
            {
                childrect = rect.GetChild(i).GetComponent<RectTransform>();
                if (childrect != null)
                {
                    ForceRebuildLayoutImmediate(childrect);
                }
                childrect = null;
            }

            if (rect.GetComponent<ContentSizeFitter>() != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
        }

        private void DestoryAll()
        {
            if (m_Pool != null)
            {
                foreach (var queues in m_Pool)
                {
                    foreach (var go in queues.Value)
                    {
                        if (go != null)
                            GameObject.Destroy(go);
                    }
                }
                m_Pool.Clear();
            }

            if (m_GoTag != null)
            {
                foreach (var it in m_GoTag)
                {
                    var go = it.Key;
                    if (go != null)
                        GameObject.Destroy(go);
                }

                m_GoTag.Clear();
            }
        }

        private void GetBestPerLine()
        {
            if (null == this._content)
                return;

            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        this.maxPerLine = (int)((this._content.rect.height + this.spacingY - this._Padding.top) / (this.cellHeight + this.spacingY));

                        break;
                    }
                case Arrangement.Vertical:
                    {
                        this.maxPerLine = (int)((this._content.rect.width + this.spacingX - this._Padding.left) / (this.cellWidth + this.spacingX));

                        break;
                    }
                default:
                    break;
            }
        }

        private void GetBestViewCount()
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        this.viewCount = Mathf.RoundToInt((this._ScrollRect.viewport.rect.width + this.spacingX - this._Padding.left) / (this.cellWidth + this.spacingX)) + 2;

                        break;
                    }
                case Arrangement.Vertical:
                    {
                        this.viewCount = Mathf.RoundToInt((this._ScrollRect.viewport.rect.height + this.spacingY - this._Padding.top) / (this.cellHeight + this.spacingY)) + 2;

                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 检查并处理自适应高度或者宽度
        /// </summary>
        private void CheckAdaptiveSize()
        {
            if (!this.IsAdaptiveSize)
                return;

            this.GetDefaultSize();

            if (null == _rectTransform)
                return;

            if (this._default_size <= 0.0001f)
                return;

            switch (this._movement)
            {
                case Arrangement.Horizontal:
                    {
                        float width = Mathf.Min(this._default_size, this._content.rect.width);
                        this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                        break;
                    }
                case Arrangement.Vertical:
                    {
                        float height = Mathf.Min(this._default_size, this._content.rect.height);
                        this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                        break;
                    }
                default:
                    break;
            }
        }

        private void GetDefaultSize()
        {
            if (null == _rectTransform)
                _rectTransform = this.GetComponent<RectTransform>();

            if (null == _rectTransform)
                return;

            if (this.IsAdaptiveSize && this._default_size <= 0)
            {
                switch (this._movement)
                {
                    case Arrangement.Horizontal:
                        {
                            this._default_size = _rectTransform.rect.width;
                            break;
                        }
                    case Arrangement.Vertical:
                        {
                            this._default_size = _rectTransform.rect.height;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void RegisterEvent()
        {
            if (null != this._ScrollRect)
            {
                this._ScrollRect.onValueChanged.RemoveAllListeners();
                this._ScrollRect.onValueChanged.AddListener(this.OnValueChange);
            }
        }

        #endregion

        [ContextMenu("ResetItems")]
        public void ResetItems()
        {
#if UNITY_EDITOR

            ResetScroller();
#endif
        }

    }
}