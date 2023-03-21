using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Animation
{
    public class BUI_FX_Rotation : MonoBehaviour
    {
        [SerializeField] private BUI_FX_Button targetButton;
        [SerializeField] private float speed = 1;
        [SerializeField] private float posterization = 0;
        private float tracker;
        private bool allowedFX = true;
        private void Start(){
            if(targetButton != null){
                targetButton.onEnter += OnSelected;
                targetButton.onExit  += OnExit;
            }
        }

        private void OnSelected(){
            allowedFX = false;
        }
        private void OnExit(){
            allowedFX = true;
        }
        private void Update(){
            if(!allowedFX) return;
            tracker += Time.deltaTime * speed;

            transform.eulerAngles = new Vector3(0, 0, posterization != 0? Utility.Posterize(tracker, posterization) : tracker);
        }
    }

}
