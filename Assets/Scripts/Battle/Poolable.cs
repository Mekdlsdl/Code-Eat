using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Poolable : MonoBehaviour
{
    public IObjectPool<Poolable> pool { get; set; }

}
