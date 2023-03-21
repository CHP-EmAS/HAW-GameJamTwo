using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI{
    //v load a scene!
    public class BUI_Ev_SceneLoader : BUI_Ev
    {
        [SerializeField] private string sceneName = "";
        public override void Event()
        {
            //Load Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);  
        }
    }
}
