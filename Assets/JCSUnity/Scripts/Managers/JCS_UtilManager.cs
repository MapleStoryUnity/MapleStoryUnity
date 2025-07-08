/**
 * $File: JCS_UtilManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manager manage all the utilities in the game.
    /// </summary>
    public class JCS_UtilManager : JCS_Manager<JCS_UtilManager>
    {
        /* Variables */

        [Separator("Check Variables (JCS_UtilManager)")]

        [Tooltip("Damage text util.")]
        [SerializeField]
        [ReadOnly]
        private JCS_MixDamageTextPool mMixDamageTextPool = null;

        [Separator("Initialize Variables (JCS_UtilManager)")]

        [Tooltip("Trasnparent sprite.")]
        [SerializeField]
        private Sprite mSpriteTransparent = null;

        /* Setter & Getter */

        public Sprite SpriteTransparent { get { return this.mSpriteTransparent; } }

        public void SetMixDamageTextPool(JCS_MixDamageTextPool tp) { this.mMixDamageTextPool = tp; }
        public JCS_MixDamageTextPool GetMixDamageTextPool() { return this.mMixDamageTextPool; }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }
    }
}
