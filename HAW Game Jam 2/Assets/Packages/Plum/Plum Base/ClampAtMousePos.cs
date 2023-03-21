using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base 
{
    public class ClampAtMousePos : MonoBehaviour
    {
        [SerializeField] private Camera refCam;
        [SerializeField] private Vector3 offset;
        private void Update(){
            //https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
            Vector3 taretPos = refCam.ScreenToWorldPoint(new Vector3(Controls.MousePosition().x, Controls.MousePosition().y, 20)) + offset;
            transform.position = taretPos;
        }
    }
   
}
