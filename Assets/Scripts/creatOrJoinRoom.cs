using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class creatOrJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text roomName;

    [SerializeField]
    private GameObject roomNameGO;

    [SerializeField]
    private GameObject columnJoinRoom;

    [SerializeField]
    private GameObject columnLeaveRoom;

    public void OnClickCreatJoinRoom()
    {
        if (!PhotonNetwork.IsConnected) return;

        RoomOptions options = new RoomOptions();
        //options.CleanupCacheOnLeave = false;
        options.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(string.Format("created {0} successfully", roomName.text));
    }

    public override void OnJoinedRoom()
    {
        columnJoinRoom.SetActive(false);
        columnLeaveRoom.SetActive(true);

        // display current room name
        roomNameGO.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.Name;

        // instantiate player prefab from resources folder for everyone
        PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(string.Format("creating {0} failed due to", roomName.text) +  message);
    }

    public override void OnLeftRoom()
    {
        columnLeaveRoom.SetActive(false);
        columnJoinRoom.SetActive(true);

        listMolecules listOfMolecules = GameObject.Find("listMolecules").GetComponent<listMolecules>();
        foreach (GameObject button in listOfMolecules.buttonList)
        {
            Destroy(button);
        }
        listOfMolecules.buttonList.Clear();
    }
}
