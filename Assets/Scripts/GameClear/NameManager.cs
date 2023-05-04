using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameManager : MonoBehaviour
{
    public List<TextMeshProUGUI> player_text = new List<TextMeshProUGUI>();

    public void GetPlayerNameState()
    {
        for (int i = 0; i < player_text.Count; i++)
        {
            int playerindex = PlayerConfigManager.instance.PlayerConfigs[i].PlayerIndex;
            player_text[i].text = $"P{playerindex + 1}";
        }
    }
}
