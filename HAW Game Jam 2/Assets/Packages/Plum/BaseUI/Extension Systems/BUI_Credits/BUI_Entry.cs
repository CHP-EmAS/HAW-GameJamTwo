using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BaseUI{
    public class BUI_Entry : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI left, middle, right;
        public virtual void Fill(string left, string middle, string right){
            this.left.text = left;
            this.middle.text = middle;
            this.right.text = right;
        }
    }
}
