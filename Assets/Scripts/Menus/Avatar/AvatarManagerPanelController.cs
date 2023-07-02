using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManagerPanelController : MonoBehaviour
{
    public GameObject UpdatePanel;

    //Call through UI UPDATE button to open update panel for the given profileIndex number
    //Profile index indicates the which avatar profile is going to update

    public void LoadUpdatePanel(int profileIndxToUpdate)
    {
        this.gameObject.SetActive(false);
        UpdatePanel.SetActive(true); //activate update panel

        UpdatePanel.GetComponent<AvatarProfileUpdatePanelController>()
                .SetProfileIndexToUpdate(profileIndxToUpdate);
    }
}
