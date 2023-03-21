using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseUI.Cursor
{
    [RequireComponent(typeof(Image))]
    public class BUI_CustomCursor : Plum.Base.Singleton<BUI_CustomCursor>
    {
        protected Vector2 velocity;
        protected Vector3 lastPosition;
        protected virtual void Start(){
            lastPosition = transform.position;
            UnityEngine.Cursor.visible = false;
        }
        public void SetCursor(bool dir){

        }
        protected virtual void Update(){
            transform.position = Plum.Base.Controls.MousePosition();
            velocity = transform.position - lastPosition;
            lastPosition = transform.position;
        }
    }
}
