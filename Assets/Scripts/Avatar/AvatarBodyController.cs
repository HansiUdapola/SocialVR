using System;
using Ubiq.Avatars;
using Ubiq.Rooms;
using UnityEngine;
public class AvatarBodyController : MonoBehaviour
{    
    RoomClient _roomClient;

    AvatarManager _avatarManager;
    public bool saveLastUsedAvatarBody = true;

    public void OnEnable(){
        _roomClient = GetComponentInParent<RoomClient>();
        _avatarManager = FindObjectOfType<AvatarManager>();
    }

//Call through the UI and pass the avatar prefab uuid (Temporary)
    public void ChangeActiveAvatarBody(string LocalPrefabUuid){
        if (LocalPrefabUuid.Length > 0)
        {            
            _roomClient.Me["ubiq.avatar.prefab"] = LocalPrefabUuid;            
            _avatarManager.UpdateAvatarBodyprefab(_roomClient.Me); 
            SaveSettings(LocalPrefabUuid);
            // if(saveLastUsedAvatarBody)
            // {
            //     SaveSettings(LocalPrefabUuid);
            // }           
        }
    }

     void SaveSettings(string avatarTypeuuid)
    {
        AvatarProfile activeAvatarRef = AvatarProfileHandler.GetActiveAvatarProfile();
        activeAvatarRef.avtarTypeUUID = avatarTypeuuid;
        
        AvatarProfileHandler.UpdateActiveProfile(activeAvatarRef);
    }
}
