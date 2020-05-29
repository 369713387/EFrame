using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EFrame
{
    public static class StringUtil
    {
        /// <summary>
        /// 扩展方法将string转换成int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            int temp = 0;
            int.TryParse(str, out temp);
            return temp;
        }

        /// <summary>
        /// 扩展方法将string转换成short
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static short ToShort(this string str)
        {
            short temp = 0;
            short.TryParse(str, out temp);
            return temp;
        }

        /// <summary>
        /// 扩展方法将string转换成long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            long temp = 0;
            long.TryParse(str, out temp);
            return temp;
        }

        /// <summary>
        /// 扩展方法将string转换成float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            float temp = 0;
            float.TryParse(str, out temp);
            return temp;
        }

        /// <summary>
        /// 扩展方法将string转换成double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double temp = 0;
            double.TryParse(str, out temp);
            return temp;
        }

        /// <summary>
        /// 扩展方法将string转换成bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            bool temp = false;
            if (str == "yes")
            {
                temp = true;
            }
            else
            {
                temp = false;
            }

            return temp;
        }

        /// <summary>
        /// 扩展方法将double转换成每3位加个逗号整数格式的string
        /// 整数部分的精度是到15位，即到兆，第15位的值会根据第16位来进行四舍五入得出
        /// 小于15位时，个位会根据小数第一位进行四舍五入
        /// </summary>
        /// <param name="pDouble"></param>
        /// <returns></returns>
        public static string NumFormat3(this double pDouble)
        {
            string str = "";
            str = pDouble.ToString("N0");
            return str;
        }

        public static double FToFloor(this float tFloat)
        {
            return System.Math.Floor(tFloat);
        }

        public static double DToFloor(this double tDouble)
        {
            return System.Math.Floor(tDouble);
        }

        /// <summary>
        /// 扩展方法将string反转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnescape(this string str)
        {
            return System.Text.RegularExpressions.Regex.Unescape(str);
        }

        #region 多语言相关
        /// <summary>
        /// 扩展方法将string作为键值，获取语言包中的文本数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLanguageText(this string str)
        {
            return MultiLanguageCtrl.Instance.GetText(str);
        }

        /// <summary>
        /// 扩展方法将string作为键值，获取语言包中的图片
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Sprite ToLanguageImg(this string str)
        {
            return MultiLanguageCtrl.Instance.GetImg(str);
        }

        /// <summary>
        /// 扩展方法将string作为键值，获取语言包中的Mul图片
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Sprite[] ToLanguageMulImg(this string str)
        {
            return MultiLanguageCtrl.Instance.GetMulImg(str);
        }
        #endregion
    }
}


