using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Poolable
{
    private void Start()
    {
        Destroy(gameObject, 0.8f);
    }
}
