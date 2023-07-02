using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Rooms;
using UnityEngine;

public class BrowseRooms : MonoBehaviour
{
    public float roomRefreshInterval = 2.0f; 
    public SocialMenu mainMenu;
    public BrowseMenuControl joinedControl;
    public Transform controlsRoot;
    public GameObject roomListPanel;
    public GameObject noRoomsMessagePanel;
    public GameObject controlTemplate;

    private List<BrowseMenuControl> controls = new List<BrowseMenuControl>();
    private List<IRoom> lastRoomArgs;
    private float nextRoomRefreshTime = -1;

    //Added to listen the room leave event
    public RoomManager roomManager;

    private void OnEnable()
    {
        roomManager.OnRoomLeft.AddListener(OnRoomLeft);
        mainMenu.roomClient.OnRoomsDiscovered.AddListener(RoomClient_OnRoomsDiscovered);
        mainMenu.roomClient.OnJoinedRoom.AddListener(RoomClient_OnJoinedRoom);
        UpdateAvailableRooms();
    }

    private void OnRoomLeft()
    {
        joinedControl.gameObject.SetActive(false);
        Debug.Log("OnLeft");
       
    }

    private void OnDisable()
    {
        if (mainMenu.roomClient)
        {
            mainMenu.roomClient.OnRoomsDiscovered.RemoveListener(RoomClient_OnRoomsDiscovered);
            mainMenu.roomClient.OnJoinedRoom.RemoveListener(RoomClient_OnJoinedRoom);

        }
    }

    private BrowseMenuControl InstantiateControl () {
        var controlTemplateGameObj = GameObject.Instantiate(controlTemplate, controlsRoot);
        controlTemplateGameObj.SetActive(true);
        return controlTemplateGameObj.GetComponent<BrowseMenuControl>();
    }

    private void RoomClient_OnJoinedRoom(IRoom room)
    {

        joinedControl.gameObject.SetActive(false);        
        UpdateAvailableRooms();

        // Immediately ask for a refresh - maybe room we left is now empty
        mainMenu.roomClient.DiscoverRooms();
    }

    private void RoomClient_OnRoomsDiscovered(List<IRoom> rooms,RoomsDiscoveredRequest request)
    {
        // Ignore filtered requests
        if (string.IsNullOrEmpty(request.joincode))
        {
            lastRoomArgs = rooms;
            UpdateAvailableRooms();
        }
    }

    private void UpdateAvailableRooms() {
        var rooms = lastRoomArgs;

        //Set No Room Msg if there are no any public rooms were found
        if (rooms == null || rooms.Count == 0)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Destroy(controls[i].gameObject);
            }

            controls.Clear();
            noRoomsMessagePanel.SetActive(true);
            roomListPanel.SetActive(false);
            return;
        }

        //Otherwise show the list of rooms by binding the data to BrowseRoomsItemTemplate
        noRoomsMessagePanel.SetActive(false);
        roomListPanel.SetActive(true);

        int controlI = 0;
        for (int roomI = 0; roomI < rooms.Count; controlI++,roomI++)
        {
            if (mainMenu.roomClient.JoinedRoom &&
                mainMenu.roomClient.Room.UUID == rooms[roomI].UUID && mainMenu.roomClient.Room.Publish)
            {
                //joinedControl v'ble refer the BrowseRoomsItemTemplate binding
                joinedControl.gameObject.SetActive(true);
                joinedControl.Bind(mainMenu.roomClient,rooms[roomI]);

                controlI--;
                continue;
            }

            if(!mainMenu.roomClient.Room.Publish)
                joinedControl.gameObject.SetActive(false);//**

            //Create a dynamic controlTemplateGameObj
            if (controls.Count <= controlI) {
                controls.Add(InstantiateControl());
            }
            //Pass data to bind on UI elements
            controls[controlI].Bind(mainMenu.roomClient,rooms[roomI]);
        }

        while (controls.Count > controlI) {
            Destroy(controls[controlI].gameObject);
            controls.RemoveAt(controlI);
        }
        
    }

    private void Update()
    {
        //periodically discover rooms to update the room list
        if (Time.realtimeSinceStartup > nextRoomRefreshTime)
        {
            mainMenu.roomClient.DiscoverRooms();
            nextRoomRefreshTime = Time.realtimeSinceStartup + roomRefreshInterval;
        }
    }
}
