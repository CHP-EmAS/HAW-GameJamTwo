using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Devtools
{
    public class GUIFps : MonoBehaviour
    {
        private const KeyCode enableCode = KeyCode.P;
        [SerializeField] private bool useToggle = false;
        private bool showConsole = true;
        private void Start()
        {
            showConsole = !useToggle;
        }

        private void Update()
        {
            if (useToggle)
            {
                if (Input.GetKeyDown(enableCode)) showConsole = !showConsole;
            }
        }

        private void OnGUI()
        {
            float y = 0;
            float x = Screen.width;

            //https://stackoverflow.com/questions/32251805/calculating-the-frame-rate-in-a-unity-scene
            float frameRate = (int)1.0f / Time.unscaledDeltaTime;
            GUI.TextField(new Rect(x - 100f, y + 5, x - 20f, 20f), "FPS: " + frameRate);
        }
    }

}
