using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ubiq.Rooms;

//This class will update the ubiq social name for given UI Text field
public class PlayerNameSetter : MonoBehaviour
{
    public Text playerNameText;
    private SocialMenu socialMenu;

    private void Awake()
    {
        socialMenu = GetComponentInParent<SocialMenu>();
    }

    private void Start()
    {
        AttemptInit();
    }

    private void OnEnable()
    {
        AttemptInit();
    }

    private void AttemptInit()
    {
        if (socialMenu && socialMenu.roomClient)
        {
            // Multiple AddListener calls still only adds one listener
            socialMenu.roomClient.OnPeerUpdated.AddListener(RoomClient_OnPeer);
            UpdatePanel();
        }
    }

    private void OnDisable()
    {
        if (socialMenu.roomClient)
        {
            socialMenu.roomClient.OnPeerUpdated.RemoveListener(RoomClient_OnPeer);
        }
    }

    private void RoomClient_OnPeer(IPeer peer)
    {
        if (socialMenu && socialMenu.roomClient && socialMenu.roomClient.Me == peer)
        {
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        if (socialMenu && socialMenu.roomClient && socialMenu.roomClient.Me != null)
        {
            var name = socialMenu.roomClient.Me["ubiq.social.name"];
            playerNameText.text = name != null ? name : "(unnamed)";
        }
    }
}
