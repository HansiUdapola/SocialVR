using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvatarProfileList
{
    static AvatarProfileList avatarProfileList;
    public List<AvatarProfile> avatarProfiles;

    public int activeProfileIndexID;
    
    public static AvatarProfileList CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<AvatarProfileList>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static AvatarProfileList GetProfilesList()
    {
        
        if(avatarProfileList == null)
        {
            avatarProfileList = new AvatarProfileList();
        }

        return avatarProfileList;
    }

}
