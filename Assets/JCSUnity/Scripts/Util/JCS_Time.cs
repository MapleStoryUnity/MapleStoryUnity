/**
 * $File: JCS_Time.cs $
 * $Date: 2023-05-27 15:15:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2023 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Provides an interface to get time information from Unity.
    /// </summary>
    public static class JCS_Time 
    {
        /// <summary>
        /// Get the delta time by type.
        /// </summary>
        public static float DeltaTime(JCS_DeltaTimeType type)
        {
            switch (type)
            {
                case JCS_DeltaTimeType.DELTA_TIME:
                    return Time.deltaTime;
                case JCS_DeltaTimeType.CAPTURE_DELTA_TIME:
                    return Time.captureDeltaTime;
                case JCS_DeltaTimeType.FIXED_DELTA_TIME:
                    return Time.fixedDeltaTime;
                case JCS_DeltaTimeType.FIXED_UNSCALED_DELTA_TIME:
                    return Time.fixedUnscaledTime;
                case JCS_DeltaTimeType.MAXIMUM_DELTA_TIME:
                    return Time.maximumDeltaTime;
                case JCS_DeltaTimeType.MAXIMUM_PARTICLE_DELTA_TIME:
                    return Time.maximumParticleDeltaTime;
                case JCS_DeltaTimeType.SMOOTH_DELTA_TIME:
                    return Time.smoothDeltaTime;
                case JCS_DeltaTimeType.UNSCALED_DELTA_TIME:
                    return Time.unscaledDeltaTime;
            }

            return 0.0f;
        }
    }
}
