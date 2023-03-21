using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace BaseUI{
    public class BUI_Scroller : MonoBehaviour
    {
        [SerializeField] private Slider scroll;
        [SerializeField] private Transform toOffset;
        [SerializeField] private Axis applicationAxis; 
        private Vector3 initialPos;
        public void InitScroll(float maxLength){
            initialPos = toOffset.position;
            scroll.value = 0;
            scroll.maxValue = maxLength * .001f;      //<- This changed through camera canvas mode <.<
        }

        public void OnScroll(){
            toOffset.position = -Utility.ReadAxis(applicationAxis) * scroll.value + initialPos;
        }
    }

}
