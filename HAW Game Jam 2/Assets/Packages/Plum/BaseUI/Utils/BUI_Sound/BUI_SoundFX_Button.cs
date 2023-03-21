using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Sound{
    //v Attach this class to make SoundFX appear when using UI
    public class BUI_SoundFX_Button : MonoBehaviour
    {
        [SerializeField] private BUI_FX_Button button;
        [SerializeField] private AudioClip onEnter, onUp, onClick;
        [SerializeField, Header("Optional - leave null to use BUI_SoundMaster")] private AudioSource source;
        private void Start(){
            if(button == null) button = GetComponent<BUI_FX_Button>();

            button.onDown += PlayOnClick;
            button.onEnter += PlayOnEnter;
            button.onUp += PlayOnUp;
        }

        private bool PlaySafe(AudioClip clip){
            if(source == null){
                BUI_SoundMaster.Instance.PlaySound(clip);
                return false;
            }
            return true;
        }
        private void PlayOnEnter(){
            if(PlaySafe(onEnter) )source.PlayFastOneShotSafe(onEnter);
        }

        private void PlayOnUp(){
            if(PlaySafe(onUp)) source.PlayFastOneShotSafe(onUp);
        }

        private void PlayOnClick(){
            if(PlaySafe(onClick)) source.PlayFastOneShotSafe(onClick);
        }
    }

}
