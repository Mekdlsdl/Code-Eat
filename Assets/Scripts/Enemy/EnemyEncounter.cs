using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(StartProblemMode());
        }
    }

    IEnumerator StartProblemMode()
    {
        Debug.Log("적과 접촉 감지, 문제풀이 모드로 들어갑니다.");

        Time.timeScale = 0;
        GameManager.instance.ChangeActionMaps("BattleMode");
        
        ProblemManager problemManager = PlayerSpawn.instance.problemManager;
        problemManager.enemyType = enemyType;
        problemManager.gameObject.SetActive(true);

        yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(1f));
        Destroy(gameObject);
    }
}
