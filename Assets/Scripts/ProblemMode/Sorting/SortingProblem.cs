using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SortingProblem : MonoBehaviour
{
    [SerializeField] private GameObject guide, people, divider, beforeDishes, afterDishes, option;
    [SerializeField] private List<GameObject> dishes;
    [SerializeField] private int problemNum;
    public TMP_Text beforeDishesText;
    public List<int> randomList;
    public static (int, int) answerDishes;
    public static List<GameObject> randomDishes;
    public static List<float> positionList; 
    private List<float> randomPositions;
    public static int sortingNum;
    private FillSorting fillSorting;
    WaitForSeconds shortWait = new WaitForSeconds(1f);
    WaitForSeconds longWait = new WaitForSeconds(2f);

    void OnEnable() {
        GetPosition();
        RandomList();
        SetPosition();
        StartCoroutine(BeginProblem());
    }

    IEnumerator BeginProblem() {
        yield return longWait;
        guide.SetActive(true);
        yield return longWait;
        guide.SetActive(false);
        yield return shortWait;
        afterDishes.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        beforeDishes.SetActive(true);
        yield return shortWait;
        divider.SetActive(true);
        yield return longWait;
        DoSort();
    }

    void RandomList() {
        randomList = new List<int>();

        while (randomList.Count < 6) {
            int ranNum = UnityEngine.Random.Range(0,6);
        
            if (!randomList.Contains(ranNum)) {
                randomList.Add(ranNum);
            }
        }
        beforeDishesText.text = String.Format("{0}          {1}          {2}          {3}          {4}          {5}", randomList[0]+1, randomList[1]+1, randomList[2]+1, randomList[3]+1, randomList[4]+1, randomList[5]+1);
    }
    
    void GetPosition() {
        positionList = new List<float> ();

        foreach (GameObject dish in dishes) {
            RectTransform dishTransform = dish.GetComponent<RectTransform>();
            float dishPosition = dishTransform.anchoredPosition.x;
            positionList.Add(dishPosition);
        }
    }

    void SetPosition() {
        for (int i=0; i<6; i++) {
            RectTransform dishTransform = dishes[randomList[i]].GetComponent<RectTransform>();
            Vector2 modifyPosition = dishTransform.anchoredPosition;
            modifyPosition.x = positionList[i];
            dishTransform.anchoredPosition = modifyPosition;
        }
    }

    IEnumerator AfterSort() {
        if (problemNum == 0) {
            yield return shortWait;
            people.SetActive(true);
            yield return new WaitForSeconds(1.5f);

            RectTransform dishTransform = afterDishes.GetComponent<RectTransform>();
            float modifyPosition = dishTransform.anchoredPosition.y;
            dishTransform.DOLocalMoveY(modifyPosition - 200, 0.8f);

            yield return shortWait;
            RectTransform peopleTransform = people.GetComponent<RectTransform>();
            float modifyPositionP = peopleTransform.anchoredPosition.y;
            peopleTransform.DOLocalMoveY(modifyPositionP - 50, 0.4f);
            dishTransform.DOLocalMoveY(modifyPosition - 250, 0.4f);

            option.SetActive(true);

            yield return new WaitForSeconds(0.5f);
            people.SetActive(false);
            afterDishes.SetActive(false);
        }
        else if (problemNum == 1) {
            fillSorting = option.GetComponent<FillSorting>();
            option.SetActive(true);
            fillSorting.SetOption();
        }
    }

    void DoSort() {
        sortingNum = UnityEngine.Random.Range(0,3);
        int step = -1;

        if (problemNum == 1) {
            step = UnityEngine.Random.Range(2,4);
        }

        switch (sortingNum) {
            case 0:
                StartCoroutine(BubbleSort(step));
                break;
            case 1:
                StartCoroutine(InsertionSort(step));
                break;
            case 2:
                StartCoroutine(SelectionSort(step));
                break;
            default:
                break;
        }
    }

    void AfterStep() {
        List<GameObject> copyDishes = new List<GameObject>(dishes);
        randomDishes = new List<GameObject>();

        int randomIndex = FillSorting.randomIndex;
        answerDishes = (randomList[randomIndex], randomList[randomIndex+1]);

        randomDishes.AddRange(copyDishes);
    }

    IEnumerator BubbleSort(int step = -1) {
        Debug.Log("bubble");
        yield return shortWait;
        for (int index=randomList.Count-1; index>0; index--) {
            // Debug.Log($"index : {index}");

            bool changed = false;
            for (int i=0; i<index; i++) {
                // Debug.Log($"i: {i}");

                if (randomList[i] > randomList[i+1]) {
                    // Debug.Log($"{randomList[i]} / {randomList[i+1]}");
                    RectTransform dishTransform1 = dishes[randomList[i]].GetComponent<RectTransform>();
                    float modifyPosition1 = dishTransform1.anchoredPosition.x;
                    RectTransform dishTransform2 = dishes[randomList[i+1]].GetComponent<RectTransform>();
                    float modifyPosition2 = dishTransform2.anchoredPosition.x;

                    // Debug.Log($"{positionList[randomList[i+1]]} / {positionList[randomList[i]]}");
                    
                    float duration = 0.2f; // 이동에 걸리는 시간

                    dishTransform1.DOLocalMoveX(modifyPosition2, duration);
                    dishTransform2.DOLocalMoveX(modifyPosition1, duration);

                    int temp = randomList[i];
                    randomList[i] = randomList[i+1];
                    randomList[i+1] = temp;

                    changed = true;
                    yield return shortWait;
                }
            }

            if (changed == false) {
                break;
            }

            if (step != -1 && randomList.Count - index == step) {
                AfterStep();
                break;
            }
        }
        StartCoroutine(AfterSort());
    }

    IEnumerator InsertionSort(int step = -1) {
        Debug.Log("insertion");

        float duration = 0.1f; // 이동에 걸리는 시간

        int index = randomList.Count;
        // Debug.Log($"index : {index}");

        for (int i=1; i<index; i++) {
            // Debug.Log($"i: {i}");
            int key = randomList[i];
            RectTransform dishTransform = dishes[randomList[i]].GetComponent<RectTransform>();
            float modifyPosition = dishTransform.anchoredPosition.y;
            dishTransform.DOLocalMoveY(modifyPosition - 200, duration);
            yield return new WaitForSeconds(0.3f);

            int j = i - 1;

            while (j >= 0 && randomList[j] > key) {
                // Debug.Log($"{randomList[i]} / {randomList[i+1]}");
                RectTransform dishTransform1 = dishes[randomList[j]].GetComponent<RectTransform>();
                float modifyPosition1 = dishTransform1.anchoredPosition.x;
                RectTransform dishTransform2 = dishes[randomList[j+1]].GetComponent<RectTransform>();
                float modifyPosition2 = dishTransform2.anchoredPosition.x;

                // Debug.Log($"{positionList[randomList[i+1]]} / {positionList[randomList[i]]}");

                dishTransform1.DOLocalMoveX(modifyPosition2, duration);
                dishTransform2.DOLocalMoveX(modifyPosition1, duration);


                int temp = randomList[j];
                randomList[j] = randomList[j+1];
                randomList[j+1] = temp;
                j--;

                yield return new WaitForSeconds(0.3f);
            }
            randomList[j+1] = key;
            RectTransform dishTransform3 = dishes[randomList[j+1]].GetComponent<RectTransform>();
            Vector2 modifyPosition3 = dishTransform3.anchoredPosition;
            dishTransform.DOLocalMove(new Vector3(modifyPosition3.x, modifyPosition, 0), duration);

            if (step != -1 && i == step) {
                AfterStep();
                break;
            }

            yield return shortWait;
        }
        StartCoroutine(AfterSort());
    }


    IEnumerator SelectionSort(int step = -1) {
        int index = randomList.Count;
        float duration = 0.4f; // 이동에 걸리는 시간


        for (int i=0; i<index; i++) {
            // Debug.Log($"i: {i}");
            int least = i;

            for (int j=i+1; j<index; j++) {
                GameObject dish = dishes[randomList[j]].transform.GetChild(0).gameObject;
                Image dishImage = dish.GetComponent<Image>();
                dishImage.color = Color.grey;

                yield return new WaitForSeconds(0.2f);

                dishImage.color = Color.white;

                if (randomList[j] < randomList[least]) {
                    least = j;
                    RectTransform dishTransform = dishes[randomList[least]].GetComponent<RectTransform>();
                    float modifyPosition = dishTransform.anchoredPosition.y;
                    dishTransform.DOLocalMoveY(modifyPosition - 50, duration);
                    yield return new WaitForSeconds(0.1f);
                    dishTransform.DOLocalMoveY(modifyPosition, duration);
                }
            }
            
        // Debug.Log($"{randomList[i]} / {randomList[i+1]}");
        RectTransform dishTransform1 = dishes[randomList[i]].GetComponent<RectTransform>();
        float modifyPosition1 = dishTransform1.anchoredPosition.x;
        RectTransform dishTransform2 = dishes[randomList[least]].GetComponent<RectTransform>();
        float modifyPosition2 = dishTransform2.anchoredPosition.x;

        // Debug.Log($"{positionList[randomList[i+1]]} / {positionList[randomList[i]]}");

        dishTransform1.DOLocalMoveX(modifyPosition2, duration);
        dishTransform2.DOLocalMoveX(modifyPosition1, duration);


        int temp = randomList[i];
        randomList[i] = randomList[least];
        randomList[least] = temp;

        if (step != -1 && i - 1 == step) {
            AfterStep();
            break;
        }

        yield return shortWait;
        }
        StartCoroutine(AfterSort());
    }
}
