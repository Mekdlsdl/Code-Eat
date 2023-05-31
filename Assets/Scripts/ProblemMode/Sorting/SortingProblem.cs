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
    public TMP_Text beforeDishesText;
    public List<int> randomList;
    public List<GameObject> randomDishes;
    private List<float> positionList, randomPositions;
    public static int problemNum;
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

    void DoSort() {
        problemNum = UnityEngine.Random.Range(0,3);

        switch (problemNum) {
            case 0:
                StartCoroutine(BubbleSort());
                break;
            case 1:
                StartCoroutine(InsertionSort());
                break;
            case 2:
                StartCoroutine(SelectionSort());
                break;
            default:
                break;
        }
    }

    IEnumerator BubbleSort() {
        Debug.Log("bubble");
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
                yield return shortWait;
                people.SetActive(true);
                break;
            }
        }
        StartCoroutine(AfterSort());
    }

    IEnumerator InsertionSort() {
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
            yield return new WaitForSeconds(0.5f);

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

                yield return new WaitForSeconds(0.5f);
            }
            randomList[j+1] = key;
            RectTransform dishTransform3 = dishes[randomList[j+1]].GetComponent<RectTransform>();
            Vector2 modifyPosition3 = dishTransform3.anchoredPosition;
            dishTransform.DOLocalMove(new Vector3(modifyPosition3.x, modifyPosition, 0), duration);
            yield return shortWait;
        }
        StartCoroutine(AfterSort());
    }


    IEnumerator SelectionSort() {
        int index = randomList.Count;
        float duration = 0.2f; // 이동에 걸리는 시간


        for (int i=0; i<index; i++) {
            // Debug.Log($"i: {i}");
            int least = i;

            for (int j=i+1; j<index; j++) {
                if (randomList[j] < randomList[least]) {
                    least = j;
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

        yield return shortWait;
        }
        StartCoroutine(AfterSort());
    }
}
