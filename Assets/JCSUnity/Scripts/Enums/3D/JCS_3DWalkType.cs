/**
 * $File: JCS_3DWalkType.cs $
 * $Date: 2020-05-03 13:13:43 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of all the walk type in 3D.
    /// </summary>
    public enum JCS_3DWalkType
    {
        // Randomly walk around the starting point with in the distance.
        SELF_IN_DISTANCE,
        // Walk to the closest point to the target transform.
        TARGET_CLOSEST_POINT,
        // Randomly walk around the point aslong is in range of the target transform.
        TARGET_IN_RANGE,
    };
}
