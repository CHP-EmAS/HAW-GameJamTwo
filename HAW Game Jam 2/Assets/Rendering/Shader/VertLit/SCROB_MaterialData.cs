using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Graphics{
    //Ought to be used with LitShaderBase
    //(!NOTE: Before currently still the old version, which does not support the MaterialData struct is in the 'exportable' folder. Change please! <.<)
    [System.Serializable]
    public struct MaterialData{
        public Texture2D albedo;
        public Texture2D normalMap;
        [Range(0, 1)]public float normalIntensity, smoothness, metallicness;
        public Color tint;
        
        public MaterialData(Texture2D albedo, Texture2D normal){
            this.albedo = albedo;
            normalMap = normal;

            normalIntensity = 1.0f;
            tint = Color.white;
            type = TypeMaterialUsage.GROUND;
            smoothness = 0;
            metallicness = 0;
        }

        public MaterialData(bool arg){
            albedo = null;
            normalMap = null;

            normalIntensity = 1.0f;
            tint = Color.white;
            type = TypeMaterialUsage.GROUND;
            smoothness = 0;
            metallicness = 0;
        }

        public void ApplyData(Material target){
            target.SetTexture("_MainTex", albedo);
            target.SetTexture("_NormalMap", normalMap);

            target.SetFloat("_NInten", normalIntensity);
            target.SetFloat("_Smoothness", smoothness);
            target.SetFloat("_Metallic", metallicness);


            target.SetColor("_Tint", tint);
            type = TypeMaterialUsage.GROUND;
        }
//DREAM

        public TypeMaterialUsage type;
    }


    [CreateAssetMenu(fileName = "MaterialData", menuName = "Graphics/MaterialData", order = 0)]
    public class SCROB_MaterialData : ScriptableObject{
        public MaterialData data = new MaterialData(false);

        [ContextMenu("Default values")]
        public void DefaultValues(){
            data.tint = Color.white;
            data.normalIntensity = 1.0f;
        }
    }

#region DREAM
    [System.Flags]  //Bitmask
    public enum TypeMaterialUsage{
        GROUND = 1,
        WALL_VERTICAL = 2,
        WALL_HORIZONTAL = 4,
        DECO = 8,
    }
#endregion
}