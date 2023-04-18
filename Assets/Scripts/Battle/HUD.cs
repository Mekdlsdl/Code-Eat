using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    private enum Type { Score, Hp, Time, EnemyHp }
    [SerializeField] private Type type;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Slider slider;
    private PlayerBattleMode player;
    private Enemy enemy;
    private PlayerConfiguration playerConfig;

    private void Awake()
    {
        if (!gameObject.CompareTag("WorldUI")) // 플레이어 내 UI가 아닌 경우 플레이어 정보 X
            player = transform.parent.parent.GetComponent<PlayerBattleMode>();
    }

    private void Start()
    {
        if (!gameObject.CompareTag("WorldUI"))
            playerConfig = player.playerConfig;
        enemy = BattleManager.instance.curEnemy.GetComponent<Enemy>();
        
    }

    public void LateUpdate()
    {
        switch (type) {
            case Type.Score:
                float curScore = playerConfig.PlayerScore;
                float maxScore = BattleManager.instance.curEnemy.maxHp;
                slider.value = curScore / maxScore;
                infoText.text = string.Format("{0:F0}", curScore);
                break;
            case Type.Hp:
                float curHp = playerConfig.PlayerHp;
                float maxHp = 100;
                slider.value = curHp / maxHp;
                infoText.text = string.Format("{0:F0} / {1:F0}", curHp, maxHp);
                break;
            case Type.Time:
                if (!BattleManager.instance.isBattleMode)
                    return;
                float remainTime = BattleManager.instance.maxBattleTime - BattleManager.instance.battleTime;
                int second = Mathf.FloorToInt(remainTime % 60);
                infoText.text = string.Format("{0:D2}:{1:D2}", 00, second); // D2 자릿수 고정. 00:00 형태
                break;
            case Type.EnemyHp:
                float curEnemyHp = enemy.hp;
                float maxEnemyHp = enemy.maxHp;
                slider.value = curEnemyHp / maxEnemyHp;
                infoText.text = string.Format("{0:F0} / {1:F0}", curEnemyHp, maxEnemyHp);
                break;
        }
    }
}
