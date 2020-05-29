using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame
{
    public static class NumberUtil
    {

        /// <summary>
        /// 金钱转单位
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string UFormat(this long num)
        {
            return DoUFormat(num);
        }

        /// <summary>
        /// 金钱转单位
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string UFormat(this double num)
        {
            return DoUFormat(num);
        }

        /// <summary>
        /// 金钱转单位
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string UFormat(this int num)
        {
            return DoUFormat(num);
        }

        /// <summary>
        /// 金钱转单位
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string UFormat(this float num)
        {
            return DoUFormat(num);
        }

        private static string DoUFormat(double _num)
        {
            double num = _num;
            string symbol = "";

            if (_num < 0)
            {
                symbol = "-";
                num = -1 * num;
            }

            if (num >= 1000000000000)
            {
                double _Z = 0;
                _Z = num / 1000000000000f;
                return symbol + _Z.ToString("f1") + "tx_code_Zhao".ToLanguageText();
            }
            else if (num >= 100000000)
            {
                double _Y = 0;
                _Y = num / 100000000f;
                return symbol + _Y.ToString("f1") + "tx_code_Yi".ToLanguageText();
            }
            else if (num >= 10000)
            {
                double _W = 0;
                _W = num / 10000f;
                return symbol + _W.ToString("f1") + "tx_code_Wan".ToLanguageText();
            }
            else
            {
                return symbol + num.ToString();
            }
        }




        public static string ChineseTimesFormat(this int num)
        {
            return DoChineseTimesFormat(num);
        }

        private static string DoChineseTimesFormat(int _num)
        {
            int num = _num;

            switch (num)
            {
                case 2:
                    return "tx_ui_multiple_two".ToLanguageText();
                case 3:
                    return "tx_ui_multiple_three".ToLanguageText();
                case 4:
                    return "tx_ui_multiple_four".ToLanguageText();
                case 5:
                    return "tx_ui_multiple_five".ToLanguageText();
                default:
                    return "tx_ui_multiple_two".ToLanguageText();
            }
        }
    }
}


