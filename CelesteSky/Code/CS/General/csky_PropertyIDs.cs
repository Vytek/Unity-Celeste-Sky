/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// PropertyIDs.
///----------------------------------------------
/// All shaders propertty IDs for Celeste Sky.
/////////////////////////////////////////////////
using System;
using UnityEngine;

namespace CelesteSky
{

    static class csky_PropertyIDs
    {
    #region [General]

        // Color.
        internal static readonly int g_ExposureID      = Shader.PropertyToID("csky_GlobalIntensity");
        internal static readonly int g_atmosphereTexID = Shader.PropertyToID("csky_AtmosphereTex");

        // Position.
        internal static readonly int g_WorldSunDirectionID  = Shader.PropertyToID("csky_WorldSunDirection");
        internal static readonly int g_WorldMoonDirectionID = Shader.PropertyToID("csky_WorldMoonDirection");
        internal static readonly int g_LocalSunDirectionID  = Shader.PropertyToID("csky_LocalSunDirection");
        internal static readonly int g_LocalMoonDirectionID = Shader.PropertyToID("csky_LocalMoonDirection");
        internal static readonly int g_ObjectToWorldID      = Shader.PropertyToID("csky_ObjectToWorld");
        internal static readonly int g_WorldToObjectID      = Shader.PropertyToID("csky_WorldToObject");

    #endregion

    #region [Atmospheric Scattering]

        // General.
        //////////////
        internal static readonly int atms_ContrastID    = Shader.PropertyToID("csky_AtmosphereContrast");
        internal static readonly int atms_GroundColorID = Shader.PropertyToID("csky_GroundColor");

        // Rayleigh.
        /////////////
        // Wavelength.
        internal static readonly int atms_WavelengthRID = Shader.PropertyToID("csky_WavelengthR");
        internal static readonly int atms_WavelengthGID = Shader.PropertyToID("csky_WavelengthG");
        internal static readonly int atms_WavelengthBID = Shader.PropertyToID("csky_WavelengthB");

        // Zenith and tickness.
        internal static readonly int atms_Tickness                 = Shader.PropertyToID("csky_Tickness");
        internal static readonly int atms_AtmosphereHazinessID     = Shader.PropertyToID("csky_AtmosphereHaziness");
        internal static readonly int atms_AtmosphereZenithOffsetID = Shader.PropertyToID("csky_AtmosphereZenithOffset");
        internal static readonly int atms_RayleighZenithLengthID   = Shader.PropertyToID("csky_RayleighZenithLength");

        // Color.
        internal static readonly int atms_BetaRayID           = Shader.PropertyToID("csky_BetaRay");
        internal static readonly int atms_SunsetDawnHorizonID = Shader.PropertyToID("csky_SunsetDawnHorizon");

        // Sun.
        internal static readonly int atms_SunBrightnessID      = Shader.PropertyToID("csky_SunBrightness");
        internal static readonly int atms_SunAtmosphereTintID  = Shader.PropertyToID("csky_SunAtmosphereTint");
        internal static readonly int atms_DayIntensityID       = Shader.PropertyToID("csky_DayIntensity");

        // Moon.
        internal static readonly int atms_MoonContributionID   = Shader.PropertyToID("csky_MoonContribution");
        internal static readonly int atms_MoonAtmosphereTintID = Shader.PropertyToID("csky_MoonAtmosphereTint");
        internal static readonly int atms_NightIntensityID     = Shader.PropertyToID("csky_NightIntensity");
        internal static readonly int atms_MoonRayleighModeID   = Shader.PropertyToID("csky_MoonRayleighMode");

        // Mie.
        /////////////
        // General.
        internal static readonly int atms_MieID             = Shader.PropertyToID("csky_Mie");
        internal static readonly int atms_MieZenithLengthID = Shader.PropertyToID("csky_MieZenithLength");
        internal static readonly int atms_BetaMieID         = Shader.PropertyToID("csky_BetaMie");

        // Sun.
        internal static readonly int atms_SunMieTintID         = Shader.PropertyToID("csky_SunMieTint");
        internal static readonly int atms_SunMieAnisotropyID   = Shader.PropertyToID("csky_SunMieAnisotropy");
        internal static readonly int atms_SunMieScatteringID   = Shader.PropertyToID("csky_SunMieScattering");
        internal static readonly int atms_PartialSunMiePhaseID = Shader.PropertyToID("csky_PartialSunMiePhase");

        // Moon.
        internal static readonly int atms_MoonMieTintID         = Shader.PropertyToID("csky_MoonMieTint");
        internal static readonly int atms_MoonMieAnisotropyID   = Shader.PropertyToID("csky_MoonMieAnisotropy");
        internal static readonly int atms_MoonMieScatteringID   = Shader.PropertyToID("csky_MoonMieScattering");
        internal static readonly int atms_PartialMoonMiePhaseID = Shader.PropertyToID("csky_PartialMoonMiePhase"); 

    #endregion
    
    #region [Deep Space]

        // Galaxu background.
        internal static readonly int gb_CubemapID   = Shader.PropertyToID("csky_GalaxyBackgroundCubemap");
        internal static readonly int gb_TintID      = Shader.PropertyToID("csky_GalaxyBackgroundTint");
        internal static readonly int gb_IntensityID = Shader.PropertyToID("csky_GalaxyBackgroundIntensity");
        internal static readonly int gb_ContrastID  = Shader.PropertyToID("csky_GalaxyBackgroundContrast");
        
        // Stars Field.
        internal static readonly int sf_CubemapID            = Shader.PropertyToID("csky_StarsFieldCubemap");
        internal static readonly int sf_NoiseCubemapID       = Shader.PropertyToID("csky_StarsFieldNoiseCubemap");
        internal static readonly int sf_TintID               = Shader.PropertyToID("csky_StarsFieldTint");
        internal static readonly int sf_IntensityID          = Shader.PropertyToID("csky_StarsFieldIntensity");
        internal static readonly int sf_NoiseMatrixID        = Shader.PropertyToID("csky_StarsFieldNoiseMatrix");
        internal static readonly int sf_ScintillationID      = Shader.PropertyToID("csky_StarsFieldScintillation");
        internal static readonly int sf_ScintillationSpeedID = Shader.PropertyToID("csky_StarsFieldScintillationSpeed");

    #endregion

    #region [Near Space]

        // Sun.
        internal static readonly int s_TexID       = Shader.PropertyToID("csky_SunTex");
        internal static readonly int s_TintID      = Shader.PropertyToID("csky_SunTint");
        internal static readonly int s_IntensityID = Shader.PropertyToID("csky_SunIntensity");

        // Moon.
        internal static readonly int m_TexID       = Shader.PropertyToID("csky_MoonTex");
        internal static readonly int m_TintID      = Shader.PropertyToID("csky_MoonTint");
        internal static readonly int m_IntensityID = Shader.PropertyToID("csky_MoonIntensity");
        internal static readonly int m_ContrastID  = Shader.PropertyToID("csky_MoonContrast");

    #endregion

    #region [Clouds]

        internal static readonly int c_TexID       = Shader.PropertyToID("csky_CloudsTex");
        internal static readonly int c_TintID      = Shader.PropertyToID("csky_CloudsTint");
        internal static readonly int c_IntensityID = Shader.PropertyToID("csky_CloudsIntensity");
        internal static readonly int c_DensityID   = Shader.PropertyToID("csky_CloudsDensity");
        internal static readonly int c_CoverageID  = Shader.PropertyToID("csky_CloudsCoverage");
        internal static readonly int c_SpeedID     = Shader.PropertyToID("csky_CloudsSpeed");
        internal static readonly int c_Speed2ID    = Shader.PropertyToID("csky_CloudsSpeed2"); 

    #endregion

    #region [Ṕost Processing]

        // Fog.
        /////////
        internal static readonly int f_DensityID       = Shader.PropertyToID("csky_FogDensity");
        internal static readonly int f_StartDistanceID = Shader.PropertyToID("csky_FogStartDistance");
        internal static readonly int f_EndDistanceID   = Shader.PropertyToID("csky_FogEndDistance");

        // Scattering.
        ///////////////
        internal static readonly int f_RayleighDepthMultID    = Shader.PropertyToID("csky_RayleighDepthMultiplier");
        internal static readonly int f_SunMiePhaseDepthMultID = Shader.PropertyToID("csky_SunMiePhaseDepthMultiplier");
        internal static readonly int f_MoonMiePhaseDepthMultID = Shader.PropertyToID("csky_MoonMiePhaseDepthMultiplier");

        internal static readonly int f_SmoothTintID = Shader.PropertyToID("csky_PPSmoothTint");
        internal static readonly int f_BlendTintID  = Shader.PropertyToID("csky_PPBlendTint");


    #endregion
    
    }
}