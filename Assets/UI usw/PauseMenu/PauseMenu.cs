using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Plum.Base;

namespace Music.UI
{
    public class PauseMenu : Plum.Base.Singleton<PauseMenu>
    {
        [SerializeField] private bool startPaused = false;
        private void Start()
        {
            paused = !startPaused;
            SwitchPause();
        }
        public static bool isPaused
        {
            get => paused;
        }
        private static bool paused = false;
        public void SwitchPause()
        {
            paused = !isPaused;
            gameObject.SetActive(isPaused);
            TimeManager.PauseOrContinue(isPaused);
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
}

