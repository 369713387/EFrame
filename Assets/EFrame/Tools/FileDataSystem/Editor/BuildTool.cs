// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/08/05 14:13:46
// 版 本：v 1.0.0
// Copyright ：SmartMelon 
// ========================================================
using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public abstract class BuildTool {

    //异或因子
    protected static byte[] xorScale = new byte[] { 45, 66, 38, 55, 23, 254, 9, 165, 90, 19, 41, 45, 201, 58, 55, 37, 254, 185, 165, 169, 19, 171 };//.data文件的xor加解密因子

    #region Read_ALLData 读取所有文本数据事件
    /// <summary>
    /// 读取所有文本数据事件
    /// </summary>
    /// <param name="arr"></param>
    public void Read_ALLData(string[] arr, bool ifCreateCode = true)
    {
        if (arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                ReadData(arr[i], ifCreateCode);
            }
        }

    }
    #endregion

    #region ReadData 读取数据
    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="path"></param>
    public void ReadData(string path, bool ifCreateCode = true)
    {
        if (string.IsNullOrEmpty(path)) return;
        path = path.Replace("\\","/");
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();


        DataTable dt = null;

        dt = result.Tables[0];

        CreateData(path, dt, ifCreateCode);
    }
    #endregion

    #region CreateData 生成加密后的文件
    /// <summary>
    /// 生成加密后的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dt"></param>
    protected abstract void CreateData(string path, DataTable dt, bool ifCreateCode = true);
    #endregion

    #region ChangeTypeName 读取数据时转换数据类型
    protected string ChangeTypeName(string type)
    {
        string str = string.Empty;

        switch (type)
        {
            case "int":
                str = ".ToInt()";
                break;
            case "short":
                str = ".ToShort()";
                break;
            case "long":
                str = ".ToLong()";
                break;
            case "float":
                str = ".ToFloat()";
                break;
            case "double":
                str = ".ToDouble()";
                break;
            case "bool":
                str = ".ToBool()";
                break;
            case "string.":
                str = ".ToUnescape()";
                break;
        }

        return str;
    }
    #endregion

    #region GetTypeName 定义数据时，获取数据类型
    protected string GetTypeName(string type)
    {
        string str = "string";

        switch (type)
        {
            case "int":
                str = "int";
                break;
            case "short":
                str = "short";
                break;
            case "long":
                str = "long";
                break;
            case "float":
                str = "float";
                break;
            case "double":
                str = "double";
                break;
            case "bool":
                str = "bool";
                break;
            case "list":
                str = "list";
                break;
            case "string.":
                str = "string";
                break;
        }

        return str;
    }
    #endregion

    /// <summary>
    /// 清除生成的代码文件
    /// </summary>
    public abstract void DeleteALLTable();

}