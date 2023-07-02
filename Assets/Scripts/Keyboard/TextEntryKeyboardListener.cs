using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEntryKeyboardListener : MonoBehaviour
{
    public Keyboard keyboard;
    public TextEntry textEntry;

    void Start()
    {
        keyboard.OnInput.AddListener(Keyboard_OnInput);
    }

    void Keyboard_OnInput(KeyCode keyCode)
    {
        var intKeyCode = (int)keyCode;
        if (keyCode == KeyCode.Backspace) //one character delete
        {
            textEntry.Backspace();
        } 
        else if(keyCode == KeyCode.Clear) //Clear entire text in text field
        {
            textEntry.ClearAll();
        }
        else if (intKeyCode >= 97 && intKeyCode <= 122) //a to z
        {
            if (keyboard.currentKeyCase == Keyboard.KeyCase.Upper)
            {
                intKeyCode -= 32; //Convert to upper case
            }
                
            textEntry.Enter((char)intKeyCode);

        }
        else if (intKeyCode >= 48 && intKeyCode <= 57) // 0 to 9
        {
            textEntry.Enter((char)intKeyCode);
        }
        else if (intKeyCode == 32) //space
        {
            textEntry.Enter((char)intKeyCode);
        }
    }
}
