/**
 * $File: JCS_Boundary.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// The boundary definition.
    /// </summary>
    public class JCS_Boundary : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_Boundary)")]

        [Tooltip("The wire color for gizmos draw.")]
        [SerializeField]
        private Color mWireColor = new Color(0.8f, 0.3f, 0.3f, 0.8f);
#endif

        /* Setter & Getter */

        /* Functions */

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = mWireColor;
            Bounds b = GetBounds();
            Gizmos.DrawWireCube(b.center, b.size);
        }
#endif

        /// <summary>
        /// The bounds of this boundary.
        /// </summary>
        public Bounds GetBounds()
        {
            // Transform all corners into world space
            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            foreach (Vector3 corner in JCS_Constants.CORNERS_CUBE)
            {
                // Scale, then rotate and translate
                Vector3 worldCorner = transform.TransformPoint(corner);

                min = Vector3.Min(min, worldCorner);
                max = Vector3.Max(max, worldCorner);
            }

            var bounds = new Bounds();
            bounds.SetMinMax(min, max);

            return bounds;
        }
    }
}
