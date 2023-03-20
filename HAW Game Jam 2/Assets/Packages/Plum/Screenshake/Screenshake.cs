using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=fn3hIPLbSn8

namespace Plum.VFX
{
    [System.Serializable]
    public class Screenshake
    {
        public float baseAmplitude, baseDuration;
        public AnimationCurve faloff;
        private float shakeRotationZ, shakeRotationX;
        public float ShakeRotationZ { get => shakeRotationZ; set => shakeRotationZ = value; }
        public float ShakeRotationX { get => shakeRotationX; set => shakeRotationX = value; }
        public Vector3 GetRotation(){
            return new Vector3(shakeRotationX, 0.0f, shakeRotationZ);
        }
        public Vector2 GetRotation2D(){
            return new Vector2(shakeRotationX, shakeRotationZ);
        }

        public IEnumerator Shake(float amplitude, float duration, Utility.VectorDelegate updateShake)
        {
            float elapsed = 0;
            float Randomization = Random.Range(0, 150);
            amplitude *= baseAmplitude;
            duration *= baseDuration;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                shakeRotationZ = PerlinShake(elapsed + Randomization, amplitude * faloff.Evaluate(elapsed / duration), true);
                shakeRotationX = PerlinShake(elapsed + 1 + (Randomization * .5f), amplitude * faloff.Evaluate(elapsed / duration), false);  //1 as "offset" to differentiate
                updateShake?.Invoke(GetRotation());
                yield return new WaitForEndOfFrame();
            }

            updateShake?.Invoke(GetRotation());
            shakeRotationZ = 0;
            shakeRotationX = 0;
        }

        private float PerlinShake(float elapsed, float intensity, bool useSin)
        {
            float perlin = (Mathf.PerlinNoise(elapsed * intensity, elapsed * intensity) + 1) * .25f;
            float tan = 0;
            if (useSin) tan = Mathf.Sin(Time.time * 50);
            else tan = Mathf.Cos(Time.time * 50);

            return tan * perlin * intensity;
        }
    }

}
