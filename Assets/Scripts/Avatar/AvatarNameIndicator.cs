using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Avatars;

namespace Ubiq.FloatingAvatar{

    [RequireComponent(typeof(Text))]
    public class AvatarNameIndicator : MonoBehaviour
    {
        private Text text;
        public Avatars.Avatar avatar;

        private void Awake()
        {
            text = GetComponent<Text>();
        }
    
        private void Start()
        {
            //avatar = GetComponentInParent<Avatars.Avatar>();

            if(avatar){
                Debug.Log("HAVE a AVATAR");
            }else{
                Debug.Log("NO  AVATAR");
            }

            if (!avatar || avatar.IsLocal)
            {
                text.enabled = false;
                
                return;
            }
            
            avatar.OnPeerUpdated.AddListener(Avatar_OnPeerUpdated);
        }

        private void OnDestroy()
        {
            if (avatar)
            {
                avatar.OnPeerUpdated.RemoveListener(Avatar_OnPeerUpdated);
            }
        }

        //On avatar joined the name TEXT indicator is updating (top of avatars)
        private void Avatar_OnPeerUpdated (IPeer peer)
        {
            UpdateName();
        }

        private void UpdateName()
        {
            text.text = avatar.Peer["ubiq.social.name"] ?? "(unnamed)";
        }  
   
    }
}
