// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/07/29 22:54:14
// 版 本：v 1.0.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

/// <summary>
/// 游戏数据转换类
/// byte,int,short,long,float,decimal,bool,string
/// </summary>
public class Game_MemoryStream : MemoryStream {

    public Game_MemoryStream()
    {

    }

    public Game_MemoryStream(byte[] buffer):base(buffer)
    {

    }

    #region Short
    /// <summary>
    /// 从流中读取一个short数据
    /// </summary>
    /// <returns></returns>
    public short ReadShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr,0,2);
        return BitConverter.ToInt16(arr,0);
    }

    /// <summary>
    /// 将一个short数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteShort(short value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region UShort
    /// <summary>
    /// 从流中读取一个ushort数据
    /// </summary>
    /// <returns></returns>
    public ushort ReadUShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);
        return BitConverter.ToUInt16(arr, 0);
    }

    /// <summary>
    /// 将一个ushort数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteUShort(ushort value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region BInt
    /// <summary>
    /// 从流中读取一个Bit数据,用作socket通讯中数据类型
    /// </summary>
    /// <returns></returns>
    public int ReadBInt()
    {
        byte[] arr = new byte[1];
        base.Read(arr, 0, 1);
        return arr[0];
    }

    /// <summary>
    /// 将一个int数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteBInt(int value)
    {
        byte[] arr = new byte[1];
        arr[0] = (byte)(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Int
    /// <summary>
    /// 从流中读取一个int数据
    /// </summary>
    /// <returns></returns>
    public int ReadInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToInt32(arr, 0);
    }

    /// <summary>
    /// 将一个int数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteInt(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region UInt
    /// <summary>
    /// 从流中读取一个uint数据
    /// </summary>
    /// <returns></returns>
    public uint ReadUInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToUInt32(arr, 0);
    }

    /// <summary>
    /// 将一个uint数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteUInt(uint value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Long 
    /// <summary>
    /// 从流中读取一个long数据
    /// </summary>
    /// <returns></returns>
    public long ReadLong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToInt64(arr, 0);
    }

    /// <summary>
    /// 将一个long数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteLong(long value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region ULong
    /// <summary>
    /// 从流中读取一个ulong数据
    /// </summary>
    /// <returns></returns>
    public ulong ReadULong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToUInt64(arr, 0);
    }

    /// <summary>
    /// 将一个ulong数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteULong(ulong value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region float 
    /// <summary>
    /// 从流中读取一个float数据
    /// </summary>
    /// <returns></returns>
    public float ReadFloat()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToSingle(arr, 0);
    }

    /// <summary>
    /// 将一个float数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteFloat(float value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Double
    /// <summary>
    /// 从流中读取一个double数据
    /// </summary>
    /// <returns></returns>
    public double ReadDouble()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToDouble(arr, 0);
    }

    /// <summary>
    /// 将一个double数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteDouble(double value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Bool
    /// <summary>
    /// 从流中读取一个bool数据
    /// </summary>
    /// <returns></returns>
    public bool ReadBool()
    {
        return base.ReadByte() == 1;
    }

    /// <summary>
    /// 将一个bool数据写入流中
    /// </summary>
    /// <param name="value"></param>
    public void WriteBool(bool value)
    {
        base.WriteByte((byte)(value == true?1:0));
    }
    #endregion

    #region UTF8 String
    /// <summary>
    /// 从流中读取一个UTF8 String类型，数据中有string长度
    /// </summary>
    /// <returns></returns>
    public string ReadUTF8String()
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];

        base.Read(arr, 0, len);
        return Encoding.UTF8.GetString(arr);
    }

    /// <summary>
    /// 从流中读取一个UTF8 String类型
    /// </summary>
    /// <param name="index">起始位置</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public string ReadUTF8String(int index, int length)
    {
        this.Position = index;
        byte[] arr = new byte[length];

        base.Read(arr, 0, length);
        return Encoding.UTF8.GetString(arr);
    }

    /// <summary>
    /// 将一个UTF8 String写入流
    /// </summary>
    /// <param name="value">数据内容</param>
    /// <param name="writeLen">是否写入数据长度</param>
    public void WriteUTF8String(string value, bool writeLen = true)
    {
        if (value != null)
        {

            byte[] arr = Encoding.UTF8.GetBytes(value);

            if (arr.Length > 65535)
            {
                throw new InvalidCastException("字符串超出了最大限制");
            }

            if (writeLen)
            {
                this.WriteUShort((ushort)arr.Length);
            }
            base.Write(arr, 0, arr.Length);
        }
        else
        {
            this.WriteUShort((ushort)0);
        }
    }
    #endregion
}
