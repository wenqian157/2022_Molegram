using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

#region Enum
public enum EMoleculeLayerType
{
    BallsAndSticks,
    VdW,
    Sas,
    Cartoon,
    Pocket,
    Ligand,
}
#endregion

public class PunLoadMolecules : MonoBehaviour
{
    #region Properties
    public string BaseURL = "http://molegram.ethz.ch/api/molecule";

    [Space(10)]
    public Text MoleculeCode;

    private object[] data;

    private PhotonView pv;

    #endregion
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public void SpawnPunMolecule()
    {
        StartCoroutine(checkMoleculeCode()); //check if the code is valid
    }

    private IEnumerator checkMoleculeCode()
    {
        string jsonUrl = string.Format("{0}/{1}", BaseURL, MoleculeCode.text);
        Debug.Log("molecule: " + MoleculeCode.text + " \nfrom: " + jsonUrl);

        using (WWW www = new WWW(jsonUrl))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error)) spawnPunMolecule();
            else Debug.Log("Code is invalid, please re-enter!!");
        }
    }

    private void spawnPunMolecule()
    {
        Debug.Log(string.Format("Spawn {0} for all", MoleculeCode.text));

        data = new object[] {MoleculeCode.text, PhotonNetwork.NickName, BaseURL};
        //PhotonNetwork.Instantiate("PunMolecule", transform.position, transform.rotation, 0, data);
        //PhotonNetwork.InstantiateRoomObject("PunMolecule", transform.position, transform.rotation, 0, data);

        pv.RPC("RPC_CreateObject_MasterClient", RpcTarget.MasterClient, data);
    }

    [PunRPC]
    void RPC_CreateObject_MasterClient(object[] data)
    {
        PhotonNetwork.InstantiateRoomObject("PunMolecule", transform.position, transform.rotation, 0, data);
    }
}
