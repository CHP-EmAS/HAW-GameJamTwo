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
};

real4 _BaseColor;
real _Smoothness;
real _Metallic;

v2f CustomVert (appdata v)
{
    v2f o;
    o.color = v.color;
    o.vertexOBJ = v.vertex;
    o.wsp = TransformObjectToWorld(v.vertex);

    real3 posOBJ = v.vertex;
    real t = sin(_Time.x * 20 + v.uv.y * 20);
    posOBJ.y += t * .1f;
    t = cos(_Time.x * 20 + v.uv.y * 20);
    posOBJ.x += t * .1f;
    o.vertex = TransformObjectToHClip(posOBJ);

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
    return o;
}