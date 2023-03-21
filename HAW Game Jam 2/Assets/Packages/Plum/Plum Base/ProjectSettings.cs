using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Plum.Base;

public static partial class ProjectSettings
{
    public const float fixedTimeStep = 0.01f;
    public const string loadKey = "ProjectSettings";
    public static ProjectSettingsSCROB settings;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //Initializes prefab dependencies
    public static void DependenciesInit(){
        try{
            settings = Addressables.LoadAssetAsync<ProjectSettingsSCROB>(loadKey).WaitForCompletion();
        }
        catch{
            Debug.Log("couldnt load projectsettings with key " + loadKey);
        }
    }
}

