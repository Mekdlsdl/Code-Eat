using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDistribution : MonoBehaviour
{
    private int[] attackCounts; // �� �÷��̾���� �����ϰ� �ִ� �Ѿ� ������ ������ �迭
    private bool[] hasAnswered; // �÷��̾ �亯�� ������ �����ϱ� ���� �迭

    void Start()
    {
        attackCounts = new int[PlayerConfigManager.instance.PlayerConfigs.Count];
        hasAnswered = new bool[PlayerConfigManager.instance.PlayerConfigs.Count];
    }

    void Update()
    {
        // ��� �÷��̾ ���� �����ߴ���
        if (CheckAllAnswered())
        {
            // �÷��̾���� ��� ���� ������ ���� ����Ƚ�� �й�
            DistributeAttacks();
            // ���� ���忡 ���� ���� ���� �迭 �ʱ�ȭ
            ResetHasAnswered();
        }
    }

    void DistributeAttacks()
    {
        // �÷��̾� index �� �Ѿ� count�� ������ Ʃ�� ��� ����
        List<(int, int, bool)> attackList = new List<(int, int, bool)>();
        for (int i = 0; i < PlayerConfigManager.instance.PlayerConfigs.Count; i++)
        {
            if (hasAnswered[i])
            {
                attackList.Add((i, attackCounts[i], hasAnswered[i]));
            }
        }

        // ���� ���� ���ο� ���� ��� ����
        List<(int, int, bool)> correctAnswers = attackList.FindAll(t => t.Item3 == true);
        List<(int, int, bool)> incorrectAnswers = attackList.FindAll(t => t.Item3 == false);

        // ������ ������ �÷��̾� �� ���
        int numCorrectAnswers = correctAnswers.Count;

        // ������ ������ �÷��̾���� ���� ������� ����
        correctAnswers.Sort((a, b) =>
        {
            int indexA = AnswerManager.instance.PlayerBattleList[a.Item1].inputAnswer;
            int indexB = AnswerManager.instance.ReturnAnswerIndex();
            return indexA.CompareTo(indexB);
        });

        // ������ ������ �÷��̾�鸸 ������� �Ѿ� ���� �й�
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
