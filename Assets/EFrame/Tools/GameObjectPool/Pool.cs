using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YiFun
{
    public class MonoPool<T> where T : MonoBehaviour, I_PoolItem
    {
        public string pf_url;

        public Transform parent;

        public int Count = 0;

        public MonoPool(string _pf_url, Transform _parent, int _Count)
        {
            pf_url = _pf_url;
            parent = _parent;
            Count = _Count;
        }

        #region 初始化通用按钮对象池

        private List<T> m_List = new List<T>();

        public List<T> List { get { return m_List; } }

        public SimpleObjectPool<T> btnPool;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitPool(Transform Iparent = null)
        {
            btnPool = new SimpleObjectPool<T>(
                () =>
                {
                    //T item = XAssetMgr.Instance.LoadPrefab(pf_url, Iparent).GetComponent<T>();
                    GameObject pf = Resources.Load<GameObject>(pf_url);
                    GameObject go = GameObject.Instantiate(pf);
                    T item = go.GetComponent<T>();
                    item.transform.SetParent(parent);
                    item.gameObject.SetActive(false);
                    return item;
                },
                null,
                Count);
        }

        public void ResetPoolItem(T item)
        {
            m_List.Remove(item);
            item.Dispose();
            btnPool.Recycle(item);
        }

        public T AllocatePoolItem(Transform parent)
        {
            T item = btnPool.Allocate();

            m_List.Add(item);

            item.transform
                .Parent(parent)
                //.LocalScale(1)
                .LocalPositionZ(0);

            item.gameObject.SetActive(true);

            item.transform.SetAsLastSibling();

            return item;
        }

        public void PoolItemRecycle()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                ResetPoolItem(m_List[i]);
            }
            m_List.Clear();
        }

        #endregion
    }

    public class ObjectPool<T> where T : new()
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private readonly UnityAction<T> m_ActionOnGet;
        private readonly UnityAction<T> m_ActionOnRelease;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            if (m_ActionOnGet != null)
                m_ActionOnGet(element);
            return element;
        }

        public void Release(T element)
        {
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            if (m_ActionOnRelease != null)
                m_ActionOnRelease(element);
            m_Stack.Push(element);
        }
    }

    public static class ListPool<T> where T : I_ObjectPoolItem
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, l => l.Clear());

        public static List<T> Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(List<T> toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}


