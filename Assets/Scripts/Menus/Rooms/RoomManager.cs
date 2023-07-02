using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ubiq.Rooms;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public SocialMenu mainMenu;
    public GameObject notInRoomPanel;
    public GameObject inRoomPanel;
    public Text InputRoomName;
    public Text JoinCode;
    public Text DisplayRoomName;
    public RawImage ScenePreview;
    private string existing;

    public UnityEvent OnRoomLeft;




    //Called through the Social Menu UI with entered room name as text
    public void CreateandJoinARoom (bool state)
    {
        mainMenu.roomClient.Join(
            name: InputRoomName.text,
            publish: state);
    }

    public void LeaveRoom ()
    {
        if (mainMenu && mainMenu.roomClient)
        {
            OnRoomLeft.Invoke();
            mainMenu.roomClient.Leave();
        }
        
    }
    public void Start(){
        UpdateControls();
        mainMenu.roomClient.OnJoinedRoom.AddListener(RoomClient_OnJoinedRoom);
    }

    private void RoomClient_OnJoinedRoom(IRoom room)
    {
        UpdateControls();
    }

    //Update RoomManage Panel Controls (Not In room/In room)
    private void UpdateControls()
    {
        //If not room client object created successfully then check is already in a room
        if (!mainMenu.roomClient || mainMenu.roomClient.JoinedRoom)
        {
            notInRoomPanel.SetActive(false); //Switch panels not in Room -> In room panel
            inRoomPanel.SetActive(true);

            JoinCode.text = mainMenu.roomClient.Room.JoinCode.ToUpperInvariant();
            DisplayRoomName.text = mainMenu.roomClient.Room.Name;

            var image = mainMenu.roomClient.Room["scene-image"];
            if (image != null && image != existing)
            {
                mainMenu.roomClient.GetBlob(mainMenu.roomClient.Room.UUID, image, (base64image) =>
                {
                    if (base64image.Length > 0)
                    {
                        var texture = new Texture2D(1, 1);
                        bool v = texture.LoadImage(Convert.FromBase64String(base64image));
                        existing = image;
                        ScenePreview.texture = texture;
                    }
                });
            }
        }else{ //Not joined a room but have connected to the server successfully
            notInRoomPanel.SetActive(true); //Switch panels In Room -> NOT In room panel
            inRoomPanel.SetActive(false);
        }

        
    }


}
