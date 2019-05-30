
#ifndef CELESTE_SKY_ATMOSPHERIC_SCATTERING_VARIABLES
#define CELESTE_SKY_ATMOSPHERIC_SCATTERING_VARIABLES

// General.
//////////////////
uniform half csky_AtmosphereContrast;
uniform half4 csky_GroundColor;

// Sun.
uniform half  csky_SunBrightness;
uniform half4 csky_SunAtmosphereTint;

// Moon.
uniform half  csky_MoonContribution;
uniform half3 csky_MoonAtmosphereTint;

// Mie.
//////////////////
// Sun
uniform float csky_SunMieAnisotropy;
uniform half4 csky_SunMieTint;
uniform half  csky_SunMieScattering;

// Partial Mie Phase.
uniform float3 csky_PartialSunMiePhase;

// Partial Mie Phase.
uniform float3 csky_PartialMoonMiePhase;

// Moon
uniform float csky_MoonMieAnisotropy;
uniform half4 csky_MoonMieTint;
uniform half  csky_MoonMieScattering;


// Post Proccesing.
////////////////////
uniform half csky_SunMiePhaseDepthMultiplier;
uniform half csky_MoonMiePhaseDepthMultiplier;
uniform half csky_RayleighDepthMultiplier;

uniform float csky_PPBlendTint;
uniform float csky_PPSmoothTint;

#endif // CELESTE SKY  ATMOSPHERIC SCATTERING VARIABLES
