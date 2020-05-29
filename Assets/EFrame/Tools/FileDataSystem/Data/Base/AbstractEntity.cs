// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2019/08/01 10:05:57
// 版 本：v 1.0.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据实体的基类
/// </summary>
[System.Serializable]
public abstract class AbstractEntity {

    /// <summary>
    /// 编号
    /// </summary>
    public int ID { get; set; }
}
