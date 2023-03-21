using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class OnMouse3D : MonoBehaviour
    {
        private void OnMouseEnter(){
            Debug.Log("I AM MOUSED");
        }
        private void OnMouseOver(){

        }

        private void OnMouseExit(){
            Debug.Log("BYE MOUSE");
        }
    }
}
