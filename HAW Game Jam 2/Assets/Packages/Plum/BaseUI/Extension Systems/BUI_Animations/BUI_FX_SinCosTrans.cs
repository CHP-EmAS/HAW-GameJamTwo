using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Animation{
    //Apply Sin/Cos effects on rotation and scale!
    public class BUI_FX_SinCosTrans : MonoBehaviour
    {
        [SerializeField] private Wave rotationWave, scaleXWave, scaleYWave;
        private float initialZrot;
        private Vector2 initialScale;
        private void Start(){
            initialZrot = transform.eulerAngles.z;
            initialScale = transform.localScale;
        }

        void Update()
        {
            transform.eulerAngles = new Vector3(0, 0, initialZrot + rotationWave.Value(Time.time));
            transform.localScale = new Vector3(initialScale.x + scaleXWave.Value(Time.time), initialScale.y + scaleYWave.Value(Time.time), transform.localScale.z);
        }
    }

}
