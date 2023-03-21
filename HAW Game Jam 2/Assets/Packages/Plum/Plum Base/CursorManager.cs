using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //Manages cursor
    public static class CursorManager
    {
        //WIP
        private static bool RequestCursorChange(bool wanted){
            bool condition = true;
            return condition;
        }

        //Enable or disable cursor
        public static void SetCursor(bool dir){
            if(dir){
                if(RequestCursorChange(dir)) EnableCursor();
            }
            else{
                if(RequestCursorChange(dir)) DisableCursor();
            }
        }

        public static void SetCursorVisibility(bool dir){
            if(BaseUI.Cursor.BUI_CustomCursor.Instance != null){
                BaseUI.Cursor.BUI_CustomCursor.Instance.SetCursor(dir);
                return;
            }
            Cursor.visible = dir;
        }

        public static void ChangeCursorSprite(Texture2D s){
            Cursor.SetCursor(s, Vector2.zero, CursorMode.Auto);
        }

        private static void EnableCursor(){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private static void DisableCursor(){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
