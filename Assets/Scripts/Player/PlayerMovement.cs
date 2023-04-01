using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float stickSensitivity;
    private PlayerConfiguration playerConfig;
    public PlayerConfiguration PlayerConfig => playerConfig;
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private Vector2 movement;
    private bool isBusy;

    void OnDisable()
    {
        playerConfig.Input.actions["Move"].Disable();
    }
    void FixedUpdate()
    {
        if (isBusy)
            return;
        
        movement = playerConfig.Input.actions["Move"].ReadValue<Vector2>();

        if (DetectInput(movement))
        {
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetFloat("moveX", movement.x);
            playerAnim.SetFloat("moveY", movement.y);

            playerRb.MovePosition(playerRb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else {
            playerAnim.SetBool("isWalking", false);
            playerRb.velocity = Vector3.zero;
        }
    }

    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        playerAnim.runtimeAnimatorController = GameManager.instance.GetCharAnimControl(playerConfig.CharacterType);

        playerConfig.Input.actions["Move"].Enable();
        playerConfig.Input.SwitchCurrentActionMap("MapControl");
        playerAnim.SetFloat("moveY", -1);
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

    private bool DetectInput(Vector2 move)
    {
        return (move.x >= stickSensitivity || move.x <= -stickSensitivity || move.y >= stickSensitivity || move.y <= -stickSensitivity);
    }

    public void SetBusyState(bool state)
    {
        isBusy = state;
    }
}
