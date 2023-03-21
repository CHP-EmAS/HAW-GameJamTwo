using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseUI{
    //v This class basically works like a gameObject container, specified for UI usage
    public class BUI_Page : MonoBehaviour
    {
        [SerializeField] private List<GameObject> associated = new List<GameObject>();
        [SerializeField, Header("Settings")] private bool isDefaultPage = false;
        [SerializeField] private bool neverUpdateStatics = false;       //<- set this to true to allow to open pages additively!
        [SerializeField, Tooltip("Only applies when neverUpdateStatic is false!"), Header("Optional")] private bool nonStaticDefault = false;
        [SerializeField] private bool addAllChildrenToList = false;
        [SerializeField] private BUI_Page parentPage;
        [SerializeField] private UnityEngine.UI.Selectable firstSelected; public UnityEngine.UI.Selectable FirstSelectable{get{return firstSelected;}}
        private bool isOpen; public bool IsOpen{get{return isOpen;}}
        public Utility.ArgumentelessDelegate onOpen, onClose;
        protected bool isPauseMenu = false;

        protected virtual void Awake()
        {
            if (parentPage != null)
            {
                parentPage.onClose += Close;
                if (neverUpdateStatics)
                {
                    parentPage.onOpen += NonStaticEnable;
                }
            }
        }

        //v optionally fills list with children & assigns default
        protected virtual void Start(){
            if(isDefaultPage){
                BUI_Manager.AssignActivePage(this);
            }

            if(addAllChildrenToList){
                associated.AddRange(transform.GetAllChildren());
            }

            if(!isPauseMenu) Set(isDefaultPage); //<- guarantee order. But ignore if pause menu

            if(neverUpdateStatics){
                Set(nonStaticDefault);
            }
        }

        private void NonStaticEnable()
        {
            if (neverUpdateStatics) Set(nonStaticDefault);
        }

        //v set the state!
        private void SetState(bool n){
            foreach (GameObject i in associated)
            {
                i.SetActive(n);
            }
        }

        //Open or Close but through an argument        
        public virtual void Set(bool n){
            System.Action action = n? Open : Close;
            action?.Invoke();
        }

        public virtual void Toggle()
        {
            //Debug.Log(gameObject.name + " is open: " + false);
            Set(!isOpen);
        }

        //v open the page
        public virtual void Open(){
            if(isDefaultPage) BUI_Manager.ClearPages();     //<- before the update we want to clear the list IF its the default page, to avoid backtracing from B1 - C1 for example
            if(!neverUpdateStatics) BUI_Manager.UpdateActivePage(this);
            SetState(true);
            onOpen?.Invoke();
            if(isDefaultPage) BUI_Manager.SetDefaultPageState(true);
            
            isOpen = true;

            OnOpen();

            //V selection-flow only necessary if controllers are active
            if(!Plum.Base.Controls.HasJoyStickActive()) return;
            if(firstSelected != null) firstSelected.Select();
        }


        //v close the page
        public virtual void Close(){
           // Debug.Log(gameObject.name + " is closing? " + associated[0].name);
            SetState(false);
            onClose?.Invoke();
            if(isDefaultPage) BUI_Manager.SetDefaultPageState(false);

            isOpen = false;
            OnClose();
        }


        protected virtual void OnOpen(){}
        protected virtual void OnClose(){}
        public void BackTrace() => BUI_Manager.BackTrace();
    }
}
