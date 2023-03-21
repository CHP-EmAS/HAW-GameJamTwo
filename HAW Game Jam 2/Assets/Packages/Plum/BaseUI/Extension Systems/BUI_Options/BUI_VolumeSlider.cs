using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI 
{
    //v use this to change and save volume!:)
    public class BUI_VolumeSlider : BUI_Ev
    {
        [SerializeField] private BUI_Page associatedPage;
        [SerializeField] private UnityEngine.UI.Slider slider;
        private void Start(){
            //v subscribing the delegate
            associatedPage.onClose += OnPageEnd;
            slider.value = BUI_PlayerSettings.Instance.GetSettings().volume;
            slider.onValueChanged.AddListener(delegate { VolUpdate(slider.value); });
        }

        private void VolUpdate(float i)
        {
            Event();
        }

        //v updating volume
        public override void Event()
        {
            BUI_PlayerSettings.Instance.UpdateAudioSettings(slider.value);
        }

        //v saving on end
        private void OnPageEnd(){
            BUI_PlayerSettings.Instance.SaveSettings();
        }
    }
 
}
