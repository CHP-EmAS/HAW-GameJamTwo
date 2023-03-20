using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class PlayerMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private Rigidbody refBody;
        [SerializeField, Range(0, 1)] private float smoothness = .1f;
        [SerializeField] private float speed = 5.0f;
        private void FixedUpdate()
        {
            Vector3 targetDir = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                0,
                Input.GetAxisRaw("Vertical"));
            Move(targetDir);
        }
        public void Move(Vector3 dir)
        {
            refBody.velocity = Vector3.SmoothDamp(refBody.velocity, dir * speed, ref rfv0, smoothness);
        }
        private Vector3 rfv0;
    }

}
