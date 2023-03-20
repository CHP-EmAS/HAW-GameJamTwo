#ifndef JR_CSFP
#define JR_CSFP
//Required Variables:
//- _BaseColor as x3
//- _Smoothness as x 
//- _Metallic as x

//- V2F HAS to have informations about the vertex-normal named 'normal' as a flaot3 or similar
//- V2F HAS to have object space vertex positions named "vertexOBJ"
//- V2F HAS to have flaot3 (or similar) wordlspace vertex position saved as "wsp"

//useful uniform variables
//_celparams (vector for cel params)
//use real2 celParams instead if _celparams unavaliable

#pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ DYNAMICLIGHTMAP_ON
#pragma multi_compile _ DIRLIGHTMAP_COMBINED
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile _ _SHADOWS_SOFT
#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
#pragma multi_compile _ SHADOWS_SHADOWMASK
#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
#pragma multi_compile _ _LIGHT_COOKIES
#pragma multi_compile _ _CLUSTERED_RENDERING

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Assets/Helper.hlsl" 
#include "URPLight.hlsl"




real4 Lit(v2f i)
{
        //v declare all core variables
    real mainLightDiffuse, mainLightSpecular;
    real3 mainLightColor;

    //v calculate later used variables
    real3 wsn = normalize(i.normalWS);
    real3 wsp = i.wsp;

    //v get Light Information
    GetMainLight(wsp, wsn, i.viewDir, _Smoothness, _Metallic, mainLightColor, mainLightDiffuse, mainLightSpecular);

    //v all info is now stored in here
    real3 ambient = unity_AmbientSky.xyz;
    real3 albedo = tex2D(_MainTex, i.uv) * _BaseColor; //<- starting at basic albedo

    //v calculate dark-spots (we want them to be more saturated and with slight hue offset)
    real3 result;
    //v get skybox reflection
    //https://www.bitshiftprogrammer.com/2018/11/reflection-probe-custom-shader-tutorial.html
    real3 refDir = reflect(-i.viewDir, i.normalWS);
    real3 environmentalReflection = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, refDir, 3 - (_Smoothness * 2)) * _Smoothness;

    //v apply main light lighting
    result = saturate((mainLightDiffuse * albedo) + (ambient + environmentalReflection));
    result += (mainLightSpecular * _Smoothness) + ambient;

    //v apply local lights
    real3 additionalLightColor, additionalLightSpecular;

    GetAdditionalLight(wsp, i.viewDir, i.normalWS, _Smoothness,
    additionalLightColor, additionalLightSpecular);

    result += additionalLightColor;
    result += additionalLightSpecular;

    return real4(result, 1);
}
real4 LitFrag(v2f i)
{
    return i.lit * tex2D(_MainTex, i.uv);
}




#endif