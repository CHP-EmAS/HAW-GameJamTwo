using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Plum.Base{
    //v basically wrapper for Addressables
    public static class AssetLoad<T>
    {
        public static T Get(string key){

            //v small problem if T == GameObject - GetComponent<GameObject> does not work, so we have to convert
            if(typeof(T) == typeof(GameObject)){
                GameObject gO = Addressables.InstantiateAsync(key).WaitForCompletion();
                return (T)System.Convert.ChangeType(gO, typeof(T));
            }

            T g = Addressables.InstantiateAsync(key).WaitForCompletion().GetComponent<T>();
            return g;
        }
    }

}
