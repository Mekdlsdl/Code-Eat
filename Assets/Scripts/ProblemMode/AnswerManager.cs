using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    public static AnswerManager instance { get; private set; }
    [SerializeField] private int answerIndex;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void SetAnswer(int index)
    {
        answerIndex = index;
    }

    public int ReturnAnswerIndex()
    {
        return answerIndex;
    }
}

public enum AnswerButton { SouthButton, EastButton, NorthButton, WestButton };
