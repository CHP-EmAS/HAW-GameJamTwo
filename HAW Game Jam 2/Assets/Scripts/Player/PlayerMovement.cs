using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Player
{
    public class PlayerMovement : MonoBehaviour, IMoveable
    {
        private const float shakeCooldown = 1.0f;
        private const float momentumDecrease = .05f;
        private const float dashIntensity = 3.0f;

        private float shakeTimer = 0.0f;
        [SerializeField] private Rigidbody refBody;
        [SerializeField, Range(0, 1)] private float smoothness = .1f;
        [SerializeField] private float speed = 5.0f;
        private Vector3 momentum, targetDir;
        private void Update()
        {
            targetDir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical"));

            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                return;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //would probably be nice on midi hit
                    AddForce(targetDir * dashIntensity);
                }
            }

        }
        private void FixedUpdate()
        {
            Move(targetDir);
        }
        public void Move(Vector3 dir)
        {
            refBody.velocity = Vector3.SmoothDamp(refBody.velocity, dir * speed, ref rfv0, smoothness) + momentum;
            momentum = Vector3.SmoothDamp(momentum, Vector3.zero, ref rfv1, momentumDecrease);
        }
        public void AddForce(Vector3 dir)
        {
            const float dirRef = 10.0f;
            float magnitude = dir.magnitude / dirRef;
            MainCam.RequestShake(1.0f * magnitude, 1.0f);
            momentum += dir;
            shakeTimer = shakeCooldown;
        }
        private Vector3 rfv0, rfv1;
    }

}