/////////////////////////////////////////////////
/// Celeste SKy
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Date Time
///----------------------------------------------
/// Date Time utility
/////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CelesteSky.Utility
{

    /// <summary></summary>
    public struct csky_DateTimeUtility
    {
 
        /// <summary> Convert hour in float value. </summary>
        /// <param name="hour"> Hour </param>
        /// <returns> Total hours in float value </returns>
        public static float GetTotalHours(int hour)
        {
            return (float)hour;
        }

        /// <summary> 
        /// Convert hours and minutes in float value.
        /// </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        /// <returns> Total hours in float value </returns>
        public static float GetTotalHours(int hour, int minute)
        {
            return (float)hour + ((float)minute / 60f);
        }

        /// <summary>
        /// Convert hours, minutes and seconds in float value.
        /// </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        /// <param name="second"> Second </param>
        /// <returns> Total hours in float value </returns>
        public static float GetTotalHours(int hour, int minute, int second)
        {
            return (float)hour + ((float)minute / 60f) + ((float)second / 3600f);
        }

        /// <summary>
        /// Convert hours, minutes, seconds and milliseconds in float value.
        /// </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        /// <param name="second"> Second </param>
        /// <param name="millisecond"> Millisecond </param>
        /// <returns> Toltal hours in float value. </returns>
        public static float GetTotalHours(int hour, int minute, int second, int millisecond)
        {
            return (float)hour + (float)minute / 60f + (float)second / 3600f + (float)millisecond / 3600000f;
        }
    }
}