/**
 * $File: JCS_CameraRenderer.cs $
 * $Date: 2017-07-23 08:28:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Show the shader effect in the editor mode.
    /// </summary>
    [ExecuteInEditMode]
    public class JCS_CameraRenderer : MonoBehaviour
    {
        /* Variables */

        [Tooltip("Material going to use render the image.")]
        [SerializeField]
        private Material mRenderMaterial = null;

        /* Setter & Getter */

        public Material RenderMaterial { get { return this.mRenderMaterial; } set { this.mRenderMaterial = value; } }

        /* Functions */

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (mRenderMaterial == null)
                return;

            Graphics.Blit(source, destination, mRenderMaterial);
        }
    }
}
