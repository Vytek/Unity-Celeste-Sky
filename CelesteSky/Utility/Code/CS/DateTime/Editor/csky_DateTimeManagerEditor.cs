using UnityEngine;
using UnityEditor;


namespace CelesteSky.Utility
{

	[CustomEditor(typeof(csky_DateTimeManager))]
    public class csky_DateTimeManagerEditor : csky_CommonEditor
    {

        // Target
        ////////////////
        csky_DateTimeManager tar;
       
        // Timeline
        ////////////////
        SerializedProperty m_AllowProgressTime;
        SerializedProperty m_TotalHours;
        //SerializedProperty m_Hour;
        //SerializedProperty m_Minute;
        //SerializedProperty m_Second;
        //SerializedProperty m_Milliseconds;

        // Date
        ///////////////
        // Time Length.
        SerializedProperty m_EnableDayNightLength;
        SerializedProperty m_DayRange;
        SerializedProperty m_DayLength;
        SerializedProperty m_NightLength;
        
        // Date.
        SerializedProperty m_Day;
        SerializedProperty m_Month;
        SerializedProperty m_Year;

        // System.
        ////////////////
        SerializedProperty m_SyncWithSystem;

        // Unity Events.
        ////////////////
        SerializedProperty m_CheckEventsType;
        SerializedProperty unity_OnHourChanged;
        SerializedProperty unity_OnMinuteChanged;
        SerializedProperty unity_OnDayChanged;
        SerializedProperty unity_OnMonthChanged;
        SerializedProperty unity_OnYearChanged;

        // Foldout.
        ////////////////
        bool m_TimeFoldout,
        m_DateFoldout,
        m_OptionsFoldout,
        m_ValueEventsFoldout,
        m_TimeEventsFoldout,
        m_DateEventsFoldout;

        protected override string Title => "Date Time Manager";
 
        /* 
        protected virtual bool OverrideDayRange
        {   
            get { return false; }
        }*/

        protected override void OnEnable()
        {

            base.OnEnable();
            tar = (csky_DateTimeManager)target;

            // Timeline.
            /////////////
            m_AllowProgressTime = serObj.FindProperty("m_AllowProgressTime");
            m_TotalHours        = serObj.FindProperty("m_TotalHours");
            //m_Hour              = serObj.FindProperty("m_Hour");
            //m_Minute            = serObj.FindProperty("m_Minute");
            //m_Second            = serObj.FindProperty("m_Second");
            //m_Milliseconds      = serObj.FindProperty("m_Millisecond");
			
            // Length.
            ////////////
            m_EnableDayNightLength = serObj.FindProperty("m_EnableDayNightLength");
            m_DayRange             = serObj.FindProperty("m_DayRange");
            m_DayLength            = serObj.FindProperty("m_DayLength");
            m_NightLength          = serObj.FindProperty("m_NightLength");

            m_Day   = serObj.FindProperty("m_Day");
            m_Month = serObj.FindProperty("m_Month");
            m_Year  = serObj.FindProperty("m_Year");

            m_SyncWithSystem = serObj.FindProperty("m_SyncWithSystem");

            // DateTime.
            //////////////
            m_CheckEventsType     = serObj.FindProperty("m_CheckEventsType");
            unity_OnHourChanged   = serObj.FindProperty("unity_OnHourChanged");
            unity_OnMinuteChanged = serObj.FindProperty("unity_OnMinuteChanged");
            unity_OnDayChanged    = serObj.FindProperty("unity_OnDayChanged");
            unity_OnMonthChanged  = serObj.FindProperty("unity_OnMonthChanged");
            unity_OnYearChanged   = serObj.FindProperty("unity_OnYearChanged");
        }
       


        #region |OnInspectorGUI|
		protected override void _OnInspectorGUI()
		{

		#region Time.

			csky_EditorGUIUtility.ShurikenFoldoutHeader("Time", ref m_TimeFoldout, csky_ShurikenStyle.Tab);

            if(m_TimeFoldout)
            {

                csky_EditorGUIUtility.ShurikenHeader("Timeline", TextTabTitleStyle, csky_ShurikenStyle.Title);
                EditorGUILayout.Separator();

                    GUI.backgroundColor = m_AllowProgressTime.boolValue ? Color.green : Color.red;

                    EditorGUILayout.PropertyField(m_AllowProgressTime, new GUIContent("Allow Progress Time"));

                    EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(m_TotalHours, new GUIContent("Timeline"));
                    EditorGUILayout.EndVertical();

                    GUI.backgroundColor = Color.white;

                EditorGUILayout.Separator();


                csky_EditorGUIUtility.ShurikenHeader("Time Length", TextTabTitleStyle, csky_ShurikenStyle.Title);
                EditorGUILayout.Separator();

                    if(!m_SyncWithSystem.boolValue)
                    {


                        GUI.backgroundColor = m_EnableDayNightLength.boolValue ? Color.green : Color.red;

                        EditorGUILayout.PropertyField(m_EnableDayNightLength, new GUIContent("Enable Day Night Length"));

                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        if(m_EnableDayNightLength.boolValue)
                        {

                            
                            // Day Range.
                            float min = m_DayRange.vector2Value.x;
                            float max = m_DayRange.vector2Value.y;

                            csky_EditorGUIUtility.ShurikenHeader("Day Range", TextTabTitleStyle, csky_ShurikenStyle.Title);
                                
                            EditorGUILayout.BeginHorizontal();
                                
                                EditorGUILayout.MinMaxSlider(ref min, ref max, 0, 24);

                                m_DayRange.vector2Value = new Vector2(min, max);

                                EditorGUILayout.PropertyField(m_DayRange, new GUIContent(""));
                            
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Separator();
                            csky_EditorGUIUtility.Separator(2);

                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));
                            EditorGUILayout.PropertyField(m_NightLength, new GUIContent("Night In Minutes"));
                        }
                        else
                        {

                            EditorGUILayout.PropertyField(m_DayLength, new GUIContent("Day In Minutes"));

                        }

                        EditorGUILayout.Separator();
                        EditorGUILayout.EndVertical();
   
                    }
                    EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }
                
		#endregion

        #region Date

            csky_EditorGUIUtility.ShurikenFoldoutHeader("Date", ref m_DateFoldout, csky_ShurikenStyle.Tab);
            if(m_DateFoldout)
            {
                EditorGUILayout.PropertyField(m_Day, new GUIContent("Day"));
                EditorGUILayout.PropertyField(m_Month, new GUIContent("Month"));
                EditorGUILayout.PropertyField(m_Year, new GUIContent("Year"));
            }

        #endregion

        #region System.

            csky_EditorGUIUtility.ShurikenFoldoutHeader("System", ref m_OptionsFoldout, csky_ShurikenStyle.Tab);
            if(m_OptionsFoldout)
            {

                EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(m_SyncWithSystem, new GUIContent("Synchronize With This System"));
                EditorGUILayout.Separator();
            }
        #endregion
			LastUpdateInspector();
		}

		protected virtual void LastUpdateInspector()
        {

        #region |Events|

            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(m_CheckEventsType, new GUIContent("Check Events Type"));
            EditorGUILayout.EndVertical();

            if(m_CheckEventsType.intValue != 1)
            {

                csky_EditorGUIUtility.ShurikenFoldoutHeader("Time Events", ref m_TimeEventsFoldout, csky_ShurikenStyle.Tab);

                if(m_TimeEventsFoldout)
                {
                    EditorGUILayout.PropertyField(unity_OnHourChanged, new GUIContent("On Hour Changed"));
                    EditorGUILayout.PropertyField(unity_OnMinuteChanged, new GUIContent("On Minute Changed"));
                }
			    //----------------------------------------------------------------------------------------------------

                csky_EditorGUIUtility.ShurikenFoldoutHeader("Date Events",  ref m_DateEventsFoldout, csky_ShurikenStyle.Tab);
                if(m_DateEventsFoldout)
                {
                    EditorGUILayout.PropertyField(unity_OnDayChanged, new GUIContent("On Day Changed"));
                    EditorGUILayout.PropertyField(unity_OnMonthChanged, new GUIContent("On Month Changed"));
                    EditorGUILayout.PropertyField(unity_OnYearChanged, new GUIContent("On Year Changed"));
                }

            }
            EditorGUILayout.EndVertical();

        #endregion

        }

        #endregion

	}

}