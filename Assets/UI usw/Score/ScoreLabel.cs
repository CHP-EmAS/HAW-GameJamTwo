using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Plum.Base;

namespace Music.UI
{
    public class ScoreLabel : Singleton<ScoreLabel>
    {
    public int score = 0;
    public TMP_Text scoreLabel;

    public void UpdateScore()
    {
        score += 100;
        scoreLabel.SetText(score.ToString());
    }
    }
}
