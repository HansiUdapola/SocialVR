using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvatarProfile
{
    public AvatarProfile(){}
    public AvatarProfile(int id, string name, string avatarprefabUUID, int UbiqSkinID)
    {
        this.profileID = id;
        this.profileName = name;
        this.avtarTypeUUID = avatarprefabUUID;
        this.ubiqAvatarSkinUUID = UbiqSkinID;
    }
    public int profileID; //0,1,2,...
    public string profileName; //player given identification tag
    public string avtarTypeUUID; //Avatar_A, Avatar_B, ....
    public int ubiqAvatarSkinUUID; //Avatar Texture Catelog id
    public UMAProperties umaProperties;
}
