using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private bool enableInput = false;
    private bool isClosingGuide = false;
    void Update()
    {
        if (!enableInput) return;

        if (!isClosingGuide && (PressKey(InputType.EASTBUTTON) || PressKey(InputType.SOUTHBUTTON)))
            CloseGuide();
    }

    void OnEnable()
    {
        Invoke("EnableInput", 1f);
    }

    void OnDisable()
    {
        PlayerConfigManager.instance.enableJoin = true;
        Destroy(gameObject);
    }

    private void EnableInput()
    {
        enableInput = true;
    }

    private void CloseGuide()
    {
        isClosingGuide = true;
        DOTween.Rewind("HideGamepadGuide");
        DOTween.Play("HideGamepadGuide");
    }

    private bool PressKey(string input_tag)
    {
        return playerInput.actions[input_tag].triggered;
    }
}
