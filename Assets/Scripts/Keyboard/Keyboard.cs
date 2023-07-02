using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public enum KeyCase
    {
        Upper,
        Lower
    }

    [System.NonSerialized] public KeyCase defaultKeyCase = KeyCase.Upper;

    [System.NonSerialized] public bool capsLockON = false;
    [System.NonSerialized] public bool ShiftKeyON = true;

    [System.Serializable]
    public class InputEvent : UnityEvent<KeyCode> {};
    public InputEvent OnInput;

    [System.Serializable]
    public class KeyCaseEvent : UnityEvent<KeyCase> {};
    public KeyCaseEvent OnKeyCaseChange;
    public KeyCase currentKeyCase { get; private set; }

    private void Awake()
    {
        currentKeyCase = defaultKeyCase;
        OnKeyCaseChange.Invoke(currentKeyCase);
        ShiftKeyButtonUpdate();
    }

    public void Input(KeyCode keyCode)
    {
        OnInput.Invoke(keyCode);        

        if (keyCode == KeyCode.LeftShift
            || keyCode == KeyCode.RightShift)
        {
            ShiftKeyON = (ShiftKeyON) ? false : true; //toggle shiftkey pressed status
            ShiftKeyButtonUpdate();  

            currentKeyCase = (currentKeyCase == KeyCase.Lower) //toggle case
                ? KeyCase.Upper : KeyCase.Lower;
                          
            OnKeyCaseChange.Invoke(currentKeyCase);
        }else if (keyCode == KeyCode.CapsLock)
        {
            ShiftKeyON = false;
            ShiftKeyButtonUpdate();

            capsLockON = (capsLockON) ? false : true; //toggle Capslock            
            currentKeyCase = (capsLockON) ? KeyCase.Upper : KeyCase.Lower;

            GameObject capslock = this.transform.Find("CapsLock").gameObject;

            if(capsLockON)
                capslock.GetComponent<Image>().color = capslock.GetComponent<Button>().colors.selectedColor;
            else
                capslock.GetComponent<Image>().color = capslock.GetComponent<Button>().colors.normalColor;
                
            OnKeyCaseChange.Invoke(currentKeyCase);
        }else if(ShiftKeyON)
        {
            ShiftKeyON = false;
            ShiftKeyButtonUpdate();
            currentKeyCase = (currentKeyCase == KeyCase.Lower) //toggle case
                ? KeyCase.Upper : KeyCase.Lower;   
            OnKeyCaseChange.Invoke(currentKeyCase);             
        }
    }

    private void ShiftKeyButtonUpdate(){
        GameObject LshiftKey, RshiftKey;            
        LshiftKey = this.transform.Find("LeftShift").gameObject;
        RshiftKey = this.transform.Find("RightShift").gameObject;

        if(ShiftKeyON){ //pressed indicator with color change
            LshiftKey.GetComponent<Image>().color = LshiftKey.GetComponent<Button>().colors.selectedColor;
            RshiftKey.GetComponent<Image>().color = RshiftKey.GetComponent<Button>().colors.selectedColor;
        }else{
            LshiftKey.GetComponent<Image>().color = LshiftKey.GetComponent<Button>().colors.normalColor;
            RshiftKey.GetComponent<Image>().color = RshiftKey.GetComponent<Button>().colors.normalColor;
        }
    }
}
