//standardized vertex shader
//v VERTEX SHADER
sampler2D _MainTex; //<- needed for UV calculations
real4 _MainTex_ST;
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Assets/Helper.hlsl" 
#include "URPLight.hlsl"

real3 Object2World(real3 inp){
    return mul(unity_ObjectToWorld, real4(inp, 0.0f)).xyz;
}


struct appdata
{
    real3 normal : NORMAL;
    real2 uv : TEXCOORD0;
    real4 vertex : POSITION;
    real4 vertexOBJ : TEXCOORD1;
    real4 wsp : TEXCOORD2;
    real4 tangent : TANGENT;
    real3 color : COLOR;
};

struct v2f
{
    real3 color : COLOR;
    real2 uv : TEXCOORD0;
    real3 normal : NORMAL;
    real3 normalWS : TEXCOORD12;
    real4 vertex : SV_POSITION;
    real4 vertexOBJ : TEXCOORD1;
    real3 wsp : TEXCOORD2;
    real3x3 transposeTangent : TEXCOORD3;            //<- per vertex tangent vector (https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html)
    real3 viewDir : TEXCOORD8;
    real4 lit : TEXCOORD6;
};

real4 _BaseColor;
real _Smoothness;
real _Metallic;
real4 GetLight(v2f i)
{
    //v declare all core variables
    real mainLightDiffuse, mainLightSpecular;
    real3 mainLightColor;

    //v calculate later used variables
    real3 wsn = normalize(i.normalWS);
    real3 wsp = TransformObjectToWorld(float3(0.0f, 0.0f, 0.0f));

    //v get Light Information
    GetMainLight(wsp, wsn, i.viewDir, _Smoothness, _Metallic, mainLightColor, mainLightDiffuse, mainLightSpecular);

    //v all info is now stored in here
    real3 ambient = unity_AmbientSky.xyz;
    real3 albedo =  _BaseColor; //<- starting at basic albedo

    //v calculate dark-spots (we want them to be more saturated and with slight hue offset)
    real3 result;
    //v get skybox reflection
    //https://www.bitshiftprogrammer.com/2018/11/reflection-probe-custom-shader-tutorial.html
    real3 refDir = reflect(-i.viewDir, i.normalWS);
    real3 environmentalReflection = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, refDir, 3 - (_Smoothness * 2)) * _Smoothness;

    //v apply main light lighting
    mainLightDiffuse = smoothstep(.4f, .6f, mainLightDiffuse);
    mainLightDiffuse = lerp(.4f, 1.0f, mainLightDiffuse);
    real3 lowerColor = 0;
    SH_RGB2HSV(albedo, lowerColor);
    lowerColor.r -= .2f;
    lowerColor.g += .2f;
    lowerColor.b *= .5f;
    SH_HSV2RGB(lowerColor, lowerColor);
    result = lerp(lowerColor, albedo, mainLightDiffuse) + (ambient + environmentalReflection);
    result += (mainLightSpecular * _Smoothness);

    //v apply local lights
    real3 additionalLightColor, additionalLightSpecular;

    GetAdditionalLightCelShaded(i.wsp, wsp, i.viewDir, i.normalWS, _Smoothness,
    additionalLightColor, additionalLightSpecular);

    result += additionalLightColor;
    result += additionalLightSpecular;

    return real4(result, 1);
}

v2f CustomVert (appdata v)
{
    v2f o;
    o.color = v.color;
    o.vertexOBJ = v.vertex;
    o.wsp = TransformObjectToWorld(v.vertex);
    o.vertex = TransformObjectToHClip(v.vertex);
    o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
    o.normal = v.normal;
    o.normalWS = Object2World(v.normal);
    o.viewDir = normalize(o.wsp - _WorldSpaceCameraPos);

    //v tangent multiplication
    real3 wTangent = Object2World(v.tangent);
    real tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    real3 wBiTangent = cross(o.normalWS, wTangent) * tangentSign;

    //v calculate tangent matrix
    o.transposeTangent = real3x3(
        real3(wTangent.x, wBiTangent.x, o.normalWS.x),
        real3(wTangent.y, wBiTangent.y, o.normalWS.y),
        real3(wTangent.z, wBiTangent.z, o.normalWS.z)
    );
    o.lit = (real4) 0;
    o.lit = GetLight(o);
    return o;
}