using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace BaseUI{
    public class BUI_EntryGrid : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI title;
        [SerializeField] private RawImage background;
        [SerializeField] protected BUI_Entry prefabEntry;
        [SerializeField] protected GridLayoutGroup layoutGroup;
        private int amountOfEntries;
        protected virtual void Start(){
            //v not needed
            prefabEntry.gameObject.SetActive(false);
            UpdateBKG();
        }
        public void Init(string title){
            this.title.text = title;
        }
        public virtual void CreateEntry(string left, string middle, string right){
            BUI_Entry entry = Instantiate(prefabEntry.gameObject, transform.position, Quaternion.identity, layoutGroup.transform).GetComponent<BUI_Entry>();
            entry.Fill(left, middle, right);
            entry.gameObject.SetActive(true);
            amountOfEntries++;
            UpdateBKG();
        }

        public virtual float GetHeight(){
            float value = background.rectTransform.sizeDelta.y + 50; //<- 25 approx. height of text
            return value;  
        }

        private void UpdateBKG(){
            background.rectTransform.sizeDelta = new Vector2(
                background.rectTransform.sizeDelta.x,
                layoutGroup.cellSize.y * amountOfEntries + layoutGroup.spacing.y * 2
            );
        }
    }

}
