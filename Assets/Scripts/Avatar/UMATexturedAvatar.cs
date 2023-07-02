using System;
using UnityEngine;
using Ubiq.Rooms;
using UMA.CharacterSystem;
using Avatar = Ubiq.Avatars.Avatar;
using Ubiq.Avatars;

namespace Ubiq.FloatingAvatar{
    [RequireComponent(typeof(Avatar))]
    public class UMATexturedAvatar : MonoBehaviour
    {
        UMAProperties _umaProperties;
        public bool SaveDNASetting;
        public DynamicCharacterAvatar dynamicCharacterAvatar;

        private Avatar avatar;

        public void Awake()
        {
            avatar = GetComponent<Avatar>();
            avatar.OnPeerUpdated.AddListener(OnPeerUpdated);            
        }

        public void Start()
        {
            if(SaveDNASetting && avatar.IsLocal)
            {
                SetUMAAvatarProperties(LoadSettings());             
            }
        }

        
        public void SetUMAAvatarProperties(UMAProperties uMAProperties)
        {
            //UpdateUMAAvatar(uMAProperties);
            SetUMAAvatarProperties(uMAProperties.SaveToString());
        }

        public void SetUMAAvatarProperties(string umaProperties)
        {
            if(String.IsNullOrEmpty(umaProperties))
                return;

            if(umaProperties == "{}")
            {
                _umaProperties = UMAProperties.GetDefaultUMAPropertySet(new UMAProperties());
                SetSettings(_umaProperties);
            }
            else            
            {
                _umaProperties = UMAProperties.CreateFromJson(umaProperties);

                if(_umaProperties.hairColor == "")
                {
                    _umaProperties = UMAProperties.GetDefaultUMAPropertySet(_umaProperties);
                    SetSettings(_umaProperties);
                }
            }

            UpdateUMAAvatar(_umaProperties);

            if(avatar.IsLocal)
            {
                avatar.Peer["uma.avatar.properties"] = _umaProperties.SaveToString();
            }
        }

        private void OnPeerUpdated(IPeer peer)
        {
            SetUMAAvatarProperties(peer["uma.avatar.properties"]);
        }

        private void SetSettings(UMAProperties umaProperties)
        {
            AvatarProfile activeAvatarRef =  AvatarProfileHandler.GetActiveAvatarProfile();
            activeAvatarRef.umaProperties = umaProperties;
            
            AvatarProfileHandler.UpdateActiveProfile(activeAvatarRef);
        }

        private string LoadSettings()
        {
            var umajson = PlayerPrefs.GetString("uma.avatar.properties", "{}");
            
            return umajson;
        }

        public void ClearSettings()
        {
            PlayerPrefs.DeleteKey("uma.avatar.properties");
        }

        //Change the UMA dynamic character's materials colors (hard coded values for now)
        public void UpdateUMAAvatar(UMAProperties uMAProperties)
        {
            dynamicCharacterAvatar.SetColor("Skin", GetRGBAColor(uMAProperties.skinColor));
            dynamicCharacterAvatar.SetColor("Hair", GetRGBAColor(uMAProperties.hairColor));  
            dynamicCharacterAvatar.UpdateColors(true);
            Debug.Log("Came to UMA upate" + GetRGBAColor(uMAProperties.hairColor)); 
        }

        private Color GetRGBAColor(string stringcolor)
        {
            Color color;
            ColorUtility.TryParseHtmlString(stringcolor, out color);

            return color;
        }
    }        
}
