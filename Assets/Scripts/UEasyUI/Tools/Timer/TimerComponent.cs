using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/**********************************************
* @说    明:定时器组件
* @作    者:zhoumingfeng
* @版 本 号:V1.00
* @创建时间:2020.09.03
**********************************************/
namespace UEasyUI
{
    public class TimerComponent : MonoBehaviour
    {
        #region 私有变量

        //游戏启动的时候的时间
        private long _startTimer = 0;
        /// <summary>
        ///  update 更新间隔时间
        /// </summary>
        private long _updaeDeltaTime = 0;
        /// <summary>
        ///  run 时间
        /// </summary>
        private long _TempCurrDateTime = 0;

        private List<TimerInvokeVo> _list = new List<TimerInvokeVo>();
        /// <summary>
        /// 当列表发生变更的时候
        /// </summary>
        private bool _isChangeList = true;
        /// <summary>
        /// 运行列表
        /// </summary>
        private TimerInvokeVo[] _tempRunList;
        /// <summary>
        /// 安全锁
        /// </summary>
        private object _LockInvokeObj = new object();
        private float _startRealtimeSinceStartup = 0;

        private float _endScaleTime = 0;
        private long _CurrDateTime = 0;

        #endregion

        #region 公有属性

        /// <summary>
        /// 获取本地时间毫秒级别的long
        /// </summary>
        /// <returns></returns>
        public long CurrLocalDateTime
        {
            get
            {
                if (_CurrDateTime == 0) _CurrDateTime = GetCurrDateTime();
                return _CurrDateTime;
            }
            private set
            {
                _CurrDateTime = value;
            }
        }

        /// <summary>
        /// update 更新间隔时间
        /// </summary>
        public long UpdaeDeltaTime
        {
            get
            {
                return _updaeDeltaTime;
            }
        }

        /// <summary>
        /// 获取 启动后到当前的时间间隔  毫秒
        /// </summary>
        /// <returns></returns>
        public long RunDeltaTime
        {
            get
            {
                return (_TempCurrDateTime - _startTimer);
            }
        }

        private static TimerComponent _instance = null;
        public static TimerComponent Instance
        {
            get { return _instance; }
        }

        #endregion

        #region 框架接口

        private void Awake()
        {
            _instance = this;

            _isChangeList = true;
            _startTimer = DateTime.Now.Ticks / 10000;
            _startRealtimeSinceStartup = Time.realtimeSinceStartup;
        }

        private long _dt = 0;
        /// <summary>
        /// 时间更新主要运行在 线程的 tick中
        /// </summary>
        public void Update()
        {
            _dt = GetCurrDateTime();
            _CurrDateTime = _dt;
            //TickTimeScale();
            _updaeDeltaTime = _dt - _TempCurrDateTime;
            _TempCurrDateTime = _dt;

            if (_list == null)
                return;

            int len = _list.Count;
            if (_isChangeList)
            {
                _tempRunList = len == 0 ? null : _list.ToArray();
                _isChangeList = false;
            }

            TimerInvokeVo item;
            for (int i = 0; i < len; i++)
            {
                item = _tempRunList[i];
                if (null == item || item.RepeatInterval == -1)
                {
                    RemoveList(item);
                    continue;
                }
                item.checkRun(_dt);
            }
        }

        #endregion

        #region 公有接口

        /// <summary>
        /// 启动定时器(delayStartTime后调用onCallback)
        /// </summary>
        /// <param name="delayStartTime">延迟时间(秒)</param>
        /// <param name="onCallback">回调方法</param>
        /// <returns>定时器ID</returns>
        public int Startup(float delayStartTime, UnityAction<float> onCallback)
        {
            return this.Startup(onCallback, null, delayStartTime, 0, 1, false);
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="repeatCount">重复执行的次数</param>
        /// <param name="repeatInterval">执行间隔时间(秒)</param>
        /// <param name="onPerTimeUnitCallback">每次执行的回调方法</param>
        /// <param name="onTimerOverCallback">结束时的回调方法</param>
        /// <returns>定时器ID</returns>
        public int Startup(short repeatCount, float repeatInterval, UnityAction<float> onPerTimeUnitCallback, UnityAction onTimerOverCallback = null)
        {
            return this.Startup(onPerTimeUnitCallback, onTimerOverCallback, 0, repeatInterval, repeatCount, false);
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="onRepeatCallback">执行的定时回调方法,结束时也会调用一次,回调的参数为从定时器开始到当前累计的时间,包括delayStartTime的时间,单位为秒</param>
        /// <param name="onTimerOverCallback">执行结束的回调方法,结束时先执行onRepeatCallback,再执行本回调</param>
        /// <param name="delayStartTime">第一次执行延迟多久执行 分为两种模式：
        ///  1.isRunStart = false 默认模式，第一次执行该方法将是在 
        ///    当前时间 + delayStartTimer + repeatInterval 后执行，之后每个 onRepeatCallback 执行一次
        ///  2.isRunStart = true ，第一次执行该方法将是在 
        ///    当前时间 + delayStartTimer 后执行，之后每个 onRepeatCallback 执行一次
        /// </param>
        /// <param name="repeatInterval">每间隔多久执行一次(秒)</param>
        /// <param name="repeatCount">执行的总次数(-1表示一直执行)</param>
        /// <param name="isRunStart">是否存执行第一次执行模式</param>
        /// <returns>定时器ID</returns>
        public int Startup(UnityAction<float> onRepeatCallback, UnityAction onTimerOverCallback = null, 
            float delayStartTime = 0, float repeatInterval = 1.0f, short repeatCount = -1, bool isRunStart = false)
        {
            lock (_LockInvokeObj)
            {
                if (repeatInterval <= -1) return -1;
                UnityAction<float> action = onRepeatCallback;
                int key = action.GetHashCode();
                long t = System.Convert.ToInt64(delayStartTime * 1000L);
                _list.Add(TimerInvokeVo.Get(key, action, onTimerOverCallback, t, Mathf.RoundToInt(repeatInterval * 1000), _list.Count, repeatCount, isRunStart));
                _isChangeList = true;
                return key;
            }
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        /// <param name="timerId">定时器ID</param>
        public void Stop(int timerId)
        {
            TimerInvokeVo vo;
            for (int i = 0, len = _list.Count; i < len; i++)
            {
                vo = _list[i];
                if (vo.Id == timerId)
                {
                    vo.RepeatInterval = -1;
                    continue;
                }
            }
        }

        /// <summary>
        /// 是否存在定时器
        /// </summary>
        /// <param name="timerId">定时器ID</param>
        public bool IsExist(int timerId)
        {
            TimerInvokeVo vo;
            for (int i = 0, len = _list.Count; i < len; i++)
            {
                vo = _list[i];
                if (vo.Id == timerId && vo.RepeatInterval != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 时间缩放
        /// </summary>
        /*
        public void StartTimeScale(float scale, float duration)
        {
            Time.timeScale = scale;
            _endScaleTime = Time.realtimeSinceStartup + duration;

            GameFramework.Log.Debug("[时间缩放] StartTimeScale ==> scale : ", scale, " duration : ", duration);
        }
        */

        /// <summary>
        /// 移除所有定时器
        /// </summary>
        public void RemoveAll()
        {
            lock (_LockInvokeObj)
            {
                for (int i = 0, len = _list.Count; i < len; i++) TimerInvokeVo.Recycling(_list[i]);
                _list.Clear();
                _isChangeList = true;
            }
        }

        #endregion

        #region 私有接口

        private long GetCurrDateTime()
        {
            return _startTimer + ((long)((Time.realtimeSinceStartup - _startRealtimeSinceStartup) * 1000));
        }

        private void RemoveList(TimerInvokeVo vo)
        {
            lock (_LockInvokeObj)
            {
                _list.Remove(vo);
                TimerInvokeVo.Recycling(vo);
                _isChangeList = true;
            }
        }

        private void TickTimeScale()
        {
//#if !UNITY_EDITOR
            if (Time.timeScale != 1 && Time.realtimeSinceStartup > _endScaleTime)
            {
                Log.Debug("[时间缩放] 时间缩放计时结束恢复正常时间 ==> timeScale : 1");

                Time.timeScale = 1;
            }
//#endif
        }

#endregion
    }
    //================================================================================
    public class TimerInvokeVo
    {
        private static Queue<TimerInvokeVo> _pool = new Queue<TimerInvokeVo>();
        public static TimerInvokeVo Get(int id, UnityAction<float> onPerTimeUnitCallback, UnityAction onTimerOverCallback, long delayStartTime, int repeatInterval, int indexd, short repeatCount = -1, bool isRunStart = false)
        {
            if (_pool.Count > 0)
            {
                var vo = _pool.Dequeue();
                vo.Init(id, onPerTimeUnitCallback, onTimerOverCallback, delayStartTime, repeatInterval, indexd, repeatCount, isRunStart);
                return vo;
            }

            return new TimerInvokeVo(id, onPerTimeUnitCallback, onTimerOverCallback, delayStartTime, repeatInterval, indexd, repeatCount, isRunStart);
        }

        public static void Recycling(TimerInvokeVo vo)
        {
            if (_pool.Count > 50 || vo == null) 
                return;

            vo.Recyc();
            _pool.Enqueue(vo);
        }
        //---------------------------------------------------------------
        public int Id { private set; get; }
        private UnityAction<float> _onPerTimeUnitCallback;
        private UnityAction _onTimerOverCallback;
        private long _delayStartTime;
        private int _repeatInterval;
        public int RepeatInterval
        {
            get
            {
                return _repeatInterval;
            }
            set
            {
                _repeatInterval = value;
            }
        }

        private short _repeatCount;
        private long _startTime;
        private long _currentStartTime;
        public int index;
        private bool _isRunStart;

        public TimerInvokeVo(int id, UnityAction<float> onPerTimeUnitCallback, UnityAction onTimerOverCallback, long delayStartTime, int repeatInterval, int indexd, short repeatCount = -1, bool isRunStart = false)
        {
            Init(id, onPerTimeUnitCallback, onTimerOverCallback, delayStartTime, repeatInterval, indexd, repeatCount, isRunStart);
        }

        public void Init(int id, UnityAction<float> onPerTimeUnitCallback, UnityAction onTimerOverCallback, long delayStartTime, int repeatInterval, int indexd, short repeatCount = -1, bool isRunStart = false)
        {
            Id = id;
            _onPerTimeUnitCallback = onPerTimeUnitCallback;
            _onTimerOverCallback = onTimerOverCallback;
            _delayStartTime = delayStartTime;
            _repeatInterval = repeatInterval;
            _repeatCount = repeatCount;
            _isRunStart = isRunStart;
            _startTime = TimerComponent.Instance.CurrLocalDateTime;
            _currentStartTime = _startTime + _delayStartTime;
            index = indexd;
        }

        public void Recyc()
        {
            Id = -1;
            _onPerTimeUnitCallback = null;
            _delayStartTime = 0;
            _repeatInterval = 0;
            _repeatCount = 0;
            _isRunStart = false;
            _currentStartTime = 0;
            index = 0;
        }
        /// <summary>
        /// 检测是否执行函数
        /// </summary>
        /// <param name="currTimer"></param>
        public bool checkRun(long currTimer)
        {
            if (_isRunStart && _currentStartTime <= currTimer)
            {
                _isRunStart = false;

                if (_onPerTimeUnitCallback != null)
                    //_onPerTimeUnitCallback((currTimer - _currentStartTime + _delayStartTime) * 0.001f);
                    _onPerTimeUnitCallback.Invoke((currTimer - _startTime) * 0.001f);

                if (_repeatCount > 0) 
                    _repeatCount--;

                return true;
            }

            long dx = _currentStartTime + _repeatInterval;

            if (currTimer < dx)
                return false;

            if (_repeatCount == 0)
            {
                _repeatInterval = -1;

                //if (null != _onTimerOverCallback)
                //    _onTimerOverCallback.Invoke();

                return true;
            }

            if (_repeatCount > 0) 
                _repeatCount--;

            //float f = (_repeatInterval + (currTimer - dx)) * 0.001f;
            float f = (currTimer - _startTime) * 0.001f;

            try
            {
                if (_onPerTimeUnitCallback != null)
                    _onPerTimeUnitCallback.Invoke(f);
            }
            catch (Exception ex)
            {
                //GameFramework.Log.Warning("Timer.checkRun => action.name = " + _onPerTimeUnitCallback.Method.Name + " , ex = " + ex.StackTrace);
            }

            try
            {
                if (_repeatCount == 0 && null != _onTimerOverCallback)
                    _onTimerOverCallback.Invoke();
            }
            catch (Exception ex)
            {
                //GameFramework.Log.Warning("Timer.checkRun => action.name = " + _onTimerOverCallback.Method.Name + " , ex = " + ex.StackTrace);
            }

            _currentStartTime = currTimer;

            return true;
        }
    }
}