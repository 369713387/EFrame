
using System;
using System.Collections.Generic;

namespace EFrame
{
    #region 对象池抽象类

    /// <summary>
    /// 观察者数量
    /// </summary>
    public interface ICountObserveAble
    {
        int CurCount { get; }
    }

    /// <summary>
    /// 对象工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFactory<T>
    {
        T Create();
    }

    public interface IPool<T>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        T Allocate();

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Recycle(T obj);
    }

    /// <summary>
    /// 对象池抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T>, ICountObserveAble
    {
        /// <summary>
        /// 获取观察者数量
        /// </summary>
        public int CurCount
        {
            get
            {
                return mCacheStack.Count;
            }
        }

        protected IObjectFactory<T> mFactory;

        protected readonly Stack<T> mCacheStack = new Stack<T>();

        /// <summary>
        /// 对象池最大容量
        /// </summary>
        protected int mMaxCount = 24;

        /// <summary>
        /// 申请一个对象
        /// </summary>
        /// <returns></returns>
        public virtual T Allocate()
        {
            return mCacheStack.Count == 0
                ? mFactory.Create()
                : mCacheStack.Pop();
        }

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool Recycle(T obj);
    }
    #endregion

    #region 简易对象池的实现

    public class SimpleObjcetFactor<T> : IObjectFactory<T>
    {
        public SimpleObjcetFactor(Func<T> FactoryMethod)
        {
            mFactoryMethod = FactoryMethod;
        }

        protected Func<T> mFactoryMethod;

        public T Create()
        {
            return mFactoryMethod();
        }
    }


    public class SimpleObjectPool<T> : Pool<T>
    {
        protected Action<T> mResetMethod;

        public SimpleObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null,int initCount = 0)
        {
            mFactory = new SimpleObjcetFactor<T>(factoryMethod);
            mResetMethod = resetMethod;

            for (int i = 0; i < initCount; i++)
            {
                mCacheStack.Push(mFactory.Create());
            }

        }


        public override bool Recycle(T obj)
        {
            if(mResetMethod != null)
            {
                mResetMethod.Invoke(obj);
            }

            mCacheStack.Push(obj);

            return true;
        }
    }

    #endregion

    #region 安全的对象池的实现

    public class DefaultObjectFactory<T> : IObjectFactory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }

    /// <summary>
    /// 池对象类型约束
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 接收重置事件
        /// </summary>
        void OnRecycled();

        /// <summary>
        /// 该对象是否被回收过
        /// </summary>
        bool IsRecycled { get; set; }
    }

    /// <summary>
    /// 池对象缓存约束
    /// </summary>
    public interface IPoolType
    {
        void Recycle2Cache();
    }

    /// <summary>
    /// 安全的对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeObjectPool<T> : Pool<T>,ISingleton where T : IPoolable, new()
    {
        #region 单例
        void ISingleton.OnSingletonInit()
        {

        }

        private SafeObjectPool()
        {
            mFactory = new DefaultObjectFactory<T>();
        }

        public static SafeObjectPool<T> Instance
        {
            get
            {
                return SingletonProperty<SafeObjectPool<T>>.Instance;
            }
        }

        public void Dispose()
        {
            SingletonProperty<SafeObjectPool<T>>.Dispose();
        }

        #endregion

        /// <summary>
        /// 对象池的最大容量
        /// </summary>
        /// <value>The max cache count.</value>
        public int MaxCacheCount
        {
            get { return mMaxCount; }
            set
            {
                mMaxCount = value;

                if (mCacheStack != null)
                {
                    if (mMaxCount > 0)
                    {
                        if (mMaxCount < mCacheStack.Count)
                        {
                            int removeCount = mCacheStack.Count - mMaxCount;
                            while (removeCount > 0)
                            {
                                mCacheStack.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化对象池的最大容量和初始容量
        /// </summary>
        /// <param name="maxCount">最大容量</param>
        /// <param name="initCount">初始容量</param>
        public void Init(int maxCount,int initCount)
        {
            MaxCacheCount = maxCount;

            if (maxCount > 0)
            {
                initCount = Math.Min(maxCount, initCount);
            }

            if(CurCount< initCount)
            {
                for (int i = CurCount; i < initCount; i++)
                {
                    Recycle(new T());
                }
            }
        }

        public override T Allocate()
        {
            T result = base.Allocate();
            result.IsRecycled = false;
            return result;
        }

        public override bool Recycle(T obj)
        {
            if (obj == null || obj.IsRecycled)
            {
                return false;
            }

            if (mMaxCount > 0)
            {
                if (mCacheStack.Count >= mMaxCount)
                {
                    obj.OnRecycled();
                    return false;
                }
            }

            obj.IsRecycled = true;
            obj.OnRecycled();
            mCacheStack.Push(obj);

            return true;
        }
    }
    #endregion


}