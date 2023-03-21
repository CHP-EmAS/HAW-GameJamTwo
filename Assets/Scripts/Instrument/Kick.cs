using UnityEngine;

namespace Instrument
{
    public class Kick : InstrumentBase
    {
        public float speed = 1.0f;
        public float duration = 1.0f;
        public AnimationCurve curve;

        private float timer = 0.0f;
        private Vector3 startPos;
        private Vector3 endPos;

        private void Start()
        {
            startPos = transform.position;
            endPos = transform.position + transform.up * 0.5f;
        }

        public override void PlayEffect()
        {
            timer = 0.0f;
        }

        private void Update()
        {
            if (timer < duration)
            {
                timer += Time.deltaTime * speed;
                float t = curve.Evaluate(timer / duration);
                transform.position = Vector3.Lerp(startPos, endPos, t);
            }
            else
            {
                transform.position = startPos;
            }
        }
    }
}