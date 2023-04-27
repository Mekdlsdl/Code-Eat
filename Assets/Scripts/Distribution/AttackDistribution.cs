using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDistribution : MonoBehaviour
{
    private int[] attackCounts; // 각 플레이어들이 보유하고 있는 총알 갯수를 저장할 배열
    private bool[] hasAnswered; // 플레이어가 답변한 내용을 추적하기 위한 배열

    void Start()
    {
        attackCounts = new int[PlayerConfigManager.instance.PlayerConfigs.Count];
        hasAnswered = new bool[PlayerConfigManager.instance.PlayerConfigs.Count];
    }

    void Update()
    {
        // 모든 플레이어가 답을 선택했는지
        if (CheckAllAnswered())
        {
            // 플레이어들의 답안 선택 순서에 따라 공격횟수 분배
            DistributeAttacks();
            // 다음 라운드에 대한 응답 순서 배열 초기화
            ResetHasAnswered();
        }
    }

    void DistributeAttacks()
    {
        // 플레이어 index 및 총알 count를 저장할 튜플 목록 생성
        List<(int, int, bool)> attackList = new List<(int, int, bool)>();
        for (int i = 0; i < PlayerConfigManager.instance.PlayerConfigs.Count; i++)
        {
            if (hasAnswered[i])
            {
                attackList.Add((i, attackCounts[i], hasAnswered[i]));
            }
        }

        // 정답 선택 여부에 따른 목록 분할
        List<(int, int, bool)> correctAnswers = attackList.FindAll(t => t.Item3 == true);
        List<(int, int, bool)> incorrectAnswers = attackList.FindAll(t => t.Item3 == false);

        // 정답을 선택한 플레이어 수 계산
        int numCorrectAnswers = correctAnswers.Count;

        // 정답을 선택한 플레이어들을 응답 순서대로 정렬
        correctAnswers.Sort((a, b) =>
        {
            int indexA = AnswerManager.instance.PlayerAnswerList[a.Item1].inputAnswer;
            int indexB = AnswerManager.instance.ReturnAnswerIndex();
            return indexA.CompareTo(indexB);
        });

        // 정답을 선택한 플레이어들만 순서대로 총알 차등 분배
        int attackCount = numCorrectAnswers;
        foreach ((int, int, bool) tuple in correctAnswers)
        {
            int playerIndex = tuple.Item1;
            int attacks = Mathf.Min(attackCount, tuple.Item2);
            attackCounts[playerIndex] += attacks;
            attackCount -= attacks;
        }

    }

    bool CheckAllAnswered()
    {
        for (int i = 0; i < PlayerConfigManager.instance.PlayerConfigs.Count; i++)
        {
            if (!hasAnswered[i])
            {
                return false;
            }
        }
        return true;
    }

    void ResetHasAnswered()
    {
        for (int i = 0; i < PlayerConfigManager.instance.PlayerConfigs.Count; i++)
        {
            hasAnswered[i] = false;
        }
    }
}
