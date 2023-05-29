using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject cinematicEffect, bossObject;
    [SerializeField] private SpriteRenderer bossSpriteRenderer;


    [Header("Custom Settings")] 
    [SerializeField] private BossType bossType;
    public List<EnemyAndPos> spawnList;
    private List<GameObject> existingEnemyList = new List<GameObject>();
    public List<GameObject> ExistingEnemyList => existingEnemyList;

    public bool SpawnEnemy()
    {
        bool enemyExists = false;

        for (int i = 0; i < spawnList.Count; i++) {
            string enemy_name = spawnList[i].enemy.GetComponent<EnemyEncounter>().enemy_type.enemyName;

            if (!GameManager.encounteredEnemyset.Contains(enemy_name)) {
                enemyExists = true;
                var enemy = Instantiate(spawnList[i].enemy, spawnList[i].spawnPos, gameObject.transform.rotation, gameObject.transform);
                existingEnemyList.Add(enemy);
            }
        }

        if (!enemyExists) StartCoroutine(SpawnBoss(0.5f, 1f, 2f));

        return enemyExists;
    }

    IEnumerator SpawnBoss(float cinematicDelay, float spawnDelay, float battleDelay)
    {
        yield return new WaitForSeconds(cinematicDelay);

        cinematicEffect.SetActive(true);
        DOTween.Rewind("CinematicEffect");
        DOTween.Play("CinematicEffect");

        yield return new WaitForSeconds(spawnDelay);

        bossObject.SetActive(true);
        bossSpriteRenderer.sprite = bossType.topdownSprite;
        DOTween.Rewind("SpawnBoss");
        DOTween.Play("SpawnBoss");

        SoundManager.instance.PlaySFX("Boss Spawn");

        yield return new WaitForSeconds(battleDelay);

        DOTween.Rewind("TrembleBoss");
        DOTween.Play("TrembleBoss");
        StartCoroutine(GameManager.instance.StartProblemMode(bossType, bossSpriteRenderer.transform.localPosition, true));
    }
}

[System.Serializable]
public class EnemyAndPos
{
    public GameObject enemy;
    public Vector3 spawnPos;
}
