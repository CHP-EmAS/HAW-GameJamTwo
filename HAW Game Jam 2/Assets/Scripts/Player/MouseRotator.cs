using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Player
{
    public class MouseRotator : MonoBehaviour
    {
        [SerializeField] private Transform toRotate;
        [SerializeField, Range(0, 1)] private float smoothness = .1f;
        private Camera refCam;
        private void Start()
        {
            refCam = Camera.main;
            Debug.Log(refCam.gameObject.name);
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
            }
        }
        private Vector3 rfv0;
    }

}
