using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace Plum.Base
{
    //Calls stuff on application Init
    //https://docs.unity.cn/Packages/com.unity.addressables@1.19/manual/LoadingAddressableAssets.html
    //https://www.youtube.com/watch?v=zJOxWmVveXU
    public static class GameInitializer
    {
        private static bool wasInitialized;
        private const string dependenciesPrefabKey = "Assets/Prefabs/Dependencies.prefab";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //Initializes prefab dependencies
        public static void DependenciesInit(){
            if(wasInitialized) return;
            RawDependenciesInit();
            wasInitialized = true;
        }

        public static void RawDependenciesInit(){
            GameObject g = Addressables.InstantiateAsync(dependenciesPrefabKey).WaitForCompletion();
            Object.DontDestroyOnLoad(g);
        }
    }

}
