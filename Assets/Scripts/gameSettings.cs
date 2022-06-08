using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSettings : MonoBehaviourPunCallbacks
{
    // this is a 'singleton' class to store info for local user.
    public static gameSettings Instance;

    [SerializeField]
    private string _nickName = "defaultUser";

    [SerializeField]
    public string gameVersion = "0.0.1";

    public PhotonView localUserPV { set; get; }

    public string AzureAnchorId { set; get; }

    public string NickName
    {
        get
        {
            int value = Random.Range(0, 999);
            return _nickName + value.ToString();
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance == this) return;
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

}
