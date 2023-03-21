using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public static partial class Controls
    {
#region UTILS
        public static bool HasJoyStickActive(){
            string[] names = Input.GetJoystickNames();
            bool result = false;
            foreach (string item in names)
            {
                if(!result){
                    //v one name is enough!
                    result = item != "";
                }
            }
            return result;
        }

#endregion

#region VECTOR
        public static Vector2 MousePosition(){
            return Input.mousePosition;
        }

        public static Vector3 MousePosWSP(){
            Vector3 mp = MousePosition();
            return ServiceLocator.mouseCamera.ScreenToWorldPoint(mp);
        }
#endregion
    }

}
