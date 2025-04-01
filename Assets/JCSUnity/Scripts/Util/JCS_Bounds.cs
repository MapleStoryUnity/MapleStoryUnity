/**
 * $File: JCS_Bounds.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Bounds util.
    /// </summary>
    public static class JCS_Bounds
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return 8 corners from the bounds.
        /// </summary>
        /// <param name="bounds"> The bounds to get from. </param>
        public static Vector3[] Corners(Bounds bounds)
        {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            return new Vector3[]
            {
                center + new Vector3(-extents.x, -extents.y, -extents.z), // Bottom-back-left
                center + new Vector3(extents.x, -extents.y, -extents.z),  // Bottom-back-right
                center + new Vector3(-extents.x, -extents.y, extents.z),  // Bottom-front-left
                center + new Vector3(extents.x, -extents.y, extents.z),   // Bottom-front-right
                center + new Vector3(-extents.x, extents.y, -extents.z),  // Top-back-left
                center + new Vector3(extents.x, extents.y, -extents.z),   // Top-back-right
                center + new Vector3(-extents.x, extents.y, extents.z),   // Top-front-left
                center + new Vector3(extents.x, extents.y, extents.z)     // Top-front-right
            };
        }
    }
}
