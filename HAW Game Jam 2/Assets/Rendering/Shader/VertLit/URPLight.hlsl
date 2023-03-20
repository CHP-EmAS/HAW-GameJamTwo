#ifndef URP_Light
#define URP_Light
//This shader Requires:

//Somewhere the camera direction (in world space) exposed as a global variable named '_CamFWD'
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

//Main Light
uniform real3 _CamFWD;
//https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Fresnel-Effect-Node.html
//extra fresnel function to not create unecessary dependencies
void Fresnel(real3 viewDirection, real3 worldNormal, real power, out real Out){
    Out = pow(1.0 - saturate(dot(normalize(worldNormal), normalize(viewDirection))), power);
}

real Specular(real3 dir, real3 viewDir, real3 normal, real smoothness){
    return saturate(pow(max(dot(normal, normalize(dir - viewDir)), 0.0), 256 * smoothness)) * smoothness;
}

void GetMainLight(real3 wsp, real3 normal, real3 viewDir, real smoothness, real metallic, out real3 mainLightColor, out real diffuse, out real specular){
    real shadow;
    float4 shadowCoord = TransformWorldToShadowCoord(wsp);
    Light mainLight = GetMainLight(shadowCoord);
    
    real3 direction = mainLight.direction;

    specular = Specular(direction, viewDir, normal, smoothness);

    real fresnel;
    Fresnel(-viewDir, normal, 5, fresnel);
    specular += fresnel * smoothness;
    
    #ifdef _MAIN_LIGHT_SHADOWS
    shadow = 1.0h;
    #else
    float shadowStrength = GetMainLightShadowStrength();
    shadow = MainLightRealtimeShadow(shadowCoord) * shadowStrength;
    shadow = saturate(shadow);
    #endif


    diffuse = dot(normal, direction) * shadow;
    mainLightColor = mainLight.color;
}

void GetAdditionalLight(real3 wsp, real3 viewDir, real3 normal, real smoothness, out real3 diff, out real3 specular){

   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; ++i)
   {
      Light light = GetAdditionalLight(i, wsp);
      
        diff += saturate(dot(light.direction, normal)) * light.color * light.distanceAttenuation;
      specular += Specular(light.direction, viewDir, normal, smoothness) * light.color * light.distanceAttenuation;
   }
}


void GetAdditionalLightCelShaded(real3 wsp, real3 objectWSP, real3 viewDir, real3 normal, real smoothness, out real3 diff, out real3 specular){

   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; ++i)
   {
      Light light = GetAdditionalPerObjectLight(i, wsp);
      Light lightOBJ = GetAdditionalPerObjectLight(i, objectWSP);

      real3 lightT = light.color * lightOBJ.distanceAttenuation * AdditionalLightRealtimeShadow(i, wsp, light.direction);
      
        diff += dot(light.direction, normal) * saturate(lightT);
        specular += step(.5f, Specular(light.direction, viewDir, normal, smoothness)) * saturate(lightT);
    }
    diff = clamp(diff, 0, 999.99f);
}

#endif