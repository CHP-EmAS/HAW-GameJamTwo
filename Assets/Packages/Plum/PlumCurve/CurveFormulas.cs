using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Curve
{
    public static class CurveFormulas
    {
        const float PI = 3.14159f;
        //programming gems 4 1.10
        public static float Smooth(float currentPos, float target, ref DynamicCurve curve, ref float v, ref float lt, float dt){
            float omega = (2.0f * curve.a1/ curve.speed * curve.a0);
            float omDT = omega * dt;
            
            float approxEXP = 1.0f / (1.0f + omDT + .48f * omDT * omDT + .235f *omDT*omDT*omDT);
            float diff = currentPos - target;

            float n = Mathf.Abs(curve.intensity);
            diff = Mathf.Clamp(diff, -curve.intensity, curve.intensity);

            float cached = (v + omega*diff) * dt;
            v = (v - omega*cached)*approxEXP;
            
            float y1 = (diff + cached)*approxEXP;
            return target + y1;
        }

        //https://www.youtube.com/watch?v=WEknchhPr7E
        public static float SecondOrder(float currentPos, float target, ref DynamicCurve curve, ref float v, ref float lastTarget, float dt){
            float k1 = curve.a1 / (PI * curve.a0);
            float k2 = 1 / ((2 * PI * curve.a0) * (2 * PI * curve.a0));
            float k3 = curve.a2 * curve.a1 / (2 * PI * curve.a0);

            v = Mathf.Clamp(v, -curve.intensity, curve.intensity);
            float approximatedNewPosition = currentPos + v * dt * curve.speed;
            float xVel = (target - lastTarget) / Mathf.Clamp(dt, .0000000001f, 999.999f);      

            float accelleration = (target + k3 * xVel - approximatedNewPosition - k1 * v) / k2;      //<- second order solved for accelleration
            v += dt * accelleration;

            lastTarget = target;
            return approximatedNewPosition;
        }
    }
}
