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
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
#endif

        /// <summary>
        /// The bounds of this boundary.
        /// </summary>
        public Bounds GetBounds()
        {
            return new Bounds(transform.position, transform.localScale);
        }
    }
}
