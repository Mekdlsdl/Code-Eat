using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProblemManager : MonoBehaviour
{
    [System.NonSerialized] public EnemyType enemyType;
    [SerializeField] private Enemy enemy;
    
    public List<Transform> optionTransforms;
    [SerializeField] private Transform problemUI;
    
    [SerializeField] private List<GameObject> problems = new List<GameObject>();
    private GameObject currentProblem;

    void OnEnable()
    {
        SetEnemy();
        SpawnProblem();
    }
    
    private void SetEnemy()
    {
        // enemy.maxhp = enemy.hp = enemyType.enemyHP;
    }
    private void SpawnProblem()
    {
        int selectedIndex = Random.Range(0, problems.Count);
        currentProblem = Instantiate(problems[selectedIndex], problemUI);
        currentProblem.GetComponent<StackProblem>().pm = this;
    }

    private void NextProblem()
    {
        Destroy(currentProblem);
        SpawnProblem();
    }

    public void HideProblem()
    {
        DOTween.Rewind("HideProblem");
        DOTween.Play("HideProblem");
    }
}