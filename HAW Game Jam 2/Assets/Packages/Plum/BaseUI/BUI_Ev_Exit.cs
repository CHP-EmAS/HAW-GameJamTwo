using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI{
    //v close the whole application
    public class BUI_Ev_Exit : BUI_Ev
    {
        public override void Event()
        {
            Application.Quit();
        }
    }
}
