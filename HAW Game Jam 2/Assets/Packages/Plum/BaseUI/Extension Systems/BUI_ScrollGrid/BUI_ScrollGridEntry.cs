using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Grid{
    public abstract class BUI_ScrollGridEntry : Plum.Base.IntersectionModule<BUI_ScrollGrid, BUI_ScrollGridEntry>
    {
        public abstract void Generate();
    }
}
