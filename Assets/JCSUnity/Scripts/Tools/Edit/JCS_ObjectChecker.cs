/**
 * $File: JCS_ObjectChecker.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Draw the gizmos on the mesh collider.
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class JCS_ObjectChecker : MonoBehaviour
    {
        /* Variables */

        public Color wireColor = Color.blue;

        /* Setter & Getter */

        /* Functions */

        private void OnDrawGizmos()
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            if (filter != null)
            {
                Gizmos.color = wireColor;
                Mesh mesh = filter.sharedMesh;
                Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
            }
        }
    }
}
