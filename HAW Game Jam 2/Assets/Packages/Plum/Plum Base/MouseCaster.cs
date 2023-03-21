using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public interface IMouseReciever{
        public void OnMouseOver();
        public void OnMouseExit();
    }
    
[RequireComponent(typeof(Camera))]
    public class MouseCaster : MonoBehaviour
    {
        [SerializeField] private WorldRayCast cast;
        private Camera cam;
        private bool isInit = false;
        private void Start(){
            cam = GetComponent<Camera>();       //<- ensured through ReqComp
            InputHandler.Instance.mousePosition += UseMouseRaycast;
            isInit = true;
        }

        private Vector3 mousePosition, mouseHit;
        private IMouseReciever current;
        private void UseMouseRaycast(Vector2 mousePos){
                mousePosition = cam.ScreenToWorldPoint(mousePos);

                GameObject target;
                if(cast.GetHit(mousePosition, cam.transform.forward, false, out target, out Vector3 v)){
                    IMouseReciever interactable = null;
                    target.TryGetComponent<IMouseReciever>(out interactable);
                    mouseHit = v;

                    if(interactable != null){
                        interactable.OnMouseOver();
                    }

                    if(current != interactable){if(current != null)current.OnMouseExit();}
                    current = interactable;
                }
                else{
                    if(current != null){
                        current.OnMouseExit();
                        current = null;
                    }
                }
        }
        private void OnDestroy(){
            InputHandler.Instance.mousePosition -= UseMouseRaycast;
        }

        private void OnDrawGizmosSelected(){
            if(!isInit) return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(mousePosition, mouseHit);
        }
    }
}
