using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Player
{
    public class MouseRotator : MonoBehaviour
    {
        private const float maxRotDegrees = 45.0f;
        [SerializeField] private Transform toRotate, toAfterRotate, head;
        [SerializeField, Range(0, 1)] private float smoothness = .1f;
        private Camera refCam;
        private Vector3 lastTargetPosition;
        private void Start()
        {
            refCam = Camera.main;
            lastTargetPosition = toAfterRotate.transform.position + toRotate.forward;
        }

        //https://docs.unity3d.com/ScriptReference/Plane.Raycast.html
        private void Update()
        {
            Plane plane = new Plane(Vector3.up, toRotate.position);
            Ray r = refCam.ScreenPointToRay(Input.mousePosition);
            float e = 0;
            if (plane.Raycast(r, out e))
            {
                Vector3 hitPoint = r.GetPoint(e);
                Vector3 nFWD = (hitPoint - toRotate.position).normalized;

                toRotate.forward = Vector3.SmoothDamp(toRotate.forward, nFWD, ref rfv0, smoothness);
                head.forward = Vector3.SmoothDamp(head.forward, Vector3.Normalize(nFWD + new Vector3(0.0f, 0.3f, 0.0f)), ref rfv2, smoothness * 2);
            }

            float angle = Vector3.Angle(toAfterRotate.forward, toRotate.forward);
            if(angle >= maxRotDegrees){
                lastTargetPosition = Vector3.SmoothDamp(lastTargetPosition, toAfterRotate.transform.position + toRotate.forward, ref rfv0, smoothness);
            }
            toAfterRotate.LookAt(lastTargetPosition + new Vector3(0, .05f, 0.0f), -Vector3.up);
        }
        private Vector3 rfv0, rfv1, rfv2;
    }

}
