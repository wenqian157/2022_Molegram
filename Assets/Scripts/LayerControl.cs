using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LayerControl : MonoBehaviour
{
    private PhotonView photonView;
    public void DisplayLayerOnOff(string layer)
    {
        photonView = gameSettings.Instance.localUserPV;
        photonView.gameObject.GetComponent<PunLayerControl>().PunDisplayLayerOnOff(layer);
    }

}
