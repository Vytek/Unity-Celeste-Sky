/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Atmospheric Scattering.
///----------------------------------------------
/// Atmospheric Scattering based on
/// Naty Hoffman and Arcot. J. Preetham papers.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;


namespace CelesteSky
{
    [Serializable] public class csky_AtmosphericScattering
    {

    #region [Fields]

        // General Settings.
        [SerializeField] csky_ShaderQuality m_ShaderQuality       = csky_ShaderQuality.PerVertex;
        [SerializeField] csky_MoonRayleighMode m_MoonRayleighMode = csky_MoonRayleighMode.OpossiteSun;
        [SerializeField] csky_UpdateType m_BetaRayUpdate          = csky_UpdateType.OnInit;
        [SerializeField] csky_UpdateType m_BetaMieUpdate          = csky_UpdateType.OnInit;

        // Params.
        [SerializeField] csky_AtmosphericScatteringParams m_Params = new csky_AtmosphericScatteringParams
        {
            applyFastTonemap       = true,
            contrast               = 0.3f,
            groundColor            = Color.gray,
            wavelength             = csky_Wavelength.EarthValues2,
            tickness               = 1.0f,
            atmosphereHaziness     = 1.0f,
            atmosphereZenithOffset = 0.06f,
            rayleighZenithLength   = 8.4e3f,
            sunBrightness          = 40f,
            sunAtmosphereTint      = Color.white,
            sunAtmosphereGradient  = new Gradient(),
            moonContribution       = 0.075f,
            moonAtmosphereTint     = Color.white,
            mie                    = 0.005f,
            mieZenithLength        = 1.25e3f,
            sunMie                 = csky_MiePhase.Default,
            moonMie                = csky_MiePhase.Default
        };

       
        /// <summary></summary>
        public Color SunAtmosphereTint =>  m_Params.sunAtmosphereTint * m_Params.sunAtmosphereGradient.Evaluate(SunEvaluteTime);

        /// <summary></summary>
        public csky_MoonRayleighMode MoonRayleighMode => m_MoonRayleighMode;

    #endregion

    #region [Constants]

        /// <summary> Index of air refraction(n). </summary>
        const float k_n = 1.0003f;

        /// <summary> Molecular density(N). </summary>
        const float k_N = 2.545e25f;

        /// <summary> Depolatization factor for standart air. </summary>
        const float k_pn = 0.035f;

        /// <summary> Molecular density exponentially squared. </summary>
        const float k_n2 = k_n * k_n;

    #endregion

    #region [Properties]

        public Vector3 SunDir{ get; set; }
        public Vector3 MoonDir{ get; set; }
        public float SunEvaluteTime{ get; set; }

        /// <summary> Day Intensity. </summary>
        public float DayIntensity => (csky_Mathf.Saturate(SunDir.y + 0.40f) * m_Params.sunBrightness); 

        /// <summary> Sunset/Dawn atmosphere horizon. </summary>
        public float SunsetDawnHorizon => csky_Mathf.Saturate(Mathf.Clamp(1.0f - (SunDir.y), 0.0f, 1f)); 

        /// <summary></summary>
        public float MoonPhasesIntensityMultiplier => Mathf.Clamp01(Vector3.Dot(-SunDir, MoonDir) + 0.45f); 

        /// <summary> Night intensity </summary>
        public float NightIntensity
        {
            get
            {
                switch(m_MoonRayleighMode)
                {
                    case csky_MoonRayleighMode.OpossiteSun:
                        return csky_Mathf.Saturate(-SunDir.y + 0.25f);

                    case csky_MoonRayleighMode.CelestialContribution:
                        return csky_Mathf.Saturate(MoonDir.y + 0.25f);
                }
                return 0.0f;
            }
        }

        /// <summary></summary>
        public float NightIntensityMultiplier
        {
            get
            {
                switch(m_MoonRayleighMode)
                {
                    case csky_MoonRayleighMode.OpossiteSun:
                        return m_Params.moonContribution;

                    case csky_MoonRayleighMode.CelestialContribution:
                        return m_Params.moonContribution * MoonPhasesIntensityMultiplier;
                }

                return 0.0f;  // Off
            }
        }

        /// <summary> One part of Mie Phase </summary>
        public Vector3 PartialSunMiePhase => PartialMiePhase(m_Params.sunMie.anisotropy); 
        
        /// <summary> One part of HenyeyGreenstein for moon. </summary>
        public Vector3 PartialMoonMiePhase => PartialMiePhase(m_Params.moonMie.anisotropy); 
        
        /// <summary></summary>
        public Vector3 BetaRay{ get; private set; }

        /// <summary></summary>
        public Vector3 BetaMie{ get; private set; }

    #endregion

    #region [Initialize]

        public void Initialize()
        {
            if(m_BetaRayUpdate == csky_UpdateType.OnInit)
                SetBetaRay();
            
            if(m_BetaMieUpdate == csky_UpdateType.OnInit)
                SetBetaMie();
        }

    #endregion

    #region [SetParams]

        /// <summary> Set Glonal Parameters to materials </summary>
        public void SetGlobalParams()
        {

            /// General Settings.
            /////////////////////
            switch(m_ShaderQuality)
            {
                case csky_ShaderQuality.PerVertex: Shader.DisableKeyword(csky_Keywords.atms_PerPixelAtmosphere); break;
                case csky_ShaderQuality.PerPixel:  Shader.EnableKeyword(csky_Keywords.atms_PerPixelAtmosphere);  break;
            }

            if(m_Params.applyFastTonemap)
                Shader.EnableKeyword(csky_Keywords.atms_ApplyFastTonemaping);
            else
                Shader.DisableKeyword(csky_Keywords.atms_ApplyFastTonemaping);

            Shader.SetGlobalFloat(csky_PropertyIDs.atms_ContrastID, m_Params.contrast);
            Shader.SetGlobalColor(csky_PropertyIDs.atms_GroundColorID, m_Params.groundColor);

            /// Rayleigh.
            ////////////////////
            Shader.SetGlobalColor(csky_PropertyIDs.atms_SunAtmosphereTintID, SunAtmosphereTint); 

            switch(m_MoonRayleighMode)
            {
                case csky_MoonRayleighMode.OpossiteSun:
                    Shader.EnableKeyword(csky_Keywords.atms_EnableMoonRayleigh);
                    Shader.SetGlobalInt(csky_PropertyIDs.atms_MoonAtmosphereTintID, 0);
                break;

                case csky_MoonRayleighMode.CelestialContribution:
                    Shader.EnableKeyword(csky_Keywords.atms_EnableMoonRayleigh);
                    Shader.SetGlobalInt(csky_PropertyIDs.atms_MoonAtmosphereTintID, 1);
                break;

                case csky_MoonRayleighMode.Off: Shader.DisableKeyword(csky_Keywords.atms_EnableMoonRayleigh); break;
            }

            Shader.SetGlobalColor
            (
                csky_PropertyIDs.atms_MoonAtmosphereTintID, 
                m_Params.moonAtmosphereTint * 
                NightIntensityMultiplier
            );

            Shader.SetGlobalFloat(csky_PropertyIDs.atms_AtmosphereHazinessID, m_Params.atmosphereHaziness);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_AtmosphereZenithOffsetID, m_Params.atmosphereZenithOffset);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_SunsetDawnHorizonID, SunsetDawnHorizon);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_RayleighZenithLengthID, m_Params.rayleighZenithLength);
           
            if(m_BetaRayUpdate == csky_UpdateType.Realtime)
                SetBetaRay();

            if(m_BetaMieUpdate == csky_UpdateType.Realtime)
                SetBetaMie();

            Shader.SetGlobalVector(csky_PropertyIDs.atms_BetaRayID, BetaRay * m_Params.tickness);
            Shader.SetGlobalVector(csky_PropertyIDs.atms_BetaMieID, BetaMie);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_DayIntensityID, DayIntensity);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_NightIntensityID, NightIntensity);

            /// Mie.
            //////////////////
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_MieZenithLengthID, m_Params.mieZenithLength);

            // Sun
            Shader.SetGlobalColor(csky_PropertyIDs.atms_SunMieTintID, m_Params.sunMie.tint);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_SunMieAnisotropyID, m_Params.sunMie.anisotropy);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_SunMieScatteringID, m_Params.sunMie.scattering);
            Shader.SetGlobalVector(csky_PropertyIDs.atms_PartialSunMiePhaseID, PartialSunMiePhase);
            
            // Moon
            Shader.SetGlobalColor(csky_PropertyIDs.atms_MoonMieTintID, m_Params.moonMie.tint);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_MoonMieAnisotropyID, m_Params.moonMie.anisotropy);
            Shader.SetGlobalFloat(csky_PropertyIDs.atms_MoonMieScatteringID, m_Params.moonMie.scattering * MoonPhasesIntensityMultiplier);
            Shader.SetGlobalVector(csky_PropertyIDs.atms_PartialMoonMiePhaseID, PartialMoonMiePhase);

        }
    #endregion

    #region [Compute]

        /// <summary></summary>
        public void SetBetaRay()
        {
            BetaRay = ComputeBetaRay();
        }

        /// <summary></summary>
        public void SetBetaMie()
        {
            BetaMie = ComputeBetaMie();
        }

        /// <summary>
        /// Compute Beta Rayleigh
        /// Based on Preetham and Hoffman papers
        /// </summary>
        /// <param name="lambda"> Wavelength * 1e-9 </param>
        public Vector3 ComputeBetaRay()
        {
            Vector3 re;
           
            // Wavelength.
            Vector3 wl;
            wl.x = Mathf.Pow(m_Params.wavelength.Lambda.x, 4.0f);
            wl.y = Mathf.Pow(m_Params.wavelength.Lambda.y, 4.0f);
            wl.z = Mathf.Pow(m_Params.wavelength.Lambda.z, 4.0f);
            
            // Beta Rayleigh
            float r = (8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(k_n2 - 1.0f, 2.0f) * (6.0f + 3.0f * k_pn));
            Vector3 t = 3.0f * k_N * wl * (6.0f - 7.0f * k_pn);
            
            re.x = (r / t.x);
            re.y = (r / t.y);
            re.z = (r / t.z);
            
            return re;
        }

        /// <summary> Rayleigh Phase </summary>
        /// <param name="cosTheta"> Cos angle </param>
        public float RayleighPhase(float cosTheta)
        {
            return csky_Mathf.k_3PI16 * (1.0f + cosTheta * cosTheta);
        } 

        /// <summary> Compute one part of the Mie phase </summary>
        /// <param name="g"> Anisotropy </param>
        public Vector3 PartialMiePhase(float g)
        {
            Vector3 result;
            {
                float g2 = g * g;
                result.x = (1.0f - g2) / (2.0f + g2);
                result.y = 1.0f + g2;
                result.z = 2.0f * g;
            }
            return result;
        }

        /// <summary>
        /// Compute Mie Phase.
        /// </summary>
        /// <param name="g"> Anisotropy </param>
        /// <param name="cosTheta"> Cos Theta </param>
        public float MiePhase(float g, float cosTheta)
        {
            float g2 = g * g;
            Vector3 PHG = PartialMiePhase(g);
            return  csky_Mathf.k_InvPI4 * (PHG.x *  ((1.0f + cosTheta * cosTheta) * Mathf.Pow(PHG.y-(PHG.z*cosTheta),-1.5f)));
        }

        /// <summary>
        /// Compute BetaMie.
        /// Based on Preetham and Hoffman papers.
        /// </summary>
        public Vector3 ComputeBetaMie()
        {
            Vector3 re;

            //float turbidity = m_Mie * 0.05f;
            //Vector3 k = new Vector3(0.685f, 0.679f, 0.670f);
            Vector3 k = new Vector3(0.660f, 0.600f, 0.400f);
            float   c = (0.2f * m_Params.mie) * 10e-18f;
            float   mieFactor = 0.434f * c * Mathf.PI;
            float   v = 4.0f;

            re.x = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / m_Params.wavelength.Lambda.x, v - 2.0f) * k.x);
            re.y = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / m_Params.wavelength.Lambda.y, v - 2.0f) * k.y);
            re.z = (mieFactor * Mathf.Pow((2.0f * Mathf.PI) / m_Params.wavelength.Lambda.z, v - 2.0f) * k.z);
           
            return re;
        }

        /// <summary></summary>
        public void CustomOpticalDepth(float pos, out Vector2 srm)
        {

            pos = csky_Mathf.Saturate(pos * m_Params.atmosphereHaziness);
            float zenith = Mathf.Acos(pos);

            zenith = Mathf.Cos(zenith) + 0.15f * Mathf.Pow( 93.885f - ((zenith * 180f) / Mathf.PI), -1.253f);
            zenith = 1.0f/(zenith + (m_Params.atmosphereZenithOffset * 0.5f));

            srm.x = zenith * m_Params.rayleighZenithLength;
            srm.y = zenith * m_Params.mieZenithLength;
        }

        /// <summary></summary>
        public void OptimizedOpticalDepth(float pos, out Vector2 srm)
        {
            pos = csky_Mathf.Saturate(pos * m_Params.atmosphereHaziness);
            pos = 1.0f/(pos + m_Params.atmosphereZenithOffset);

            srm.x = pos * m_Params.rayleighZenithLength;
            srm.y = pos * m_Params.mieZenithLength;
        }

        /// <summary> Combined Extinction factor</summary>
        public Vector3 ComputeCEF(Vector2 srm)
        {
            Vector3 re;
            re.x = Mathf.Exp(-(BetaRay.x * srm.x + BetaMie.x * srm.y));
            re.y = Mathf.Exp(-(BetaRay.y * srm.x + BetaMie.y * srm.y));
            re.z = Mathf.Exp(-(BetaRay.z * srm.x + BetaMie.z * srm.y));
            return re;
        }

        /// <sammary></summary>
        public Vector3 ComputeAtmosphericScattering(Vector3 inCEF, float sunCosTheta, Vector3 sunMiPhase, Vector3 moonMiePhase)
        {
            Vector3 re; Vector3 cef;
            cef.x = Mathf.Lerp(1.0f-inCEF.x, (1.0f-inCEF.x) * inCEF.x, SunsetDawnHorizon);
            cef.y = Mathf.Lerp(1.0f-inCEF.y, (1.0f-inCEF.y) * inCEF.y, SunsetDawnHorizon);
            cef.z = Mathf.Lerp(1.0f-inCEF.z, (1.0f-inCEF.z) * inCEF.z, SunsetDawnHorizon);
            cef   = csky_Mathf.Saturate(cef);

            // Sun/Day calculations.
            //////////////////////////
            float sunRayleighPhase = RayleighPhase(sunCosTheta);
            Vector3 sunBRT = BetaRay * sunRayleighPhase;
           
            Vector3 sunBMT;
            sunBMT.x = BetaMie.x * sunMiPhase.x;
            sunBMT.y = BetaMie.y * sunMiPhase.y;
            sunBMT.z = BetaMie.z * sunMiPhase.z;

            Vector3 sunBRMT;
            sunBRMT.x = (sunBRT.x + sunBMT.x) / (BetaRay.x + BetaMie.x);
            sunBRMT.y = (sunBRT.y + sunBMT.y) / (BetaRay.y + BetaMie.y);
            sunBRMT.z = (sunBRT.z + sunBMT.z) / (BetaRay.z + BetaMie.z);

            // Scattering result for sun light.
            Vector3 sunScatter;
            sunScatter.x = DayIntensity * (sunBRMT.x * cef.x) * m_Params.sunAtmosphereTint.r;
            sunScatter.y = DayIntensity * (sunBRMT.y * cef.y) * m_Params.sunAtmosphereTint.g;
            sunScatter.z = DayIntensity * (sunBRMT.z * cef.z) * m_Params.sunAtmosphereTint.b;

            sunScatter.x = Mathf.Lerp(sunScatter.x * (1.0f - inCEF.x), sunScatter.x, SunAtmosphereTint.a);
            sunScatter.y = Mathf.Lerp(sunScatter.y * (1.0f - inCEF.y), sunScatter.y, SunAtmosphereTint.a);
            sunScatter.z = Mathf.Lerp(sunScatter.z * (1.0f - inCEF.z), sunScatter.z, SunAtmosphereTint.a);

            // Moon/Night calculations.
            ///////////////////////////
            
            // Used simple calculations for more performance.
            if(m_MoonRayleighMode != csky_MoonRayleighMode.Off)
            {
                Vector3 moonScatter;
                moonScatter.x = NightIntensity * (1.0f - inCEF.x) * m_Params.moonAtmosphereTint.r;
                moonScatter.y = NightIntensity * (1.0f - inCEF.y) * m_Params.moonAtmosphereTint.g;
                moonScatter.z = NightIntensity * (1.0f - inCEF.z) * m_Params.moonAtmosphereTint.b;

                moonScatter += moonMiePhase;

                re = sunScatter + moonScatter;
            }
            else
            {
                re = sunScatter + moonMiePhase;
            }
            return re;
        }

        /* 
        /// <summary> WIP </summary>
        public Color GetAmosphereColor(Vector3 pos)
        {

            Color re = Color.white;

            float sunCosTheta = Vector3.Dot(SunDir, pos);

            Vector2 srm;
            CustomOpticalDepth(pos.y, out srm);

            // Combined extinction factor
            Vector3 cef = ComputeCEF(srm);
            Vector3 atm = ComputeAtmosphericScattering(cef, sunCosTheta, Vector3.one, Vector3.one);

            re.r = atm.x;
            re.g = atm.y;
            re.b = atm.z;
            re.a = 1.0f;

            return re;
        }*/

    #endregion

    }
}