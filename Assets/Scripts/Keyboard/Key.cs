using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Keyboard keyboard;
    public KeyCode keyCode;
    public Text keyText;
    public Image keyImage;
    public string uppercaseKeyString;
    public string lowercaseKeyString;
    public Sprite uppercaseKeySprite;
    public Sprite lowercaseKeySprite;

    private void OnEnable()
    {
        UpdateKeyCase(keyboard.currentKeyCase);
        keyboard.OnKeyCaseChange.AddListener(Keyboard_OnKeyCaseChange);
    }

    private void OnDisable()
    {
        keyboard.OnKeyCaseChange.RemoveListener(Keyboard_OnKeyCaseChange);
    }

    private void Keyboard_OnKeyCaseChange(Keyboard.KeyCase newCase)
    {
        UpdateKeyCase(keyboard.currentKeyCase);
    }

    private void UpdateKeyCase(Keyboard.KeyCase newCase)
    {
        if (keyText)
        {
            keyText.text = newCase == Keyboard.KeyCase.Upper
                ? uppercaseKeyString : lowercaseKeyString;
        }

        if (keyImage)
        {
            keyImage.sprite = newCase == Keyboard.KeyCase.Upper
                ? uppercaseKeySprite : lowercaseKeySprite;
        }
    }

    public void Press()
    {
        keyboard.Input(keyCode);
    }
}
