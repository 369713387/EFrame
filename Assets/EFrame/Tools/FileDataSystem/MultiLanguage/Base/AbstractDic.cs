using ReadExcel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame
{
    public abstract class AbstractDic<T>
    where T : class, new()
    {
        public AbstractDic()
        {
            LoadData();
        }

        #region 单例
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        #endregion

        #region 需要子类实现的属性和方法
        /// <summary>
        /// 数据文件名称
        /// </summary>
        protected abstract string FileName { get; }

        /// <summary>
        /// 创建字典内容
        /// </summary>
        /// <param name="parse"></param>
        /// <returns></returns>
        protected abstract void MakeDic(GameDataTableParser parse);
        #endregion

        #region 加载数据loadData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            using (GameDataTableParser parse = new GameDataTableParser(string.Format(Application.streamingAssetsPath + "/AutoLanguage/{0}", FileName)))
            {
                while (!parse.Eof)
                {
                    MakeDic(parse);

                    parse.Next();
                }
            }
        }
        #endregion
    }
}


