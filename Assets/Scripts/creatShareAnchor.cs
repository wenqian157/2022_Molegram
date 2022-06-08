using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatShareAnchor : MonoBehaviour
{
    private AnchorModuleScript anchorModuleScript;

    void Start()
    {
        anchorModuleScript = GetComponent<AnchorModuleScript>();
;    }

    public void creatAndShareAnchor(GameObject anchorObj)
    {
        if (checkCreateID()) {
            Debug.Log("\nID found, donnot create again!!");
            return;
        }
        anchorModuleScript.CreateAzureAnchor(anchorObj);
        StartCoroutine(ShareAzureAnchor());
    }

   IEnumerator ShareAzureAnchor()
    {
        while(true)
        {
            if (checkCreateID())
            {
                Debug.Log("\n" + PhotonNetwork.NickName + " ShareAzureAnchor");
                gameSettings.Instance.AzureAnchorId = anchorModuleScript.currentAzureAnchorID;
                Debug.Log("gameSettings.Instance.azureAnchorId: " + gameSettings.Instance.AzureAnchorId);

                var photonViewLocal = gameSettings.Instance.localUserPV;
                var networkedPlayerLocal = photonViewLocal.gameObject.GetComponent<NetworkedPlayer>();

                networkedPlayerLocal.ShareAzureAnchorId();
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        
    }

    public void GetAzureAnchor()
    {
        Debug.Log("\nGetAzureAnchor()");
        Debug.Log("azureAnchorId: " + gameSettings.Instance.AzureAnchorId);

        anchorModuleScript.FindAzureAnchor(gameSettings.Instance.AzureAnchorId);

        GameObject table = GameObject.Find("/Functions/tableAnchor");
        Debug.Log("new table location: " + table.transform.position);
    }

    private bool checkCreateID()
    {
        if (anchorModuleScript.currentAzureAnchorID != "") {
            Debug.Log("ID has been created!!");
            return true;
        } 
        return false;
    }


}
