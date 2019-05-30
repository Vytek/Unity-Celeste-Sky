/////////////////////////////////////////////////
/// Celeste Sky.
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Mathf.
///----------------------------------------------
/// Description: Extensions for math.
/////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteSky.Utility
{
    public partial struct csky_Mathf
    {
        #region [PI]

        /// <summary> PI/2 </summary>
        public const float k_HalfPI = 1.570796f;

        /// <summary> 1 / (PI/2) </summary>
        public const float k_InvHalfPI = 0.636619f;

        /// <summary> PI*2 </summary>
        public const float k_Tau = 6.283185f;

        /// <summary> 1/(PI*2) </summary>
        public const float k_InvTau = 0.159154f;

        /// <summary> PI*4 </summary>
        public const float k_PI4 = 12.566370f;

        /// <summary> 1/(PI*4) </summary>
        public const float k_InvPI4 = 0.079577f;

        /// <summary> 3/(PI*8) </summary>
        public const float k_3PIE = 0.119366f;

        /// <summary> 3/(PI*16) </summary>
        public const float k_3PI16 = 0.059683f;

        #endregion

        #region [Generic]

        /// <summary></summary>
        public static float Saturate(float x) => Mathf.Clamp01(x);
        
        /// <summary></summary>
        public static Vector3 Saturate(Vector3 vec)
        {
            Vector3 re;
            re.x = Mathf.Clamp01(vec.x);
            re.y = Mathf.Clamp01(vec.y);
            re.z = Mathf.Clamp01(vec.z);
            return re;
        }

        /// <summary></summary>
        /// <param name="x"> Degrees </param>
        public static float Rev(float x)
        {
            return x - Mathf.Floor(x/360f) * 360f;
        }

        #endregion

        #region [Coordinate System]

        /// <summary> Convert spherical coordinates to cartesian coordinates. </summary>
        /// <param name="theta"> Theta. </param>
        /// <param name="pi"> PI. </param>
        /// <returns> XYZ coordinates. </returs>
        /// See: https://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
        public static Vector3 SphericalToCartesian(float theta, float pi)
        {
            Vector3 re;

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI    = Mathf.Sin(pi);
            float cosPI    = Mathf.Cos(pi);

            re.x = sinTheta * sinPI;
            re.y = cosTheta;
            re.z = sinTheta * cosPI;

            return re;
        }

        /// <summary> Convert spherical coordinates to cartesian coordinates. </summary>
        /// <param name="theta"> Theta. </param>
        /// <param name="pi"> PI. </param>
        /// <param name="rad"> Radius. </param>
        /// <returns> XYZ coordinates. </returs>
        /// See: https://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
        public static Vector3 SphericalToCartesian(float theta, float pi, float rad)
        {
            Vector3 re; rad = Mathf.Max(0.5f, rad);

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPI    = Mathf.Sin(pi);
            float cosPI    = Mathf.Cos(pi);

            re.x = rad * sinTheta * sinPI;
            re.y = rad * cosTheta;
            re.z = rad * sinTheta * cosPI;

            return re;
        }
        #endregion
    }

}
