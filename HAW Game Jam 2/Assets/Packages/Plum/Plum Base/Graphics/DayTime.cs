using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    [RequireComponent(typeof(Light))]
    public class DayTime : Singleton<DayTime>
    {
        [SerializeField, Range(0, 24)] private float time; public float GetTime{get{return time;}}
        [SerializeField] private Gradient dayTime;
        [SerializeField] private AnimationCurve intensityCurve;
        [SerializeField] private Light light;
        [SerializeField] private Vector3 beginRot, endRot;
        public Utility.FloatDelegate onTimeUpdated;
        [SerializeField, Header("Debug")] private bool updateContiniously = false, startRandom = false;

        protected override void Awake(){
            base.Awake();
            if(light == null) light = GetComponent<Light>();
            if(startRandom) time = Random.Range(0, 24.0f);
            UpdateTime();
        }
        private void Update(){
            if(updateContiniously) UpdateTime();
        }
        [ContextMenu("Update-Time")]
        private void UpdateTime(){
            float t = time / 24;
            Color col = dayTime.Evaluate(t);
            float intensity = intensityCurve.Evaluate(t);

            light.intensity = intensity;
            light.color = col;

            float newT = time / 12.0f;      //<- 24 is max so 24/2 = 12
            float rotT = newT > 1? newT - 1 : newT;
            Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(beginRot), Quaternion.Euler(endRot), rotT);
            light.transform.rotation = rotation;
            onTimeUpdated?.Invoke(time);
        }

        public void UpdateControlled(float time, Color col){
            float t = time / 24;
            float intensity = intensityCurve.Evaluate(t);

            light.intensity = intensity;
            light.color = col;

            float newT = time / 12.0f;      //<- 24 is max so 24/2 = 12
            float rotT = newT > 1? newT - 1 : newT;
            Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(beginRot), Quaternion.Euler(endRot), rotT);
            light.transform.rotation = rotation;
            onTimeUpdated?.Invoke(time);
        }
    }
}
