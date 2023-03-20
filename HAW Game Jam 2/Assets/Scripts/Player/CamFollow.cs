using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Player
{
    [RequireComponent(typeof(Camera))]
    public class CamFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField, Range(0, 5)] private float smoothness = 1.0f;
        private Camera cam;
        [SerializeField]  private Vector3 offset;
        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = target.position;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref rfv0, smoothness);
        }
        private Vector3 rfv0;
    }

}
