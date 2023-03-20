#ifndef HELP_INCL      
#define HELP_INCL
//This is a script for helper functions used in shaders. Created 20.04.21
//Not on how to use:
// #include "Assets/Helper.hlsl"
// #ifdef HELP_INCL
// #endif
//https://www.ronja-tutorials.com/post/023-postprocessing-blur/#gaussian-blur
#define PI 3.14159265359  
#define E 2.71828182846
    //GREYSCALE HELP

    //returns input as a greyscale image
    real4 GreyScale(real4 input)
    {
      real grayScaleAvg = (input.x + input.y + input.z) / 3.0f;
     return real4(grayScaleAvg, grayScaleAvg, grayScaleAvg, input.z);
    }

    //returns input as a greyscale image, stepped
    real4 GreyScalestep(real4 input, real edge)
    {
        real grayScaleAvg = (input.x + input.y + input.z) / 3.0f;
        grayScaleAvg = step(edge, grayScaleAvg);
        return real4(grayScaleAvg, grayScaleAvg, grayScaleAvg, input.z);
    }

    real4 GreyScaleStepSat(real4 input, real edge){
        real grayScaleAvg = (saturate(input.x) + saturate(input.y) + saturate(input.z)) / 3.0f;
        grayScaleAvg = step(edge, grayScaleAvg);
        return real4(grayScaleAvg, grayScaleAvg, grayScaleAvg, input.z);
    }


    //IMAGE EXTRUSION

    static real2 Extrusion[9] =
    {
        real2(-1, 2), real2(0, 2), real2(1, 2), //y = 1
        real2(-1.4f, 0), real2(0, 0), real2(1.4f, 0), //y = 0
        real2(-1, -2), real2(0, -2), real2(1, -2) //y = -1
    };

static real2 ExtrusionRaw[9] =
{
    real2(-1, 1), real2(0, 1), real2(1, 1), //y = 1
        real2(-1, 0), real2(0, 0), real2(1, 0), //y = 0
        real2(-1, -1), real2(0, -1), real2(1, -1) //y = -1
};

    real4 ExtrudeImage(sampler2D input, real2 uv, real intensity)
    {    
        real4 extruded = 0;
    
        [unroll]
        for (uint i = 0; i < 9; i++)
        { 
        real4 cached = tex2D(input, uv + (Extrusion[i] * intensity));
            extruded += cached;
        }   
    
        return extruded;
    }

    real4 ExtrudeImageSaturated(sampler2D input, real2 uv, real intensity)
    {
        real4 extruded = 0;
    
        [unroll]
        for (uint i = 0; i < 9; i++)
        {
            real4 cached = tex2D(input, uv + (Extrusion[i] * intensity));
            extruded += cached;
        }
    
        return saturate(extruded);
    }

    real4 ExtrudeImageStep(sampler2D input, real2 uv, real intensity)
{
    real4 extruded = 0;
    
         [unroll]
    for (uint i = 0; i < 9; i++)
    {
        real4 cached = tex2D(input, uv + (Extrusion[i] * intensity));
        extruded += cached;
    }
    
    real4 ret = 0;
    ret = saturate(extruded);
    ret = 1 - step(ret, 0.5);
    return ret;
}
    
    
        //MISC ALGORYTHMS
static int2 ScharrOperator[9] =
{
    int2(47, 47), int2(0, 162), int2(-47, 47),
    int2(162, 0), int2(0, 0), int2(-162, 0),
    int2(47, -47), int2(0, -162), int2(-47, -47),
};

static int2 SobelOperator[9] =
{
    int2(1, 1), int2(0, 2), int2(-1, 1),
    int2(2, 0), int2(0, 0), int2(-2, 0),
    int2(1, -1), int2(0, -2), int2(-1, -1),
};
    void GetEdges(sampler2D input, real2 uv, real intensity, out real Out)       
    {
    real2 edge = 0;
    
        [unroll]
    for (uint i = 0; i < 9; i++)
    {
        real4 cached = tex2D(input, uv + (Extrusion[i] * intensity));                      //IMAGE SHOULD EITHER BE GRAYSCALE OR JUST WRITE TO R CHANNEL
        real2 final = cached.r * real2(ScharrOperator[i].x, ScharrOperator[i].y);
        edge += final;

    }
    
    Out = length(edge);
}  

//Blurs

real4 RadialBlur(sampler2D tex, int samples, real2 center, real2 uv, real width){
    real4 col = real4(0, 0, 0, 0);
    real2 ray = uv - center.xy;
    for(int i = 0; i < samples; i++){
        real scale = 1.0f - width * (real(i) / real(samples - 1));
        col.xyz += tex2D(tex, (ray * scale) + center.xy).xyz / real(samples);
    }
    return col;
}

real4 RadialBlur3D(sampler2D tex, int samples, real3 center, real2 uv, real width){
    real4 col = real4(0, 0, 0, 0);
    real2 ray = uv - center.xy;
    for(int i = 0; i < samples; i++){
        real scale = 1.0f - width * (real(i) / real(samples - 1)) * sign(distance(center, real3(center.xy, 0)) * -sign(center.z));
        col.xyz += tex2D(tex, (ray * scale) + center.xy).xyz / real(samples);
    }
    return col;
}

//https://www.ronja-tutorials.com/post/023-postprocessing-blur/#gaussian-blur for both following
    real4 SimpleBlur(sampler2D input, real2 uv, uint intensity, real2 texelSize){
        real4 savedColor = tex2D(input, uv);
        real4 thisColor = savedColor;

        if(intensity == 0) return savedColor;

        for(int n = 1; n < intensity + 1; n++){
            for(int i = 0; i < 9; i++){
                savedColor += tex2D(input, uv + (Extrusion[i] * texelSize * n));
            }
        }

        savedColor /= ((real)intensity + .000001f) * 9;

        real t = step(.5f, saturate(intensity));
        real4 result = lerp(tex2D(input, uv), savedColor, t);   //if higher than 0 give the "original" color
        return result;
    }

    real4 GaussBlur(sampler2D input, real2 uv, int samples, real deviation, real intensity, real2 texelSize){
        real4 savedColor = tex2D(input, uv);
        if(all(samples == 0 || deviation == 0)) return savedColor;

        real4 thisColor = savedColor;
        real sum;
        real sinv = _ScreenParams.y / _ScreenParams.x;
        for(real n = 0; n < samples; n++){
            real offset = (n/(samples-1)-.5f) * intensity * sinv;
            real yM = sin(n);
            real xM = cos(n);
            real2 updatedUV = uv + real2(offset * xM, offset * yM);

            real devSqrd = deviation * deviation;
            real gauss = (1/sqrt(2*PI*devSqrd)) * pow(E, -((offset*offset)/(2+devSqrd)));
            sum += gauss;
            savedColor += tex2D(input, updatedUV) * gauss;
        }

        savedColor /= sum;
        return savedColor;
    }

//Noise
//Source for most noise functions: https://docs.unity3d.com/Packages/com.unity.shadergraph@7.1/manual/Simple-Noise-Node.html

//Gradient noise
real GradientNoiseDir(real2 p)
{
    p = p % 289;
    real x = (34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(real2(x - floor(x + 0.5), abs(x) - 0.5));

}

real GradientNoiseExecute(real2 p)
{
    real2 ip = floor(p);
    real2 fp = frac(p);
    real d00 = dot(GradientNoiseDir(ip), fp);
    real d01 = dot(GradientNoiseDir(ip + real2(0, 1)), fp - real2(0, 1));
    real d10 = dot(GradientNoiseDir(ip + real2(1, 0)), fp - real2(1, 0));
    real d11 = dot(GradientNoiseDir(ip + real2(1, 1)), fp - real2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
}

void GradientNoise(real2 uv, real scale, out real Out)
{
    Out = GradientNoiseExecute(uv * scale) + 0.5;
}

//https://answers.unity.com/questions/938178/3d-perlin-noise.html
real FixedNoise(real x, real y, real scale){
    real n;
    real2 xy = real2(x, y);
    GradientNoise(xy, scale, n);
    return sin(PI * n);
}

void Noise3D(real3 pos, real scale, out real Out){
    real3 position = pos;
    position.x += 1;
    position.z += 2;

    real xy, xz, yz, yx, zx, zy;

    xy = FixedNoise(position.x, position.y, scale);
    xz = FixedNoise(position.x, position.z, scale);
    yz = FixedNoise(position.y, position.z, scale);
    yx = FixedNoise(position.y, position.x, scale);
    zx = FixedNoise(position.z, position.x, scale);
    zy = FixedNoise(position.z, position.y, scale);

    Out = xy * xz * yz * yx * zx * zy;
}

real NoiseOctaved(real2 uv, real scale, uint iterations, real offset)
{
    real cached = 0;
    GradientNoise(uv, scale, cached);
    
    if (iterations > 9)
        iterations = 9;
    else if (iterations < 0)
        iterations = 1;
    
         [unroll]
        for (uint i = 0; i < iterations; i++)
        {
        real localCached = 0;
        GradientNoise(uv + ExtrusionRaw[i] * offset, scale, localCached);
        cached *= localCached;

    }
    
    
    return cached;
}

//https://valeriomarty.medium.com/raymarched-volumetric-lighting-in-unity-urp-e7bc84d31604
real random(real2 p){
    return frac(sin(dot(p, real2(41, 289)))* 45758.5453 )-.5f;
}

real random01(real2 p){
    return frac(sin(dot(p, real2(41, 289)))* 45758.5453);
}


//TILE

real TileValue(real input, uint steps, uint smoothness){
    input = saturate(input);
    input = input * smoothness;
    input = floor(input);
    input /= steps;
    return input;
}


real TileStep(real input, real edge){
    input = step(input, edge);
    return input;
}

real Posterize(real input, real steps){
    return floor(input / (1 / steps)) * (1 / steps);
}

real3 PosterizeF3(real3 input, real steps){
    real r, g, b;
    r = Posterize(input.x, steps);
    g = Posterize(input.y, steps);
    b = Posterize(input.z, steps);

    return real3(r, g, b);
}


//FRESNEL

real Fresnel(real3 viewDir, real3 normal, real power)
{
    return pow(1 - saturate(dot(viewDir, normal)), power);
}

//Color


real3 HSV2RGB(real3 HSV){
    real3 RGB = real3(HSV.z, HSV.z, HSV.z);

    real h = HSV.x * 6;
    real i = floor(h);
    real i1 = HSV.z * (1.0 - HSV.y);
    real i2 = HSV.z * (1.0 - HSV.y *(h - i));
    real i3 = HSV.z * (1.0 - HSV.y * (1-(h-i)));

    if(i == 0){ RGB = real3(HSV.z, i3, i1); }
    else if (i == 1){ RGB = real3(i2, HSV.z, i1); }
    else if (i == 2){ RGB = real3(i1, HSV.z, i3); }
    else if (i == 3){ RGB = real3(i1, i2, HSV.z); }
    else if (i == 4){ RGB = real3(i3, i1, HSV.z); }
    else RGB = real3(HSV.z, i1, i2);

    return RGB;
}

real epsilon = 1e-10;
//https://www.chilliant.com/rgb2hsv.html
real3 RGB2HCV(real3 RGB){
    real4 p = (RGB.g < RGB.b)? real4(RGB.gb, -1.0f, 2.0/3.0) : real4(RGB.gb, 0.0f, -1.0f/3.0f);
    real4 Q = (RGB.r < p.x)? real4(p.xyw, RGB.r) : real4(RGB.r, p.yzx);
    real C = Q.x - min(Q.w, Q.y);
    real H = abs((Q.w - Q.y) / (6 * C + epsilon) + Q.z);
    return real3(H, C, Q.x);
}

real3 RGB2HSV(real3 RGB){
    real3 HCV = RGB2HCV(RGB);
    real S = HCV.y / (HCV.z + epsilon);
    return real3(HCV.x, S, HCV.z);
}

//https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Colorspace-Conversion-Node.html
void SH_RGB2HSV(real3 In, out real3 Out)
{
    real4 K = real4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    real4 P = lerp(real4(In.bg, K.wz), real4(In.gb, K.xy), step(In.b, In.g));
    real4 Q = lerp(real4(P.xyw, In.r), real4(In.r, P.yzx), step(P.x, In.r));
    real D = Q.x - min(Q.w, Q.y);
    real  Ec = 1e-10;
    Out = real3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + Ec)), D / (Q.x + Ec), Q.x);
}

//Same link as SH_RGB2HSV
void SH_HSV2RGB(real3 In, out real3 Out)
{
    real4 K = real4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    real3 P = abs(frac(In.xxx + K.xyz) * 6.0 - K.www);
    Out = In.z * lerp(K.xxx, saturate(P - K.xxx), In.y);
}

//misc

real InvLerp(real from, real to, real val){
    return(val - from) / (to - from);
}

real Remap(real ogFrom, real ogTo, real targetFrom, real targetTo, real value){
    real f = InvLerp(ogFrom, ogTo, value);
    return lerp(targetFrom, targetTo, f);
}

//https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Contrast-Node.html
void Contrast(real3 In, real Contrast, out real3 Out)
{
    real midpoint = pow(0.5, 2.2);
    Out = (In - midpoint) * Contrast + midpoint;
}

real Contrastreal(real In, real Contrast)
{
    real midpoint = pow(0.5, 2.2);
    return (In - midpoint) * Contrast + midpoint;
}

//https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Saturation-Node.html
real3 SaturationAdjustement(real3 In, real Saturation)
{
    real luma = dot(In, real3(0.2126729, 0.7151522, 0.0721750));
    return luma.xxx + Saturation.xxx * (In - luma.xxx);
}

void SpecularBlinnPhong(real3 worldNormal, real3 lightDirection, real3 viewDirWorld, real smoothness, out real specular){
    //real3 reflectionVector = 2 * dot(worldNormal, lightDirection) * worldNormal - lightDirection;
    //specular = pow(dot(reflectionVector, viewDirWorld), smoothness);

    //https://nedmakesgames.medium.com/creating-custom-lighting-in-unitys-shader-graph-with-universal-render-pipeline-5ad442c27276
    real spec = saturate(dot(worldNormal, normalize(lightDirection + viewDirWorld)));
    specular = pow(spec,  exp2(10 * smoothness + 1));
}

    uniform real4x4 _WorldToSreenUVMatrix;
    //Convert world space position to screen UV
    real2 WSP2SUV(real3 wsp){
        real4 projected = mul(_WorldToSreenUVMatrix, real4(wsp, 1.0f));
        real2 uv = (projected.xy / projected.w) * .5f + .5f;
        return 1 - uv;
    }

    //https://www.gamedev.net/forums/topic/153354-scale-a-vector-about-an-arbitrary-axis/
    real3 ProjectVector(real3 vec, real3 axis){
        real3 resultLCL = (axis * vec) * axis + (vec - (axis * vec) * axis);
        return resultLCL;
    }


    //Vector utils
    real3 ScaleAlongAxis(real3 vec, real3 axis, real s){
        real3 axisLCL = normalize(axis);

        real3 vectorLCL = ProjectVector(vec, axis);
        real3 newVec = s*(axis * vectorLCL)*axis + (vectorLCL - (axis * vectorLCL)*axis);

        return newVec;
    }

#endif