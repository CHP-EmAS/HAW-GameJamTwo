Shader "Lit/Tail"
{
    //This Shader is the main opaque shader.
    Properties
    {
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}      //<- this can be ignored and is only here to aquire the right texture index in the GBuffer-pass (2nd pass)       
        [MainColor][HDR]_BaseColor("Color", color) = (0, 0, 0, 1)
        [KeywordEnum(None, Front, Back)] _Cull("Cull", float) = 2.0
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _Metallic("Metallicness", Range(0, 1)) = 0
        _Fres("Fresnel", Range(0, 1)) = 0
    }
    SubShader
    {   

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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_Cull]
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            #pragma vertex CustomVert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"           
            //!Note: Fog does not work in this shader as of now
            //Shamelessly copy pasted from Shadergraph

            #include "CustomVertLitTail.hlsl"
            #include "LitFragPass.hlsl"

            real4 Frag(v2f i) : SV_Target
            {
                return real4(LitFrag(i).xyz, .5f);
            }

            ENDHLSL
        }
        
    }
}
