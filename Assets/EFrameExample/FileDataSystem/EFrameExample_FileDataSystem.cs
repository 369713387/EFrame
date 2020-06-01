using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFrame.Example.FileDataSystem
{
    public class EFrameExample_FileDataSystem : MonoBehaviour
    {
        public Button btn_Change;

        public Image img_Language;

        private void Start()
        {
            btn_Change = GameObject.Find("btn_change").GetComponent<Button>();
            img_Language = GameObject.Find("img_show").GetComponent<Image>();
            img_Language.sprite = "img_EframeExample_01".ToLanguageImg();
            btn_Change.onClick.AddListener(() =>
            {
                if (MultiLanguageCtrl.Sys_Language == PlatLanguage.zh_CN){
                    MultiLanguageCtrl.Instance.ChangeLanguagePack(PlatLanguage.en);                   
                }                    
                else{
                    MultiLanguageCtrl.Instance.ChangeLanguagePack(PlatLanguage.zh_CN);
                }
                img_Language.sprite = "img_EframeExample_01".ToLanguageImg();
            });
        }
    }
}


