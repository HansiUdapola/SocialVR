using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ubiq.Rooms;

public class JoinRoom : MonoBehaviour
{
    public SocialMenu mainMenu;
    public GameObject RoomManagerPanel;
    public Text joincodeText;
    public TextEntry textEntry;
    public Image textInputArea;
    public string failMessage;
    public Color failTextInputAreaColor;

    private Color defaultTextInputAreaColor;
    private string lastRequestedJoincode;

    private void OnEnable()
    {
        mainMenu.roomClient.OnJoinedRoom.AddListener(RoomClient_OnJoinedRoom);
        mainMenu.roomClient.OnJoinRejected.AddListener(RoomClient_OnJoinRejected);
        defaultTextInputAreaColor = textInputArea.color;
    }

    private void OnDisable()
    {
        mainMenu.roomClient.OnJoinedRoom.RemoveListener(RoomClient_OnJoinedRoom);
        mainMenu.roomClient.OnJoinRejected.RemoveListener(RoomClient_OnJoinRejected);
        textEntry.SetText("",textEntry.defaultTextColor,true);
        textInputArea.color = defaultTextInputAreaColor;
    }

    private void RoomClient_OnJoinedRoom(IRoom room)
    {
        RoomManagerPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void RoomClient_OnJoinRejected(Rejection rejection)
    {
        if (rejection.joincode != lastRequestedJoincode)
        {
            return;
        }

        textEntry.SetText(failMessage,textEntry.defaultTextColor,true);
        textInputArea.color = failTextInputAreaColor;
        StartCoroutine(ResetTextFieldToDefault(3));
    }

    // Called through the UI Join Button
    public void Join()
    {
        lastRequestedJoincode = joincodeText.text.ToLowerInvariant();

        //Entering already joined room code
        if(mainMenu.roomClient.JoinedRoom 
        && mainMenu.roomClient.Room.JoinCode.ToLower() == lastRequestedJoincode.ToLower())
        {
             textEntry.SetText("Already Joined!",textEntry.defaultTextColor,true);
             StartCoroutine(ResetTextFieldToDefault(3));
             return;
         }

        mainMenu.roomClient.Join(joincode:lastRequestedJoincode);        
    }

    IEnumerator ResetTextFieldToDefault(int secs)
    {
        yield return new WaitForSeconds(secs);
        textEntry.SetText("",textEntry.defaultTextColor,true);
        textInputArea.color = defaultTextInputAreaColor;
    }
}
