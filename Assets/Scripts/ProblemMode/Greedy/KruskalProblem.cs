using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Random = System.Random;
using DG.Tweening;

public class KruskalProblem : MonoBehaviour
{
    [SerializeField] private GameObject guide, problemUI, optionUI;
    [SerializeField] private List<HouseEdge> houseEdges;
    [SerializeField] private List<Sprite> optionSprites;

    private ProblemManager pm;
    private Random random = new Random();
    private List<Sprite> existingOptions = new List<Sprite>();
    private List<GameObject> spawnedOptions = new List<GameObject>();

    private int totalVertexes = 6;
    private int totalEdges = 10;
    private int cacheEdgeIndex, answerEdgeIndex;
    private List<int> parentList = new List<int> { 0 };
    private List<HouseEdge> sortedEdges;

    private bool showOnce = false;

    void OnEnable()
    {
        pm = ProblemManager.instance;
        for (int i = 1; i < totalVertexes + 1; i++)
            parentList.Add(i);
        
        var costs = ReturnRandomCosts();
        for (int i = 0; i < costs.Count; i++) {
            houseEdges[i].UpdateCost(costs[i]);
        }
        
        StartCoroutine(BeginProblem());
    }

    void OnDisable()
    {
        foreach (GameObject option in spawnedOptions)
            Destroy(option);
    }

    void Update()
    {
        if (pm.isShowingAnswer && !showOnce) {
            showOnce = true;
            DOTween.Rewind($"{sortedEdges[answerEdgeIndex].gameObject.name}");
            DOTween.Play($"{sortedEdges[answerEdgeIndex].gameObject.name}");
        }
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(3.5f);
        guide.SetActive(false);

        yield return CreateProblem();
        problemUI.SetActive(true);
        
        yield return new WaitForSeconds(3f);
        CreateOptions();
    }

    IEnumerator CreateProblem()
    {
        sortedEdges = houseEdges.OrderBy(x => x.cost).ToList();
        
        for (int i = 0; i < sortedEdges.Count; i++) {
            (int cost, int a, int b) = (sortedEdges[i].cost, sortedEdges[i].nodeNumberA, sortedEdges[i].nodeNumberB);

            if ((i >= 4) || (FindParent(a) == FindParent(b))) {
                cacheEdgeIndex = i;
                CalculateAnswer();
                yield break;
            }
            else if (FindParent(a) != FindParent(b)) {
                UnionParent(a, b);
                sortedEdges[i].HighlightEdge();
            }
        }
    }

    private void CalculateAnswer()
    {
        for (int i = cacheEdgeIndex; i < sortedEdges.Count; i++) {
            (int cost, int a, int b) = (sortedEdges[i].cost, sortedEdges[i].nodeNumberA, sortedEdges[i].nodeNumberB);

            if (FindParent(a) != FindParent(b)) {
                UnionParent(a, b);
                answerEdgeIndex = i;
                return;
            }
        }
    }

    private int FindParent(int x)
    {
        if (parentList[x] != x)
            parentList[x] = FindParent(parentList[x]);
        return parentList[x];
    }

    private void UnionParent(int a, int b)
    {
        a = FindParent(a);
        b = FindParent(b);
        if (a < b)
            parentList[b] = a;
        else
            parentList[a] = b;
    }

    private List<int> ReturnRandomCosts()
    {
        return Enumerable.Range(10, 40).OrderBy(x => random.Next())
            .Take(totalEdges)
            .ToList();
    }

    private List<List<int>> ReturnOptionValues(int answer_edge_index)
    {
        List<int> numbers = Enumerable.Range(0, sortedEdges.Count).ToList();
        for (int i = 0; i < cacheEdgeIndex; i++)
            numbers.Remove(i);
        numbers.Remove(answer_edge_index);

        List<List<int>> vertexNumberComb = new List<List<int>>();
        var shuffled = numbers.OrderBy(x => random.Next())
            .Take(3)
            .ToList();
        
        foreach(int index in shuffled) {
            List<int> comb = new List<int> { sortedEdges[index].nodeNumberA, sortedEdges[index].nodeNumberB };
            vertexNumberComb.Add(comb);
        }
        return vertexNumberComb;
    }

    private void CreateOptions()
    {
        List<int> indexList = new List<int> { 0, 1, 2, 3 };
        List<List<int>> optionValues = ReturnOptionValues(answerEdgeIndex);

        for (int i = 0; i < 3; i++) {
            int idx = random.Next(0, indexList.Count);
            int randomIdx = indexList[idx];
            indexList.RemoveAt(idx);

            GameObject option = Instantiate(optionUI, pm.optionTransforms[randomIdx]);
            spawnedOptions.Add(option);
            option.transform.GetChild(0).GetComponent<Image>().sprite = optionSprites[optionValues[i][0]];
            option.transform.GetChild(1).GetComponent<Image>().sprite = optionSprites[optionValues[i][1]];
        }
        int answerIndex = indexList[0];
        GameObject answerOption = Instantiate(optionUI, pm.optionTransforms[answerIndex]);
        spawnedOptions.Add(answerOption);
        answerOption.transform.GetChild(0).GetComponent<Image>().sprite = optionSprites[sortedEdges[answerEdgeIndex].nodeNumberA];
        answerOption.transform.GetChild(1).GetComponent<Image>().sprite = optionSprites[sortedEdges[answerEdgeIndex].nodeNumberB];

        AnswerManager.instance.SetProblemAnswer(answerIndex);
        //Debug.Log($"다음 간선은 {sortedEdges[answerEdgeIndex].gameObject.name} : {sortedEdges[answerEdgeIndex].cost}");
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }
}
