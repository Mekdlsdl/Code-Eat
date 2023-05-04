using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ScoreManager: MonoBehaviour
{

    public List<TextMeshProUGUI> score_text = new List<TextMeshProUGUI>();

    public void GetPlayerScoreState()
    {
        for (int i = 0; i < score_text.Count; i++)
        {
            int score = PlayerConfigManager.instance.PlayerConfigs[i].PlayerScore;
            score_text[i].text = $"{score}pt";
        }
    }
}
