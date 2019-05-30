/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Skydome.
///----------------------------------------------
/// Render.
///----------------------------------------------
/// Skydome Render.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    public partial class csky_Dome : MonoBehaviour
    {

        [SerializeField] private Camera m_MainCam    = null;
        [SerializeField] private bool m_FollowCamera = false;
        private Transform m_MainCamTr = null;

        private void InitializeCamera()
        {
            if(m_MainCam == null)
            {
                m_MainCamTr = null;
                m_MainCam = Camera.main;
                return;
            }
            m_MainCamTr               = m_MainCam.transform;
            m_MainCam.clearFlags      = CameraClearFlags.SolidColor;
            m_MainCam.backgroundColor = Color.black;
        }

        private void FollowCamera()
        {
            if(m_MainCam == null && m_MainCamTr == null)
            {
                return;
            }
            _Transform.position = m_MainCamTr.position + m_MainCamTr.rotation * Vector3.one;
        }

        private void RenderElements()
        {
            if(m_RenderAtmosphere)
            {
                Graphics.DrawMesh(
                    GetAtmosphereMesh(m_AtmosphereMeshQuality),
                    m_AtmosphereTrRef.transform.localToWorldMatrix,
                    m_Resources.atmosphereMaterial,
                    m_AtmosphereLayerIndex
                );
            }

            if(m_RenderDeepSpace)
            {
                
                Graphics.DrawMesh(
                    m_Resources.sphereLOD3,
                    m_GalaxyBrgTrRef.transform.localToWorldMatrix,
                    m_Resources.deepSpaceMaterial, m_DeepSpaceLayerIndex
                );
            }

            if(m_RenderSun)
            {
                
                Graphics.DrawMesh(
                    GetQuadMesh,
                    m_SunTrRef.transform.localToWorldMatrix,
                    m_Resources.sunMaterial,
                    m_SunLayerIndex
                );
            }

            if(m_RenderMoon)
            {
                
                Graphics.DrawMesh(
                    GetSphereMesh(csky_Quality4.Low),
                    m_MoonTrRef.transform.localToWorldMatrix,
                    m_Resources.moonMaterial,
                    m_MoonLayerIndex
                );
            }

            if(m_RenderClouds)
            {
                
                Graphics.DrawMesh(
                    GetHemisphereMesh(csky_Quality4.Low),
                    m_CloudsTrRef.transform.localToWorldMatrix,
                    m_Resources.cloudsMaterial,
                    m_CloudsLayerIndex
                );

            }
        }
    }
}