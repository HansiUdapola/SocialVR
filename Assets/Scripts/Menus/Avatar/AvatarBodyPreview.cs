using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Avatars;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.FloatingAvatar;
using UnityEngine.UI;
using System;

//show a copy of Avatar that RoomClient.Me is loaded in the SocialMenu - Avatar Tab
public class AvatarBodyPreview : MonoBehaviour
{
    //Preview Containers
    public GameObject AvatarMajorPreviewContainer;
    
    //Profile preview slots
    public GameObject AvatarProfilePreview1;
    public GameObject AvatarProfilePreview2;
    public GameObject AvatarProfilePreview3;

    GameObject AvatarPreviewContainer_P1;
    GameObject AvatarPreviewContainer_P2;
    GameObject AvatarPreviewContainer_P3;

    Text ProfileTitle;

    public SocialMenu socialMenu;    
    public AvatarCatalogue AvatarCatalogue;
    public List<AvatarTextureCatalogue> skinList;
   
    AvatarManager avatarManager;
    NetworkScene networkScene;
    RoomClient roomClient;

    static AvatarProfile activeAvatarProfile;
    static GameObject _activeAvatarObjRef;

    //On Enable load the current saved avatar into item container
    public void Start()
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

        AvatarPreviewContainer_P1 = AvatarProfilePreview1.transform.FindDeepChild("AvatarPreview1").gameObject;
        AvatarPreviewContainer_P2 = AvatarProfilePreview2.transform.FindDeepChild("AvatarPreview2").gameObject;
        AvatarPreviewContainer_P3 = AvatarProfilePreview3.transform.FindDeepChild("AvatarPreview3").gameObject;




        RefreshAllPreviews();

        AvatarProfileHandler.GetMyEvent().AddListener(On_AvatarChange);
    }

    void On_AvatarChange(int profileChangedIndx)
    {
        LoadUserProfilePreviews(profileChangedIndx);
        LoadCurrentlyActiveAvatar();
    }

    void RefreshAllPreviews()
    {
        LoadCurrentlyActiveAvatar();
        LoadUserProfilePreviews();
    }

    

    //Load currently active avatar on Major Avatar container and set selected color for relavent avatar profile
    void LoadCurrentlyActiveAvatar()
    {
        activeAvatarProfile = AvatarProfileHandler.GetActiveAvatarProfile();
        ClearItemContainer(AvatarMajorPreviewContainer);

        var localAvatarPrefab = AvatarCatalogue.GetPrefab(activeAvatarProfile.avtarTypeUUID);
        _activeAvatarObjRef = Instantiate(localAvatarPrefab, AvatarMajorPreviewContainer.gameObject.transform);

        LoadSkinSettings(activeAvatarProfile, _activeAvatarObjRef);

        VisualizeActiveAvatarProfile(AvatarProfileHandler.GetActiveAvatarProfileID());
    }

    //Load other user profile previews on given profile Avatar containers
    void LoadUserProfilePreviews()
    {
        int controler = AvatarProfileHandler.GetProfileCount();
        //Debug.Log("AvatarProfileHandler.GetProfileCount()" + AvatarProfileHandler.GetProfileCount());
        //controler = 3;
        AvatarProfile avatarProfileDet;
        GameObject avatarProfileObj = null;

        while(controler > 0)
        {
            controler--;

            avatarProfileDet = AvatarProfileHandler.GetAvatarProfile(controler);
            switch(controler)
            {
                case 0:
                ClearItemContainer(AvatarPreviewContainer_P1);
                avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                    (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P1.gameObject.transform);
                AvatarProfilePreview1.transform.FindDeepChild("ProfileTitle").gameObject
                        .GetComponent<Text>().text = avatarProfileDet.profileName;
                break;

                case 1:
                ClearItemContainer(AvatarPreviewContainer_P2);
                avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                    (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P2.gameObject.transform);
                AvatarProfilePreview2.transform.FindDeepChild("ProfileTitle").gameObject
                        .GetComponent<Text>().text = avatarProfileDet.profileName;
                break;

                case 2:
                ClearItemContainer(AvatarPreviewContainer_P3);
                avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                    (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P3.gameObject.transform);
                AvatarProfilePreview3.transform.FindDeepChild("ProfileTitle").gameObject
                        .GetComponent<Text>().text = avatarProfileDet.profileName;
                break;
            }

            if(avatarProfileObj != null)
                LoadSkinSettings(avatarProfileDet, avatarProfileObj);
        }
    }

    //Load only specific user profile previews on given index
    void LoadUserProfilePreviews(int indx)
    {
        
        AvatarProfile avatarProfileDet = AvatarProfileHandler.GetAvatarProfile(indx);
        GameObject avatarProfileObj = null;
        
        switch(indx)
        {
            case 0:
            ClearItemContainer(AvatarPreviewContainer_P1);
            avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P1.gameObject.transform);
            break;

            case 1:
            ClearItemContainer(AvatarPreviewContainer_P2);
            avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P2.gameObject.transform);
            break;

            case 2:
            ClearItemContainer(AvatarPreviewContainer_P3);
            avatarProfileObj = Instantiate(AvatarCatalogue.GetPrefab
                (avatarProfileDet.avtarTypeUUID), AvatarPreviewContainer_P3.gameObject.transform);
            break;
        }

        if(avatarProfileObj != null)
            LoadSkinSettings(avatarProfileDet, avatarProfileObj);
        
    }

    //Currently disable the currently active user profile
    void VisualizeActiveAvatarProfile(int activeProfileID)
    {
        switch(activeProfileID)
        {
            case 0:
            AvatarProfilePreview1.GetComponent<Button>().enabled = false;
            AvatarProfilePreview2.GetComponent<Button>().enabled = true;
            AvatarProfilePreview3.GetComponent<Button>().enabled = true;
            break;

            case 1:
            AvatarProfilePreview1.GetComponent<Button>().enabled = true;
            AvatarProfilePreview2.GetComponent<Button>().enabled = false;
            AvatarProfilePreview3.GetComponent<Button>().enabled = true;
            break;

            case 2:
            AvatarProfilePreview1.GetComponent<Button>().enabled = true;
            AvatarProfilePreview2.GetComponent<Button>().enabled = true;
            AvatarProfilePreview3.GetComponent<Button>().enabled = false;
            break;

            default:
            Debug.Log("Error at LoadCurrentlyActiveAvatar Check");
            break;
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

    void ClearItemContainer(GameObject itemContainer)
    {
        int control = itemContainer.transform.childCount;
        while(control > 0)
        {
            Destroy(itemContainer.transform.GetChild(--control).gameObject);
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

    //If selected avatar body type is Avatar_C - Animal Body OR Avatar_B - Low poly Humanoid
    void LoadSKinTexture(AvatarProfile avatarProfileDet, GameObject createdObjRef)
    {
        var texturedAvatar = createdObjRef.GetComponent<TexuredAvatar>();
        if (texturedAvatar)
        {
           texturedAvatar.SetUbiqSkins(avatarProfileDet.ubiqAvatarSkinUUID);
        }
    }

    //expect to call over UI
    public void ChangeActiveAvatarProfile(int i)
    {
        AvatarProfileHandler.ChangeActiveAvatarProfile(i);
        roomClient.Me["ubiq.avatar.prefab"] = AvatarProfileHandler.GetActiveAvatarProfile().avtarTypeUUID; 
        avatarManager.UpdateAvatarBodyprefab(roomClient.Me);
        
        VisualizeActiveAvatarProfile(AvatarProfileHandler.GetActiveAvatarProfileID());
    }
}
