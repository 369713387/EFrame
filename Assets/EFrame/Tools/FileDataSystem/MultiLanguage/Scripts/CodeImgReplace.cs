// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFrame
{
    public class CodeImgReplace : CodeReplaceBase<Image>
    {
        /// <summary>
        /// 执行刷新
        /// </summary>
        protected override void doRefesh()
        {
            Sprite content = this.cur_keyName.ToLanguageImg();
            m_target.sprite = content;
        }
    }
}


