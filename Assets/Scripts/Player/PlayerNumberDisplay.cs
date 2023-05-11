using TMPro;
using UnityEngine;

public class PlayerNumberDisplay : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI number;

    void Start()
    {
        PlayerConfiguration playerConfig = playerMovement.PlayerConfig;
        number.text = $"P{playerConfig.PlayerIndex + 1}";
        number.color = GameManager.instance.PlayerColors[playerConfig.PlayerIndex];
    }
}
