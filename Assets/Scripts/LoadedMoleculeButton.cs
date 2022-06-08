using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using MLS_Backend_Structure;
using TMPro;

public class LoadedMoleculeButton : MonoBehaviour
{
    public string MoleculeCode;
    public string BaseUrl;

    private MoleculeBase moleculeBase;
    private GameObject unloader;
    private GameObject moleculeNameText;
    private GameObject moleculeInfoText;

    private void Start()
    {
        unloader = GameObject.Find("moleculeUnloader");
        moleculeNameText = GameObject.Find("Text (TMP) CodeName");
        moleculeInfoText = GameObject.Find("Text (TMP) CodeInfo");
    }

    public void ClickDisplayInfo()
    {
        unloader.GetComponent<PunUnloadMolecule>().MoleculeCode = MoleculeCode;
        moleculeNameText.GetComponent<TextMeshProUGUI>().text = MoleculeCode;
        StartCoroutine(loadMoleculeBaseInfo());
    }

    public void ClearDisplayedInfo()
    {
        unloader.GetComponent<PunUnloadMolecule>().MoleculeCode = "";
        moleculeNameText.GetComponent<TextMeshProUGUI>().text = "Select a loaded Molecule";
        moleculeInfoText.GetComponent<TextMeshProUGUI>().text =
            "Detail Info \n" +
            "......\n" +
            "......\n" +
            "......\n";
    }

    private IEnumerator loadMoleculeBaseInfo()
    {
        string jsonUrl = string.Format("{0}base/{1}", BaseUrl, MoleculeCode);
        using (WWW www = new WWW(jsonUrl))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                moleculeBase = JsonConvert.DeserializeObject<Molecule>(www.text, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            else Debug.Log("Base info cannt be found");
        }

        string infoText = string.Format(
            "Name: {0}\n\n" +
            "AtomCount: {1}\n\n" +
            "SequenceCount: {2}\n\n" +
            "Method: {3}\n\n" +
            "Resolution: {4}\n\n" +
            "LastModified: {5}\n\n", 
            moleculeBase.Name, moleculeBase.AtomCount, moleculeBase.SequenceCount, moleculeBase.Method, moleculeBase.Resolution, moleculeBase.LastModified
            );

        moleculeInfoText.GetComponent<TextMeshProUGUI>().text = infoText;
    }
}
