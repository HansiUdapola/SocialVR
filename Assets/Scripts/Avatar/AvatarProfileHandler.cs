using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AvatarProfileHandler : MonoBehaviour
{
    public static AvatarProfileList _avatarProfileList;
    static AvatarProfile  _activeAvatarProfile;

    //UnityEvent m_MyEvent = new UnityEvent();
     public class ProfileChangeEvent : UnityEvent<int> {}; //New Unity Event
     public static ProfileChangeEvent OnAvatarProfilesChange = new ProfileChangeEvent();

    private void Start()
    {
        _avatarProfileList = AvatarProfileList.GetProfilesList();

        //update and load values from player prefs
        _avatarProfileList = GetAllAvatarProfiles(); 

        //Getcurrently active avatar profile 
        _activeAvatarProfile = GetActiveAvatarProfile();
    }

    //Get Currently active avatar profile
    public static AvatarProfile GetActiveAvatarProfile()
    {
        if(_avatarProfileList == null)
            _avatarProfileList = GetAllAvatarProfiles(); 

        return _avatarProfileList.avatarProfiles[_avatarProfileList.activeProfileIndexID];
    }

    //Get Currently active avatar profile Index 0,1,2
    public static int GetActiveAvatarProfileID()
    {
        return _avatarProfileList.activeProfileIndexID;
    }

    //Get the reference of avatar-profile for given index i.e 0,1,2
    public static AvatarProfile GetAvatarProfile(int index){
        if(index < 0 || index >= _avatarProfileList.avatarProfiles.Count)
            return null;
        
        return _avatarProfileList.avatarProfiles[index];
    }

    //Change the active profile index i.e. 0, 1, 2
    public static void ChangeActiveAvatarProfile(int indexID){
         if(indexID < 0 || indexID >= _avatarProfileList.avatarProfiles.Count)
            return;

        _avatarProfileList.activeProfileIndexID = indexID;
      
        SaveSettings();
        OnAvatarProfilesChange.Invoke(_avatarProfileList.activeProfileIndexID);        
    }

    //Change active profile - This is the global method to call over other classes to update the active profile
    public static void UpdateActiveProfile(AvatarProfile activeAvatarRef){
        _activeAvatarProfile = activeAvatarRef;
        if(_activeAvatarProfile.avtarTypeUUID == "Avatar_D" 
                    || _activeAvatarProfile.avtarTypeUUID == "Avatar_E"){
            _activeAvatarProfile.ubiqAvatarSkinUUID = -1;
            if(_activeAvatarProfile.umaProperties.skinColor == "")
                UMAProperties.GetDefaultUMAPropertySet(_activeAvatarProfile.umaProperties);
        }else{ //Avatar_A Avatar_B or Avatar_C has direct skin textures
            if(_activeAvatarProfile.ubiqAvatarSkinUUID == -1)
                _activeAvatarProfile.ubiqAvatarSkinUUID = 0;
            _activeAvatarProfile.umaProperties = new UMAProperties();
        }

        SaveSettings();
        OnAvatarProfilesChange.Invoke(AvatarProfileHandler.GetActiveAvatarProfileID());
    }

    public static int GetProfileCount()
    {
        return GetAllAvatarProfiles().avatarProfiles.Count;
    }

    public static ProfileChangeEvent GetMyEvent(){
        return OnAvatarProfilesChange;
    }
    
    //Recieve all avatar profiles
    private static AvatarProfileList GetAllAvatarProfiles()
    {
        if(LoadProfilesFromPlayerPrefs() == "{}"){
            LoadDefaultProfileSettings();
            SaveSettings();
        }
        else
        {
            _avatarProfileList = AvatarProfileList.CreateFromJson(LoadProfilesFromPlayerPrefs());
        }
        return _avatarProfileList;
    }

    //Recieve from player prefs
    private static string LoadProfilesFromPlayerPrefs()
    {
        var avatarProfileList = PlayerPrefs.GetString("avatar.profiles", "{}");
            
        return avatarProfileList;
    }

    //Save into Player prefs - Centralized method to call change player prefs
    private static void SaveSettings()
    {
        PlayerPrefs.SetString("avatar.profiles", _avatarProfileList.SaveToString());
        
        SetAvatarPlayerPrefs(GetActiveAvatarProfile().avtarTypeUUID, GetActiveAvatarProfile().ubiqAvatarSkinUUID
            , GetActiveAvatarProfile().umaProperties);

        Debug.Log("Avatar Profile Saver " + _avatarProfileList.SaveToString());
    }

    //Keep the last saved avatar body  type, skin id and properties
    private static void SetAvatarPlayerPrefs(string avatarUUID, int skinUUID, UMAProperties uMAProperties)
    {
        /*
        For easy access and share low data over network, currently using avatar body type
        and skin uuid for Avatar_B and C and UMA properties has been saved separately
        */

        PlayerPrefs.SetString("ubiq.avatar.prefab", avatarUUID);
        PlayerPrefs.SetString("ubiq.avatar.skin.uuid", skinUUID + "");
        PlayerPrefs.SetString("uma.avatar.properties", uMAProperties.SaveToString());
        
    }

    /*
    For the first time load default settings to save into avatar profiles
    This method will be run only time until save the settings to Player Prefs
    */
    private static void LoadDefaultProfileSettings()
    {
        List<AvatarProfile> profiles = new List<AvatarProfile>();

        AvatarProfile avatarProfile;
            
        avatarProfile = new AvatarProfile(0,"Profile A","Avatar_C",0);
        avatarProfile.umaProperties = new UMAProperties();
        profiles.Add(avatarProfile);
            
        avatarProfile = new AvatarProfile(1,"Profile B","Avatar_C",0);
        avatarProfile.umaProperties = new UMAProperties();
        profiles.Add(avatarProfile);

        avatarProfile = new AvatarProfile(2,"Profile C","Avatar_D",-1);
        avatarProfile.umaProperties = UMAProperties.GetDefaultUMAPropertySet(new UMAProperties());
        profiles.Add(avatarProfile); 

        if(_avatarProfileList == null)
            _avatarProfileList = new AvatarProfileList();
            
        _avatarProfileList.avatarProfiles = profiles;   
        _avatarProfileList.activeProfileIndexID = 1;
    }

    
}
