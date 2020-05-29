// ========================================================
// Des：数据表格自动加密，数据脚本自动生成
// Autor：Polaris 
// CreateTime：2019/08/01 15:43:43
// 版 本：v 1.0.0
// Copyright ：SmartMelon 
// ========================================================
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;
using System.Text;
using System.Collections.Generic;

public class GameDataAutoBuild : Editor {

    static string FileNames = "";

    //异或因子
    static byte[] xorScale = new byte[] { 45, 66, 38, 55, 23, 254, 9, 165, 90, 19, 41, 45, 201, 58, 55, 37, 254, 185, 165, 169, 19, 171 };//.data文件的xor加解密因子

    #region 自动生成目录下所有数据表
    [MenuItem("DataBuild/生成代码和数据（程序常用）/目录ALL", false, 11)]
    static void BuildALLTable()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在："+ dir);
                return;
            }

            string filePath = FileNames.Substring(0, FileNames.LastIndexOf('\\') + 1);

            if (Directory.Exists(filePath))
            {
                DirectoryInfo direction = new DirectoryInfo(filePath);
                FileInfo[] files = direction.GetFiles("*.xlsx", SearchOption.AllDirectories);

                //Debug.Log(files.Length);

                if (files.Length > 0)
                {
                    string[] arr = new string[files.Length];
                    BuildGameData m_BuildGameData = new BuildGameData();

                    for (int i = 0; i < files.Length; i++)
                    {
                        arr[i] = files[i].FullName;
                        //Debug.Log("Name:" + files[i].FullName);  //打印出来这个文件夹下的所有文件                                   ;  
                    }

                    m_BuildGameData.Read_ALLData(arr);

                    Debug.Log(string.Format("生成{0}张数据表与构建代码，完毕！", files.Length));
                }
            }
        }
    }
    #endregion

    #region 自动生成单张数据表
    [MenuItem("DataBuild/生成代码和数据（程序常用）/单张表", false, 12)]
    static void BuildOneTable()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildGameData m_BuildGameData = new BuildGameData();

            m_BuildGameData.ReadData(FileNames);

            Debug.Log(string.Format("生成{0}数据表与构建代码，完毕！", openFileName.fileTitle));
        }
    }
    #endregion

    #region 自动生成目录下所有数据表
    [MenuItem("DataBuild/生成数据（策划常用）/目录ALL", false, 11)]
    static void BuildALLTable_D()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            string filePath = FileNames.Substring(0, FileNames.LastIndexOf('\\') + 1);

            if (Directory.Exists(filePath))
            {
                DirectoryInfo direction = new DirectoryInfo(filePath);
                FileInfo[] files = direction.GetFiles("*.xlsx", SearchOption.AllDirectories);

                //Debug.Log(files.Length);

                if (files.Length > 0)
                {
                    string[] arr = new string[files.Length];
                    BuildGameData m_BuildGameData = new BuildGameData();

                    for (int i = 0; i < files.Length; i++)
                    {
                        arr[i] = files[i].FullName;
                        //Debug.Log("Name:" + files[i].FullName);  //打印出来这个文件夹下的所有文件                                   ;  
                    }

                    m_BuildGameData.Read_ALLData(arr,false);

                    Debug.Log(string.Format("生成{0}张数据表与构建代码，完毕！", files.Length));
                }
            }
        }
    }
    #endregion

    #region 自动生成单张数据表
    [MenuItem("DataBuild/生成数据（策划常用）/单张表", false, 12)]
    static void BuildOneTable_D()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildGameData m_BuildGameData = new BuildGameData();

            m_BuildGameData.ReadData(FileNames, false);

            Debug.Log(string.Format("生成{0}数据表与构建代码，完毕！", openFileName.fileTitle));
        }
    }
    #endregion

    #region 自动生成单张数据表Ext
    [MenuItem("DataBuild/生成数据及扩展代码（程序常用）/EntityExt", false, 11)]
    static void BuildOneTableEntityExt()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildGameData m_BuildGameData = new BuildGameData();
            m_BuildGameData.ReadData(FileNames);
            Debug.Log(string.Format("生成{0}数据表与构建代码，完毕！", openFileName.fileTitle));
            if (m_BuildGameData.CreateEntityExt(FileNames))
            {
                Debug.Log(string.Format("生成{0}数据表实例的 扩展代码，完毕！", openFileName.fileTitle));
            }
        }
    }
    #endregion

    #region 自动生成单张数据表Ext
    [MenuItem("DataBuild/生成数据及扩展代码（程序常用）/DBModelExt", false, 12)]
    static void BuildOneTableDBModelExt()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildGameData m_BuildGameData = new BuildGameData();
            m_BuildGameData.ReadData(FileNames);
            Debug.Log(string.Format("生成{0}数据表与构建代码，完毕！", openFileName.fileTitle));
            if (m_BuildGameData.CreateDBModelExt(FileNames))
            {
                Debug.Log(string.Format("生成{0}数据表数据管理类的 扩展代码，完毕！", openFileName.fileTitle));
            }
        }
    }
    #endregion

    #region 自动生成单张数据表Ext
    [MenuItem("DataBuild/生成数据及扩展代码（程序常用）/EntityExt And DBModelExt", false, 13)]
    static void BuildOneTableEntityAndDBModelExt()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Excel";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildGameData m_BuildGameData = new BuildGameData();
            m_BuildGameData.ReadData(FileNames);
            Debug.Log(string.Format("生成{0}数据表与构建代码，完毕！", openFileName.fileTitle));
            if (m_BuildGameData.CreateEntityExt(FileNames))
            {
                Debug.Log(string.Format("生成{0}数据表实例的 扩展代码，完毕！", openFileName.fileTitle));
            }
            if (m_BuildGameData.CreateDBModelExt(FileNames))
            {
                Debug.Log(string.Format("生成{0}数据表数据管理类的 扩展代码，完毕！", openFileName.fileTitle));
            }
        }
    }
    #endregion










    #region 调出系统选择文件的弹窗
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    #endregion

    [MenuItem("DataBuild/生成代码和数据（程序常用）/Dic(语言包))", false, 31)]
    static void BuildLanguagePackage()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Language";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildLanguagePack m_BuildLanguagePack = new BuildLanguagePack();

            m_BuildLanguagePack.ReadData(FileNames);

            Debug.Log(string.Format("生成{0}语言包构建完毕！", openFileName.fileTitle));
        }
    }

    [MenuItem("DataBuild/生成数据（策划常用）/Dic(语言包)", false, 31)]
    static void BuildLanguagePackage_D()
    {
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/Language";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildLanguagePack m_BuildLanguagePack = new BuildLanguagePack();

            m_BuildLanguagePack.ReadData(FileNames,false);

            Debug.Log(string.Format("生成{0}语言包构建完毕！", openFileName.fileTitle));
        }
    }

    [MenuItem("DataBuild/重新生成渠道配置参数)", false, 41)]
    static void BuildChannelConfig()
    { 
        string dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        dir = dir + "www/ChannelConfig";

        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Excel文件(*.xlsx)\0*.xlsx";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = dir;//默认路径
        openFileName.title = "窗口标题";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (GetOpenFileName(openFileName))
        {
            FileNames = openFileName.file;

            if (!FileNames.Replace('\\', '/').Contains(dir))
            {
                Debug.Log("请将数据文件放在：" + dir);
                return;
            }

            BuildChannelConfig m_Build = new BuildChannelConfig();

            m_Build.ReadData(FileNames);

            Debug.Log(string.Format("生成{0}渠道配置完毕！", openFileName.fileTitle));
        }
    }

    #region 清除自动生成的文件与代码架构
    [MenuItem("DataBuild/清除所有文件与代码", false, 91)]
    static void DeleteALLTable()
    {
        BuildGameData m_BuildGameData = new BuildGameData();
        m_BuildGameData.DeleteALLTable();
    }
    #endregion

}

#region 系统窗口类
/// <summary>
/// 系统窗口类
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;

}
#endregion
