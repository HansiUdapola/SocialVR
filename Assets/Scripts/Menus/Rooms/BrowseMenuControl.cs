using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//BrowseRoomsItemTemplate Menu Binding process
public class BrowseMenuControl : MonoBehaviour
{
    public Text RoomName;
    public Text SceneName; //Scene Name (Change the scene Name Into readable name)
    public RawImage ScenePreview;
    public Text NoOfPeers;

    [System.Serializable]
    public class BindEvent : UnityEvent<RoomClient, IRoom> { }; //New Unity Event
    public BindEvent OnBind;

    private string existing;

    private RoomClient roomClient;
    private string joincode;

    public void Bind(RoomClient client, IRoom roomInfo)
    {
        RoomName.text = roomInfo.Name;
        SceneName.text = roomInfo["scene-name"];

        var image = roomInfo["scene-image"];
        if (image != null && image != existing)
        {
            client.GetBlob(roomInfo.UUID, image, (base64image) =>
            {
                if (base64image.Length > 0)
                {
                    var texture = new Texture2D(1, 1);
                    texture.LoadImage(Convert.FromBase64String(base64image));
                    existing = image;
                    ScenePreview.texture = texture;
                }
            });
        }
            this.roomClient = client;
            this.joincode = roomInfo.JoinCode;

            OnBind.Invoke(client,roomInfo);
    }

    //called through the Social Menu Public rooms search -> join button onclick
    public void JoinToAPublicRoom()
    {
        if (!roomClient || string.IsNullOrEmpty(joincode))
        {
            return;
        }

        roomClient.Join(joincode:joincode);
    }

}
