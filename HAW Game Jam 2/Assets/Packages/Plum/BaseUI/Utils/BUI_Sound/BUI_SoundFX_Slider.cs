using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseUI.Sound{
    public class BUI_SoundFX_Slider : MonoBehaviour
    {
        [SerializeField] private Slider button;
        [SerializeField] private AudioClip onChange;
        [SerializeField, Header("Optional - leave null to use BUI_SoundMaster")] private AudioSource source;
        private void Start(){
            if(button == null) button = GetComponent<Slider>();

            button.onValueChanged.AddListener(PlayOnEnter);
        }

        private bool PlaySafe(AudioClip clip){
            if(source == null){
                BUI_SoundMaster.Instance.PlaySound(clip);
                return false;
            }
            return true;
        }
        private void PlayOnEnter(float v){
            if(PlaySafe(onChange) )source.PlayFastOneShotSafe(onChange);
        }
    }

}
