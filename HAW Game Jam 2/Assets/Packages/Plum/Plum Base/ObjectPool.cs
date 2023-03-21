using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //Initialize objects in the form of an object pool
    public class ObjectPool<T> : MonoBehaviour where T:MonoBehaviour
    {
        private enum InitializationType{
            SeperateObjects,
            Components,
        }
        [SerializeField] private InjectionPoint injectionPoint = InjectionPoint.START;
        [SerializeField] private InitializationType initializeAs = InitializationType.SeperateObjects;
        [SerializeField] private GameObject parentObject;
        [SerializeField] private uint amount;
        protected List<T> objectList = new List<T>();   public List<T> Objects{get{return objectList;}}



        private void Init(){
            if(parentObject == null) parentObject = gameObject;

            //Initialize all objects
            for (int i = 0; i < amount; i++)
            {
                T cachedInstance;
                switch (initializeAs)
                {
                    //initialize as seperate objects
                    case InitializationType.SeperateObjects:
                        GameObject cached = new GameObject();
                        cached.transform.parent = parentObject.transform;
                        cached.transform.localPosition = Vector3.zero;
                        cachedInstance = cached.AddComponent<T>();
                    break;

                    case InitializationType.Components:
                        cachedInstance = parentObject.AddComponent<T>();
                    break;

                    //Default as component
                    default:
                        cachedInstance = parentObject.AddComponent<T>();
                    break;

                }
                objectList.Add(cachedInstance);
            }
        }   
    }

}
