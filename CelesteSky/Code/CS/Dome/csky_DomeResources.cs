/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Skydome.
///----------------------------------------------
/// Resources.
///----------------------------------------------
/// Skydome resources.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    public partial class csky_Dome : MonoBehaviour
    {
  
        [Header("Resources")]

        [SerializeField] private csky_Resources m_Resources = null;

        /// <summary></summary>
        public csky_Resources Resources => m_Resources;
        
        /// <summary></summary>
        public bool CheckResources
        {
            get
            {
            #region [Mesh]

                // Check resources scriptable object.
                if(m_Resources == null) return false;
                
                // Check all meshes.
                if(m_Resources.sphereLOD0 == null)     return false;
                if(m_Resources.sphereLOD1 == null)     return false;
                if(m_Resources.sphereLOD2 == null)     return false;
                if(m_Resources.sphereLOD3 == null)     return false;

                if(m_Resources.hemisphereLOD0 == null) return false;
                if(m_Resources.hemisphereLOD1 == null) return false;
                if(m_Resources.hemisphereLOD2 == null) return false;
                if(m_Resources.hemisphereLOD3 == null) return false;

                if(m_Resources.atmosphereLOD0 == null) return false;
                if(m_Resources.atmosphereLOD1 == null) return false;
                if(m_Resources.atmosphereLOD2 == null) return false;
                if(m_Resources.atmosphereLOD3 == null) return false;

                if(m_Resources.QuadMesh == null) return false;

            #endregion

            #region [Shaders]

                // Check all shaders.
                if(m_Resources.atmosphereShader == null)       return false;
                if(m_Resources.deepSpaceShader == null)        return false;
                if(m_Resources.sunShader == null)              return false;
                if(m_Resources.moonShader == null)             return false;
                if(m_Resources.cloudsShader == null)           return false;
                if(m_Resources.ambientSkyboxShader == null)    return false;
                //if(m_Resources.skymapShader == null)           return false;

            #endregion

            #region [Material]

                // Check all materials.
                if(m_Resources.deepSpaceMaterial == null)        return false;
                if(m_Resources.sunMaterial == null)              return false;
                if(m_Resources.moonMaterial == null)             return false;
                if(m_Resources.atmosphereMaterial == null)       return false;
                if(m_Resources.cloudsMaterial == null)           return false;
                if(m_Resources.ambientSkyboxMaterial == null)    return false;
                //if(m_Resources.skymapMaterial == null)           return false;

            #endregion

                return true;
            }
        }

        /// <summary></summary>
        public bool IsReady{ get; private set; }


        /// <summary></summary>
        private void SetShadersToMaterials()
        {
            m_Resources.deepSpaceMaterial.shader        = m_Resources.deepSpaceShader;
            m_Resources.sunMaterial.shader              = m_Resources.sunShader;
            m_Resources.moonMaterial.shader             = m_Resources.moonShader;
            m_Resources.atmosphereMaterial.shader       = m_Resources.atmosphereShader; 
            m_Resources.cloudsMaterial.shader           = m_Resources.cloudsShader;
            m_Resources.ambientSkyboxMaterial.shader    = m_Resources.ambientSkyboxShader;
            //m_Resources.skymapMaterial.shader           = m_Resources.skymapShader;
        }

        /// <summary></summary>
        /// <param="quality"></param>
        private Mesh GetSphereMesh(csky_Quality4 quality)
        {
            switch(quality)
            {
                case csky_Quality4.Low: 
                    return m_Resources.sphereLOD3;

                case csky_Quality4.Medium:
                    return m_Resources.sphereLOD2; 

                case csky_Quality4.High: 
                    return m_Resources.sphereLOD1;

                case csky_Quality4.Ultra: 
                    return m_Resources.sphereLOD0;
            }
            return null;
        }

        /// <summary></summary>
        /// <param="quality"></param>
        private Mesh GetHemisphereMesh(csky_Quality4 quality)
        {
            switch(quality)
            {
                case csky_Quality4.Low:
                    return m_Resources.hemisphereLOD3; 

                case csky_Quality4.Medium: 
                    return m_Resources.hemisphereLOD2; 

                case csky_Quality4.High: 
                    return m_Resources.hemisphereLOD1; 

                case csky_Quality4.Ultra: 
                    return m_Resources.hemisphereLOD0; 
            }
            return null;      
        }

        /// <summary></summary>
        /// <param="quality"></param>
        private Mesh GetAtmosphereMesh(csky_Quality4 quality)
        {
            switch(quality)
            {
                case csky_Quality4.Low: 
                    return m_Resources.atmosphereLOD3; 

                case csky_Quality4.Medium: 
                    return m_Resources.atmosphereLOD2; 

                case csky_Quality4.High: 
                    return m_Resources.atmosphereLOD1;

                case csky_Quality4.Ultra:
                    return m_Resources.atmosphereLOD0; 
            }
            return null;
        }

        /// <summary></summary>
        private Mesh GetQuadMesh => m_Resources.QuadMesh;
    }
}