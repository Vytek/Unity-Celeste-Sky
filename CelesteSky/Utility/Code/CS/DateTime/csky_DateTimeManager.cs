/////////////////////////////////////////////////
/// Celeste Sky.
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Date Time Manager.
///----------------------------------------------
/// Manager for System.DateTime
/////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CelesteSky.Utility
{
    [ExecuteInEditMode]
    [AddComponentMenu("Celeste Sky/Utility/DateTime/DateTime Manager")]
    public class csky_DateTimeManager : MonoBehaviour
    {
    #region [Fields]

        // Options
        /////////////////
        [SerializeField] bool m_AllowProgressTime = true;
        [SerializeField] bool m_SyncWithSystem   = false;

        // Time
        ////////////////
        [SerializeField] float m_TotalHours;
        //[SerializeField, Range(0, 24)]   int m_Hour        = 7;
        //[SerializeField, Range(0, 60)]   int m_Minute      = 30;
        //[SerializeField, Range(0, 60)]   int m_Second      = 0;
        //[SerializeField, Range(0, 1000)] int m_Millisecond = 0;
        const int k_TotalHours = 24;

        // Length
        ////////////////
        [SerializeField] bool m_EnableDayNightLength = true;
        [SerializeField] Vector2 m_DayRange = new Vector2(4.5f, 19f);

        // In minutes.
        [SerializeField] float m_DayLength   = 15.0f;
        [SerializeField] float m_NightLength = 7.5f;

        // Date.
        ////////////////
        [SerializeField, Range(1, 31)]   int m_Day   = 10;
        [SerializeField, Range(1, 12)]   int m_Month = 3;
        [SerializeField, Range(0, 9999)] int m_Year  = 2019;

    #endregion

    #region [Properties]

        /// <summary></summary>
        public bool OverrideDayState{ get; set; }

        bool m_IsDay;
        /// <summary></summary>
        public bool IsDay
        {
            get
            {
                if(!OverrideDayState)
                    m_IsDay = (m_TotalHours >= m_DayRange.x && m_TotalHours < m_DayRange.y);

                return m_IsDay;
            }
            set
            {
                if(OverrideDayState)
                    m_IsDay = value;
            }
        }

        /// <summary></summary>
        public float DurationCycle
        {
            get
            {
                if(m_EnableDayNightLength)
                    return IsDay ? 60 * m_DayLength * 2 : 60 * m_NightLength * 2;
                else
                    return m_DayLength * 60;
            }
        }

        /// <summary></summary>
        public DateTime SystemDateTime{ get; private set; }

    #endregion

    #region [Events]

        [SerializeField] protected csky_EventType m_CheckEventsType = csky_EventType.Unity;

        // Unity Events.
        ////////////////
        // They are triggered when the values of time and date change.
        public UnityEvent unity_OnHourChanged,
        unity_OnMinuteChanged, unity_OnDayChanged,
        unity_OnMonthChanged,  unity_OnYearChanged;

        // Delegate Events.
        ///////////////////
        public delegate void OnDateTimeChanged();
        public static event OnDateTimeChanged OnHourChanged, OnMinuteChanged, OnDayChanged, OnMonthChanged, OnYearChanged;

        // They are used to trigger events.
        protected int m_LastHour, m_LastMinute, m_LastDay, m_LastMonth, m_LastYear;

    #endregion

    #region [Initialize]

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {

            // Initialize timeline.
            m_TotalHours = m_SyncWithSystem ? (float)SystemDateTime.TimeOfDay.TotalHours : m_TotalHours;

            // Initialize TotalHours.
            // AddTime(m_Hour, m_Minute, m_Second, m_Millisecond);

            // Initialize Last Date.
            m_LastYear   = SystemDateTime.Year;
            m_LastMonth  = SystemDateTime.Month;
            m_LastDay    = SystemDateTime.Day;
            m_LastHour   = SystemDateTime.Hour;
            m_LastMinute = SystemDateTime.Minute;

        }

    #endregion

    #region [Update]

        void Update()
        {
            ProgressTime();
            GetDateTime();
            CheckEvents();
        }

    #endregion

    #region [Get|Set]

        void GetDateTime()
        {
            if(m_SyncWithSystem)
            {

                DateTime dateTime = DateTime.Now;
                if(m_AllowProgressTime)
                    m_TotalHours = (float)dateTime.TimeOfDay.TotalHours;
                    
                #if UNITY_EDITOR
                m_Year        = dateTime.Year;
                m_Month       = dateTime.Month;
                m_Day         = dateTime.Day;
                //m_Hour        = dateTime.Hour;
                //m_Minute      = dateTime.Minute;
                //m_Second      = dateTime.Second;
                //m_Millisecond = dateTime.Millisecond;
                #endif
            }
            else
            {
                SetDateTime();
            }
        }

        void SetDateTime()
        {
            SystemDateTime = new DateTime(0, DateTimeKind.Utc);
            RepeatDateTimeCycle();
                 
            SystemDateTime = SystemDateTime.AddYears(m_Year - 1);
            SystemDateTime = SystemDateTime.AddMonths(m_Month - 1);
            SystemDateTime = SystemDateTime.AddDays(m_Day - 1);

            // Use total hours for timeline.
            SystemDateTime = SystemDateTime.AddHours(m_TotalHours); 

            m_Year  = SystemDateTime.Year;
            m_Month = SystemDateTime.Month;
            m_Day   = SystemDateTime.Day;
  
            SetTime(SystemDateTime.Hour, SystemDateTime.Minute, SystemDateTime.Second, SystemDateTime.Millisecond);
        }

        void RepeatDateTimeCycle()
        {

            // Fordward.
            /////////////
            if(m_Year == 9999 && m_Month == 12 && m_Day == 31 && m_TotalHours >= 23.999999f)
            {
                m_Year = 1; m_Month = 1; m_Day = 1; m_TotalHours = 0.0f;
            }

            // Backward.
            /////////////
            if(m_Year == 1 && m_Month == 1 && m_Day == 1 && m_TotalHours < 0.0f)
            {
                m_Year = 9999; m_Month = 12; m_Day = 31; m_TotalHours = 23.999999f;
            }
        } 

        void ProgressTime()
        {
            if(m_AllowProgressTime && !m_SyncWithSystem)
            {
                m_TotalHours += (DurationCycle != 0 && Application.isPlaying) ? 
                    (Time.deltaTime / DurationCycle) * k_TotalHours : 0.0f;
            }
        }

    #endregion
    
    #region [Helpers]

        /// <summary> Add hour in timeline </summary>
        /// <param name="hour"> Hour </param>
        public void SetTime(int hour)
        {
            m_TotalHours = csky_DateTimeUtility.GetTotalHours(hour);
        }

        /// <summary> Add hour and minute in timeline </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        public void SetTime(int hour, int minute)
        {
            m_TotalHours = csky_DateTimeUtility.GetTotalHours(hour, minute);
        }

        /// <summary> Add hour minute, and second in timeline </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        /// <param name="second"> Second </param>
        public void SetTime(int hour, int minute, int second)
        {
            m_TotalHours = csky_DateTimeUtility.GetTotalHours(hour, minute, second);
        }

        /// <summary> Add hour, minute, second, millisecond in timeline </summary>
        /// <param name="hour"> Hour </param>
        /// <param name="minute"> Minute </param>
        /// <param name="second"> Second </param>
        /// <param name="second"> Millisecond </param>
        public void SetTime(int hour, int minute, int second, int millisecond)
        {
            m_TotalHours = csky_DateTimeUtility.GetTotalHours(hour, minute, second, millisecond);
        }

    #endregion

    #region [Events]

        void CheckEvents()
        {
            switch(m_CheckEventsType)
            {
                case csky_EventType.Unity:     CheckUnityEvents();    break;
                case csky_EventType.Delegates: CheckDelegateEvents(); break;
                case csky_EventType.Both:
                    CheckUnityEvents();
                    CheckDelegateEvents();
                break;
            }
        }

        void CheckUnityEvents()
        {

            if(m_LastHour != SystemDateTime.Hour)
            {
                unity_OnHourChanged.Invoke();   // Debug.Log("OnHour");
                m_LastHour = SystemDateTime.Hour;
            }
           
            if(m_LastMinute != SystemDateTime.Minute)
            {
                unity_OnMinuteChanged.Invoke(); // Debug.Log("OnMinute");
                m_LastMinute = SystemDateTime.Minute;
            }

            if(m_LastDay != SystemDateTime.Day)
            {
                unity_OnDayChanged.Invoke(); //Debug.Log("OnDay");
                m_LastDay = SystemDateTime.Day;
            }

            if(m_LastMonth != SystemDateTime.Month)
            {
                unity_OnMonthChanged.Invoke(); //Debug.Log("OnMonth");
                m_LastMonth = SystemDateTime.Month;
            }

            if(m_LastYear != SystemDateTime.Year)
            {
                unity_OnYearChanged.Invoke(); //Debug.Log("OnYear");
                m_LastYear = SystemDateTime.Year;
            }
        }

        void CheckDelegateEvents()
        {
            if(m_LastHour != SystemDateTime.Hour)
            {
                OnHourChanged();   // Debug.Log("OnHour");
                m_LastHour = SystemDateTime.Hour;
            }
           
            if(m_LastMinute != SystemDateTime.Minute)
            {
                OnMinuteChanged(); // Debug.Log("OnMinute");
                m_LastMinute = SystemDateTime.Minute;
            }

            if(m_LastDay != SystemDateTime.Day)
            {
                OnDayChanged(); //Debug.Log("OnDay");
                m_LastDay = SystemDateTime.Day;
            }

            if(m_LastMonth != SystemDateTime.Month)
            {
                OnMonthChanged(); //Debug.Log("OnMonth");
                m_LastMonth = SystemDateTime.Month;
            }

            if(m_LastYear != SystemDateTime.Year)
            {
                OnYearChanged(); //Debug.Log("OnYear");
                m_LastYear = SystemDateTime.Year;
            }

        }

    #endregion

    #region [Properties|Access]

        // Timeline
        ////////////
        /// <summary></summary>
        public bool AllowProgessTime
        {
            get => m_AllowProgressTime;
            set => m_AllowProgressTime = value;
        }

        /// <summary> Timeline </summary>
        public float TotalHours
        {
            get => m_TotalHours;
            set
            {
                if(value > 0.0f && value < 24.000001f && !m_SyncWithSystem)
                    m_TotalHours = value;
            }
        }

        /// <summary></summary>
        public int Hour
        {
            get => SystemDateTime.Hour;
            set
            {
                if(value < 25 && value > 0)
                    SetTime(value, SystemDateTime.Minute, SystemDateTime.Second, SystemDateTime.Millisecond);
            }
        }

        /// <summary></summary>
        public int Minute
        {
            get => SystemDateTime.Minute;
            set
            {
                if(value > 0 && value < 61)
                    SetTime(SystemDateTime.Hour, value, SystemDateTime.Second, SystemDateTime.Millisecond);
            }
        }

        /// <summary></summary>
        public int Second
        {
            get => SystemDateTime.Second;
            set
            {
                if(value > 0 && value < 61)
                    SetTime(SystemDateTime.Hour, SystemDateTime.Minute, value, SystemDateTime.Millisecond);
            }
        }

        /// <summary></summary>
        public int Millisecond
        {
            get => SystemDateTime.Millisecond;
            set
            {
                if(value > 0 && value < 1001)
                    SetTime(SystemDateTime.Hour, SystemDateTime.Minute, SystemDateTime.Second, value);
            }
        }

        // Length
        ////////////
        /// <summary></summary>
        public bool EnableDayNightLength
        {
            get => m_EnableDayNightLength;
            set => m_EnableDayNightLength = value;
        }

        /// <summary></summary>
        public Vector2 DayRange
        {
            get => m_DayRange;
            set => m_DayRange = value;
        }

        /// <summary> Duration day in minutes. </summary>
        public float DayLength
        {
            get => m_DayLength;
            set => m_DayLength = value;
        }

        /// <summary> Duration night in minutes. </summary>
        public float NightLength
        {
            get => m_NightLength;
            set => m_NightLength = value;
        }

        // Date
        ///////////
        /// <summary></summary>
        public int Day 
        {
            get => m_Day;
            set
            {
                if(value > 0 && value < 32 && !m_SyncWithSystem)
                    m_Day = value;
            }
        }

        /// <summary> </summary>
        public int Month 
        {
            get => m_Month;
            set
            {
                if(value > 0 && value < 13 && !m_SyncWithSystem)
                    m_Month = value;
            }
        }

        /// <summary></summary>
        public int Year
        {
            get => m_Year;
            set
            {
                if(value > 0 && value < 10000 && !m_SyncWithSystem)
                    m_Year = value;
            }
        }

        // System- 
        ////////////
        /// <summary></summary>
        public bool SyncWithSystem
        {
            get => m_SyncWithSystem;
            set => m_SyncWithSystem = value;
        }

        // Events.
        ///////////
        /// <summary></summary>
        public int LastHour => m_LastHour;

        /// <summary></summary>
        public int LastMinute => m_LastMinute;

        /// <summary></summary>
        public int LastDay => m_LastDay;

        /// <summary></summary>
        public int LastMonth => m_LastMonth;

        /// <summary></summary>
        public int LastYear => m_LastYear;

    #endregion
    }
}