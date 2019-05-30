/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Common Editor:
///----------------------------------------------
/// Common for custom inspectos.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;

namespace CelesteSky.Utility
{
    public abstract class csky_CommonEditor : Editor
    {

        // Set Obj.
        protected SerializedObject serObj;

        /// <summary> Title Styule. </summary>
        protected GUIStyle TextTitleStyle
        {
            get
            {
                GUIStyle s  = new GUIStyle(EditorStyles.label);
                s.fontStyle = FontStyle.Bold;
                s.fontSize  = 20;
                return s;
            }
        }

        /// <summary></summary>
        protected GUIStyle TextTabTitleStyle
        {
            get
            {
                GUIStyle s  = new GUIStyle(EditorStyles.label);
                s.fontStyle = FontStyle.Bold;
                s.fontSize  = 10;
                return s;
            }
        }

        /// <summary></summary>
        protected virtual string Title => "New Class";

      
        protected virtual void OnEnable()
        {
            serObj = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            csky_EditorGUIUtility.ShurikenHeader(Title, TextTitleStyle, csky_ShurikenStyle.TitleHeader);

            _OnInspectorGUI();

            serObj.ApplyModifiedProperties();

        }
        protected abstract void _OnInspectorGUI();
    }
}
