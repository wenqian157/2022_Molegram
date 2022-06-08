using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    [SerializeField]
    private GameObject lobby;

    [SerializeField]
    private GameObject ASA;

    [SerializeField]
    private GameObject loadMolecules;

    [SerializeField]
    private GameObject displaySettings;

    [SerializeField]
    private GameObject lobbyButton;

    [SerializeField]
    private GameObject ASAButton;

    [SerializeField]
    private GameObject loadMoleculesButton;

    [SerializeField]
    private GameObject displaySettingsButton;

    public void DisplayLobbyMenu()
    {
        lobby.SetActive(!lobby.activeSelf);
    }

    public void DisplayASAMenu()
    {
        ASA.SetActive(!ASA.activeSelf);
    }

    public void DisplayLoadingMenu()
    {
        loadMolecules.SetActive(!loadMolecules.activeSelf);
    }

    public void DisplaySettingMenu()
    {
        displaySettings.SetActive(!displaySettings.activeSelf);
    }

    public void OnOffLobbyToggle()
    {
        lobbyButton.GetComponent<Interactable>().IsToggled = false;
    }
    public void OnOffASAToggle()
    {
        ASAButton.GetComponent<Interactable>().IsToggled = false;
    }
    public void OnOffLoadingToggle()
    {
        loadMoleculesButton.GetComponent<Interactable>().IsToggled = false;
    }
    public void OnOffDisplaySettingToggle()
    {
        displaySettingsButton.GetComponent<Interactable>().IsToggled = false;
    }
}
