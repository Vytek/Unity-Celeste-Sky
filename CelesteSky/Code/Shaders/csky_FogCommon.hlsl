#ifndef CELESTE_SKY_FOG_COMMON
#define CELESTE_SKY_FOG_COMMON

// Distance
////////////
inline float CSky_FogDistance(float depth)
{
    float dist = depth * _ProjectionParams.z;
    return dist - _ProjectionParams.y;
}


/// Fog Factor
///////////////
// See: https://docs.microsoft.com/en-us/windows/desktop/direct3d9/fog-formulas
inline float CSky_FogExpFactor(float depth, float density)
{
    float dist = CSky_FogDistance(depth);
    return 1.0 - saturate(exp2(-density * dist));
}

inline float CSky_FogExp2Factor(float depth, float density)
{
    float re = CSky_FogDistance(depth);
    re       = density * re;
    return 1.0 - saturate(exp2(-re * re));
}

inline float CSky_FogLinearFactor(float viewDir, float2 startEnd)
{
    float dist = CSky_FogDistance(viewDir);
    dist       = (startEnd.y - dist) / (startEnd.y - startEnd.x);
    return 1.0 - saturate(dist);
}

#endif // CELESTE SKY FOG INCLUDED.
