using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAllSet : MonoBehaviour
{
    public void DeleteAllPlayerPrefsSettings(){
        PlayerPrefs.DeleteAll();
    }
}
