using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

public class NetworkedPlayer : MonoBehaviourPun
{

    [SerializeField]
    private Text nameGO;

    private PhotonView photonView;

    void Start()
    {
        GameObject tableAnchor = GameObject.Find("/TableAnchor");
        if (tableAnchor)
        {
            Debug.Log("found table");
            transform.SetParent(tableAnchor.transform);
        }

        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine) return;

        gameSettings.Instance.localUserPV = photonView;
        photonView.RPC("PunRPC_SetNicName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void ShareAzureAnchorId()
    {
        if (photonView != null)
            photonView.RPC("PunRPC_ShareAzureAnchorId", RpcTarget.AllBuffered,
                gameSettings.Instance.AzureAnchorId);
        else
            Debug.LogError("photonView is null");
    }

    [PunRPC]
    public void PunRPC_SetNicName(string name)
    {
        gameObject.name = name;
        nameGO.text = name;
    }

    [PunRPC]
    private void PunRPC_ShareAzureAnchorId(string ID)
    {
        gameSettings.Instance.AzureAnchorId = ID;

        Debug.Log("azureAnchorId: " + gameSettings.Instance.AzureAnchorId);
        Debug.Log("Azure Anchor ID shared by user: " + photonView.Controller.UserId);
    }

}

