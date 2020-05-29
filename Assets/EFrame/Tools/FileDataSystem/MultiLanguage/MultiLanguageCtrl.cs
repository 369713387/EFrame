
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EFrame
{
    public class MultiLanguageCtrl:Singleton<MultiLanguageCtrl>
    {

        public static PlatLanguage Sys_Language = PlatLanguage.zh_CN;

        /// <summary>
        /// 文本dic
        /// </summary>
        static Dictionary<string, string> cur_languageTPack = new Dictionary<string, string>();

        /// <summary>
        /// 图片dic
        /// </summary>
        static Dictionary<string, string> cur_languageIPack = new Dictionary<string, string>();

        /// <summary>
        /// 保存的图片single Sprite
        /// </summary>
        static Dictionary<string, Sprite> img_Sprite = new Dictionary<string, Sprite>();

        /// <summary>
        /// 保存的图片multiple Sprite
        /// </summary>
        static Dictionary<string, Sprite[]> img_MulSprite = new Dictionary<string, Sprite[]>();

        public delegate void DoDelegate(); // 声明委托类型
        public event DoDelegate replace_Handler; // 语言替换事件委托

        private MultiLanguageCtrl()
        {

        }

        public override void OnSingletonInit()
        {
            LoadData();
        }

        /// <summary>
        /// 根据系统语言加载语言包
        /// </summary>
        private void LoadData()
        {
            img_Sprite.Clear();
            img_MulSprite.Clear();

            //以下内容需要根据excle表格进行定义
            switch (Sys_Language)
            {
                case PlatLanguage.zh_CN:
                    cur_languageTPack = LanguageTextDic.Instance.CN_content;
                    cur_languageIPack = LanguageImgDic.Instance.CN_img_Path;
                    break;
                case PlatLanguage.en:
                    cur_languageTPack = LanguageTextDic.Instance.EN_content;
                    cur_languageIPack = LanguageImgDic.Instance.EN_img_Path;
                    break;
                case PlatLanguage.none:
                    cur_languageTPack = LanguageTextDic.Instance.CN_content;
                    cur_languageIPack = LanguageImgDic.Instance.CN_img_Path;
                    break;
            }
        }


        /// <summary>
        /// 根据键值获取文本信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if (cur_languageTPack.ContainsKey(key))
            {
                return cur_languageTPack[key];
            }
            else
            {
                Debug.Log("不存在文本键值：" + key);
                return null;
            }
        }

        /// <summary>
        /// 根据键值获取图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite GetImg(string key)
        {
            if (cur_languageIPack.ContainsKey(key))
            {
                if (img_Sprite.ContainsKey(key))
                {
                    return img_Sprite[key];
                }
                else
                {
                    Sprite tex = Resources.Load<Sprite>(cur_languageIPack[key]);

                    if (tex != null)
                    {
                        img_Sprite[key] = tex;
                    }
                    else
                    {
                        Assert.IsNotNull(tex, "图片加载结果为Null，图片路径为" + cur_languageIPack[key]);
                    }


                    return tex;
                }
            }
            else
            {
                Debug.Log("不存在图片键值：" + key);
                return null;
            }
        }

        /// <summary>
        /// 根据键值获取图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite[] GetMulImg(string key)
        {
            if (cur_languageIPack.ContainsKey(key))
            {
                if (img_MulSprite.ContainsKey(key))
                {
                    return img_MulSprite[key];
                }
                else
                {
                    Sprite[] tex = Resources.LoadAll<Sprite>(cur_languageIPack[key]);

                    if (tex != null)
                    {
                        img_MulSprite[key] = tex;
                    }
                    else
                    {
                        Assert.IsNotNull(tex, "图片加载结果为Null，图片路径为" + cur_languageIPack[key]);
                    }


                    return tex;
                }
            }
            else
            {
                Debug.Log("不存在图片键值：" + key);
                return null;
            }
        }

        public void ReleaseImg()
        {
            img_Sprite.Clear();
            img_MulSprite.Clear();
        }

        /// <summary>
        /// 切换系统语言
        /// </summary>
        public void ChangeLanguagePack(PlatLanguage m_Language)
        {
            //切换语言的时候
            //1.先更改游戏的语言设置
            //2.加载新的语言包
            //3.更新场景中所有显示着的UI文本和图片
            Sys_Language = m_Language;
            LoadData();

            if (replace_Handler != null)
            {
                replace_Handler();
            }
        }
    }
}


