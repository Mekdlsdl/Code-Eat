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
    public string SelectedCharacterName {get; private set; }

    void Update()
    {
        CharacterSelect();
        MapSelectControl.instance.MapSelect(playerConfig);
    }

    private void CharacterSelect()
    {
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
        }
        else if (PressKey(InputType.RIGHT))
        {
            selectedCharIndex--;
            if (selectedCharIndex < 0)
                selectedCharIndex = GameManager.instance.UnlockedCharacters.Count - 1;
        }
        else if (PressKey(InputType.SOUTHBUTTON))
        {
            ReadyPlayer();
            return;
        }

        if (previousIndex == selectedCharIndex) return;

        SelectedCharacterName = GameManager.instance.UnlockedCharacters[selectedCharIndex].characterName;
        SetCharacter(SelectedCharacterName);
    }

    public void SetPlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        PlayerText.text = $"P{config.PlayerIndex + 1}";
        PlayerText.color = GameManager.instance.PlayerColors[config.PlayerIndex];

        Invoke("EnableInput", 0.4f);
    }
    private void EnableInput()
    {
        inputEnabled = true;
    }
    private void SetCharacter(string character_type)
    {
        if (!inputEnabled)
            return;
        
        playerConfig.CharacterType = character_type;
        anim.Play(character_type);
    }
    private void ReadyPlayer()
    {
        if (!inputEnabled) return;
        inputEnabled = false;

        playerConfig.CharacterType = GameManager.instance.UnlockedCharacters[selectedCharIndex].characterName;
        
        playerConfig.IsReady = true;
        //anim.Play($"{selectedCharacterName}_Ready");
        statusIcon.sprite = ReadyIcon;
        Arrows.SetActive(false);

        StartCoroutine(GameManager.instance.TryMapSelect());
    }

    private void CancelReady()
    {
        inputEnabled = true;

        playerConfig.IsReady = false;
        statusIcon.sprite = WaitingIcon;
        Arrows.SetActive(true);
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}