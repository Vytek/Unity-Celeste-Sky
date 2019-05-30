#ifndef CELESTE_SKY_ATMOSPHERIC_SCATTERING_COMMON
#define CELESTE_SKY_ATMOSPHERIC_SCATTERING_COMMON
////////////////////////////////////////////////
/// For Preetham model and Eric Brunetton model
/// Mie phase multiplier(m) is 1.0/(PI*4)
/// Rayleigh phase multiplier is 3/(PI*16)
////////////////////////////////////////////////

// Rayleigh Phase
////////////////////
inline float CSky_RayleighPhase(float m, float cosTheta)
{
    return m * (1.0 + cosTheta * cosTheta);
}

// Mie Phase
///////////////////
inline float3 CSky_PartialMiePhase(float g)
{
    float g2 = g*g;
    return float3((1.0 - g2) / (2.0 + g2), 1.0 + g2, 2.0 * g);
}

inline float CSky_MiePhase(float m, float cosTheta, float g, half scattering)
{
    float3 PHG = CSky_PartialMiePhase(g);
    return (m * PHG.x * ((1.0 + cosTheta * cosTheta) * pow(PHG.y - (PHG.z * cosTheta), -1.5))) * scattering;
}

inline float CSky_PartialMiePhase(float m, float cosTheta, float3 partialMiePhase, half scattering)
{
    return
    (
        m * partialMiePhase.x * ((1.0 + cosTheta * cosTheta) *
        pow(partialMiePhase.y - (partialMiePhase.z * cosTheta), -1.5))
    ) * scattering;
}

// Color Correction
/////////////////////
inline void CSky_ATMSColorCorrection(inout half3 col, half exposure, half contrast)
{
    #if defined(CSKY_ATMS_APPLY_FAST_TONEMAP)
    col.rgb = CSky_FastTonemap(col.rgb, exposure);
    #else
    col.rgb *= exposure;
    #endif

    col.rgb = CSky_Pow3(col.rgb, contrast);
    #if defined(UNITY_COLORSPACE_GAMMA)
    col.rgb = CSKY_LINEAR_TO_GAMMA(col.rgb);
    #endif
}

inline void CSky_ATMSColorCorrection(inout half3 col, half3 groundCol, half exposure, half contrast)
{
    CSky_ATMSColorCorrection(col.rgb, exposure, contrast);
    groundCol.rgb *= groundCol.rgb;
}

// Ground
////////////////////
inline half CSky_GroundMask(float pos)
{
    return saturate(-pos*100);
}

inline half3 CSky_ATMSApplyGroundColor(float pos, half3 skyCol)
{
    fixed mask = CSky_GroundMask(pos);
    return lerp(skyCol.rgb, csky_GroundColor.rgb, mask);
}

#endif // CELESTE SKY ATMOSPHERIC SCATTERING COMMON INCLUDED.