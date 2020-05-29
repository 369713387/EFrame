// ========================================================
// Des：自动生成请勿修改
// Autor：Polaris 
//CreateTime：2020-05-29 17:25:37
//版 本：v 1.0.0
//===================================================
using ReadExcel;
using System;
using System.Collections;
using System.Collections.Generic;
using EFrame;
/// <summary>
/// 生成LanguageImgDicdic数据包类
/// </summary>
public partial class LanguageImgDic : AbstractDic<LanguageImgDic>
{
    /// <summary>
    /// 中文图片路径
    /// </summary>
    public Dictionary<string, string> CN_img_Path = new Dictionary<string, string> ();

    /// <summary>
    /// 英文图片路径
    /// </summary>
    public Dictionary<string, string> EN_img_Path = new Dictionary<string, string> ();

    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "LanguageImgDic.data"; } }

    /// <summary>
    /// 创建字典内容 
    /// </summary>
    protected override void MakeDic(GameDataTableParser parse)
    {
        if (parse.GetFieldValue("keyName") != "" && !parse.GetFieldValue("keyName").Contains("//"))
        {
            CN_img_Path.Add(parse.GetFieldValue("keyName"), parse.GetFieldValue("CN_img_Path"));
            EN_img_Path.Add(parse.GetFieldValue("keyName"), parse.GetFieldValue("EN_img_Path"));
        }
    }

}
