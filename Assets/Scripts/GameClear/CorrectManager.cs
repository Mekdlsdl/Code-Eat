using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrectManager : MonoBehaviour
{
    public List<TextMeshProUGUI> correct_text = new List<TextMeshProUGUI>();

    public void GetPlayerProblemState()
    {
        for (int i = 0; i < correct_text.Count; i++)
        {
            int correct = PlayerConfigManager.instance.PlayerConfigs[i].CorrectProblemCount;
            int total = ProblemManager.totalProblemCount;
            correct_text[i].text = $"{correct} / {total}";
        }

    }
}
