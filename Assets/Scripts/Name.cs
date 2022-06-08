using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Name : MonoBehaviourPun
{
    [SerializeField]
    private Text nameGO;

    private void OnEnable()
    {
        nameGO.text = PhotonNetwork.NickName;

        //if (!photonView.IsMine)
        //{
        //nameGO.text = photonView.Owner.NickName;

        //}
    }

}
