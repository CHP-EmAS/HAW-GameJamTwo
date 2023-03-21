using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseUI.Utils{
    public class BUI_ColorPicker : MonoBehaviour
    {
        [SerializeField] private Slider rSlid, gSlid, bSlid;
        [SerializeField] private Image colorDisplay;
        public UnityEngine.Events.UnityEvent onColorChanged;
        public Utility.GenericDelegate<Color> onColorUpdated;
        private Color color; public Color Color{get{return color;}}
        
        public void LoadColor(Color col){
            rSlid.value = col.r;
            gSlid.value = col.g;
            bSlid.value = col.b;
            OnValueChanged();
        }
        public void OnValueChanged(){
            color = new Color(rSlid.value, gSlid.value, bSlid.value, 1.0f); //<- currently ignoring alpha channel
            colorDisplay.color = color;
            onColorUpdated?.Invoke(color);

            onColorChanged?.Invoke();
        }
    }

}
