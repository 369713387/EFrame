using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame
{
    public static class TimeStrUtil
    {

        /// <summary>
        /// 扩展方法将float转换成时间格式
        /// XX小时X分X秒
        /// </summary>
        /// <param name="value">时间（秒）</param>
        /// <returns></returns>
        public static string ToTimeFormat_01(this float value)
        {
            string result = "";

            int time = System.Convert.ToInt32(value);

            result = DoTimeFormat_01(time);

            return result;
        }

        /// <summary>
        /// 扩展方法将float转换成时间格式
        /// XX小时X分X秒
        /// </summary>
        /// <param name="value">时间（秒）</param>
        /// <returns></returns>
        public static string ToTimeFormat_01(this int value)
        {
            string result = "";

            result = DoTimeFormat_01(value);

            return result;
        }

        /// <summary>
        /// 具体的执行方法
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string DoTimeFormat_01(int time)
        {
            string result = "";

            if (time <= 0)
            {
                return result;
            }

            int seconds = 0;
            int minutes = 0;
            int hours = 0;

            //取得小时
            if (time > 3600)
            {
                hours = time / 3600;
                time -= hours * 3600;

                result += string.Format("code_time_hours".ToLanguageText()
                        , hours);
            }

            //取得分钟
            if (time > 60)
            {
                minutes = time / 60;
                time -= minutes * 60;

                result += string.Format("code_time_minutes".ToLanguageText()
                            , minutes);
            }

            seconds = time;

            //取得秒
            if (seconds > 0)
            {
                result += string.Format("code_time_seconds".ToLanguageText()
                            , seconds);
            }

            return result;
        }


        /// <summary>
        /// 扩展方法将float转换成时间格式
        /// XX:XX:XX
        /// </summary>
        /// <param name="value">时间（秒）</param>
        /// <returns></returns>
        public static string ToTimeFormat_02(this float value)
        {
            string result = "";

            int time = System.Convert.ToInt32(value);

            result = DoTimeFormat_02(time);

            return result;
        }

        /// <summary>
        /// 扩展方法将float转换成时间格式
        /// XX小时X分X秒
        /// </summary>
        /// <param name="value">时间（秒）</param>
        /// <returns></returns>
        public static string ToTimeFormat_02(this int value)
        {
            string result = "";

            result = DoTimeFormat_02(value);

            return result;
        }

        /// <summary>
        /// 具体的执行方法 格式 XX:XX:XX
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string DoTimeFormat_02(int time)
        {
            string result = "";

            if (time <= 0)
            {
                return result;
            }

            int seconds = 0;
            int minutes = 0;
            int hours = 0;

            //取得小时
            if (time > 3600)
            {
                hours = time / 3600;
                time -= hours * 3600;
            }

            //取得分钟
            if (time > 60)
            {
                minutes = time / 60;
                time -= minutes * 60;
            }

            seconds = time;

            result = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            return result;
        }


    }
}


