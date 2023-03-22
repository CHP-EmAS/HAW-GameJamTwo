using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //v Singleton base class
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static Singleton<T> instance;
        public static T Instance{
            get{
                return instance as T;
            }
        }

        protected T GetInstanceSafe()
        {
            if (instance == null) instance = this;
            return instance as T;
        }

        protected virtual void Awake()
        {
            if(instance != null && instance != this){
                Debug.Log("Tried to set a singleton instance of type: " + typeof(T) + " <- trying to be assigned at" + gameObject + " but the instance is already set to " + instance);
                return;
            }
            instance = this;
        }


        private void OnDestroy(){
            //v Clean up
            if(instance == this){
                instance = null;
                Destroy(this);
            }
        }
    }

}
