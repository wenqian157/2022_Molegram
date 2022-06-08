using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class listPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerName;

    private List<Player> playerList = new List<Player>();

    private List<GameObject> playerListGo = new List<GameObject>();

    public override void OnJoinedRoom()
    {
        foreach (KeyValuePair<int, Player> playerinfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerList(playerinfo.Value);
        }
    }

    private void AddPlayerList(Player player)
    {
        GameObject menu = GameObject.Find("/UGUI/Menu");
        Transform parentTrans = menu.transform.Find("LobbyUGUI/UGUIScrollViewContent/Scroll View/Viewport/Content/GridLayout/ColumnPlayerList");
        GameObject playerGo = Instantiate(playerName, parentTrans);
        playerGo.GetComponent<TextMeshProUGUI>().text = player.NickName;
        playerList.Add(player);
        playerListGo.Add(playerGo);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerList.FindIndex(x => x == otherPlayer);
        Destroy(playerListGo[index].gameObject);
        playerListGo.RemoveAt(index);
        playerList.RemoveAt(index);
    }

    public override void OnLeftRoom()
    {
        foreach (GameObject playerGo in playerListGo)
        {
            Destroy(playerGo);
        }

        playerListGo.Clear();
        playerList.Clear();
    }
}
