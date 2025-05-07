/**
 * $File: JCS_Time.cs $
 * $Date: 2023-05-27 15:15:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2023 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Provides an interface to get time information from Unity.
    /// </summary>
    public static class JCS_Time
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return the time by type.
        /// </summary>
        /// <param name="type"> The type of the time you want to get. </param>
        public static float ItTime(JCS_TimeType type)
        {
            switch (type)
            {
                case JCS_TimeType.DELTA_TIME:
                    return Time.deltaTime;
                case JCS_TimeType.CAPTURE_DELTA_TIME:
                    return Time.captureDeltaTime;
                case JCS_TimeType.FIXED_DELTA_TIME:
                    return Time.fixedDeltaTime;
                case JCS_TimeType.FIXED_UNSCALED_TIME:
                    return Time.fixedUnscaledTime;
                case JCS_TimeType.MAXIMUM_DELTA_TIME:
                    return Time.maximumDeltaTime;
                case JCS_TimeType.MAXIMUM_PARTICLE_DELTA_TIME:
                    return Time.maximumParticleDeltaTime;
                case JCS_TimeType.SMOOTH_DELTA_TIME:
                    return Time.smoothDeltaTime;
                case JCS_TimeType.UNSCALED_DELTA_TIME:
                    return Time.unscaledDeltaTime;
            }

            return 0.0f;
        }

        /// <summary>
        /// Return the time largest unit time.
        /// 
        /// - For day: 1d
        /// - For hour: 2h
        /// - For minute: 3m
        /// - For second: 4s
        /// </summary>
        /// <param name="val"> Time value. </param>
        public static string FormatLargest(double val)
        {
            TimeSpan time = TimeSpan.FromSeconds(val);

            if (time.Days > 0)
                return $"{time.Days}d";
            if (time.Hours > 0)
                return $"{time.Hours}h";
            if (time.Minutes > 0)
                return $"{time.Minutes}m";
            if (time.Seconds >= 0)
                return $"{time.Seconds}s";

            return "";
        }

        /// <summary>
        /// Return the time string in full colon format.
        /// </summary>
        /// <param name="val"> Time value. </param>
        public static string FormatFull(double val)
        {
            TimeSpan ts = TimeSpan.FromSeconds(val);
            return ts.ToString(@"hh\:mm\:ss");
        }
    }
}
