#ifndef QUANTUM_NOISE_INCLUDED
#define QUANTUM_NOISE_INCLUDED

#define Q_NOISE_SEED 43758.5453123

#define Q_RAND(x, postfix) \
inline float q_rand_1##postfix(inout float seed, float n){ \
    return frac(sin(x)*Q_NOISE_SEED); \
} \
inline float2 q_rand_2##postfix(inout float seed, float n){ \
    return frac(sin(float2(x, x))*Q_NOISE_SEED); \
} \
inline float3 q_rand_3##postfix(inout float seed, float n){ \
    return frac(sin(float3(x, x, x))*Q_NOISE_SEED); \
} \
inline float4 q_rand_4##postfix(inout float seed, float n){ \
    return frac(sin(float4(x, x, x, x))*Q_NOISE_SEED); \
} \

Q_RAND(seed++*n,)
Q_RAND(_SinTime.x+seed++*n,t)

// all the sub_noise multipliers will be condensed into one by the compiler, so this is a single multiplication and addition, not 3.
#define Q_BETTER_QUALITY_NOISE(dim,method_name) \
inline float##dim method_name##p(inout float seed, float n){ \
    float##dim sub_noise = method_name(seed, n) - 0.5f; \
    return frac(method_name(seed, n) + sub_noise / 4.0 + sub_noise / 16.0 + sub_noise / 64.0); \
} \

Q_BETTER_QUALITY_NOISE(, q_rand_1)
Q_BETTER_QUALITY_NOISE(, q_rand_1t)
Q_BETTER_QUALITY_NOISE(2, q_rand_2)
Q_BETTER_QUALITY_NOISE(2, q_rand_2t)
Q_BETTER_QUALITY_NOISE(3, q_rand_3)
Q_BETTER_QUALITY_NOISE(3, q_rand_3t)
Q_BETTER_QUALITY_NOISE(4, q_rand_4)
Q_BETTER_QUALITY_NOISE(4, q_rand_4t)

#endif