using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private Transform aimCursor;
    [SerializeField] private float aimSensitivity;
    private PlayerConfiguration playerConfig;
    private Vector2 movement;
    
    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        playerConfig.Input.actions["Aim"].Enable();
    }

    void Update()
    {
        movement = playerConfig.Input.actions["Aim"].ReadValue<Vector2>();
        aimCursor.Translate(movement * aimSensitivity * Time.unscaledDeltaTime);
    }
}
