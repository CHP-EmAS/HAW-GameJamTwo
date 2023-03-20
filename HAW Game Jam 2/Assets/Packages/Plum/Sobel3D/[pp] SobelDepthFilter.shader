Shader "Plum/SobelFilter" 
{
    //Notes:
    // step gibt 0 oder 1 zurück, basierend ob input X größer ist als input Y
    // smoothstep ist wie lerp, nur statt linear hat es eine steilere interpolations"kurve"
    Properties
    {
        //v This enum chooses which stage of the shader should be shown. For debug purposes.
        [KeywordEnum(Full, DepthTexture, NormalTexture, DepthSobel, NormalSobel, normalDot, NoiseLUT, AbledoTexture, AlbedoSobel, DepthCorrection)] _Mode("Fullscreen-Mode", float) = 0
        [HideInInspector][NOSCALEOFFSET]_MainTex ("Texture", 2D) = "white" {}    //<- frame buffer which is set in URP-Blit pass per code
        [Toggle] _UDepth("use corrected depth", float) = 1
        _Lerp("Final Intensity", Range(0, 1)) = 1
        _color("Edge Color", Color) = (0, 0, 0, 1)          //<- farbe der outline
        _Extrusion("Sobel extrusion", Range(0.001, 4)) = 1
        _NoiseExtrusion("Noise extrusion intensity", float) = 1
        _Smoothness("Sobel step smoothness", Range(0, 1)) = 0       //<- smoothness of the depth cut
        _NoiseLUT("Edge Noise LUT", 2D) = "white"{}
        _NoiseT("Noise usage", Range(0, 1)) = 0
        _edgeDepth("sobel edge Depth", Range(0, .505)) = 1                 //<- depth edge
        _edgeNormal("sobel edge Normal", Range(0, 14)) = 1               //<- normal edge
        _edgeAlbedo("sobel edge albedo", Range(0, 4)) = 1               //<- normal edge

        _depthT("Depth usage", Range(0, 1)) = 1
        _normalT("Normal usage", Range(0, 1)) = 1
        _albedoT("Albedo usage", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //Includes für URP basics & Depth-Texture
            #include"Assets/URPUtils.hlsl"
            #include"Assets/URPGbufferUtils.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            //Variables
            //Wie auch in einigen anderen Programiersprachen müssen varaiblen & methoden deklariert/definiert werden bevor sie benutzt werden können
            sampler2D _MainTex;
            real4 _MainTex_ST;
            uniform real4 _MainTex_TexelSize;   //<- die größe der Pixel um mehr präzision zu haben. Vermutlich wird die größe des frame-buffers(also dieser textur) genauso groß sein wie die vom depth-buffer
        
            real4 _color;

//SOBEL FILTER 
            //https://en.wikipedia.org/wiki/Sobel_operator

            //Utility for sampling texture
            static real2 samplePoints[9] = {
				real2(-1, 1), real2(0, 1), real2(1, 1),
				real2(-1, 0), real2(0, 0), real2(1, 1),
				real2(-1, 1), real2(0, -1), real2(1, -1),
            };

            //float[9] = float3x3
            static real sobelOperatorX[9] = {
                1, 0, -1,
                2, 0, -2,
                1, 0, -1
            };

            static real sobelOperatorY[9] = {
                1, 2, 1,
                0, 0, 0,
                -1, -2, -1
            };



            real _edgeDepth = 0.05f;
            real _edgeNormal = 0;
            real _edgeAlbedo = 0;
            real _Extrusion;

            real _depthT;
            real _normalT;
            real _albedoT;

            real _Smoothness = .0f;
            real _NoiseExtrusion;

            real _UDepth;

            uniform sampler2D _TransparentDistance;

            //Sobel Customization
            real PolishSobel(real sobel, real edge){
                real removal = lerp(0, edge, _Smoothness);
                sobel = smoothstep(edge - removal, edge, sobel);
                return sobel;
            }

            //Actual sobel edge calculation
            real ApplySobel(real2 uv, real noise, out real depthT, out real normalT, out real albedoT, out real nDotO){
                real gX = 0, gY = 0;    //<- depth sobel
                real3 gnX = 0, gnY = 0;  //<- normal sobel
                real3 gaX = 0, gaY = 0;  //<- albedo sobel
                real nDot = 0;

                [unroll]    //<- calculates loop until the end (I think atleast .-.)
                for(unsigned int i = 0; i < 9; i++){
                    //Calculate of with the appropriate offset
                    real2 calculatedUV = uv + samplePoints[i] * _MainTex_TexelSize.xy * (_Extrusion + noise * _NoiseExtrusion);  //<- texelsize for pixel accuracy
                    real3 normal = SampleGBufferNormals(calculatedUV);
                    real3 albedo = SampleGBufferAlbedo(calculatedUV);

                    //transforming to view space may avoid some funky behaiviour in specific angles
                    real normalDot = dot(TransformWorldToView(normalize(normal)), 
                    real3(0, 0, 1)); //<- normals are in world-space so....yea

                    nDot += normalDot;

                    real depth = lerp(SampleSceneDepth(calculatedUV), tex2D(_TransparentDistance, calculatedUV).r, _UDepth);
                    gX += sobelOperatorX[i] * depth;        //<- depth ist über alle kanäle gleich daher reicht das sampling einer dimension
                    gY += sobelOperatorY[i] * depth;       //<- function aus den includes welche depth auf der gegebenen screen-uv zurückgibt

                    gnX += sobelOperatorX[i] * normal;
                    gnY += sobelOperatorY[i] * normal;    

                    gaX += sobelOperatorX[i] * albedo;
                    gaY += sobelOperatorY[i] * albedo;
                }

                //calculate sobel value
                real g = sqrt(pow(gX, 2) + pow(gY, 2));
                real gn = sqrt(pow((length(gnX)), 2) + pow((length(gnY)), 2));
                real ga = sqrt(pow((length(gaX)), 2) + pow((length(gaY)), 2));
                nDot /= 9;

                //"Customize" sobel value
                g = PolishSobel(g, _edgeDepth);
                gn = PolishSobel(gn, _edgeNormal);
                ga = PolishSobel(ga, _edgeAlbedo);

                //apply weights
                g = lerp(0, g, _depthT);
                gn = lerp(0, gn, _normalT);
                ga = lerp(0, ga, _albedoT);

                //out settings
                depthT = g;
                normalT = gn;
                nDotO = nDot;
                albedoT = ga;



                return saturate(g + gn + ga);
            }


//END SOBEL FILTER

            struct appdata
            {
                //real4 ist ein (Ich glaube vom unity gegebener?) Datentyp, der je nach plattform entweder die präzision von float oder half hat.
                real4 vertex : POSITION;
                real2 uv : TEXCOORD0;
            };

            struct v2f
            {
                real2 uv : TEXCOORD0;
                real4 vertex : SV_POSITION;
            };

            //Default vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                //v alternative (URP) Funktion um Object>clip zu transformieren
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            sampler2D _NoiseLUT;
            real4 _NoiseLUT_ST;
            real _Mode;
            //Allow for debug view
            real4 ResultSwitch(real4 sobel, real2 uv, real depthT, real normalT, real nDot, real albedoT){
                [BRANCH]switch(_Mode)
                {
                    default:
                        return sobel;

                    //Default effect
                    case 0:
                        return sobel;

                    //Depth Texture
                    case 1:
                        real depth = lerp(SampleSceneDepth(uv), tex2D(_TransparentDistance, uv).r, _UDepth);
                        return real4(depth, depth, depth, 1.0f);

                    //Normal from G-Buffer
                    case 2:
                        real3 normal = SampleGBufferNormals(uv);
                        return real4(normal.xyz, 1.0f);

                    //Depth Sobel T
                    case 3:
                        return real4(depthT, depthT, depthT, 1.0f);

                    //Normal T
                    case 4:
                        return real4(normalT, normalT, normalT, 1.0f);

                    //Normal Dot T
                    case 5:
                        return real4(nDot, nDot, nDot, 1.0f);

                    //Noise texture
                    case 6:
                        return real4(tex2D(_NoiseLUT, uv * _NoiseLUT_ST));

                    //Albedo texture
                    case 7:
                        real3 albedo = SampleGBufferAlbedo(uv);
                        return real4(albedo, 1.0f);

                    //Albedo Depth T
                    case 8:
                        return real4(albedoT, albedoT, albedoT, 1.0f);

                    //depth corrected texture
                    case 9:
                        return tex2D(_TransparentDistance, uv);
                }
            }

            real _Lerp;
            real _NoiseT;
            //Fragment shader
            real4 frag (v2f i) : SV_Target
            {
                //glitch compensation
                real2 uv = i.uv;               
                real2 depthUV = real2(uv.x, uv.y);   //<- again weird glitch usually not happening

                //Sample frame buffer
                real4 col = tex2D(_MainTex, uv);        //<- UV glitch again, usually not there   

                real depthT = 0; 
                real normalT = 0;
                real nDot = 0;
                real albedoT = 0;
                real noise = tex2D(_NoiseLUT, i.uv * _NoiseLUT_ST).r;
                //apply depth sobel edge
                real depthEdge = ApplySobel(depthUV, noise, depthT, normalT, albedoT, nDot);
                depthEdge = saturate(depthEdge);
                //real2 noiseUV = WorldPos(uv).xy * _NoiseLUT_ST;
                
                //based on depthEdge, we can interpolate between the edge-color and the default frame
                real4 result = lerp(col, lerp(_color, noise, _NoiseT), depthEdge);
                result = lerp(col, ResultSwitch(result, uv, depthT, normalT, nDot, albedoT), _Lerp);

                return result;    //<- ResultSwitch allows to display debug views
            }
            ENDHLSL
        }
    }
}
