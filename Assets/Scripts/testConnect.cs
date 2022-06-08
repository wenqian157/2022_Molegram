using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class testConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text localPlayerName;

    [SerializeField]
    private GameObject columnJoinServer;

    [SerializeField]
    private GameObject columnJoinRoom;

    [SerializeField]
    private GameObject columnLeaveRoom;

    [SerializeField]
    private GameObject columnPlayerList;

    public void OnClickConnectServer()
    {
        PhotonNetwork.NickName = localPlayerName.text;
        PhotonNetwork.AutomaticallySyncScene = true; // to sync clients level

        Debug.Log("connect to server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("server connected.");
        //Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        PhotonNetwork.JoinLobby(); // join default lobby to receive room list update

        // update interface on the popped out Lobby menu
        columnJoinServer.SetActive(false);
        columnJoinRoom.SetActive(true);
        columnPlayerList.SetActive(true);
    }

    public void OnClickDisconnectServer()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconnected to server due to" + cause.ToString());

        // update interface on the popped out Lobby menu
        columnJoinServer.SetActive(true);
        columnJoinRoom.SetActive(false);
        columnLeaveRoom.SetActive(false);
        columnPlayerList.SetActive(false);
    }

}
