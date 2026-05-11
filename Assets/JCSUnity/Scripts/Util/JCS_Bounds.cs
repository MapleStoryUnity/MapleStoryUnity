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
        /// Return the default bounds.
        /// </summary>
        public static Bounds Default(GameObject go)
        {
            return new(go.transform.position, Vector3.zero);
        }

        /// <summary>
        /// Return true if the renderer should be ignored 
        /// when calculating bounds.
        /// </summary>
        public static bool IgnoreComponents(Component renderer)
        {
            // NOTE(jenchieh): Normally a particle system could
            // mess up the bounds; simply ignore it here.
            if (renderer is ParticleSystemRenderer)
                return true;

            // UI element should be ignored.
            if (renderer.GetComponent<RectTransform>())
                return true;

            return false;
        }

        /// <summary>
        /// Return the bounds by renderer.
        /// </summary>
        public static Bounds ByRender(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

            bool first = false;

            Bounds bounds = Default(go);

            foreach (Renderer renderer in renderers)
            {
                if (IgnoreComponents(renderer))
                    continue;

                if (!first)
                {
                    bounds = renderer.bounds;

                    first = true;

                    continue;
                }

                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }

        /// <summary>
        /// Return the bounds by mesh filter.
        /// </summary>
        public static Bounds ByMesh(GameObject go)
        {
            MeshFilter[] mfs = go.GetComponentsInChildren<MeshFilter>();

            bool first = false;

            Bounds bounds = Default(go);

            foreach (MeshFilter mf in mfs)
            {
                if (IgnoreComponents(mf))
                    continue;

                if (!first)
                {
                    bounds = mf.mesh.bounds;

                    first = true;

                    continue;
                }

                bounds.Encapsulate(mf.mesh.bounds);
            }

            return bounds;
        }

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
