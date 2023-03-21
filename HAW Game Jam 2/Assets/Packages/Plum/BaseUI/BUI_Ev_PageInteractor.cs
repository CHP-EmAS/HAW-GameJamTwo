using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI{
    //v interact with a page
    public class BUI_Ev_PageInteractor : BUI_Ev
    {
        [SerializeField] BUI_Page toOpen;
        [SerializeField] private bool closeInstead;
        public override void Event()
        {
            if(toOpen != null) toOpen.Set(!closeInstead);
        }
    }

}
