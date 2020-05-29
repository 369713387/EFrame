// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/08/05 14:46:48
// 版 本：v 1.0.0
// Copyright ：SmartMelon 
// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildChannelConfig : BuildTool
{
    #region CreateData生成数据文件
    /// <summary>
    /// 生成加密后的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dt"></param>
    protected override void CreateData(string path, DataTable dt, bool ifCreateCode = true)
    {
        DeleteALLTable();

        //数据格式 行数 列数 二维数组每项的值 这里不做判断 都用string存储

        string filePath = path.Substring(0, path.LastIndexOf('/') + 1);
        string fileFullName = path.Substring(path.LastIndexOf('/') + 1);
        string fileName = fileFullName.Substring(0, fileFullName.LastIndexOf('.'));

        //指定文件夹目录
        string dir = "";

        byte[] buffer = null;
        string[,] dataArr = null;

        using (Game_MemoryStream ms = new Game_MemoryStream())
        {
            int row = dt.Rows.Count;
            int columns = dt.Columns.Count;

            dataArr = new string[columns, row];

            ms.WriteInt(row);
            ms.WriteInt(columns);
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < columns; j++)
                {
                    dataArr[j, i] = dt.Rows[i][j].ToString().Trim();

                    ms.WriteUTF8String(dt.Rows[i][j].ToString().Trim());
                }
            }
            buffer = ms.ToArray();
        }

        //------------------
        //第1步：xor加密
        //------------------
        int iScaleLen = xorScale.Length;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(buffer[i] ^ xorScale[i % iScaleLen]);
        }

        //------------------
        //第2步：压缩
        //------------------
        //压缩后的字节流
        buffer = ZlibHelper.CompressBytes(buffer);

        //------------------
        //第3步：写入文件
        //------------------
        string dataPath = Application.dataPath;
        string buildPath = dataPath + "/StreamingAssets/AutoConfig/" + dir;

        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        //Debug.Log(fileName + "  写入目录：" + buildPath);

        FileStream fs = new FileStream(string.Format("{0}{1}", buildPath, fileName + ".data"), FileMode.Create);
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();

        if (ifCreateCode)
        {
            string codePath = dataPath + "/ThirdPlugins/Config/Create/";
            CreatePackage(codePath, fileName, dir, dataArr); 
        }
    }
    #endregion

    #region DeleteALLTable清除生成的代码文件
    /// <summary>
    /// 清除生成的代码文件
    /// </summary>
    public override void DeleteALLTable()
    {
        string dataPath = Application.dataPath;
        string buildPath = dataPath + "/StreamingAssets/AutoConfig";

        if (Directory.Exists(buildPath))
        {
            Directory.Delete(buildPath, true);

            Debug.Log("清除数据表完毕");
        }
    }
    #endregion

    #region CreatePackage创建语言包脚本代码
    /// <summary>
    /// 创建实体脚本代码
    /// </summary>
    public void CreatePackage(string filePath, string fileName, string dir, string[,] dataArr)
    {
        if (dataArr == null) return;

        string path = string.Format("{0}" + dir, filePath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        List<string> m_FieldNameList = new List<string>();

        StringBuilder sbr = new StringBuilder();
        sbr.Append("// ========================================================\r\n");
        sbr.Append("// Des：自动生成请勿修改\r\n");
        sbr.Append("// Autor：Yifun \r\n");
        sbr.AppendFormat("//CreateTime：{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append("//版 本：v 1.0.0\r\n");
        sbr.Append("//===================================================\r\n");

        sbr.Append("using ReadExcel;\r\n");
        sbr.Append("using System;\r\n");
        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");        
        sbr.Append("\r\n");

        sbr.Append("namespace EFrame\r\n");
        sbr.Append("{\r\n");


        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}枚举值\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public enum {0}Type\r\n", fileName);
        sbr.Append("{\r\n");

        for (int i = 3; i < dataArr.GetLength(1); i++)
        {
            if (dataArr[0, i].ToString().Trim() != null && dataArr[0, i].ToString().Trim() != ""
                       && !dataArr[0, i].ToString().Contains("//"))
            {
                sbr.AppendFormat("    {0},\r\n", dataArr[0, i]);
            }
        }

        sbr.Append("}\r\n");


        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// 生成{0}Config数据包类\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0} : AbstractConfig<{0}>\r\n", fileName);
        sbr.Append("{\r\n");

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            if (i == 0) continue;

            sbr.Append("    /// <summary>\r\n");
            sbr.AppendFormat("    /// {0}\r\n", dataArr[i, 2]);
            sbr.Append("    /// </summary>\r\n");
            sbr.AppendFormat("    public Dictionary<{0}Type, {1}> {2} = new Dictionary<{0}Type, {1}> ();\r\n", fileName, GetTypeName(dataArr[i, 1]), dataArr[i, 0]);
            sbr.Append("\r\n");
        }

        sbr.Append("    /// <summary>\r\n");
        sbr.Append("    /// 文件名称\r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.AppendFormat("    protected override string FileName {{ get {{ return \"{0}.data\"; }} }}\r\n", dir + fileName);
        sbr.Append("\r\n");


        sbr.Append("    /// <summary>\r\n");
        sbr.AppendFormat("    /// 创建字典内容 \r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.AppendFormat("    protected override void MakeDic(GameDataTableParser parse)\r\n");
        sbr.Append("    {\r\n");
        sbr.AppendFormat("        if (parse.GetFieldValue(\"{0}\") != \"\" && !parse.GetFieldValue(\"{0}\").Contains(\"//\"))\r\n", dataArr[0, 0]);
        sbr.Append("        {\r\n");
        sbr.AppendFormat("            string key = parse.GetFieldValue(\"{0}\");\r\n", dataArr[0, 0]);
        sbr.AppendFormat("            {0}Type type = ({0}Type)Enum.Parse(typeof({0}Type), key);\r\n", fileName);
        sbr.AppendFormat("            if ({0}Type.IsDefined(typeof({0}Type), key))\r\n", fileName);
        sbr.Append("            {\r\n");

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            if (i == 0) continue;

            sbr.AppendFormat("                {0}.Add(type, parse.GetFieldValue(\"{0}\"){1});\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
        }
        sbr.Append("            }\r\n");
        sbr.Append("        }\r\n");
        sbr.Append("    }\r\n");
        sbr.Append("\r\n");


        sbr.Append("}\r\n");

        sbr.Append("}\r\n");

        string codefilePath = path + string.Format("{0}.cs", fileName);

        using (FileStream fs = new FileStream(codefilePath, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }
    }
    #endregion
}
