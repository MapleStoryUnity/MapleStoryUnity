/**
 * $File: JCS_Gizmos.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Gizmos utilities.
    /// </summary>
    public static class JCS_Gizmos
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Execution body with in transform maxtrix.
        /// </summary>
        public static void WithTransformMatrix(Transform trans, Action body)
        {
            // Save old matrix
            Matrix4x4 oldMatrix = Gizmos.matrix;

            // Apply transform's position, rotation, and scale
            Gizmos.matrix = Matrix4x4.TRS(trans.position, trans.rotation, trans.localScale);

            body?.Invoke();

            // Restore old matrix
            Gizmos.matrix = oldMatrix;
        }
    }
}
