using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    protected IObjectPool<Poolable> pool;

    [Header("Pool Setting")]
    [SerializeField] private Poolable prefab;
    [SerializeField] private int maxSize;
    

    protected virtual void Awake()
    {
        pool = new ObjectPool<Poolable>(
            CreateObject,
            OnGet,
            OnRelease,
            OnDestroyObject,
            maxSize: maxSize
        );
    }
    
    private Poolable CreateObject()
    {
        var obj = Instantiate(prefab, transform);
        obj.pool = pool;
        return obj;
    }

    private void OnGet(Poolable obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnRelease(Poolable obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Poolable obj)
    {
        Destroy(obj.gameObject);
    }

}
