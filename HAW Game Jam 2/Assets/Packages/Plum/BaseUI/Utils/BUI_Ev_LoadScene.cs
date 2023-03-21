using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Utils
{
    //v simply loads a scene
    public class BUI_Ev_LoadScene : BUI_Ev
    {
        [SerializeField] private string sceneName;
        public override void Event()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}

