/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Post Processing Base
///----------------------------------------------
/// Base for post processing.
/////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteSky.Legacy
{

    [RequireComponent(typeof(Camera))]
    public abstract partial class csky_PostProcessingBase : MonoBehaviour
    {

        // Camera.
        ////////////////
        protected Camera m_Camera = null;
        protected Transform m_CameraTransform = null;

        // Resources.
        ////////////////
        [SerializeField] protected Shader m_Shader = null;
        public Shader FXShader
        {
            get{ return m_Shader; }
            set{ m_Shader=value; }
        }

        protected Material m_Material = null;
        public Material FXMaterial
        {
            get
            {
                if(m_Material == null && m_Shader != null)
                    m_Material = new Material(m_Shader);
                
                return m_Material;
            }
        }

        [SerializeField] protected bool m_IsReady = false;
        protected bool CheckResources
        {
            get
            {
                if(m_Camera == null)          return false;
                if(m_CameraTransform == null) return false;
                if(m_Shader == null)          return false;

                return true;
            }

        }

        protected bool CheckSupport
        {
            get
            {

                // Check image effect support.
                //if(!SystemInfo.supportsImageEffects)
                //    return false;

                // Chack render texture support.
                if(!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
                    return false;

                // Check shader support.
                if(!FXMaterial.shader.isSupported)
                    return false;

                return true;
            }

        }

        protected virtual void Start()
        {
            m_Camera = GetComponent<Camera>();
            m_CameraTransform = m_Camera.transform;

            if(CheckResources && CheckSupport)
            {
                m_IsReady = true;
            }
        }

        /// <summary></summary>
        protected Matrix4x4 FrustumCorners()
        {

            float cameraNear = m_Camera.nearClipPlane;
            float cameraFar  = m_Camera.farClipPlane;
            float cameraFOV  = m_Camera.fieldOfView;
            float cameraAspectRatio = m_Camera.aspect;

            //---------------------------------------------
            Vector3 right   = m_CameraTransform.right;
            Vector3 forward = m_CameraTransform.forward;
            Vector3 up      = m_CameraTransform.up;

            //---------------------------------------------
            float fovWHalf = cameraFOV * 0.5f;

            //---------------------------------------------
            Matrix4x4 res = Matrix4x4.identity;

            Vector3 toRight = right * cameraNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * cameraAspectRatio;
            Vector3 toTop   = up    * cameraNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);
            Vector3 topLeft = (forward * cameraNear - toRight + toTop);

            //----------------------------------------------
            float camScale = topLeft.magnitude * cameraFar/cameraNear;
            topLeft.Normalize();
            topLeft *= camScale;

            //----------------------------------------------
            Vector3 topRight = forward * cameraNear + toRight + toTop;
            topRight.Normalize();
            topRight *= camScale;

            //----------------------------------------------
            Vector3 bottomRight = forward * cameraNear + toRight - toTop;
            bottomRight.Normalize();
            bottomRight *= camScale;

            //----------------------------------------------
            Vector3 bottomLeft = forward * cameraNear - toRight - toTop;
            bottomLeft.Normalize();
            bottomLeft *= camScale;

            //----------------------------------------------
            res.SetRow(0, bottomLeft);
            res.SetRow(1, bottomRight);
            res.SetRow(2, topLeft);    
            res.SetRow(3, topRight);

            return res;
        }

        /// <summary></summary>
        protected static void CustomBlit(RenderTexture source, RenderTexture destination, Material material, int passNr = 0)
        {
            RenderTexture.active = destination;
            material.SetTexture("_MainTex", source);

            //------------------------------------------------
            GL.PushMatrix();
            GL.LoadOrtho();

            //------------------------------------------------
            material.SetPass(passNr);

            //------------------------------------------------

            GL.Begin(GL.QUADS);

            //------------------------------------------------
            GL.MultiTexCoord2(0, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

            //------------------------------------------------
            GL.MultiTexCoord2(0, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 2.0f); // BR
            
            //------------------------------------------------
            GL.MultiTexCoord2(0, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

            //------------------------------------------------
            GL.MultiTexCoord2(0, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f); // TL
            //------------------------------------------------

            GL.End();
            GL.PopMatrix();

        }

        protected virtual void OnDisable()
        {
            DestroyImmediate(m_Material);
        }
    }
}
