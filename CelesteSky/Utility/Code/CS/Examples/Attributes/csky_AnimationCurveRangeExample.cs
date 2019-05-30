using System;
using UnityEngine;

//--------------------
using CelesteSky.Utility;
//====================

namespace CelesteSky.Utility.Examples
{
    public class rlec_AnimationCurveRangeExample : MonoBehaviour
    {
        [csky_AnimationCurveRange(0.0f, 0.0f, 1.0f, 1.0f, 0)]
        public AnimationCurve curve =  AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        [SerializeField, csky_AnimationCurveRange(0.0f, 0.0f, 5.0f, 2.0f, 1)]
        public AnimationCurve curve1 =  AnimationCurve.Linear(0.0f, 1.0f, 5.0f, 2.0f);
    }
}


