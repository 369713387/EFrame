using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFrame
{
    public class ImgReplace : ReplaceBase<Image>
    {
        /// <summary>
        /// 执行刷新
        /// </summary>
        protected override void doRefesh()
        {
            Sprite content = this.name.ToLanguageImg();
            m_target.sprite = content;
        }


    }

}


