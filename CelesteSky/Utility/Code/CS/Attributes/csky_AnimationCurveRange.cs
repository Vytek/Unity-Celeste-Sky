/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Animation Curve Range:
///----------------------------------------------
/// Range for AnimationCurve fields.
/////////////////////////////////////////////////
using System;
using UnityEngine;

namespace CelesteSky.Utility
{
    /// <summary> Range values for animation curves </summary>
    public class csky_AnimationCurveRange : PropertyAttribute
    {
    #region [Ranges]
        public readonly float timeStart;
        public readonly float valueStart;
        public readonly float timeEnd;
        public readonly float valueEnd;
    #endregion
    
    #region [Settings]
        public readonly int colorIndex;
    #endregion

        public csky_AnimationCurveRange(float _timeStart, float _valueStart, float _timeEnd, float _valueEnd, int _colorIndex)
        {
            this.timeStart  = _timeStart;
            this.valueStart = _valueStart;
            this.timeEnd    = _timeEnd;
            this.valueEnd   = _valueEnd;
            this.colorIndex = _colorIndex;
        }

    }

}

