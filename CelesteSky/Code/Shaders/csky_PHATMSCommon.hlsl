#ifndef CELESTE_SKY_PREEHTAM_HOFFMAN_ATMOSPHERIC_SCATTERING
#define CELESTE_SKY_PREEHTAM_HOFFMAN_ATMOSPHERIC_SCATTERING

#include "csky_ATMSVariables.hlsl"
#include "csky_ATMSCommon.hlsl"
//////////////////////////////////////////////////
/// Atmospheric Scattering based on 
/// Naty Hoffman and Arcot. J. Preetham papers.
//////////////////////////////////////////////////

// Params
////////////////////
uniform float csky_AtmosphereHaziness;
uniform float csky_AtmosphereZenithOffset;

uniform float csky_RayleighZenithLength;
uniform float csky_MieZenithLength;

uniform float3 csky_BetaRay;
uniform float3 csky_BetaMie;

uniform float csky_SunsetDawnHorizon;
uniform half csky_DayIntensity;
uniform half csky_NightIntensity;

// Optical Depth
//////////////////
inline void CustomOpticalDepth(float pos, inout float2 srm)
{
    pos = saturate(pos * csky_AtmosphereHaziness); 
    float zenith = acos(pos);
    zenith       = cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180) / UNITY_PI), -1.253);
    zenith       = 1.0/(zenith + (csky_AtmosphereZenithOffset * 0.5));

    srm.x = zenith * csky_RayleighZenithLength;
    srm.y = zenith * csky_MieZenithLength;
}

inline void OptimizedOpticalDepth(float pos, inout float2 srm)
{
    pos   = saturate(pos * csky_AtmosphereHaziness);
    pos   = 1.0/(pos + csky_AtmosphereZenithOffset);

    srm.x = pos * csky_RayleighZenithLength;
    srm.y = pos * csky_MieZenithLength;
}

// cOMBINED EXTINCTION FACTOR.
inline float3 CSky_ComputeCEF(float2 srm)
{
    return exp(-(csky_BetaRay * srm.x + csky_BetaMie * srm.y));
}

inline half3 CSky_ComputePHAtmosphericScattering(float3 inCef, float sunCosTheta, float3 sunMiePhase, float3 moonMiePhase, float depth)
{
    
    float3 cef = saturate(lerp(1.0-inCef, (1.0-inCef) * inCef, csky_SunsetDawnHorizon));

    // Sun/Day calculations
    //////////////////////////
    float sunRayleighPhase = CSky_RayleighPhase(csky_3PI16, sunCosTheta);
    float3 sunBRT          = csky_BetaRay * sunRayleighPhase;

    // Multiply per zdepth
    #if defined(CSKY_POST_PROCESSING)
    float depthmul = depth * csky_RayleighDepthMultiplier;
    sunBRT *= depthmul;
    #endif

    float3 sunBMT  = csky_BetaMie * sunMiePhase;
    float3 sunBRMT = (sunBRT + sunBMT) / (csky_BetaRay + csky_BetaMie);

    // Scattering result for sun light
    half3 sunScatter = csky_DayIntensity * (sunBRMT*cef) * csky_SunAtmosphereTint;
    sunScatter       = lerp(sunScatter * (1.0-inCef), sunScatter, csky_SunAtmosphereTint.a);

    // Moon/Night calculations
    ///////////////////////////
    // Used simple calculations for more performance
    #if defined(CSKY_ENABLE_MOON_RAYLEIGH)
        half3 moonScatter = csky_NightIntensity.x * (1.0-inCef) * csky_MoonAtmosphereTint;

        // Multiply per zdepth
        #if defined(CSKY_POST_PROCESSING)
        moonScatter *= depthmul;
        #endif

        moonScatter += moonMiePhase; // Add moon mie phase
        return (sunScatter + moonScatter);
    #else
        return (sunScatter + moonMiePhase);
    #endif
}

inline half3 CSky_RenderPHATMS(float3 pos, out float3 sunMiePhase, out float3 moonMiePhase, float2 depth)
{
    half3 re = csky_Zero3;
    half3 multParams = csky_One3;

    // Get common multipliers
    #if defined(CSKY_POST_PROCESSING)
    multParams.x = (depth.x * csky_SunMiePhaseDepthMultiplier);
    multParams.y = (depth.x * csky_MoonMiePhaseDepthMultiplier);
    #else
    multParams.z = 1.0 - CSky_GroundMask(pos.y); // Get upper sky mask
    #endif

    float2 cosTheta = float2(
        dot(pos.xyz, csky_LocalSunDirection.xyz), // Sun
        dot(pos.xyz, csky_LocalMoonDirection.xyz) // Moon
    );

    // Compute post processing y position
    #if defined(CSKY_POST_PROCESSING)
    float3 p;
    p.x = saturate(depth.x + 2.0);
    p.y = saturate(depth.x - 2.0);
    p.z = smoothstep(p.x, p.y, csky_PPBlendTint);
    pos.y   = lerp(pos.y, p.z, csky_PPSmoothTint);
    #endif

    // Compute optical depth
    float2 srm;
    #if defined(SHADER_API_MOBILE)
    OptimizedOpticalDepth(pos.y, srm);
    #else
    CustomOpticalDepth(pos.y, srm);
    #endif

    // Get combined extinction factor
    float3 cef = CSky_ComputeCEF(srm);

    #if defined(CSKY_ENABLE_MIE_PHASE)
    sunMiePhase   = CSky_PartialMiePhase(csky_INVPI4, cosTheta.x, csky_PartialSunMiePhase, csky_SunMieScattering);
    sunMiePhase  *= multParams.x * csky_SunMieTint.rgb * multParams.z;
    moonMiePhase  = CSky_PartialMiePhase(csky_INVPI4, cosTheta.y, csky_PartialMoonMiePhase, csky_MoonMieScattering);
    moonMiePhase *= multParams.y * csky_MoonMieTint.rgb * multParams.z;
    re.rgb = CSky_ComputePHAtmosphericScattering(cef, cosTheta.x, sunMiePhase, moonMiePhase, depth.y);
    #else
    sunMiePhase = csky_Zero3;
    moonMiePhase = csky_Zero3;
    re.rgb = CSky_ComputePHAtmosphericScattering(cef, cosTheta.x, sunMiePhase, moonMiePhase, depth.y);
    #endif

    CSky_ATMSColorCorrection(re.rgb, csky_GroundColor.rgb, csky_GlobalIntensity, csky_AtmosphereContrast);
    //re = CSky_ATMSApplyGroundColor(pos.y, re);

    return re;
}

#endif // CELESTE SKY PREETHAM AND HOFFMAN ATMOSPHERIC SCATTERING INCLUDED.
