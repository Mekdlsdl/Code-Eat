using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    private enum Type { Score, Hp }
    [SerializeField] private Type type;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Slider slider;
    private PlayerBattleMode player;

    private void Awake()
    {
        player = transform.parent.parent.GetComponent<PlayerBattleMode>();
    }

    public void LateUpdate()
    {
        var playerConfig = player.playerConfig;
        switch (type) {
            case Type.Score:
                float curScore = playerConfig.PlayerScore;
                float maxScore = 100;
                slider.value = curScore / maxScore;
                infoText.text = string.Format("{0:F0}", curScore);
                break;
            case Type.Hp:
                float curHp = playerConfig.PlayerHp;
                float maxHp = 100;
                slider.value = curHp / maxHp;
                infoText.text = string.Format("{0:F0} / {1:F0}", curHp, maxHp);
                break;
        }
    }
}
