using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Plum.Base;
using UnityEngine.SceneManagement;

namespace Music.UI
{
    public class DeathScreen : Singleton<DeathScreen>
    {
        [SerializeField] AudioSource RecordScratch;
        public TMP_Text GameOverLabel;
        private int highscore;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void GameOver(int score)
        {
            highscore = PlayerPrefs.GetInt("HighScore");
            if (score > highscore)
            {
                GameOverLabel.SetText("new highscore " + score.ToString());
                PlayerPrefs.SetInt("HighScore", score);
            } else
            {
                GameOverLabel.SetText("game over");
            }
        }

        public void HoverOverButton(TMP_Text hoverButton)
        {
            hoverButton.fontStyle = FontStyles.Bold;
            RecordScratch.Play();
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
            Plum.Base.TimeManager.PauseOrContinue(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
