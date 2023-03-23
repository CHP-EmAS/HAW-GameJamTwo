using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Unpause()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
