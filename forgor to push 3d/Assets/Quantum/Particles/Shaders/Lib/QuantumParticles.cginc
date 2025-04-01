#ifndef QUANTUM_GPU_PARTICLES_INCLUDED
#define QUANTUM_GPU_PARTICLES_INCLUDED

#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"


// multiply these values with any time based calculations to make them framerate independent
// and adjust to the simulation speed.
uniform float _SimSpeed;
// Calculates the adjusted delta time based on the current delta time and simulation speed.
inline float adjustedDeltaTime(){return unity_DeltaTime.x * _SimSpeed;}
inline float adjustedSmoothDeltaTime(){return unity_DeltaTime.z * _SimSpeed;}
// Calculates the adjusted time based on the current time and simulation speed.
inline float adjustedTime(){return _Time.y * _SimSpeed;}

// use these methods to convert between world and simulation space
// only meant for vectors like distances, lengths and local coordinates, but not world coordinates
uniform float _SimScale;
inline float simToWorldScale(float v){return v * _SimScale;}
inline float worldToSimScale(float v){return v / _SimScale;}
inline float2 simToWorldScale(float2 vec){return vec * _SimScale;}
inline float2 worldToSimScale(float2 vec){return vec / _SimScale;}
inline float3 simToWorldScale(float3 vec){return vec * _SimScale;}
inline float3 worldToSimScale(float3 vec){return vec / _SimScale;}

// the first few pixels of the data texture contain the simulator data
// this means the actual particle id is shifted by the simulator count
uniform uint _SimCount;
inline float2 uvFromSimID(int simID, float4 texelSize){
    return (float2(simID % texelSize.z, simID / texelSize.z)) * texelSize.xy;
}

inline float2 uvFromParticleID(int particleID, float4 texelSize){
    return uvFromSimID(particleID + _SimCount, texelSize);
}

inline uint simIDFromUV(float2 uv, float4 texelSize){
    return floor(uv.x * texelSize.z) + floor((uv.y % 0.5) * texelSize.w) * texelSize.z;
}

inline uint maxIDFromTexelSize(float4 texelSize){
    return texelSize.z * texelSize.w / 2.0 - _SimCount;
}

// TODO: this can still break if an uneven number of scales are negative
inline float3x3 unscaledMatrix(float3x3 m){
    float3x3 n = float3x3(normalize(m._m00_m10_m20), normalize(m._m01_m11_m21), normalize(m._m02_m12_m22));
    return float3x3(n._m00_m10_m20, n._m01_m11_m21, n._m02_m12_m22);
}

inline float4 getTranslation(float4x4 m){
    return m._m03_m13_m23_m33;
}

inline float3 unscaledMul(float4x4 m, float3 v){
    return mul(unscaledMatrix(float3x3(m[0].xyz, m[1].xyz, m[2].xyz)), v);
}

inline float4 unscaledMul(float4x4 m, float4 v){
    return float4(mul(unscaledMatrix(float3x3(m[0].xyz, m[1].xyz, m[2].xyz)), v.xyz), 0.0) + getTranslation(m) * v.w;
}

inline float textureClamp(float v){
    return clamp(v, 0.0f, 0.999f);
}

inline float4 sampleSimPos(sampler2D_float dataTex, float4 texelSize, int simID){
    return tex2Dlod(dataTex, float4((float(simID) + 0.5) * texelSize.x, 0.5 * texelSize.y, 0.0, 0.0));
}

inline float4 sampleSimRot(sampler2D_float dataTex, float4 texelSize, int simID){
    return tex2Dlod(dataTex, float4((float(simID) + 0.5) * texelSize.x, 0.5 * texelSize.y + 0.5, 0.0, 0.0));
}

inline float4 samplePosGlobal(sampler2D_float dataTex, float2 uv){
    return tex2Dlod(dataTex, float4(uv.x, uv.y % 0.5, 0.0, 0.0));
}

inline float4 samplePosLocal(sampler2D_float dataTex, float2 uv, float3 pos, float3x3 rot, float3 scale){
    float4 rawPos = samplePosGlobal(dataTex, uv);
    return float4(unscaledMul(unity_ObjectToWorld, float4(mul(rot, simToWorldScale(rawPos.xyz) * scale) + simToWorldScale(pos), 1.0)).xyz, rawPos.w);
}

inline float4 sampleVelGlobal(sampler2D_float dataTex, float2 uv){
    return tex2Dlod(dataTex, float4(uv.x, uv.y % 0.5 + 0.5, 0.0, 0.0));
}

inline float4 sampleVelLocal(sampler2D_float dataTex, float2 uv, float3x3 rot, float3 scale){
    float4 rawVel = sampleVelGlobal(dataTex, uv);
    return float4(unscaledMul(unity_ObjectToWorld, mul(rot, rawVel.xyz * scale)), rawVel.w);
}

inline float3 safeNormalize(float3 v){
    float len = length(v);
    return v * step(UNITY_HALF_MIN, len) / max(len, UNITY_HALF_MIN);
}

inline float2 safeNormalize(float2 v){
    float len = length(v);
    return v * step(UNITY_HALF_MIN, len) / max(len, UNITY_HALF_MIN);
}

inline float safePow(float x, float y){
    return pow(max(x, UNITY_HALF_MIN), y);
}

inline uint posMod(int x, int m){
    return uint((x % m + m) % m);
}

inline uint offsetParticleID(uint particleID, int offset, uint maxID){
    return posMod(int(particleID) + offset, maxID);
}

inline float2 offsetParticleUV(uint particleID, int offset, uint maxID, float4 texelSize){
    return uvFromParticleID(offsetParticleID(particleID, offset, maxID), texelSize);
}

inline int sampleOffsetTex(sampler2D_float offsetTex, float2 uv){
    return tex2Dlod(offsetTex, float4(uv.x, uv.y % 0.5, 0.0, 0.0)).x;
}

#endif