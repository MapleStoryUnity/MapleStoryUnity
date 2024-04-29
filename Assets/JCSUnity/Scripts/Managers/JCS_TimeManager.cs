/**
 * $File: JCS_TimeManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Holding all the information about the time in real life.
    /// </summary>
    public class JCS_TimeManager : JCS_Manager<JCS_TimeManager>
    {
        /* Variables */

        [Separator("Check Variables (JCS_TimeManager)")]

        [Tooltip("Time range at the current region.")]
        [SerializeField]
        [ReadOnly]
        private JCS_TimeRange mCurrentTimeRange = JCS_TimeRange.UNKNOWN;

        [Tooltip("Season current time.")]
        [SerializeField]
        [ReadOnly]
        private JCS_SeasonType mCurrentSeasonType = JCS_SeasonType.UNKNOWN;

        [Tooltip("Weather current time period.")]
        [SerializeField]
        [ReadOnly]
        private JCS_WeatherType mCurrentWeatherType = JCS_WeatherType.UNKNOWN;

        /* Setter & Getter */

        public void SetCurrentTimeRange()
        {
            mCurrentTimeRange = LoadCurrentTimeRange();
        }
        public void SetCurrentSeasonType()
        {
            mCurrentSeasonType = GetCurrentSeason();
        }
        public void SetCurrentWeather()
        {
            //mCurrentWeatherType = GetCurrentWeather();
            //StartCoroutine(GetCurrentWeather());
        }

        public JCS_TimeRange GetCurrentTimeRange() { return this.mCurrentTimeRange; }
        public JCS_SeasonType GetCurrentSeasonType() { return this.mCurrentSeasonType; }
        public JCS_WeatherType GetCurrentWeatherType() { return this.mCurrentWeatherType; }

        /* Functions */

        private void Awake()
        {
            instance = this;

            SetCurrentTimeRange();
            SetCurrentSeasonType();
            SetCurrentWeather();
        }

        /// <summary>
        /// Get the current time in base on time zone in OS.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime()
        {
            return System.DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Get the current time base on the OS time.
        /// </summary>
        /// <returns></returns>
        public static JCS_TimeRange LoadCurrentTimeRange()
        {
            if (isInTheMorning())
                return JCS_TimeRange.IN_THE_MORNING_5AM_8AM;
            if (isMorning())
                return JCS_TimeRange.MORNING_8AM_12PM;
            if (isNoon())
                return JCS_TimeRange.NOON_12PM_14PM;
            if (isAfternoon())
                return JCS_TimeRange.AFTERNOON_14PM_17PM;
            if (isEvening())
                return JCS_TimeRange.EVENING_17PM_19PM;
            if (isAtNight())
                return JCS_TimeRange.NIGHT_19PM_22PM;
            if (isLateAtNight())
                return JCS_TimeRange.LATE_AT_NIGHT_22PM_2AM;
            if (isBeforeDawn())
                return JCS_TimeRange.BEFORE_DAWN_2AM_5AM;

            return JCS_TimeRange.UNKNOWN;
        }

        // 晨一般是指5-8点人们起床的时间
        public static bool isInTheMorning() { return CheckTimeRange(5, 8); }
        // 上午是指8-12点工作时间
        public static bool isMorning() { return CheckTimeRange(8, 12); }
        // 中午是指12-14点午休时间
        public static bool isNoon() { return CheckTimeRange(12, 14); }
        // 下午是指14-17(或18)点下午虹作时间
        public static bool isAfternoon() { return CheckTimeRange(14, 17); }
        // 傍晚是指晚饭后至19点这段时间
        public static bool isEvening() { return CheckTimeRange(17, 19); }
        // 晚上是指19-22(或23)点晚上娱乐时间
        public static bool isAtNight() { return CheckTimeRange(19, 22); }
        // 深夜是指22(或23)-次日2(或3)点
        public static bool isLateAtNight() { return CheckTimeRange(22, 2); }
        // 凌晨是指2(或3)-5点之间
        public static bool isBeforeDawn() { return CheckTimeRange(2, 5); }

        /// <summary>
        /// Get the season base on the OS's time's month.
        /// </summary>
        /// <returns></returns>
        public static JCS_SeasonType GetCurrentSeason()
        {
            if (isSpring())
                return JCS_SeasonType.SPRING_3_5;
            if (isSummer())
                return JCS_SeasonType.SUMMER_6_8;
            if (isFall())
                return JCS_SeasonType.FALL_9_11;
            if (isWinter())
                return JCS_SeasonType.WINTER_12_2;

            return JCS_SeasonType.UNKNOWN;
        }
        // 3 - 5
        public static bool isSpring()
        {
            string sMonth = DateTime.Now.ToString("MM");
            return (sMonth == "03" || sMonth == "04" || sMonth == "05");
        }
        // 6 - 8
        public static bool isSummer()
        {
            string sMonth = DateTime.Now.ToString("MM");
            return (sMonth == "06" || sMonth == "07" || sMonth == "08");
        }
        // 9 - 11
        public static bool isFall()
        {
            string sMonth = DateTime.Now.ToString("MM");
            return (sMonth == "09" || sMonth == "10" || sMonth == "11");
        }
        // 12 - 2
        public static bool isWinter()
        {
            string sMonth = DateTime.Now.ToString("MM");
            return (sMonth == "12" || sMonth == "01" || sMonth == "02");
        }

        /// <summary>
        /// Get the current weather base on the website.
        /// 
        /// TODO(jenchieh): not working yet.
        /// </summary>
        /// <returns> Time out. </returns>
        public static IEnumerator GetCurrentWeather()
        {
            /**
                City ID: http://openweathermap.org/help/city_list.txt
            */
            string cityId = "1668341";
            string url = "http://api.openweathermap.org/data/2.5/forecast/city?id=" + cityId + "&APPID=ffbfe3797e856b051ec15a41ba1360c8";
#if UNITY_2018_1_OR_NEWER
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
#else
            WWW www = new WWW(url);
            yield return www;
#endif

            if (www.error == null)
            {
#if UNITY_2018_1_OR_NEWER
                Debug.Log("Loaded following XML " + www.downloadHandler.text);
#else
                Debug.Log("Loaded following XML " + www.text);
#endif

                XmlDocument xmlDoc = new XmlDocument();
#if UNITY_2018_1_OR_NEWER
                xmlDoc.LoadXml(www.downloadHandler.text);
#else
                xmlDoc.LoadXml(www.text);
#endif

                //Debug.Log("City: " + xmlDoc.SelectSingleNode("cities/list/item/city/@name").InnerText);
                //Debug.Log("Temperature: " + xmlDoc.SelectSingleNode("cities/list/item/temperature/@value").InnerText);
                //Debug.Log("humidity: " + xmlDoc.SelectSingleNode("cities/list/item/humidity /@value").InnerText);
                //Debug.Log("Cloud : " + xmlDoc.SelectSingleNode("cities/list/item/clouds/@value").InnerText);
                //Debug.Log("Title: " + xmlDoc.SelectSingleNode("cities /list/item/weather/@value").InnerText);
            }
            else
            {
                Debug.Log("ERROR: " + www.error);

            }

        }

        /// <summary>
        /// Check what time range are in the OS time.
        /// </summary>
        /// <param name="startTime"> starting time. </param>
        /// <param name="endTime"> end time. </param>
        /// <returns>
        /// true : in this time range.
        /// false : vice versa.
        /// </returns>
        private static bool CheckTimeRange(int startTime, int endTime)
        {
            TimeSpan start = new TimeSpan(startTime, 0, 0);
            TimeSpan end = new TimeSpan(endTime, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                return true;
            }

            return false;
        }
    }
}
