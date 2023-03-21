using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;

namespace BaseUI{
    //v manage the whole UI-System
    public static class BUI_Manager
    {
#region INPUT
        private static bool inputApplied;                       //<- the input is only relevant on page management
        private static List<BUI_Page> pageList = new List<BUI_Page>(); public static int AmountOfPages(){return pageList.Count;}
        public static void ClearPages()
        {
            pageList.Clear();
        }

        public static bool ReadyToBackTrace(BUI_Page from){
            return pageList.Count > 1;
        }
        public static void BackTrace(){
            OnEscape();
        }
        private static void OnEscape(){
            if (pageList.Count <= 1) return;    //<- no backtrace if not enough pages to do so
            if(defaultPageIsOn) return;     //<- no backtrace on default page
            try{
                //v first we selected the intended pages
                BUI_Page currentPage = pageList[pageList.Count - 1];
                BUI_Page lastPage = pageList[pageList.Count - 2];       //<- we get last at max-1, so 

                //v because we now trace back, we can remove the active page
                pageList.Remove(activePage);                            //<- the active page can also go to hell
                //v then we actually close the activePage
                UpdateActivePage(lastPage);                             //<- lastPage gets removed in there
                //v and open the prev page
                lastPage.Open();
            }
            catch{
#if UNITY_EDITOR
                Debug.Log("Backtrace failed! - full page list: ");
                GenericUtility<BUI_Page>.LogWholeArray(pageList.ToArray());
#endif
                //no last page, we are back at default
            }
        }

#endregion
        //v pause menu management
#region PAUSEMENU
        private static bool pauseMenuIsOn, pauseMenuIsEnabled;
        public static void UpdatePauseMenu(bool isOn, bool isEnabled){
            pauseMenuIsOn = isOn;
            pauseMenuIsEnabled = isEnabled;
        }
#endregion

        //v page management
#region PAGES
        private static BUI_Page activePage; public static BUI_Page GetActivePage(){return activePage;} 
        private static BUI_Page lastActive; public static BUI_Page GetLastActivePage(){return lastActive;} 
        private static bool defaultPageIsOn = true; public static void SetDefaultPageState(bool dir){
            defaultPageIsOn = dir;
        }
        public static void UpdateActivePage(BUI_Page newActive){
            if(activePage != null && activePage != newActive) activePage.Close();       //<- avoiding closure while opening- this could lead to weird behaviour
            activePage = newActive;

            //v this would mean we actually backtraced, so it can be deleted
            if(pageList.Contains(newActive)){
                pageList.Remove(newActive);
            }
            else{
                pageList.Add(newActive);
            }
        }

        public static void ForceClose(){
            if(activePage != null) activePage.Close();
            ClearPages();
        }


        public static void AssignActivePage(BUI_Page active){
            //v this should always happen on some Init-State
            //v first check necessary input
            if(!inputApplied){
                //v thus the input can be applied here, when a new page is selected!
                InputHandler.Instance.OnPaused += OnEscape;
            }

            if(activePage != null){
                if(active != null) Debug.Log("Warning! Page was updated-despite it already containing the instance of " + active + " " + active.GetInstanceID());
            }
            activePage = active;

            //v this indicates a new UI-Subsystem so we can clear the whole list - UpdateActivePage should be called afterwards either way
            if(active == null){
                ClearPages();
            }
        }
#endregion
    }
}
