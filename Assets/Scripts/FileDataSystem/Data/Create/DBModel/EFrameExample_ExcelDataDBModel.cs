// ========================================================
// Des：自动生成请勿修改
// Autor：Polaris 
//CreateTime：2020-05-29 17:10:33
//版 本：v 1.0.0
//===================================================
using ReadExcel;
using System.Collections;
using System.Collections.Generic;
using System;
using EFrame;
/// <summary>
/// EFrameExample_ExcelData数据管理
/// </summary>
public partial class EFrameExample_ExcelDataDBModel : AbstractDBModel<EFrameExample_ExcelDataDBModel, EFrameExample_ExcelData>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "EFrameExample_ExcelData.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override EFrameExample_ExcelData MakeEntity(GameDataTableParser parse)
    {
        EFrameExample_ExcelData entity = new EFrameExample_ExcelData();
        entity.ID = parse.GetFieldValue("ID").ToInt();
        entity.mstring = parse.GetFieldValue("mstring");
        entity.mint = parse.GetFieldValue("mint").ToInt();
        entity.mfloat = parse.GetFieldValue("mfloat").ToFloat();
        entity.mlong = parse.GetFieldValue("mlong").ToLong();
        return entity;
    }
}
