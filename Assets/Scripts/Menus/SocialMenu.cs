using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ubiq.Rooms;
using Ubiq.Messaging;
using Ubiq.XR;

public class SocialMenu : MonoBehaviour
{
    //To keep reference the social menu is open or closed.
    public enum State
    {
        Open,
        Closed
    }

    public NetworkScene networkSceneOverride;
    // To Provide central access to NetworkScene for the whole socialMenu
    private NetworkScene _networkScene;
    public NetworkScene networkScene
    {
        get
        {
            if (networkSceneOverride)
            {
                return networkSceneOverride;
            }

            if (!_networkScene)
            {
                _networkScene = NetworkScene.FindNetworkScene(this);
            }

            return _networkScene;
        }
    }

    // To Provide central access to RoomClient for the whole socialMenu
    private RoomClient _roomClient;
    public RoomClient roomClient
    {
        get
        {
            if (!_roomClient)
            {
                if (networkScene)
                {
                     _roomClient = networkScene.GetComponent<RoomClient>();
                 }
            }

            return _roomClient;
        }
    }

    [Serializable]
    public class SocialMenuEvent : UnityEvent<SocialMenu,State> { }
    public SocialMenuEvent OnStateChange;

    [System.NonSerialized]
    public State state;

    //On enable set as social menu is opened.
    private void OnEnable()
    {
        state = State.Open;
        OnStateChange.Invoke(this,state);
    }

    //On disable set as social menu is closed.
    private void OnDisable()
    {
        state = State.Closed;
        OnStateChange.Invoke(this,state);
    }

    //On request to open the social menu spawn it relative to the player transform
    public void Request ()
    {
        var cam = Camera.main.transform;

        //transform.position = cam.TransformPoint(spawnRelativeTransform.localPosition);
        //transform.rotation = cam.rotation * spawnRelativeTransform.localRotation;
        //gameObject.SetActive(true);
    }

}
