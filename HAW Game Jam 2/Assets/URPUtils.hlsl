#ifndef URPUTILS_INCL      
#define URPUTILS_INCL

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
    #include"Assets/Helper.hlsl"

    float3 ViewDirFromScreenUV(float2 uv){
		float p1122 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
		return -normalize(float3(((uv) * 2 - 1) / p1122, -1)); //Dont multipy with texelsize...? Why? Looks kinda like nDotL to me or some shit xD //* _MainTex_TexelSize.xy
	}

    float2 ViewToScreenUV(float3 pos){
        float2 uv =0;
        float3 toCam = pos;
        float camPosZ = toCam.z;
        float height = 2 * camPosZ / unity_CameraProjection._m11;
        float width = _ScreenParams.x / _ScreenParams.y * height;
        uv.x = (toCam.x + width / 2)/width;
        uv.y = (toCam.y + height / 2)/height;
        return uv;
    }
            
    float3 ViewToWorld(float3 input){
        return mul(UNITY_MATRIX_I_V, float4(input, 1)).xyz;
    }

    //https://forum.unity.com/threads/shaderhow-to-convert-a-worldspace-position-to-screenpos-like-camera-worldtoviewportpoint.562078/
    float2 WorldToScreenPos(float3 pos){
        pos = normalize(pos - _WorldSpaceCameraPos)*(_ProjectionParams.y + (_ProjectionParams.z - _ProjectionParams.y))+_WorldSpaceCameraPos;
        float2 uv =0;
        float3 toCam = mul(unity_WorldToCamera, pos);
        float camPosZ = toCam.z;
        float height = 2 * camPosZ / unity_CameraProjection._m11;
        float width = _ScreenParams.x / _ScreenParams.y * height;
        uv.x = (toCam.x + width / 2)/width;
        uv.y = (toCam.y + height / 2)/height;
        return uv;
    }

    float3 CameraRelativePosition(float depth, float2 uv){
        float3 ws = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
        return _WorldSpaceCameraPos - ws;
    }

    float3 WorldSpacePositionFromDepth(float depth, float2 uv){
        return ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
    }

    real3 WorldPos(float2 uv){
        return WorldSpacePositionFromDepth(SampleSceneDepth(uv), uv);
    }

    void BinarySearchedRay(real3 beforeHit, real3 afterHit, real2 hitUV, real rlength, real distanceThreshhold, int steps, out real3 approxPos, out real2 approxUV){
        real lengthLocal = rlength * .5f;
        real distanceThreshholdLocal = distanceThreshhold;
        real decreaseMultiplier = .9f;  //<- get 50% more accurate each iteration

        //v we want to calculate from after to before
        real3 direction = beforeHit - afterHit;
        real3 iterativePosition = afterHit;
        int dirMul = 1; //<- this determines the direction the "direction" goes :D

        real3 approxPosLCL = afterHit;
        real2 approxUVLCL = hitUV;

        for(uint i = 0; i < steps; i++){
            //iterate over the ray-Position
            iterativePosition += (direction * dirMul) * lengthLocal;

            //now do basically the same as in RayMarchWS
            real2 thisUV = WSP2SUV(iterativePosition);
            real3 thisWSP = WorldPos(thisUV);

            //v this should be the longer distance to hit
            real distanceIT = length(iterativePosition - _WorldSpaceCameraPos);
            real actualDistance = length(thisWSP - _WorldSpaceCameraPos);

            //hit!
            real dist = distanceIT - actualDistance;

            //if hit we want to change direction and accuracly
            if(all((dist > distanceThreshhold))){
                //hit
                distanceThreshholdLocal *= decreaseMultiplier;
                dirMul = 1;
            }
            else{
                dirMul = -1;
            }
            lengthLocal *= decreaseMultiplier;

            approxUVLCL = thisUV;
            approxPosLCL = iterativePosition;
        }

        //apply and finish!
        approxPos = approxPosLCL;
        approxUV = approxUVLCL;
    }

    bool RayMarchWS(real3 startPosition, real3 direction, real stepLength, real distanceThreshhold, real maxDist, uint maxSteps, out real3 hitPos, out real2 hitUV, out real collisions){


            //Iterative position
            real3 posIT = startPosition;
            real3 lastPos = startPosition;

            for(uint i = 0; i < maxSteps; i++){
                posIT += direction * stepLength;
                real2 thisUV = WSP2SUV(posIT);
                //thisUV.x = 1 - thisUV.x;
                real3 thisWSP = WorldPos(thisUV);

                //v this should be the longer distance to hit
                real distanceIT = length(posIT - _WorldSpaceCameraPos);
                real actualDistance = length(thisWSP - _WorldSpaceCameraPos);

                //hit!
                real dist = distanceIT - actualDistance;
                if(all((dist > distanceThreshhold) && (length(posIT - thisWSP) < maxDist))){

                    //v this can be implemented more safetly
                    //if result is still "onscreen"
                    if(all(thisUV.x <= 1.0f && thisUV.y <= 1.0f && thisUV.x > 0 && thisUV.y > 0)){
                        //!!HIT!!
                        //if(length(thisUV - initialUV) <= _UVDiffThreshhold) continue;
                        //hitPos = posIT;
                        //hitUV = thisUV;
                        BinarySearchedRay(lastPos, posIT, thisUV, stepLength, distanceThreshhold, 15, hitPos, hitUV);
                        collisions = 1;
                        return true;
                        break;
                    }
                }

                lastPos = posIT;
            }
            hitPos = startPosition;
            collisions = 0;
            hitUV = real2(0, 0);
            return false;
    }


#endif