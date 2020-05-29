// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/08/01 14:13:12
// 版 本：v 1.0.0
// Copyright ：SmartMelon 
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadExcel;

/// <summary>
/// 数据管理基类
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="P"></typeparam>
public abstract class AbstractDBModel<T,P>
    where T:class,new()
    where P:AbstractEntity
{
    private List<P> m_lst;
    protected Dictionary<int, P> m_dic;

    public AbstractDBModel()
    {
        m_lst = new List<P>();
        m_dic = new Dictionary<int, P>();

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
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected abstract P MakeEntity(GameDataTableParser parse);
    #endregion

    #region 加载数据loadData
    /// <summary>
    /// 加载数据
    /// </summary>
    private void LoadData()
    {
        using (GameDataTableParser parse = new GameDataTableParser(string.Format(Application.streamingAssetsPath + "/AutoCreate/{0}", FileName)))
        {
            while (!parse.Eof)
            {
                //创建实体
                P p = MakeEntity(parse);
                m_lst.Add(p);
                m_dic[p.ID] = p;
                parse.Next();
            }
        }
    }
    #endregion

    #region 获取实体集合
    /// <summary>
    /// 根据实体集合
    /// </summary>
    /// <returns></returns>
    public List<P> GetList()
    {
        return m_lst;
    }
    #endregion

    #region 根据编号获取实体
    /// <summary>
    /// 根据商品编号查询商品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public P Get(int id)
    {
        if (m_dic.ContainsKey(id))
        {

            return m_dic[id];
        }
        return null;
    }
    #endregion
}
