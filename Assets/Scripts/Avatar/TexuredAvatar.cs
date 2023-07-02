using System;
using Ubiq.Dictionaries;
using Ubiq.Avatars;
using UnityEngine.Events;
using UnityEngine;
using Avatar = Ubiq.Avatars.Avatar;
using Ubiq.Rooms;

namespace Ubiq.FloatingAvatar{
[RequireComponent(typeof(Avatar))]
    public class TexuredAvatar : MonoBehaviour
    {
        public AvatarTextureCatalogue AvatarSkins; //List of textures of avatars skins
        /*
        ubiq.avatar.skin.uuid is the key of Avatar Skins Catelogue which holds the 
        value of the index.
        */
        //public bool RandomTextureOnSpawn; //On spawan shall allocate random texture?
        public bool SaveTextureSetting; //Will save the avatar skin settings for future?

        //Create a unity event to invoke on texture/skin is changed.
        [Serializable]
        public class TextureEvent : UnityEvent<Texture2D> { }
        public TextureEvent OnTextureChanged;

        private Avatar avatar;
        private string uuid;
        private Texture2D cached; // Cache for GetTexture. Do not do anything else with this; use the uuid
        private void Awake()
        {
            avatar = GetComponent<Avatar>();
            avatar.OnPeerUpdated.AddListener(OnPeerUpdated);            
        }

        //For now when peer is updated change only skin uuid, 
        //Should include if change other properties
        private void OnPeerUpdated(IPeer peer)
        {
            SetTexture(peer["ubiq.avatar.skin.uuid"]);
            Debug.Log("Peed updated: " + peer["ubiq.social.name"] + "-" + peer["ubiq.avatar.skin.uuid"]);
        }

        private void Start()
        {
            if (avatar.IsLocal)
            {
                LoadSettings();
            }
        }

        //This is only to change the skin textures of UI menu -> preview avatars. 
        public void SetUbiqSkins(int skinUUID)
        {
            var texture = AvatarSkins.Get(skinUUID+"");
            OnTextureChanged.Invoke(texture);
        }

        public void SetTexture(string uuid)
        {
            if(String.IsNullOrWhiteSpace(uuid))
            {
                return;
            }

            if (this.uuid != uuid)
            {
                int intUUID = Convert.ToInt16(uuid);
                if(intUUID >= AvatarSkins.Count || intUUID < 0)
                {
                    //when change the avatar bodies, there are different no. of skins in AvatarSkins catlog.
                    //Therefore update the uuid if the previously loaded uuid is not valid index for
                    //currently selected avatar body skins catelogue list
                    intUUID = UnityEngine.Random.Range(0, AvatarSkins.Count);
                    SetSettings(intUUID);
                }

                
    
                this.uuid = intUUID + ""; 
                var texture = AvatarSkins.Get(this.uuid);
                this.cached = texture;
                
                OnTextureChanged.Invoke(texture);
                
                if(avatar.IsLocal)
                {
                    avatar.Peer["ubiq.avatar.skin.uuid"] = this.uuid;
                }
            }
        }

        private void LoadSettings()
        {
            //var uuid = PlayerPrefs.GetString("ubiq.avatar.skin.uuid", "0");
            AvatarProfile activeAvatarRef = AvatarProfileHandler.GetActiveAvatarProfile();
            var uuid = activeAvatarRef.ubiqAvatarSkinUUID == -1 
                ? UnityEngine.Random.Range(0, AvatarSkins.Count) : activeAvatarRef.ubiqAvatarSkinUUID;
            
            SetTexture(uuid+"");
        }

        private void SetSettings(int skinuuid)
        {
            AvatarProfile activeAvatarRef =  AvatarProfileHandler.GetActiveAvatarProfile();
            activeAvatarRef.ubiqAvatarSkinUUID = skinuuid;

            AvatarProfileHandler.UpdateActiveProfile(activeAvatarRef);
        }

        public void ClearSettings()
        {
            PlayerPrefs.DeleteKey("ubiq.avatar.skin.uuid");
        }

        public Texture2D GetTexture()
        {
            return cached;
        }
    }
}
