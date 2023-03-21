using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base.Markers
{
    [RequireComponent(typeof(Camera))]
    public class PMark_MouseCam : MonoBehaviour
    {
        private void Awake(){
            ServiceLocator.mouseCamera = GetComponent<Camera>();
        }
    }
}
