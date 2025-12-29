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

        [Separator("📋 Check Variabless (JCS_UtilManager)")]

        [Tooltip("Damage text util.")]
        [SerializeField]
        [ReadOnly]
        private JCS_MixDamageTextPool mMixDamageTextPool = null;

        [Separator("🌱 Initialize Variables (JCS_UtilManager)")]

        [Tooltip("Trasnparent sprite.")]
        [SerializeField]
        private Sprite mSpriteTransparent = null;

        /* Setter & Getter */

        public Sprite spriteTransparent { get { return mSpriteTransparent; } }

        public void SetMixDamageTextPool(JCS_MixDamageTextPool tp) { mMixDamageTextPool = tp; }
        public JCS_MixDamageTextPool GetMixDamageTextPool() { return mMixDamageTextPool; }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);
        }
    }
}
