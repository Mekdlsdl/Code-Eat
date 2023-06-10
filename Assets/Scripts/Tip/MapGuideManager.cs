using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGuideManager : MonoBehaviour
{
    public static MapGuideManager instance { get; private set; }
    public GameObject tipPlayers;
    private List<bool> tipOffPlayers;
    private int totalPlayer;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    
    void OnEnable() {
        tipOffPlayers = new List<bool> {false, false, false, false, false};
        totalPlayer = PlayerConfigManager.instance.GetPlayerConfigs().Count;

        SetTipPlayers();
    }

    public void SetTipPlayers()
    {
        for (int i=4; i>=totalPlayer; i--) {
            GameObject tipPlayer = tipPlayers.transform.GetChild(i).gameObject;
            Image tipPlayersImage = tipPlayer.GetComponent<Image>();
            tipPlayersImage.color = Color.grey;
        }
    }

    public void ResetOffPlayer()
    {
        for (int i=0; i<totalPlayer; i++) {
            GameObject tipPlayer = tipPlayers.transform.GetChild(i).gameObject;
            Image tipPlayersImage = tipPlayer.GetComponent<Image>();
            tipPlayersImage.color = Color.white;

            tipOffPlayers[i] = false;
        }
    }

    public void CheckOffPlayer(int playerIndex)
    {
        GameObject tipPlayer = tipPlayers.transform.GetChild(playerIndex).gameObject;
        Image tipPlayersImage = tipPlayer.GetComponent<Image>();
        tipPlayersImage.color = new Color32(255, 210, 238, 255);

        tipOffPlayers[playerIndex] = true;
    }

    public bool isAllOff()
    {
        int count = 0;
        
        for (int i=0; i<totalPlayer; i++) {
            if (tipOffPlayers[i]) {
                count ++;
            } else {
                return false;
            }
        }

        if (count == totalPlayer) {
            return true;
        }

        return false;
    }
}
