using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using Ubiq.Avatars;
using Ubiq.Messaging;
using Ubiq.FloatingAvatar;

public class UMAAvatarSkinController : MonoBehaviour
{
    public GameObject instanobj;
    public SocialMenu socialMenu;
    public Color defaultColor;


    private NetworkScene networkScene;
    public void ChangeMaterialBody()
    {
        UMAProperties umaAvatarProps = new UMAProperties();
        umaAvatarProps.eyeColor = "#" + ColorUtility.ToHtmlStringRGBA(defaultColor);
        umaAvatarProps.skinColor = "#" + ColorUtility.ToHtmlStringRGBA(defaultColor);
        umaAvatarProps.hairColor = "#" + ColorUtility.ToHtmlStringRGBA(defaultColor);
        umaAvatarProps.lipstickColor = "#" + ColorUtility.ToHtmlStringRGBA(defaultColor);
        umaAvatarProps.height = 0.5f;
        SetUMAAvatar(umaAvatarProps);
    }

    public void SetUMAAvatar(UMAProperties umaProps)
    {
        if (!networkScene)
        {
            networkScene = socialMenu.networkScene;
            if (!networkScene)
            {
                return;
            }
        }

        var avatar = networkScene.GetComponentInChildren<AvatarManager>().LocalAvatar;
        var texturedAvatar = avatar.GetComponent<UMATexturedAvatar>();
        if (texturedAvatar)
        {
            texturedAvatar.SetUMAAvatarProperties(umaProps.SaveToString());
            Debug.Log(umaProps.SaveToString());
        }
    }
}
