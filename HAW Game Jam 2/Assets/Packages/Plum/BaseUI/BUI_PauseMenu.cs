using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;

namespace BaseUI{
    //v Pause menu!
    public partial class BUI_PauseMenu : BUI_Page
    {
        [SerializeField, Header("Pause menu specifics")] public bool scaleTimeTo0 = true;
        [SerializeField] private List<BUI_Page> childPages = new List<BUI_Page>();
        //v here as a singleton
        public static BUI_PauseMenu Instance{
            get{
                return instance;
            }
        }
        private static BUI_PauseMenu instance;

        public static Utility.ArgumentelessDelegate onOpened, onClosed;

        //v enabling the interaction with this menu
        public void SetStateRaw(bool i){
            //v disable
            if(!i){
                //v if disable, remove method
                InputHandler.Instance.OnPaused -= OnEscape;
                //v but is currently enabled
                if(currentState){
                    Set(false);
                    currentState = false;
                }
            }
            else{
                //if enabled, add method
                InputHandler.Instance.OnPaused += OnEscape;
                BUI_Manager.ClearPages();       //<- no need for pages if whole UI is closed
                base.Start();       //<- we want to init on IsEnabled!:) Also note: Please disable addAllChildren on start to save some performance xD
            }

            BUI_Manager.UpdatePauseMenu(currentState, i);      //<- never starts open
            canBeDone = i;
            currentState = false;                             
        }
        protected override void Awake(){
            instance = this;
            base.Awake();

            if(childPages.Count == 0){
                childPages.AddRange(transform.GetComponentsInChildren<BUI_Page>(true));
            }
        }

        protected override void Start()
        {
            isPauseMenu = true;
            SetStateRaw(true);
            Set(false);
            //We actually dont want to execute Start on Start
            return;
        }
        private bool canBeDone;
        private bool currentState;

        public void Continue(){
            ResumeGame();
            Close();
        }

        public void ResumeGame(){
            if (scaleTimeTo0) TimeManager.ResumeGame();
        }

        public void PauseGame(){
            if (scaleTimeTo0) TimeManager.PauseGame();
        }

        //v manual state updates
        public override void Close()
        {
            base.Close();
            currentState = false;
            onClosed?.Invoke();
        }

        public override void Open()
        {
            base.Open();
            currentState = true;
            onOpened?.Invoke();
        }

        private void OnEscape(){
            if(canBeDone) ChangeState();
        }


        public override void Set(bool n)
        {
            base.Set(n);
            if(n) PauseGame();
            else ResumeGame();
        }

        //v change the state
        public void ChangeState(){
            //v if there is still possibility to backtrace we ignore
            if(BUI_Manager.ReadyToBackTrace(this)) return;

            //v otherwise we change and update
            Set(!currentState);
            BUI_Manager.UpdatePauseMenu(currentState, true);       //<- callback if we want some further events
            if(!currentState) BUI_Manager.ForceClose();
        }
    }
}
