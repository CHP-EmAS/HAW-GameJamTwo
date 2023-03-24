using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music.UI
{
    public partial class Slider0 : MonoBehaviour
    {
        public partial void OnSliderT(float i)
        {

        }
    }

#if UNITY_EDITOR
    public class SliderInspector : Plum.Base.CustomInspector<SliderInspector>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif
}

