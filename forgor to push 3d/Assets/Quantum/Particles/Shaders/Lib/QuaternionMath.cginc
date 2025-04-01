#ifndef QUATERNION_MATH_INCLUDED
#define QUATERNION_MATH_INCLUDED

// mostly taken from https://gist.github.com/mattatz/40a91588d5fb38240403f198a938a593

// check alternatives: https://github.com/Thryrallo/UdonPrefabs/blob/af7351157515382214136567f2e3fb94f2aeedc0/ReactiveFloor/Includes/CGI_PoiMath.cginc#L4
// and: https://github.com/wraikny/VertexTransformShader/blob/master/src/quaternion.cginc

inline float4 q_unify(float4 q){
    return q / (q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
}

// get a quarternion from a rotation matrix
inline float4 quatFromMatrix(float3x3 m){
    // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
    float4 q;
    q.w = sqrt(max(0, 1 + m[0][0] + m[1][1] + m[2][2])) / 2;
    q.x = sqrt(max(0, 1 + m[0][0] - m[1][1] - m[2][2])) / 2;
    q.y = sqrt(max(0, 1 - m[0][0] + m[1][1] - m[2][2])) / 2;
    q.z = sqrt(max(0, 1 - m[0][0] - m[1][1] + m[2][2])) / 2;
    q.x *= sign(q.x * (m[2][1] - m[1][2]));
    q.y *= sign(q.y * (m[0][2] - m[2][0]));
    q.z *= sign(q.z * (m[1][0] - m[0][1]));
    return q_unify(q);
}

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
inline float4 qmul(float4 q1, float4 q2){
    return q_unify(float4(
        q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz),
        q1.w * q2.w - dot(q1.xyz, q2.xyz)
    ));
}

// Vector rotation with a quaternion
// http://mathworld.wolfram.com/Quaternion.html
inline float3 rotate_vector(float3 v, float4 r){
    if(length(v) == 0) return v;
    float4 r_c = r * float4(-1, -1, -1, 1);
    return qmul(r, qmul(float4(v, 0), r_c)).xyz;
}

inline float4 q_conj(float4 q){
    return float4(-q.x, -q.y, -q.z, q.w);
}

// https://jp.mathworks.com/help/aeroblks/quaternioninverse.html
inline float4 q_inverse(float4 q){
    float4 conj = q_conj(q);
    return q_unify(conj);
}

inline float4 q_diff(float4 q1, float4 q2){
    return qmul(q2, q_inverse(q1));
}

#endif