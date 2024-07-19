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

        [Tooltip("In Game Log System util.")]
        [SerializeField]
        [ReadOnly]
        private JCS_IGLogSystem mIGLogSystem = null;

        [Tooltip("In Game Dialogue System.")]
        [SerializeField]
        [ReadOnly]
        private JCS_DialogueSystem mDialogueSystem = null;

        [Separator("Initialize Variables (JCS_UtilManager)")]

        [Tooltip("Trasnparent sprite.")]
        [SerializeField]
        private Sprite mSpriteTransparent = null;

        /* Setter & Getter */

        public void SetMixDamageTextPool(JCS_MixDamageTextPool tp) { this.mMixDamageTextPool = tp; }
        public JCS_MixDamageTextPool GetMixDamageTextPool() { return this.mMixDamageTextPool; }

        public void SetIGLogSystem(JCS_IGLogSystem sys) { this.mIGLogSystem = sys; }
        public JCS_IGLogSystem GetIGLogSystem() { return this.mIGLogSystem; }

        public void SetDiaglogueSystem(JCS_DialogueSystem ds) { this.mDialogueSystem = ds; }
        public JCS_DialogueSystem GetDialogueSystem() { return this.mDialogueSystem; }

        public Sprite SpriteTransparent { get { return this.mSpriteTransparent; } }

        /* Functions */

        private void Awake()
        {
            instance = this;   
        }
    }
}
