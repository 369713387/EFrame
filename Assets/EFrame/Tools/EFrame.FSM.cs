using System;
using System.Collections.Generic;

namespace EFrame
{
    /// <summary>
    /// FSM
    /// </summary>
    /// <typeparam name="TStateEnum">状态枚举</typeparam>
    /// <typeparam name="TEventEnum">事件枚举</typeparam>
    public class FSM<TStateEnum, TEventEnum> : IDisposable
    {
        private Action<TStateEnum, TStateEnum> mOnStateChanged = null;

        public FSM(Action<TStateEnum, TStateEnum> onStateChanged = null)
        {
            mOnStateChanged = onStateChanged;
        }

        /// <summary>
		/// FSM状态机状态改变回调.
		/// </summary>
		public delegate void FSMOnStateChagned(params object[] param);

        /// <summary>
        /// 状态
        /// </summary>
        public class FSMState<TName>
        {
            public TName Name;

            public Action EnterCallback; //回调函数

            public Action ExitCallback; //回调函数

            public FSMState(TName name, Action enterCallback = null , Action exitCallback = null)
            {
                Name = name;
                EnterCallback = enterCallback;
                ExitCallback = exitCallback;
            }

            /// <summary>
            /// 状态转换关系字典
            /// </summary>
            public readonly Dictionary<TEventEnum, FSMTranslation<TName, TEventEnum>> TranslationDict =
                new Dictionary<TEventEnum, FSMTranslation<TName, TEventEnum>>();
        }

        /// <summary>
        /// 状态转换
        /// </summary>
        /// <typeparam name="TStateName">状态名</typeparam>
        /// <typeparam name="KEventName">事件名</typeparam>
        public class FSMTranslation<TStateName, KEventName>
        {
            public TStateName FromState;
            public KEventName Name;
            public TStateName ToState;
            public Action<object[]> OnTranslationCallback; // 回调函数,执行的状态事件

            public FSMTranslation(TStateName fromState, KEventName name, TStateName toState,
                Action<object[]> onStateChagned)
            {
                FromState = fromState;
                ToState = toState;
                Name = name;
                OnTranslationCallback = onStateChagned;
            }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        TStateEnum mCurState;

        /// <summary>
        /// 当前状态
        /// </summary>
        public TStateEnum State
        {
            get { return mCurState; }
        }

        /// <summary>
        /// 状态字典
        /// </summary>
        Dictionary<TStateEnum, FSMState<TStateEnum>> mStateDic = new Dictionary<TStateEnum, FSMState<TStateEnum>>();


        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enter"></param>
        public void AddState(TStateEnum name, Action enterCallback = null, Action exitCallback = null)
        {
            if (!mStateDic.ContainsKey(name))
            {
                mStateDic[name] = new FSMState<TStateEnum>(name);

                if (enterCallback != null)
                {
                    mStateDic[name].EnterCallback = enterCallback;
                }

                if (enterCallback != null)
                {
                    mStateDic[name].ExitCallback = exitCallback;
                }
            }
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="fsmState"></param>
        public void AddState(FSMState<TStateEnum> fsmState)
        {
            if (!mStateDic.ContainsKey(fsmState.Name))
            {
                mStateDic[fsmState.Name] = fsmState;
            }
        }

        /// <summary>
        /// 添加状态转换关系
        /// </summary>
        /// <param name="fromState">起始状态</param>
        /// <param name="name">事件名字</param>
        /// <param name="toState">目标状态</param>
        /// <param name="onStateChagned">回调函数</param>
        public void AddTransition(TStateEnum fromState, TEventEnum name, TStateEnum toState, Action<object[]> onStateChagned = null)
        {
            mStateDic[fromState].TranslationDict[name] = new FSMTranslation<TStateEnum, TEventEnum>(fromState, name, toState, onStateChagned);
        }

        /// <summary>
        /// 添加状态转换关系
        /// </summary>
        /// <param name="fromState">起始状态</param>
        /// <param name="name">事件名字</param>
        /// <param name="toState">目标状态</param>
        /// <param name="onStateChagned">回调函数</param>
        public void AddTransition(FSMTranslation<TStateEnum, TEventEnum> fsmTranslation)
        {
            if (mStateDic.ContainsKey(fsmTranslation.FromState) && mStateDic.ContainsKey(fsmTranslation.ToState))
            {
                mStateDic[fsmTranslation.FromState].TranslationDict[fsmTranslation.Name] = fsmTranslation;
            }
        }

        /// <summary>
        /// 启动状态机，以目标状态
        /// </summary>
        /// <param name="name">状态名</param>
        public void Start(TStateEnum name)
        {
            mCurState = name;
        }

        /// <summary>
        /// 处理当前状态下的事件
        /// </summary>
        /// <param name="name">状态名字</param>
        /// <param name="param">参数组</param>
        public void HandleEvent(TEventEnum name, params object[] param)
        {
            if (mCurState != null && mStateDic[mCurState].TranslationDict.ContainsKey(name))
            {
                var tempTranslation = mStateDic[mCurState].TranslationDict[name];

                if (tempTranslation.OnTranslationCallback != null)
                {
                    tempTranslation.OnTranslationCallback.Invoke(param);
                }

                if (mOnStateChanged != null)
                {
                    mOnStateChanged.Invoke(mCurState, tempTranslation.ToState);
                }

                if (mStateDic[mCurState].ExitCallback != null)
                {
                    mStateDic[mCurState].ExitCallback.Invoke();
                }

                mCurState = tempTranslation.ToState;

                if (mStateDic[mCurState].EnterCallback != null)
                {
                    mStateDic[mCurState].EnterCallback.Invoke();
                }
            }
        }

        /// <summary>
        /// 销毁状态机
        /// </summary>
        void Clear()
        {
            foreach (var keyValuePair in mStateDic)
            {
                foreach (var translationDictValue in keyValuePair.Value.TranslationDict.Values)
                {
                    translationDictValue.OnTranslationCallback = null;
                }

                keyValuePair.Value.TranslationDict.Clear();
            }

            mStateDic.Clear();
        }

        /// <summary>
        /// 状态机销毁
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
    }
}
