using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.UI
{
    public class UI_Handler : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu.Instance.SwitchPause();
            }
        }
    }
}

