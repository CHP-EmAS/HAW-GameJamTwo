#ifndef URPGBUFFERUTILS_INCL      
#define URPGBUFFERUTILS_INCL

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Deferred.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

    //Unity's surfacdata Struct description:
    //v copied from SurfaceData.hlsl
    //struct SurfaceData
    //{
        //half3 albedo;
        //half3 specular;
        //half  metallic;
        //half  smoothness;
        //half3 normalTS;
        //half3 emission;
        //half  occlusion;
        //half  alpha;
        //half  clearCoatMask;
        //half  clearCoatSmoothness;
    //};

    #define GBUFFER0 0
    #define GBUFFER1 1
    #define GBUFFER2 2
    #define GBUFFER3 3
    TEXTURE2D_X_HALF(_GBuffer0);
    TEXTURE2D_X_HALF(_GBuffer1);
    TEXTURE2D_X_HALF(_GBuffer2);
    SamplerState my_point_clamp_sampler;
    //This is the StencilDeffered.shader technique from built in URP stuff
    //^for sampling the G-Buffer

    SurfaceData GetGBufferSurfaceData(real2 screenUV){
        real4 gbuffer0 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer0, my_point_clamp_sampler, screenUV, 0);
        real4 gbuffer1 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer1, my_point_clamp_sampler, screenUV, 0);
        real4 gbuffer2 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer2, my_point_clamp_sampler, screenUV, 0);
        SurfaceData surfaceData = SurfaceDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2, kLightingSimpleLit);
        return surfaceData;
    }


    real SampleGBufferSmoothness(real2 screenUV){
        //return exp2(10 * surfaceData.smoothness + 1); //This is the equation used in the original sample
        SurfaceData surfaceData = GetGBufferSurfaceData(screenUV);
        return surfaceData.smoothness;
    }

    real SampleGBufferMetallicness(real2 screenUV){
        SurfaceData surfaceData = GetGBufferSurfaceData(screenUV);
        return surfaceData.specular;
    }

    real3 UnpackNormalLocal(real3 pn)
    {
        real2 remappedOctNormalWS = float2(Unpack888ToFloat2(pn));          // values between [ 0, +1]
        real2 octNormalWS = remappedOctNormalWS.xy * real(2.0) - real(1.0);// values between [-1, +1]
        return real3(UnpackNormalOctQuadEncode(octNormalWS));              // values between [-1, +1]
    }

    real3 SampleGBufferNormals(real2 screenUV){
        real4 gbuffer0 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer0, my_point_clamp_sampler, screenUV, 0);
        real4 gbuffer1 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer1, my_point_clamp_sampler, screenUV, 0);
        real4 gbuffer2 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer2, my_point_clamp_sampler, screenUV, 0);
        return UnpackNormalLocal(gbuffer2.xyz);
    }


    real3 SampleGBufferAlbedo(real2 screenUV){
        SurfaceData surfaceData = GetGBufferSurfaceData(screenUV);
        return surfaceData.albedo;
    }

#endif