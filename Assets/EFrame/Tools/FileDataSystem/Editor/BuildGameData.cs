// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/08/05 14:13:29
// 版 本：v 1.0.0
// Copyright ：SmartMelon 
// ========================================================
using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildGameData : BuildTool
{
    #region CreateData生成数据文件
    /// <summary>
    /// 生成加密后的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dt"></param>
    protected override void CreateData(string path, DataTable dt, bool ifCreateCode = true)
    {
        //数据格式 行数 列数 二维数组每项的值 这里不做判断 都用string存储

        string filePath = path.Substring(0, path.LastIndexOf('/') + 1);
        string fileFullName = path.Substring(path.LastIndexOf('/') + 1);
        string fileName = fileFullName.Substring(0, fileFullName.LastIndexOf('.'));

        //指定文件夹目录
        string dir = filePath.Substring(0, filePath.LastIndexOf('/'));
        dir = dir.Substring(dir.LastIndexOf('/') + 1) + "/";

        if (dir == "Excel/")
        {
            dir = "";
        }

        byte[] buffer = null;
        string[,] dataArr = null;

        using (Game_MemoryStream ms = new Game_MemoryStream())
        {
            int row = dt.Rows.Count;
            int columns = dt.Columns.Count;

            dataArr = new string[columns, 3];

            ms.WriteInt(row);
            ms.WriteInt(columns);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i < 3)
                    {
                        dataArr[j, i] = dt.Rows[i][j].ToString().Trim();
                    }

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
        string buildPath = dataPath + "/StreamingAssets/AutoCreate/" + dir;

        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        //Debug.Log(fileName + "  写入目录：" + buildPath);

        FileStream fs = new FileStream(string.Format("{0}{1}", buildPath, fileName + ".data"), FileMode.Create);
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();

        if(ifCreateCode)
        {
            string codePath = dataPath + "/Scripts/FileDataSystem/Data/";
            CreateEntity(codePath, fileName, dir, dataArr);
            CreateDBModel(codePath, fileName, dir, dataArr);
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
        string buildPath = dataPath + "/StreamingAssets/AutoCreate";
        string codePath = dataPath + "/Scripts/FileDataSystem/Data/LocalData/Create";
        string buildDicPath = dataPath + "/StreamingAssets/AutoLanguage";
        string codeDicPath = dataPath + "/Scripts/FileDataSystem/MultiLanguage/LocalData/Create";

        if (Directory.Exists(buildPath))
        {
            Directory.Delete(buildPath, true);

            Debug.Log("清除数据表完毕");
        }

        if (Directory.Exists(buildDicPath))
        {
            Directory.Delete(buildDicPath, true);

            Debug.Log("清除Dic数据表完毕");
        }

        if (Directory.Exists(codePath))
        {
            Directory.Delete(codePath, true);

            Debug.Log("清除数据结构代码完毕");
        }
        if (Directory.Exists(codeDicPath))
        {
            Directory.Delete(codeDicPath, true);

            Debug.Log("清除Dic结构代码完毕");
        }
    }
    #endregion

    #region CreateEntity创建实体脚本代码
    /// <summary>
    /// 创建实体脚本代码
    /// </summary>
    public void CreateEntity(string filePath, string fileName, string dir, string[,] dataArr)
    {
        if (dataArr == null) return;

        string path = string.Format("{0}Create/Entity/" + dir, filePath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //if (!Directory.Exists(string.Format("{0}CreateLua", filePath)))
        //{
        //    Directory.CreateDirectory(string.Format("{0}CreateLua", filePath));
        //}

        List<string> m_FieldNameList = new List<string>();

        StringBuilder sbr = new StringBuilder();
        sbr.Append("// ========================================================\r\n");
        sbr.Append("// Des：自动生成请勿修改\r\n");
        sbr.Append("// Autor：Yifun \r\n");
        sbr.AppendFormat("//CreateTime：{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append("//版 本：v 1.0.0\r\n");
        sbr.Append("//===================================================\r\n");
        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");
        sbr.Append("\r\n");

        sbr.Append("namespace EFrame\r\n");
        sbr.Append("{\r\n");

        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}实体\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0} : AbstractEntity\r\n", fileName);
        sbr.Append("{\r\n");

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            if (i == 0) continue;

            if (dataArr[i, 0] == "null" || dataArr[i, 0] == "") continue;

            if (!dataArr[i, 1].Contains("list"))
            {
                sbr.Append("    /// <summary>\r\n");
                sbr.AppendFormat("    /// {0}\r\n", dataArr[i, 2]);
                sbr.Append("    /// </summary>\r\n");
                sbr.AppendFormat("    public {0} {1} {{ get; set; }}\r\n", GetTypeName(dataArr[i, 1]), dataArr[i, 0]);
                sbr.Append("\r\n");
            }
            else
            {

                string name = dataArr[i, 0];
                string type = dataArr[i, 1];

                string listName = name.Substring(0, name.LastIndexOf('_'));
                string listType = type.Substring(type.LastIndexOf('_') + 1, type.LastIndexOf('.') - type.LastIndexOf('_') - 1);

                if (m_FieldNameList.Contains(listName))
                {
                    continue;
                }
                else
                {
                    sbr.Append("    /// <summary>\r\n");
                    sbr.AppendFormat("    /// {0}\r\n", dataArr[i, 2]);
                    sbr.Append("    /// </summary>\r\n");
                    sbr.AppendFormat("    public List<{0}> {1} {{ get; set; }}\r\n", GetTypeName(listType), listName);
                    sbr.Append("\r\n");

                    m_FieldNameList.Add(listName);
                }
            }

        }

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

        ////=======================创建Lua的实体
        //sbr.Clear();

        //sbr.AppendFormat("{0}Entity = {{ ", fileName);

        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{

        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\"", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0", dataArr[i, 0]);
        //        }
        //    }
        //    else
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\", ", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0, ", dataArr[i, 0]);
        //        }
        //    }
        //}
        //sbr.Append(" }\r\n");

        //sbr.Append("\r\n");
        //sbr.Append("--这句是重定义元表的索引，就是说有了这句，这个才是一个类\r\n");
        //sbr.AppendFormat("{0}Entity.__index = {0}Entity;\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}Entity.New(", fileName);
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        sbr.AppendFormat("{0}", dataArr[i, 0]);
        //    }
        //    else
        //    {
        //        sbr.AppendFormat("{0}, ", dataArr[i, 0]);
        //    }
        //}
        //sbr.Append(")\r\n");
        //sbr.Append("    local self = { }; --初始化self\r\n");
        //sbr.Append("");
        //sbr.AppendFormat("    setmetatable(self, {0}Entity); --将self的元表设定为Class\r\n", fileName);
        //sbr.Append("\r\n");
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    sbr.AppendFormat("    self.{0} = {0};\r\n", dataArr[i, 0]);
        //}
        //sbr.Append("\r\n");
        //sbr.Append("    return self;\r\n");
        //sbr.Append("end");

        //using (FileStream fs = new FileStream(string.Format("{0}CreateLua/{1}Entity.lua", filePath, fileName), FileMode.Create))
        //{
        //    using (StreamWriter sw = new StreamWriter(fs))
        //    {
        //        sw.Write(sbr.ToString());
        //    }
        //}
    }
    #endregion

    #region CreateDBModel创建数据管理类代码
    /// <summary>
    /// 创建数据管理类代码
    /// </summary>
    public void CreateDBModel(string filePath, string fileName, string dir, string[,] dataArr)
    {
        if (dataArr == null) return;

        string path = string.Format("{0}Create/DBModel/" + dir, filePath);
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
        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");
        sbr.Append("using System;\r\n");
        sbr.Append("\r\n");

        sbr.Append("namespace EFrame\r\n");
        sbr.Append("{\r\n");

        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}数据管理\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0}DBModel : AbstractDBModel<{0}DBModel, {0}>\r\n", fileName);
        sbr.Append("{\r\n");
        sbr.Append("    /// <summary>\r\n");
        sbr.Append("    /// 文件名称\r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.AppendFormat("    protected override string FileName {{ get {{ return \"{0}.data\"; }} }}\r\n", dir + fileName);
        sbr.Append("\r\n");
        sbr.Append("    /// <summary>\r\n");
        sbr.Append("    /// 创建实体\r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.Append("    /// <param name=\"parse\"></param>\r\n");
        sbr.Append("    /// <returns></returns>\r\n");
        sbr.AppendFormat("    protected override {0} MakeEntity(GameDataTableParser parse)\r\n", fileName);
        sbr.Append("    {\r\n");
        sbr.AppendFormat("        {0} entity = new {0}();\r\n", fileName);

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            if (dataArr[i, 0] == "null" || dataArr[i, 0] == "") continue;

            if (!dataArr[i, 1].Contains("list"))
            {
                sbr.AppendFormat("        entity.{0} = parse.GetFieldValue(\"{0}\"){1};\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
            }
            else
            {

                string name = dataArr[i, 0];
                string type = dataArr[i, 1];

                string listName = name.Substring(0, name.LastIndexOf('_'));
                string listType = type.Substring(type.LastIndexOf('_') + 1, type.LastIndexOf('.') - type.LastIndexOf('_') - 1);
                string listCount = type.Substring(type.LastIndexOf('.') + 1);

                if (m_FieldNameList.Contains(listName))
                {
                    continue;
                }
                else
                {
                    sbr.AppendFormat("        entity.{0} = new List<{1}>();\r\n", listName, GetTypeName(listType));
                    sbr.AppendFormat("        for(int i = 0;i< {0};i++)\r\n", listCount);
                    sbr.Append("        {\r\n");
                    sbr.AppendFormat("            entity.{0}.Add(parse.GetFieldValue(\"{0}_\" + i){1});\r\n", listName, ChangeTypeName(listType));
                    sbr.Append("        }\r\n");

                    m_FieldNameList.Add(listName);
                }
            }
        }
        sbr.Append("        return entity;\r\n");
        sbr.Append("    }\r\n");
        sbr.Append("}\r\n");
        sbr.Append("}\r\n");

        string codefilePath = path + string.Format("{0}DBModel.cs", fileName);


        using (FileStream fs = new FileStream(codefilePath, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }

        ////===============生成lua的DBModel
        //sbr.Clear();
        //sbr.Append("");

        //sbr.AppendFormat("require \"Download/XLuaLogic/Data/Create/{0}Entity\"\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.Append("--数据访问\r\n");
        //sbr.AppendFormat("{0}DBModel = {{ }}\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.AppendFormat("local this = {0}DBModel;\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.AppendFormat("local {0}Table = {{ }}; --定义表格\r\n", fileName.ToLower());
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}DBModel.New()\r\n", fileName);
        //sbr.Append("    return this;\r\n");
        //sbr.Append("end\r\n");
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}DBModel.Init()\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.Append("    --这里从C#代码中获取一个数组\r\n");
        //sbr.Append("\r\n");
        //sbr.AppendFormat("    local gameDataTable = CS.LuaHelper.Instance:GetData(\"{0}.data\");\r\n", fileName);
        //sbr.Append("");
        //sbr.Append("    --表格的前三行是表头 所以获取数据时候 要从 3 开始\r\n");
        //sbr.Append("    --print(\"行数\"..gameDataTable.Row);\r\n");
        //sbr.Append("    --print(\"列数\"..gameDataTable.Column);\r\n");
        //sbr.Append("\r\n");
        //sbr.Append("    for i = 3, gameDataTable.Row - 1, 1 do\r\n");
        //sbr.AppendFormat("        {0}Table[#{0}Table+1] = {1}Entity.New( ", fileName.ToLower(), fileName);

        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("gameDataTable.Data[i][{0}]", i);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("tonumber(gameDataTable.Data[i][{0}])", i);
        //        }
        //    }
        //    else
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("gameDataTable.Data[i][{0}], ", i);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("tonumber(gameDataTable.Data[i][{0}]), ", i);
        //        }
        //    }
        //}
        //sbr.Append(" );\r\n");
        //sbr.Append("    end\r\n");
        //sbr.Append("\r\n");
        //sbr.Append("end\r\n");
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}DBModel.GetList()\r\n", fileName);
        //sbr.AppendFormat("    return {0}Table;\r\n", fileName.ToLower());
        //sbr.Append("end");
        //sbr.Append("\r\n");
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}DBModel.GetEntity(id)\r\n", fileName);
        //sbr.AppendFormat("    local ret = nil;\r\n");
        //sbr.AppendFormat("    for i = 1, #{0}Table, 1 do\r\n", fileName.ToLower());
        //sbr.AppendFormat("        if ({0}Table[i].Id == id) then\r\n", fileName.ToLower());
        //sbr.AppendFormat("            ret = {0}Table[i];\r\n", fileName.ToLower());
        //sbr.AppendFormat("            break;\r\n");
        //sbr.AppendFormat("        end\r\n");
        //sbr.AppendFormat("    end\r\n");
        //sbr.AppendFormat("    return ret;\r\n");
        //sbr.AppendFormat("end");

        //using (FileStream fs = new FileStream(string.Format("{0}CreateLua/{1}DBModel.lua", filePath, fileName), FileMode.Create))
        //{
        //    using (StreamWriter sw = new StreamWriter(fs))
        //    {
        //        sw.Write(sbr.ToString());
        //    }
        //}
    }
    #endregion

    #region 生成数据文件的代码的 扩展代码
    /// <summary>
    /// CreateEntityExt生成数据文件的扩展代码
    /// </summary>
    /// <param name="path"></param>
    public bool CreateEntityExt(string path)
    {
        string filePath = path.Substring(0, path.LastIndexOf('\\') + 1);
        string fileFullName = path.Substring(path.LastIndexOf('\\') + 1);
        string fileName = fileFullName.Substring(0, fileFullName.LastIndexOf('.'));

        //指定文件夹目录
        string dir = filePath.Substring(0, filePath.LastIndexOf('\\'));
        dir = dir.Substring(dir.LastIndexOf('\\') + 1) + "/";

        if (dir == "Excel/")
        {
            dir = "";
        }




        string dataPath = Application.dataPath;
        string codePath = dataPath + "/Scripts/FileDataSystem/Data/Ext/Entity/";

        string path2 = string.Format("{0}", codePath);
        if (!Directory.Exists(path2))
        {
            Directory.CreateDirectory(path2);
        }


        StringBuilder sbr = new StringBuilder();
        sbr.Append("/* \r\n");
        sbr.Append(" * ********************************************************\r\n");
        sbr.Append(" * Des: 自动生成的默认实例类扩展类\r\n");
        sbr.Append(" * Autor: Jarvis \r\n");
        sbr.AppendFormat(" * CreateTime: {0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append(" * Version: v1.0.0\r\n");
        sbr.Append(" * ********************************************************\r\n");
        sbr.Append(" */ \r\n");

        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");
        sbr.Append("\r\n");
        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0} 的实体类扩展代码\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0} : AbstractEntity\r\n", fileName);
        sbr.Append("{\r\n");
        sbr.Append("\r\n");
        sbr.Append("}\r\n");

        string fileName2 = string.Format("{0}Ext.cs", fileName);
        string codefilePath = path2 + fileName2;
        if (File.Exists(codefilePath))
        {
            Debug.Log(string.Format("已存在{0}, 请删除后重新生成(注意备份原有内容以免丢失)", codefilePath));
            return false;
        }
        using (FileStream fs = new FileStream(codefilePath, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }

        return true;

        ////=======================创建Lua的实体
        //sbr.Clear();

        //sbr.AppendFormat("{0}Entity = {{ ", fileName);

        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{

        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\"", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0", dataArr[i, 0]);
        //        }
        //    }
        //    else
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\", ", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0, ", dataArr[i, 0]);
        //        }
        //    }
        //}
        //sbr.Append(" }\r\n");

        //sbr.Append("\r\n");
        //sbr.Append("--这句是重定义元表的索引，就是说有了这句，这个才是一个类\r\n");
        //sbr.AppendFormat("{0}Entity.__index = {0}Entity;\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}Entity.New(", fileName);
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        sbr.AppendFormat("{0}", dataArr[i, 0]);
        //    }
        //    else
        //    {
        //        sbr.AppendFormat("{0}, ", dataArr[i, 0]);
        //    }
        //}
        //sbr.Append(")\r\n");
        //sbr.Append("    local self = { }; --初始化self\r\n");
        //sbr.Append("");
        //sbr.AppendFormat("    setmetatable(self, {0}Entity); --将self的元表设定为Class\r\n", fileName);
        //sbr.Append("\r\n");
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    sbr.AppendFormat("    self.{0} = {0};\r\n", dataArr[i, 0]);
        //}
        //sbr.Append("\r\n");
        //sbr.Append("    return self;\r\n");
        //sbr.Append("end");

        //using (FileStream fs = new FileStream(string.Format("{0}CreateLua/{1}Entity.lua", filePath, fileName), FileMode.Create))
        //{
        //    using (StreamWriter sw = new StreamWriter(fs))
        //    {
        //        sw.Write(sbr.ToString());
        //    }
        //}


    }
    #endregion

    #region CreateDBModel创建数据管理类代码的 扩展代码
    /// <summary>
    /// CreateDBModelExt
    /// </summary>
    /// <param name="path"></param>
    public bool CreateDBModelExt(string path)
    {
        string filePath = path.Substring(0, path.LastIndexOf('\\') + 1);
        string fileFullName = path.Substring(path.LastIndexOf('\\') + 1);
        string fileName = fileFullName.Substring(0, fileFullName.LastIndexOf('.'));

        //指定文件夹目录
        string dir = filePath.Substring(0, filePath.LastIndexOf('\\'));
        dir = dir.Substring(dir.LastIndexOf('\\') + 1) + "/";

        if (dir == "Excel/")
        {
            dir = "";
        }




        string dataPath = Application.dataPath;
        string codePath = dataPath + "/Scripts/FileDataSystem/Data/Ext/DBModel/";

        string path2 = string.Format("{0}", codePath);
        if (!Directory.Exists(path2))
        {
            Directory.CreateDirectory(path2);
        }


        StringBuilder sbr = new StringBuilder();
        sbr.Append("/* \r\n");
        sbr.Append(" * ********************************************************\r\n");
        sbr.Append(" * Des: 自动生成的默认数据管理类扩展类\r\n");
        sbr.Append(" * Autor: Jarvis \r\n");
        sbr.AppendFormat(" * CreateTime: {0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append(" * Version: v1.0.0\r\n");
        sbr.Append(" * ********************************************************\r\n");
        sbr.Append(" */ \r\n");

        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");
        sbr.Append("\r\n");
        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}DBModel 的数据管理类扩展代码\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0}DBModel : AbstractDBModel<{0}DBModel, {0}>\r\n", fileName);
        sbr.Append("{\r\n");
        sbr.Append("\r\n");
        sbr.Append("}\r\n");

        string fileName2 = string.Format("{0}DBModelExt.cs", fileName);
        string codefilePath = path2 + fileName2;
        if (File.Exists(codefilePath))
        {
            Debug.Log(string.Format("已存在{0}, 请删除后重新生成(注意备份原有内容以免丢失)", codefilePath));
            return false;
        }
        using (FileStream fs = new FileStream(codefilePath, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }

        return true;

        ////=======================创建Lua的实体
        //sbr.Clear();

        //sbr.AppendFormat("{0}Entity = {{ ", fileName);

        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{

        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\"", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0", dataArr[i, 0]);
        //        }
        //    }
        //    else
        //    {
        //        if (dataArr[i, 1].Equals("string", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            sbr.AppendFormat("{0} = \"\", ", dataArr[i, 0]);
        //        }
        //        else
        //        {
        //            sbr.AppendFormat("{0} = 0, ", dataArr[i, 0]);
        //        }
        //    }
        //}
        //sbr.Append(" }\r\n");

        //sbr.Append("\r\n");
        //sbr.Append("--这句是重定义元表的索引，就是说有了这句，这个才是一个类\r\n");
        //sbr.AppendFormat("{0}Entity.__index = {0}Entity;\r\n", fileName);
        //sbr.Append("\r\n");
        //sbr.AppendFormat("function {0}Entity.New(", fileName);
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    if (i == dataArr.GetLength(0) - 1)
        //    {
        //        sbr.AppendFormat("{0}", dataArr[i, 0]);
        //    }
        //    else
        //    {
        //        sbr.AppendFormat("{0}, ", dataArr[i, 0]);
        //    }
        //}
        //sbr.Append(")\r\n");
        //sbr.Append("    local self = { }; --初始化self\r\n");
        //sbr.Append("");
        //sbr.AppendFormat("    setmetatable(self, {0}Entity); --将self的元表设定为Class\r\n", fileName);
        //sbr.Append("\r\n");
        //for (int i = 0; i < dataArr.GetLength(0); i++)
        //{
        //    sbr.AppendFormat("    self.{0} = {0};\r\n", dataArr[i, 0]);
        //}
        //sbr.Append("\r\n");
        //sbr.Append("    return self;\r\n");
        //sbr.Append("end");

        //using (FileStream fs = new FileStream(string.Format("{0}CreateLua/{1}Entity.lua", filePath, fileName), FileMode.Create))
        //{
        //    using (StreamWriter sw = new StreamWriter(fs))
        //    {
        //        sw.Write(sbr.ToString());
        //    }
        //}


    }
    #endregion

}
