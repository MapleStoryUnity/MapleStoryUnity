/**
 * $File: JCS_TimeType.cs $
 * $Date: 2023-05-27 15:15:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2023 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of time type.
    /// </summary>
    public enum JCS_TimeType
    {
        DELTA_TIME,
        CAPTURE_DELTA_TIME,
        FIXED_DELTA_TIME,
        FIXED_UNSCALED_TIME,
        MAXIMUM_DELTA_TIME,
        MAXIMUM_PARTICLE_DELTA_TIME,
        SMOOTH_DELTA_TIME,
        UNSCALED_DELTA_TIME,
    }
}
