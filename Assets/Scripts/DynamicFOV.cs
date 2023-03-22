using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class DynamicFOV : MonoBehaviour
    {
        [SerializeField] private Rigidbody refBody;
        [SerializeField] private float refVelocity;
        [SerializeField, Header("FOV")] private float lowerFOV = 50, upperFOV = 90;
        [SerializeField, Range(0, 1)] private float smoothness = .5f;
        private Camera cam;
        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            float t = refBody.velocity.magnitude / refVelocity;
            float fov = Mathf.Lerp(lowerFOV, upperFOV, t);
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, fov, ref rfv0, smoothness);
        }
        private float rfv0;
    }

}
