using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Curve;
//https://www.youtube.com/watch?v=9hTnlp9_wX8&t=650s

namespace Plum.DD
{
    //v line animation
    public class LineAnim3D : MonoBehaviour
    {
        [SerializeField] private DynamicCurve movementCurve;
        private struct Segment
        {
            public Vector3 Move(ref DynamicCurve movementCurve, Vector3 targetPosition, Vector3 refPos, float maxDST, float dt)
            {
                position = movementCurve.GetValueV3(position, targetPosition, ref refV1, ref refv2, dt);

                //v do we want to clamp max?
                if (maxDST > 0)
                {
                    //v if so, figure out if it overshoots the threshhold
                    Vector3 distance = position - refPos;
                    if (distance.magnitude > maxDST)
                    {
                        //v and if so, get it back!
                        Vector3 newPosition = refPos + (distance.normalized) * maxDST;
                        position = newPosition;
                    }
                }

                return position;
            }

            public Vector3 position;
            private Vector3 refV1, refv2;
        }

        [SerializeField] private UpdateMode mode = UpdateMode.UPDATE;

        [Header("Line Settings"), Range(1, 100)]
        [SerializeField] private bool clampDST = true;
        [SerializeField, Range(0, 100)] private float maxDST = 1.0f;
        //[SerializeField, Range(0, 10.0f)] private float moveTimeInSec = 1.0f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        [SerializeField, Range(-2, 2)] private float gravityScale = 1.0f;
        [SerializeField] private Transform[] transforms;
        private Vector3[] positions;
        private Segment[] segments;
        private void Awake()
        {
            segments = new Segment[transforms.Length];
            positions = new Vector3[transforms.Length];
            InitSegments();
        }

        private void InitSegments()
        {
            segments[0].position = transform.position;
            positions[0] = transform.position;
            transforms[0].position = transform.position;

            for (int i = 1; i < segments.Length; i++)
            {
                Vector3 pos = segments[i - 1].position + Vector3.up * maxDST;
                positions[i] = pos;
                transforms[i].position = pos;
            }
        }

        private void Update()
        {
            if (mode == UpdateMode.UPDATE) UpdateLine(Time.deltaTime);
            transforms[0].position = transform.position;
        }

        private void FixedUpdate()
        {
            if (mode == UpdateMode.FIXEDUPDATE) UpdateLine(Time.fixedDeltaTime);
        }


        private void UpdateLine(float dt)
        {
            segments[0].position = transform.position;
            positions[0] = transform.position;
            transforms[0].position = transform.position;

            for (int i = 1; i < segments.Length; i++)
            {
                Vector3 pos = segments[i - 1].position + (segments[i].position - segments[i - 1].position).normalized * maxDST;
                float maxDSTlcl = clampDST ? maxDST : -1;
                positions[i] = segments[i].Move(ref movementCurve, pos + (gravity * gravityScale), segments[i - 1].position, maxDSTlcl, dt);
                transforms[i].position = pos;
                transforms[i].up = (transforms[i].position - transforms[i - 1].position).normalized;
            }
        }
    }
}
