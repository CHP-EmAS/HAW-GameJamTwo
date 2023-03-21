using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace BaseUI{
    public class BUI_FullScreen : BUI_Ev
    {
        [SerializeField] private BUI_Page associatedPage;
        [SerializeField] private UnityEngine.UI.Toggle toggle;
        private void Start(){
            //v subscribing the delegate
            associatedPage.onClose += OnPageEnd;
            toggle.isOn = BUI_PlayerSettings.Instance.GetSettings().fullScreen;
            toggle.onValueChanged.AddListener(delegate{ToggleChanged(toggle.isOn);});
        }

        private void ToggleChanged(bool ev){
            Event();
        }

        //v updating volume
        public override void Event()
        {
            BUI_PlayerSettings.Instance.UpdateFullScreenSettings(toggle.isOn);
        }

        //v saving on end
        private void OnPageEnd(){
            BUI_PlayerSettings.Instance.SaveSettings();
        }
    }

}
