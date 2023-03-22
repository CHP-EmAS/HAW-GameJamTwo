using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void HoverOverButton(TMP_Text hoverButton)
    {
        hoverButton.fontStyle = FontStyles.Bold;
    }

    public void HoverExitButton(TMP_Text hoverButton)
    {
        hoverButton.fontStyle = FontStyles.Normal;
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        //Spiel von vorne starten
        return;
    }
}
