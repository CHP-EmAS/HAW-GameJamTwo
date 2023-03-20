Shader "Lit/VertLit"
{
    //This Shader is the main opaque shader.
    Properties
    {
        [MainTexture] [NoScaleOffset] _BaseMap("Albedo", 2D) = "white" {}      //<- this can be ignored and is only here to aquire the right texture index in the GBuffer-pass (2nd pass)       
        [MainColor][HDR]_BaseColor("Color", color) = (0, 0, 0, 1)
        [KeywordEnum(None, Front, Back)] _Cull("Cull", float) = 2.0
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _Metallic("Metallicness", Range(0, 1)) = 0
    }
    SubShader
    {   
        //Shadowcaster
        LOD 100
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
        Pass{
            Tags{ "LightMode" = "ShadowCaster"}
        }
        Pass{//v shader needs to be marked for TextureRenderer
            Tags{"LightMode" = "DepthOnly"}
        }
        //Gbuffer & Depth
        Pass{
            //Before rendering stuff, we need to up this material data into the GBuffer. Copied from Lit.shader
            Tags{"LightMode" = "UniversalGBuffer"}
            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            #pragma vertex LitGBufferPassVertex
            #pragma fragment LitGBufferPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitGBufferPass.hlsl"
            ENDHLSL
        }

        //Main
        Pass
        {
            Tags{
                "RenderType" = "Opaque" "
                RenderPipeline" = "UniversalPipeline" 
                "UniversalMaterialType" = "Lit" 
                "IgnoreProjector" = "True" 
                "ShaderModel"="4.5"
                }
            Cull [_Cull]
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            #pragma vertex CustomVert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"           
            //!Note: Fog does not work in this shader as of now
            //Shamelessly copy pasted from Shadergraph

            #include "CustomVertexShaderDefault.hlsl"
            #include "LitFragPass.hlsl"

            real4 Frag(v2f i) : SV_Target
            {
                return LitFrag(i);
            }

            ENDHLSL
        }
        
    }
}
