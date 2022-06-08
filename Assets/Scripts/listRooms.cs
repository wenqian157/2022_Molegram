using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class listRooms : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject joinRoomButton;

    private List<RoomInfo> roomInfoList = new List<RoomInfo>();
    private List<GameObject> roomObjectList = new List<GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("updating room list");

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = roomInfoList.FindIndex(x => x.Name == info.Name);
                if (index != -1) {
                    roomInfoList.RemoveAt(index);
                    Debug.Log("to be destroyed index: " + index + ", room object:" + roomObjectList[index]);
                    Destroy(roomObjectList[index]);
                    roomObjectList.RemoveAt(index);
                } 
            }
            else
            {
                //do not create buttons for the same room
                int index = roomInfoList.FindIndex(x => x.Name == info.Name);
                if (index != -1) return;
                else
                {
                    roomInfoList.Add(info);

                    Transform parentTransform = GameObject.Find("ColumnJoinRoom").transform;
                    GameObject newRoomButton = Instantiate(joinRoomButton, parentTransform);
                    newRoomButton.transform.Find("Text").gameObject.GetComponent<Text>().text = info.Name;
                    roomObjectList.Add(newRoomButton);
                }

            }
        }

        Debug.Log("current room count: " + roomList.Count.ToString());

    }

    public override void OnLeftRoom()
    {
        //delete all roombuttons
        foreach (GameObject roomGo in roomObjectList)
        {
            Destroy(roomGo);
        }
        roomInfoList.Clear();
        roomObjectList.Clear();
    }

}

