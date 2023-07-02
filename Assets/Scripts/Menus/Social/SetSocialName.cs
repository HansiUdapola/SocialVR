using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSocialName : MonoBehaviour
{
    public NameManager nameManager;
    public Text nameTextInput;

    // Expected to be called by a UI element
    public void SetName()
    {
        if (nameTextInput && nameManager)
        {
            nameManager.SetName(nameTextInput.text);
        }
    }
}
