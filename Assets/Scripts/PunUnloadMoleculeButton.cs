using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PunUnloadMoleculeButton : MonoBehaviour
{
    private PhotonView pv;
    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }
    
    public void PUN_unloadMoleculeButton(string moleculeName)
    {
        pv.RPC("PunRPC_unloadMoleculeButton", RpcTarget.AllBuffered, moleculeName);
    }

    [PunRPC]
    private void PunRPC_unloadMoleculeButton(string moleculeName)
    {
        GameObject ListMoleculesGO = GameObject.Find("listMolecules");
        ListMoleculesGO.GetComponent<listMolecules>().RemoveButton(moleculeName);
    }
}
