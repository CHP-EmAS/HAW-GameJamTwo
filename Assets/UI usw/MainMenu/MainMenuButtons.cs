using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject CreditsPanel;

    private void Start()
    {
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Leos Devroom");
    }

    public void OpenSettings(TMP_Text buttonText)
    {
        buttonText.fontStyle = FontStyles.Normal;

        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OpenCredits(TMP_Text buttonText)
    {
        buttonText.fontStyle = FontStyles.Normal;

        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu(TMP_Text buttonText)
    {
        buttonText.fontStyle = FontStyles.Normal;

        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void HoverOverButton(TMP_Text hoverButton)
    {
        hoverButton.fontStyle = FontStyles.Bold;
    }

    public void HoverExitButton(TMP_Text hoverButton)
    {
        hoverButton.fontStyle = FontStyles.Normal;
    }
}
