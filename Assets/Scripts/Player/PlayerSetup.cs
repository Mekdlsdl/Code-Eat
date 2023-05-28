using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    private int selectedCharIndex;
    private PlayerConfiguration playerConfig;

    [SerializeField] private Animator anim;
    public Animator PlayerAnim => anim;

    [SerializeField] private Image statusIcon;
    [SerializeField] private Sprite ReadyIcon, WaitingIcon;
    [SerializeField] private TextMeshProUGUI PlayerText;
    [SerializeField] private GameObject Arrows;

    private bool inputEnabled = false;

    void Update()
    {
        CharacterSelect();
    }

    private void CharacterSelect()
    {
        if (PauseMenu.isPaused) return;

        if (!inputEnabled) {
            if (PressKey(InputType.EASTBUTTON))
                CancelReady();
            return;
        }

        int previousIndex = selectedCharIndex;

        if (PressKey(InputType.LEFT))
        {
            selectedCharIndex++;
            if (selectedCharIndex >= GameManager.instance.UnlockedCharacters.Count)
                selectedCharIndex = 0;
            SoundManager.instance.PlaySFX("Change");
        }
        else if (PressKey(InputType.RIGHT))
        {
            selectedCharIndex--;
            if (selectedCharIndex < 0)
                selectedCharIndex = GameManager.instance.UnlockedCharacters.Count - 1;
            SoundManager.instance.PlaySFX("Change");
        }
        else if (PressKey(InputType.SOUTHBUTTON))
        {
            ReadyPlayer();
            return;
        }

        if (previousIndex == selectedCharIndex) return;

        string selectedCharacterName = GameManager.instance.UnlockedCharacters[selectedCharIndex].characterName;
        SetCharacter(selectedCharacterName);
    }
    private void EnableInput()
    {
        inputEnabled = true;
    }
    public void SetPlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        PlayerText.text = $"P{config.PlayerIndex + 1}";
        PlayerText.color = GameManager.instance.PlayerColors[config.PlayerIndex];

        Invoke("EnableInput", 0.4f);
    }
    private void SetCharacter(string character_type)
    {
        if (!inputEnabled) return;
        anim.Play(character_type);
    }
    private void ReadyPlayer()
    {
        if (!inputEnabled) return;
        inputEnabled = false;

        playerConfig.CharacterTypeIndex = selectedCharIndex;
        playerConfig.CharacterName = GameManager.instance.UnlockedCharacters[selectedCharIndex].characterName;
        playerConfig.IsReady = true;

        statusIcon.sprite = ReadyIcon;
        Arrows.SetActive(false);

        SoundManager.instance.PlaySFX("Confirm");
        
        StartCoroutine(GameManager.instance.TryMapSelect());
    }

    private void CancelReady()
    {
        inputEnabled = true;

        playerConfig.IsReady = false;
        statusIcon.sprite = WaitingIcon;
        Arrows.SetActive(true);

        SoundManager.instance.PlaySFX("Cancel");
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}