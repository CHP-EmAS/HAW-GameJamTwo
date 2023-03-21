using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Plum.Base;

namespace Plum.Curve
{
    public delegate float CurveMethod(float currentPos, float target, ref DynamicCurve from, ref float v, ref float lt, float dt);
    public enum EquationType{
        SMOOTH = 0,
        SPRING = 1
    }

    [System.Serializable]
    public struct DynamicCurve
    {
        public Vector3 lastTarget;
        public EquationType equationType;
        public float speed;
        public float a0, a1, a2, intensity;
        private Vector3 refVel; public void SetRefVel(Vector3 val) => refVel = val;
        public static CurveMethod GetFunc(EquationType type){
            switch (type)
            {
                case EquationType.SMOOTH:
                    return CurveFormulas.Smooth;

                case EquationType.SPRING:
                    return CurveFormulas.SecondOrder;

                default:
                return null;
            }
        }

        private float GetValue(float from, float to, ref float v, ref float lastTarget, float dt){
            dt = Mathf.Clamp(dt, .0001f, 999.999f);
            return GetFunc(equationType)(from, to, ref this, ref v, ref lastTarget, dt);
        }

        public float GetValueFloat(float from, float to, float dt){
            return GetValue(from, to, ref refVel.x, ref lastTarget.x, dt);
        }

        public float GetValueFloat(float from, float to, float dt, int slice){
            if(slice == 0) {
                return GetValue(from, to, ref refVel.x, ref lastTarget.x, dt);
            } else if (slice == 1){
                return GetValue(from, to, ref refVel.y, ref lastTarget.y, dt);
            } else{
                return GetValue(from, to, ref refVel.z, ref lastTarget.z, dt);
            }
        }

        public float GetValueFloat(float from, float to, ref float v, ref float lt, float dt){
            return GetValue(from, to, ref v, ref lt, dt);
        }

        public Vector3 GetValueV3(Vector3 from, Vector3 to, float dt){
            return GetValueV3(from, to, ref refVel, ref lastTarget, dt);
        }
        
        public Vector3 GetValueV3(Vector3 from, Vector3 to, ref Vector3 v, ref Vector3 lt, float dt){
            Vector3 product = new Vector3(
                GetValue(from.x, to.x, ref v.x, ref lt.x, dt),
                GetValue(from.y, to.y, ref v.y, ref lt.y, dt),
                GetValue(from.z, to.z, ref v.z, ref lt.z, dt)
            );
            return product;
        }

        public Vector2 GetValueV2(Vector2 from, Vector2 to, float dt){
            return GetValueV2(from, to, ref refVel, ref lastTarget, dt);
        }

        public Vector2 GetValueV2(Vector3 from, Vector3 to, ref Vector3 v, ref Vector3 lt, float dt){
            Vector2 product = new Vector3(
                GetValue(from.x, to.x, ref v.x, ref lt.x, dt),
                GetValue(from.y, to.y, ref v.y, ref lt.y, dt)
            );
            return product;
        }
    }
}
