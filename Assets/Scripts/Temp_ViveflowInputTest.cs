using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp_ViveflowInputTest : MonoBehaviour
{

    public Text temp;

    public void Pressed_Button(string x){
        Debug.Log(x + "Pressed");
        temp.text = x + "Pressed";
    }
}
