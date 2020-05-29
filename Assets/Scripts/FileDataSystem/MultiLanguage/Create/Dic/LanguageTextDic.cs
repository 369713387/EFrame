// ========================================================
// Des：自动生成请勿修改
// Autor：Yifun 
//CreateTime：2020-05-29 18:14:32
//版 本：v 1.0.0
//===================================================
using ReadExcel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EFrame
{
/// <summary>
/// 生成LanguageTextDicdic数据包类
/// </summary>
public partial class LanguageTextDic : AbstractDic<LanguageTextDic>
{
    /// <summary>
    /// 中文语言包
    /// </summary>
    public Dictionary<string, string> CN_content = new Dictionary<string, string> ();

    /// <summary>
    /// 英文语言包
    /// </summary>
    public Dictionary<string, string> EN_content = new Dictionary<string, string> ();

    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "LanguageTextDic.data"; } }

    /// <summary>
    /// 创建字典内容 
    /// </summary>
    protected override void MakeDic(GameDataTableParser parse)
    {
        if (parse.GetFieldValue("keyName") != "" && !parse.GetFieldValue("keyName").Contains("//"))
        {
            CN_content.Add(parse.GetFieldValue("keyName"), parse.GetFieldValue("CN_content").ToUnescape());
            EN_content.Add(parse.GetFieldValue("keyName"), parse.GetFieldValue("EN_content").ToUnescape());
        }
    }

}
}
