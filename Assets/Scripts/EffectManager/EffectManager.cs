using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YiFun;

namespace Yifun
{
    public class EffectManager : MonoBehaviour
    {
        #region 单例
        private static EffectManager instance;

        public static EffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = GameObject.Find(typeof(EffectManager).Name);
                    instance = obj.GetComponent<EffectManager>();
                    DontDestroyOnLoad(obj);                    
                }
                return instance;
            }
        }
        #endregion

        public const string effect_Path = "Prefab/Effect";

        public List<string> list_key = new List<string>();

        public Dictionary<string, MonoPool<EffectBase>> dic = new Dictionary<string, MonoPool<EffectBase>>();

        private void Awake()
        {
            list_key = new List<string>() { effect_Path };

            for (int i = 0; i < list_key.Count; i++)
            {
                MonoPool<EffectBase> pool = new MonoPool<EffectBase>(list_key[i], transform, 5);
                pool.InitPool();
                dic.Add(list_key[i], pool);
            }
        }
        public void CreateEffect(string key, Vector3 position)
        {
            EffectBase go = dic[key].AllocatePoolItem(transform);
            go.transform.position = position;
        }

        public void RecycleEffect(EffectBase effect)
        {
            dic[effect.res_Path].ResetPoolItem(effect);
        }
    }
}


