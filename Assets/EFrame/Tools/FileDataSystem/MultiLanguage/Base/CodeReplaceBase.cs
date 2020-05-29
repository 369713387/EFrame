using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame
{
    public abstract class CodeReplaceBase<T> : MonoBehaviour
    {
        protected T m_target;

        protected PlatLanguage cur_language = PlatLanguage.none;

        protected string cur_keyName = "";

        protected string[] arr_Params;

        void OnEnable()
        {
            if (m_target == null)
            {
                m_target = this.gameObject.GetComponent<T>();
            }

            //UI显示的时候绑定委托事件
            MultiLanguageCtrl.Instance.replace_Handler += Refesh;
        }

        void OnDisable()
        {
            //UI不显示的时候解绑委托事件
            MultiLanguageCtrl.Instance.replace_Handler -= Refesh;
        }

        public void doSet(string key, string[] arr = null)
        {
            cur_keyName = key;

            arr_Params = arr;

            Refesh();
        }

        /// <summary>
        /// 根据语言版本刷新物体上的组件
        /// </summary>
        private void Refesh()
        {
            if (cur_language != MultiLanguageCtrl.Sys_Language)
            {
                if (m_target != null)
                {
                    //执行刷新UI
                    doRefesh();
                }
                else
                {
                    Debug.Log("该物体上无目标组件");
                }
            }
        }

        /// <summary>
        /// 执行刷新
        /// </summary>
        protected abstract void doRefesh();
    }
}


