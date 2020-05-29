// ========================================================
// Des：
// Autor：Polaris 
// CreateTime：2020/02/17 17:57:40
// 版 本：v 1.0.0
// Copyright ：Polaris 
// ========================================================
using UnityEngine;

static public class UnityEngineExtensions
{
    /// <summary>
    /// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
    /// </summary>
    /// <typeparam name="T">The type of Component to return.</typeparam>
    /// <param name="gameObject">The GameObject this Component is attached to.</param>
    /// <returns>Component</returns>
    static public T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T result = gameObject.GetComponent<T>();
        if (result == null)
        {
            result = gameObject.AddComponent<T>();
        }

        return result;
    }
}