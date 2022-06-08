using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PunUnloadMolecule : MonoBehaviourPun
{
    public GameObject ListMolecules;
    private GameObject loadedMolecule;
    private PhotonView localUserPV;

    [HideInInspector]
    public string MoleculeCode = "";

    public void ClickDestroyMolecule()
    {
        if (MoleculeCode == "") return;
        else
        {
            // pun destroy loaded molecule game object
            Debug.Log("about to destroy " + MoleculeCode);
            loadedMolecule = GameObject.Find(MoleculeCode.ToString());
            PhotonView pv = loadedMolecule.GetComponent<PhotonView>();
            if (!pv.IsMine) pv.RequestOwnership();

            StartCoroutine(destroyWithDelay());

            // pun destroy loaded molecule button on canvas
            localUserPV = gameSettings.Instance.localUserPV;
            localUserPV.gameObject.GetComponent<PunUnloadMoleculeButton>().PUN_unloadMoleculeButton(MoleculeCode);
            //ListMolecules.GetComponent<listMolecules>().RemoveButton(MoleculeCode.ToString());
        }

    }
    public void ClickResetMolecule()
    {
        if (MoleculeCode == "") return;
        else
        {
            Debug.Log("reset the transform of " + MoleculeCode);
            loadedMolecule = GameObject.Find(MoleculeCode.ToString());
            PhotonView pv = loadedMolecule.GetComponent<PhotonView>();

            pv.RPC("PunRPC_resetMolecule", RpcTarget.AllBuffered);
        }
    }

    private IEnumerator destroyWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        PhotonNetwork.Destroy(loadedMolecule);
    }

}
