using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.VFX;

namespace Music.Player
{
    public class MainCam : Plum.Base.Singleton<MainCam>
    {
        [SerializeField] private Screenshake shake;
        private Vector3 initialRot;
        private void Start()
        {
            initialRot = transform.localEulerAngles;
        }
        public static void RequestShake(float ampMul = 1.0f, float lMul = 1.0f)
        {
            Instance.StartCoroutine(Instance.shake.Shake(ampMul, lMul, Instance.UpdateShake));
        }

        private void UpdateShake(Vector3 s)
        {
            transform.localEulerAngles = initialRot + s;
        }
    }

}
