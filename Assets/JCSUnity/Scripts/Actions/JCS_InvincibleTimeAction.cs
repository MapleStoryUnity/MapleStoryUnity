/**
 * $File: JCS_InvincibleTimeAction.cs $
 * $Date: 2016-12-22 05:22:24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Make live object invincible for a certain time.
    /// </summary>
    [RequireComponent(typeof(JCS_LiveObject))]
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public class JCS_InvincibleTimeAction
        : JCS_UnityObject
    {
        /* Variables */

        private JCS_LiveObject mLiveObject = null;
        private JCS_OrderLayerObject mOrderLayerObject = null;

        [Header("** Runtime Variables (JCS_InvincibleTimeAction) **")]

        [Tooltip("How long the invincible time are?")]
        [SerializeField]
        private float mInvicibleTime = 1.0f;

        // timer.
        private float mInvicibleTimer = 0.0f;

        // trigger the invincible time action?
        private bool mTriggerAction = false;

        [Header("- Flash Effect (JCS_InvincibleTimeAction) ")]

        [Tooltip("Color when is invincible.")]
        [SerializeField]
        private Color mInvincibleColor = new Color(141.0f, 141.0f, 141.0f);

        // record down the previoud color
        private Color mRecordColor = Color.white;

        [Tooltip("How fast it flashs back and forth.")]
        [SerializeField]
        [Range(0.01f, 3.0f)]
        private float mFlashTime = 0.1f;

        // timer to do flashy effect.
        private float mFlashTimer = 0;

        // just a boolean record down what the 
        // current color is.
        private bool mFlashToggle = false;

        [Tooltip("Play once while triggered.")]
        [SerializeField]
        private AudioClip mTriggerSound = null;

        /* Setter & Getter */

        public float InvicibleTime { get { return this.mInvicibleTime; } set { this.mInvicibleTime = value; } }
        // Use to check if this effect is active?
        public bool IsInvincible { get { return this.mTriggerAction; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mLiveObject = this.GetComponent<JCS_LiveObject>();
            mOrderLayerObject = this.GetComponent<JCS_OrderLayerObject>();
        }

        private void Update()
        {
            // check if effect?
            if (!mTriggerAction)
                return;


            mInvicibleTimer += Time.deltaTime;

            // do flash algorithm.
            DoFlash();

            // check if the time reach?
            if (mInvicibleTime > mInvicibleTimer)
                return;

            mLiveObject.CanDamage = true;

            // set back the local color to record color.
            if (mOrderLayerObject.GetSpriteRenderer() != null)
                mOrderLayerObject.GetSpriteRenderer().color = mRecordColor;
            for (int index = 0;
                index < mOrderLayerObject.SpriteRenderers().Count;
                ++index)
            {
                /* 
                 * No need to check null, this will always be there. 
                 * Because 'JCS_OrderLayerObject' will be remove the 
                 * empty slot before we use it.
                 */
                mOrderLayerObject.SpriteRenderersAt(index).color = mRecordColor;
            }

            // reset timer.
            mInvicibleTimer = 0.0f;

            this.mFlashToggle = false;

            // cancel action
            mTriggerAction = false;
        }

        /// <summary>
        /// Trigger invincible action.
        /// </summary>
        public void TriggerAction()
        {
            TriggerAction(mInvicibleTime);
        }

        /// <summary>
        /// Trigger invincible action.
        /// </summary>
        /// <param name="time"> time to be invincible </param>
        public void TriggerAction(float time)
        {
            // if effect is on, return.
            if (mTriggerAction)
                return;

            // get the time.
            this.mInvicibleTime = time;

            // reset timer.
            this.mInvicibleTimer = 0.0f;

            // set the live object's can damage immdeiate to false.
            // so it won't get damage in the same frame.
            mLiveObject.CanDamage = false;

            // record down the current color.
            if (mOrderLayerObject.GetSpriteRenderer() != null)
                mRecordColor  = mOrderLayerObject.GetSpriteRenderer().color;
            else 
                mRecordColor = Color.white;

            // reset the flash toggle.
            this.mFlashToggle = false;

            // start the action.
            mTriggerAction = true;

            // play the sound once.
            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mTriggerSound);
        }

        /// <summary>
        /// Do flash from color 1 to color 2 and color 1... continue.
        /// </summary>
        private void DoFlash()
        {
            mFlashTimer += Time.deltaTime;

            if (mFlashTime > mFlashTimer)
                return;

            // set the start is "false"
            if (!mFlashToggle)
            {
                if (mOrderLayerObject.GetSpriteRenderer() != null)
                    mOrderLayerObject.GetSpriteRenderer().color = mInvincibleColor;

                for (int index = 0; index < mOrderLayerObject.SpriteRenderers().Count; ++index)
                {
                    /* 
                     * No need to check null, this will always be there. 
                     * Because 'JCS_OrderLayerObject' will be remove the 
                     * empty slot before we use it.
                     */
                    mOrderLayerObject.SpriteRenderersAt(index).color = mInvincibleColor;
                }
            }
            else
            {
                if (mOrderLayerObject.GetSpriteRenderer() != null)
                    mOrderLayerObject.GetSpriteRenderer().color = mRecordColor;

                for (int index = 0; index < mOrderLayerObject.SpriteRenderers().Count; ++index)
                {
                    /* 
                     * No need to check null, this will always be there. 
                     * Because 'JCS_OrderLayerObject' will be remove the 
                     * empty slot before we use it.
                     */
                    mOrderLayerObject.SpriteRenderersAt(index).color = mRecordColor;
                }
            }

            // toggle it.
            mFlashToggle = !mFlashToggle;

            // reset flash timer.
            mFlashTimer = 0.0f;
        }
    }
}
