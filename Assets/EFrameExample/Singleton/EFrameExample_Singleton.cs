using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame.Example.Singleton
{

    public class EFrameExample_Singleton : MonoBehaviour
    {
        private void Start()
        {
            var intance = EFrameExample_MonoSingleton_Test.Instance;

            var intance2 = EMonoSingletonProperty<EFrameExample_MonoSingleton_Property>.Instance;

            var intance3 = Singleton<EFrameExample_Singleton_Test>.Instance;

            var intance4 = SingletonProperty<EFrameExample_Singleton_Property>.Instance;


        }
    }

    [EMonoSingletonPath("Example/EFrameExample_MonoSingleton_Test")]
    public class EFrameExample_MonoSingleton_Test : EMonoSingletion<EFrameExample_MonoSingleton_Test>
    {
        public override void OnSingletonInit()
        {
            Debug.Log("我是EFrameExample_MonoSingleton_Test的Init");
        }

        private void Start()
        {
            Debug.Log("我是EFrameExample_MonoSingleton_Test的Start");
        }
    }

    [EMonoSingletonPath("Example/EFrameExample_MonoSingleton_Property")]
    public class EFrameExample_MonoSingleton_Property : MonoBehaviour, ISingleton
    {
        public void OnSingletonInit()
        {
            Debug.Log("我是EFrameExample_MonoSingleton_Property的Init");
        }

        private void Start()
        {
            Debug.Log("我是EFrameExample_MonoSingleton_Property的Start");
        }
    }

    public class EFrameExample_Singleton_Test : Singleton<EFrameExample_Singleton_Test>
    {
        public override void OnSingletonInit()
        {
            Debug.Log("我是EFrameExample_Singleton_Test的Init");
        }

        //不继承Mono的单例需要定义一个非Public的无参数构造函数，用于创建实例
        private EFrameExample_Singleton_Test()
        {

        }
    }

    public class EFrameExample_Singleton_Property : ISingleton
    {
        public void OnSingletonInit()
        {
            Debug.Log("我是EFrameExample_Singleton_Property的Init");
        }
        //不继承Mono的单例需要定义一个非Public的无参数构造函数，用于创建实例
        private EFrameExample_Singleton_Property()
        {

        }
    }
}


