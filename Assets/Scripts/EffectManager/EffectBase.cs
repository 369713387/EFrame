using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour, I_PoolItem
{
    public string res_Path { get { return "Prefab/" + gameObject.name.Substring(0, gameObject.name.Length - 7); } }

    public float time = 1f;

    public void Dispose()
    {
        this.gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(IE_AutoDispose());
    }

    IEnumerator IE_AutoDispose()
    {
        yield return new WaitForSeconds(time);

        EffectManager.Instance.RecycleEffect(this);
    }
}
