using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class listMolecules : MonoBehaviour
{
    public GameObject CodeButton;
    public GameObject ButtonParent;

    [HideInInspector]
    public List<GameObject> buttonList = new List<GameObject>();

    public bool instantiateCodeButton(string baseUrl, string codeName)
    {
        // check if button already exist
        int index = buttonList.FindIndex(x => x.name == codeName);
        if (index != -1)
        {
            Debug.Log(string.Format("{0} already exists!!", codeName));
            return false;
        }

        // instantiate a loaded code button
        GameObject newCodeButton = Instantiate(CodeButton, ButtonParent.transform);

        // change the name of the button to code name
        newCodeButton.name = codeName;
        newCodeButton.transform.Find("Text").gameObject.GetComponent<Text>().text = codeName;

        // pass on the code name and base Url to button instance
        LoadedMoleculeButton loadedMoleculeButton = newCodeButton.GetComponent<LoadedMoleculeButton>();
        loadedMoleculeButton.MoleculeCode = codeName;
        loadedMoleculeButton.BaseUrl = baseUrl;

        buttonList.Add(newCodeButton);
        return true;
    }

    public void RemoveButton(string codeName)
    {
        int index = buttonList.FindIndex(x => x.name == codeName);
        if (index != -1)
        {
            Destroy(buttonList[index]);
            buttonList.RemoveAt(index);
        }
    }
}
