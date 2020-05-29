using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFrame
{
    public class CodeTextReplace : CodeReplaceBase<Text>
    {
        /// <summary>
        /// 执行刷新
        /// </summary>
        protected override void doRefesh()
        {
            string content = this.cur_keyName.ToLanguageText();

            if (arr_Params.Length > 0)
            {
                string.Format("content", arr_Params);
            }
            else
            {
                m_target.text = content;
            }
        }
    }
}


