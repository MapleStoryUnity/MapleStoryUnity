/**
 * $File: JCS_RandomTextureAction.cs $
 * $Date: 2023-08-19 06:10:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    public class JCS_RandomTextureAction : MonoBehaviour
    {
        /* Variables */

        private Renderer mRenderer = null;

        [Separator("Initialize Variables (JCS_RandomTextureAction)")]

        [Tooltip("List of textures to use.")]
        [SerializeField]
        public List<Texture> textures = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mRenderer = this.GetComponent<Renderer>();

            UpdateTexture();
        }

        /// <summary>
        /// Randomly update the texture once.
        /// </summary>
        public void UpdateTexture()
        {
            SetTexture(textures);
        }

        /// <summary>
        /// Update current texture with list of textures.
        /// </summary>
        public void SetTexture(List<Texture> textures)
        {
            Texture tex = JCS_Random.ChooseOne(textures);
            mRenderer.material.SetTexture("_MainTex", tex);
        }
    }
}
