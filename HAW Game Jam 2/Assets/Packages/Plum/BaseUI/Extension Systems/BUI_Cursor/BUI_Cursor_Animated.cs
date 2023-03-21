using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Cursor
{
    public class BUI_Cursor_Animated : BUI_CustomCursor
    {
        [SerializeField] private Plum.Curve.DynamicCurve curve;
        private float last;
        protected override void Start(){
            base.Start();
        }
        protected override void Update(){
            base.Update();
            float newScale = Mathf.Lerp(0, 1, velocity.magnitude * .02f);
            transform.up = velocity.normalized;     //<- should be smoothed out
            last = curve.GetValueFloat(last, newScale, Time.unscaledDeltaTime, 0);
            float value = velocity.magnitude >= .1f? newScale : last;;
            transform.localScale = Vector3.one + new Vector3(-Mathf.Clamp(value, 0.0f, .9f), Mathf.Clamp(value, 0.0f, 1.0f), 0.0f);
        }
    }
}
