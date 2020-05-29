using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame.Example
{
    public class EFrameSimpleMsg
    {
        public string Msg = "";

        public void Reset()
        {
            Msg = "";
        }
    }

    public class EFrameSafeMsg:IPoolType,IPoolable
    {

        public string Msg = "";

        #region IPoolable接口实现
        public bool IsRecycled { get; set; }

        public void OnRecycled()
        {
            Msg = "";
            Debug.Log("回收到对象池进行，重新进行初始化操作");
        }
        #endregion


        #region IPoolType接口实现

        public static EFrameSafeMsg Allocate()
        {
            return SafeObjectPool<EFrameSafeMsg>.Instance.Allocate();
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<EFrameSafeMsg>.Instance.Recycle(this);
        }

        #endregion


    }

    public class EFrameExample_ObjectPool : MonoBehaviour
    {
        private void Start()
        {

            #region 简易对象池示例
            SimpleObjectPool<EFrameSimpleMsg> simpleMsgPool = new SimpleObjectPool<EFrameSimpleMsg>(
                () => new EFrameSimpleMsg(),
                simpleMsg => { simpleMsg.Reset(); },
                10
                );

            Debug.Log(string.Format("simpleMsgPool.CurCount: {0}", simpleMsgPool.CurCount));

            var t_simpleMsg = simpleMsgPool.Allocate();

            Debug.Log(string.Format("simpleMsgPool.CurCount: {0}", simpleMsgPool.CurCount));

            var res = simpleMsgPool.Recycle(t_simpleMsg);

            Debug.Log(string.Format("simpleMsgPool.CurCount: {0}", simpleMsgPool.CurCount));

            EFrameSimpleMsg[] msgs = new EFrameSimpleMsg[5];

            for (int i = 0; i < msgs.Length; i++)
            {
                msgs[i] = simpleMsgPool.Allocate();
            }

            Debug.Log(string.Format("simpleMsgPool.CurCount: {0}", simpleMsgPool.CurCount));

            for (int i = 0; i < msgs.Length; i++)
            {
                simpleMsgPool.Recycle(msgs[i]);
            }

            Debug.Log(string.Format("simpleMsgPool.CurCount: {0}", simpleMsgPool.CurCount));
            #endregion

            #region 安全对象池示例
            SafeObjectPool<EFrameSafeMsg> safeMsgPool = SafeObjectPool<EFrameSafeMsg>.Instance;

            safeMsgPool.Init(100, 50);

            Debug.Log(string.Format("safeMsgPool.CurCount: {0}", safeMsgPool.CurCount));

            var msg = EFrameSafeMsg.Allocate();

            Debug.Log(string.Format("safeMsgPool.CurCount: {0}", safeMsgPool.CurCount));

            msg.Recycle2Cache();

            Debug.Log(string.Format("safeMsgPool.CurCount: {0}", safeMsgPool.CurCount));

            EFrameSafeMsg[] safeMsgs = new EFrameSafeMsg[40];

            for (int i = 0; i < safeMsgs.Length; i++)
            {
                safeMsgs[i] = EFrameSafeMsg.Allocate();
            }

            Debug.Log(string.Format("safeMsgPool.CurCount: {0}", safeMsgPool.CurCount));

            for (int i = 0; i < safeMsgs.Length; i++)
            {
                safeMsgs[i].Recycle2Cache();
            }

            Debug.Log(string.Format("safeMsgPool.CurCount: {0}", safeMsgPool.CurCount));

            #endregion

        }
    }
}


