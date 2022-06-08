using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;

public class PunLayerControl : MonoBehaviour
{
    private Camera cam;
    private PhotonView photonView;
    private GameObject buttonParent;
    private List<string> layersList;


    private void Awake()
    {
        cam = Camera.main;
        photonView = GetComponent<PhotonView>();

        string[] layersArray = { "Default", "UI", "BallsAndSticks", "SAS", }; //, "Cartoon", "VanDerWaals",  "Pocket", "Ligand" };
        layersList = new List<string>(layersArray);

        GameObject menu = GameObject.Find("/UGUI/Menu");
        buttonParent = menu.transform.Find("DisplayUGUI/ButtonCollection").gameObject;

        cam.cullingMask = LayerMask.GetMask(layersList.ToArray());
    }

    public void PunDisplayLayerOnOff(string layer)
    {
        photonView.RPC("PunRPC_setCullingMask", RpcTarget.AllBuffered, layer);
    }

    [PunRPC]
    private void PunRPC_setCullingMask(string layer)
    {
        //cam.cullingMask = 1 << 0;
        //int layer1 = LayerMask.NameToLayer("SAS");
        //cam.cullingMask = 1 << layer1;
        //int layer2 = LayerMask.NameToLayer("MyLayer2");
        //cam.cullingMask = (1 << layer1) | (1 << layer2);

        if (!this.layersList.Contains(layer))
        {
            this.layersList.Add(layer);
        }
        else
        {
            this.layersList.Remove(layer);
        }
        cam.cullingMask = LayerMask.GetMask(this.layersList.ToArray());

        //sync button states
        if (!photonView.IsMine)
        {
            GameObject button = buttonParent.transform.Find(layer).gameObject;
            Interactable interactable = button.GetComponent<Interactable>();
            interactable.IsToggled = !interactable.IsToggled;
        }
    }

}
