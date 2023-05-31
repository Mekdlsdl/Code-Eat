using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhatSorting : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> contents;
    [SerializeField] private List<int> randomOptions;
    // private SortingProblem sortingProblem;
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
        sortType = new List<string>() {"버블\n정렬", "삽입\n정렬", "선택\n정렬", "힙\n정렬"};

        for (int i=0; i<randomOptions.Count; i++) {
            contents[i].text = sortType[randomOptions[i]];
        }

        int answerNum = SortingProblem.problemNum;
        Debug.Log($"answerNum : {answerNum}");
        int answerIndex = randomOptions.IndexOf(answerNum);
        AnswerManager.instance.SetProblemAnswer(answerIndex);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }
}
