using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("적과 접촉 감지");
    }
}
