using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Grid{
    public class BUI_ScrollGrid : Plum.Base.IntersectionNode<BUI_ScrollGrid, BUI_ScrollGridEntry>
    {
        //START has to be called BEFORE
        [SerializeField] private BUI_ScrollGridEntry referencePrefab;
        protected void GenerateGrid(int amt, System.Action<BUI_ScrollGridEntry> onInstanceCreated){
            for (int i = 0; i < amt; i++)
            {
                BUI_ScrollGridEntry g = Instantiate(referencePrefab, transform.position, Quaternion.identity, referencePrefab.transform.parent).GetComponent<BUI_ScrollGridEntry>();
                g.gameObject.name = "scrollEntry | " + i;
                onInstanceCreated?.Invoke(g);
                InitChild(g);
            }
        }
    }
}
