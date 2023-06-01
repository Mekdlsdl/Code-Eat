using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhatSorting : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> contents;
    [SerializeField] private List<int> randomOptions;
    private List<string> sortType;

    void OnEnable() {
        RandomIndex();
        SetOption();
    }

    void RandomIndex() {
        randomOptions = new List<int>();

        while (randomOptions.Count < 4) {
            int ranNum = UnityEngine.Random.Range(0,4);

            if (!randomOptions.Contains(ranNum)) {
                randomOptions.Add(ranNum);
            }
        }
    }

    void SetOption() {
        sortType = new List<string>() {"버블\n정렬", "삽입\n정렬", "선택\n정렬"};
        List<string> otherType = new List<string>() {"힙\n정렬", "합병\n정렬", "퀵\n정렬", "힙\n정렬", "쉘\n정렬", "기수\n정렬"};
        int ranIdx = UnityEngine.Random.Range(0,otherType.Count);
        sortType.Add(otherType[ranIdx]);

        for (int i=0; i<randomOptions.Count; i++) {
            contents[i].text = sortType[randomOptions[i]];
        }

        int answerNum = SortingProblem.sortingNum;
        Debug.Log($"answerNum : {answerNum}");
        int answerIndex = randomOptions.IndexOf(answerNum);
        AnswerManager.instance.SetProblemAnswer(answerIndex);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }
}
