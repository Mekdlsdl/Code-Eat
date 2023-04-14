using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(collision.gameObject.GetComponent<PlayerInteract>().FlickExclamation());
            StartCoroutine(GameManager.instance.StartProblemMode(enemyType, collision.transform.localPosition));
        }
    }
}
