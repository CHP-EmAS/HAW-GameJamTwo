using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;

namespace BaseUI.Utils{
    public abstract class BUI_SelectableCard<T> : IntersectionModule<BUI_SelectionParent<T>, BUI_SelectableCard<T>>
    {
        protected T heldData;
        public virtual void Load(T data){
            heldData = data;
            OnLoad();
        }
        public abstract void OnSelect();
        protected virtual void OnLoad() { }
    }
}
