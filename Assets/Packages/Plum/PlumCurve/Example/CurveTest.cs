using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bun
{
    public class CurveTest : MonoBehaviour
    {
        [SerializeField] private Plum.Curve.DynamicCurve curve;
        [SerializeField] private Transform pivot, moveable;
        private Vector3 lastTarPos;

        private void Update(){
            //float xVel = (pivot.position - lastTarPos).x;
            //float x = curve.GetValueFloat(moveable.position.x, pivot.position.x);
            //moveable.position = new Vector3(x, moveable.position.y, moveable.position.z);
            lastTarPos = pivot.position;
            moveable.position = curve.GetValueV3(moveable.position, pivot.position, Time.deltaTime);
        }
    }
}
