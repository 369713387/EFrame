using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFrame
{
    public class TextReplace : ReplaceBase<Text>
    {
        /// <summary>
        /// 执行刷新
        /// </summary>
        protected override void doRefesh()
        {
            string content = MultiLanguageCtrl.Instance.GetText(this.name);

            m_target.text = content;
        }
    }
}


