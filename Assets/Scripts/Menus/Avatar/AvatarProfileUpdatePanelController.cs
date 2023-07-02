using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Avatars;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.FloatingAvatar;
using UnityEngine.UI;

public class AvatarProfileUpdatePanelController : MonoBehaviour
{
    public SocialMenu socialMenu;   
    public AvatarCatalogue AvatarCatalogue;
    public GameObject AvatarPreviewContainer;

    public Text PanelTitle;

    int _profileIndxToUpdate;
    AvatarProfile _profileToUpdate;

    AvatarManager avatarManager;
    NetworkScene networkScene;
    RoomClient roomClient;

    // Start is called before the first frame update
    void Start()
    {
        if (!networkScene)
        {
            networkScene = socialMenu.networkScene;
            if (!networkScene)
            {
                return;
            }
        }

        avatarManager = networkScene.GetComponentInChildren<AvatarManager>();
        roomClient = socialMenu.roomClient;        
    }

    //Load preview of going to update for the first time
    void LoadAvatarPreview()
    {
        GameObject avatarProfileObj = null;

        //Get the selected profile index (i.e. 0, 1, or 2) from the AvatarProfileHandler
        _profileToUpdate = AvatarProfileHandler.GetAvatarProfile(_profileIndxToUpdate);
        PanelTitle.text = _profileToUpdate.profileName;

        ClearItemContainer(AvatarPreviewContainer);
        avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
            (_profileToUpdate.avtarTypeUUID), AvatarPreviewContainer.gameObject.transform);

        
        //Load the skin uuid/uma props 
        LoadSkinSettings(_profileToUpdate,avatarProfileObj);
    }

    public void SetProfileIndexToUpdate(int i)
    {
        _profileIndxToUpdate = i;
        Debug.Log("SetProfileIndexToUpdate " + i);

        //load the avatar preview that going to be updated (skin/uma props)
        LoadAvatarPreview();

        //Load the relavant panel from Tab Panels to show specific settings available for avatar type
        FindObjectOfType<TabController>().HideAllTabContents();
        string avatarType = AvatarProfileHandler.GetAvatarProfile(_profileIndxToUpdate).avtarTypeUUID;
        int tabToActivate = 0;

        switch (avatarType)
        {
            case "Avatar_A":
            tabToActivate = 0;
             break;

            case "Avatar_B":
            tabToActivate = 1;
             break;

            case "Avatar_C":
            tabToActivate = 2;
             break;

            case "Avatar_D":
            tabToActivate = 3;
             break;

            case "Avatar_E":
            tabToActivate = 4;
             break;
        }

        FindObjectOfType<TabController>().ShowTab(tabToActivate);

    }

    void ClearItemContainer(GameObject itemContainer)
    {
        int control = itemContainer.transform.childCount;
        while(control > 0)
        {
            Destroy(itemContainer.transform.GetChild(--control).gameObject);
        }    
    }

    void LoadSkinSettings(AvatarProfile avatarProfileDet, GameObject createdObjRef)
    {
        //Debug.Log("Loading SKin..." + avatarProfileDet.avtarTypeUUID + "-" + createdObjRef.name + " " + avatarProfileDet.ubiqAvatarSkinUUID);
        switch (avatarProfileDet.avtarTypeUUID)
        {
            case "Avatar_A":
            case "Avatar_B":
            case "Avatar_C":
            LoadSKinTexture(avatarProfileDet, createdObjRef);
            break;

            case "Avatar_D":
            case "Avatar_E":
            LoadUMAProperties(avatarProfileDet, createdObjRef);
            SetUMAAvatarAPose(createdObjRef);
            break;
        }
    }

    //If selected avatar body type is Avatar_C - Animal Body OR Avatar_A/B - Low poly Humanoid
    void LoadSKinTexture(AvatarProfile avatarProfileDet, GameObject createdObjRef)
    {
        var texturedAvatar = createdObjRef.GetComponent<TexuredAvatar>();
        if (texturedAvatar)
        {
           texturedAvatar.SetUbiqSkins(avatarProfileDet.ubiqAvatarSkinUUID);
        }
    }

    //If selected avatar body type is Avatar_D - UMA humonoid body
    void LoadUMAProperties(AvatarProfile avatarProfileDet, GameObject createdObjRef)
    {
        var texturedUMAAvatar = createdObjRef.GetComponent<UMATexturedAvatar>();
        if (texturedUMAAvatar)
        {
           Debug.Log("UMA Details " + avatarProfileDet.umaProperties.eyeColor);
           texturedUMAAvatar.SetUMAAvatarProperties(avatarProfileDet.umaProperties);
        }
    }

    //Just added to position the avatar preview in correct position (offsets)
    void SetUMAAvatarAPose(GameObject umaObj){
        umaObj.transform.FindDeepChild("LTarget").localPosition = new Vector3(0,0,0);
        umaObj.transform.FindDeepChild("RTarget").localPosition = new Vector3(0,0,0);
        umaObj.transform.FindDeepChild("LHint").localPosition = new Vector3(0,0,0);
        umaObj.transform.FindDeepChild("RHint").localPosition = new Vector3(0,0,0);
        umaObj.transform.localPosition = new Vector3(0, 0.79f, 0);
    }

}
