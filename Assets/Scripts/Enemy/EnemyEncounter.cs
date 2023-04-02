using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private GameObject problem;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("적과 접촉 감지, 문제풀이 모드로 들어갑니다.");

            Time.timeScale = 0;
            // Input Action Map 을 문제 풀이 전용으로 변경
            // 문제풀이 모드에 problem 을 Instantiate
            // 문제풀이 모드 켜기
            
            // Destroy(gameObject, 1f);
        }
    }
}
