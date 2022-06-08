using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class joinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text roomName;
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

}
