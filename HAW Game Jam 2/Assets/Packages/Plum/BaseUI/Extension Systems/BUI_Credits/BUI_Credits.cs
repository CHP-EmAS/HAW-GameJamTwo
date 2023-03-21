using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BaseUI.Credits{
    public class BUI_Credits : BUI_Page
    {
        [SerializeField, Header("Credit-Options")] private BUI_SCROB_Credits loadedCredits;
        [SerializeField] private BUI_Page returnPage;
        [SerializeField] private RectTransform moveTransform;
        [SerializeField] private TextMeshProUGUI left, middle, right;
        [SerializeField] private float spacingY = 10, endSpacing = 20;
        [SerializeField] private BUI_EntryGrid prefabGrid;
        [SerializeField] private bool generateOnStart = true;
        [SerializeField] private float baseMoveSpeed = 1.0f;
        private float overallLength = 0;
        private float initialY = 0;
        protected override void Start(){
            base.Start();

            //v config
            left.text = loadedCredits.left;
            middle.text = loadedCredits.middle;
            right.text = loadedCredits.right;
            prefabGrid.gameObject.SetActive(false);
            initialY = prefabGrid.transform.position.y;

            //v generate
            if(generateOnStart) GenerateCredits();
        }

        protected override void OnOpen(){
            //reset move transform
            moveTransform.anchoredPosition3D = new Vector3(moveTransform.transform.position.x, initialY, 0);
        }

        private void EndCredits(){
            moveTransform.anchoredPosition3D = new Vector3(moveTransform.transform.position.x, initialY, 0);
            returnPage.Open();      //<- if this page isn't static it will automatically close
        }

        private void Update(){
            if(!IsOpen) return;
            float speed = !Input.anyKey? 1.0f : 4.0f;
            moveTransform.anchoredPosition3D += new Vector3(0, speed * baseMoveSpeed, 0);
            if(moveTransform.anchoredPosition3D.y >= overallLength + endSpacing){
                EndCredits();
            }
        }

        //actually generates the credits
        private void GenerateCredits(){
            for (int i = 0; i < loadedCredits.credits.Length; i++)
            {
                CreditCategory c = loadedCredits.credits[i];
                BUI_EntryGrid grid = CreateGrid(c.title);

                foreach (CreditEntry item in c.entries)
                {
                    grid.CreateEntry(item.title, item.second, item.third);    
                }
                
                overallLength += grid.GetHeight() + (spacingY * 2);
            }
        }

        //util
        private BUI_EntryGrid CreateGrid(string title){
            Vector3 targetPosition = prefabGrid.transform.position - new Vector3(0, overallLength, 0);
            BUI_EntryGrid grid = Instantiate(prefabGrid.gameObject, targetPosition, Quaternion.identity, prefabGrid.transform.parent).GetComponent<BUI_EntryGrid>();
            grid.GetComponent<RectTransform>().anchoredPosition = targetPosition;       //https://answers.unity.com/questions/1210522/trying-to-set-xy-poisiton-of-rect-transform.html
            grid.gameObject.SetActive(true);
            grid.Init(title);
            return grid;
        }
    }
}
