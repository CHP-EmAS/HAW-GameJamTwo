using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;

namespace BaseUI.Utils{
    [RequireComponent(typeof(UnityEngine.UI.GridLayoutGroup))]
    public abstract class BUI_SelectionParent<T> : IntersectionNode<BUI_SelectionParent<T>, BUI_SelectableCard<T>>
    {
        [SerializeField, Header("DEPENDENCY: SelectableCard is attached at this gameObject!")] private GameObject selectionCardPrefab;
        private List<BUI_SelectableCard<T>> spawned = new List<BUI_SelectableCard<T>>();
        protected override void Start()
        {
            base.Start();
            selectionCardPrefab.SetActive(false);
        }
        public void Load(T[] toLoad){
            List<BUI_SelectableCard<T>> additionalCards = new List<BUI_SelectableCard<T>>();
            for (int i = 0; i < toLoad.Length; i++)
            {
                //v still in range!
                if(i < spawned.Count){
                    spawned[i].Load(toLoad[i]);
                }else{
                    //outside of range, new Card is required
                    GameObject spawned = Instantiate(selectionCardPrefab, transform);
                    spawned.SetActive(true);
                    BUI_SelectableCard<T> card = spawned.GetComponent<BUI_SelectableCard<T>>();
                    card.Initialize(this);
                    card.Load(toLoad[i]);
                    additionalCards.Add(card);
                }
            }
            spawned.AddRange(additionalCards);
        }
    }
}
