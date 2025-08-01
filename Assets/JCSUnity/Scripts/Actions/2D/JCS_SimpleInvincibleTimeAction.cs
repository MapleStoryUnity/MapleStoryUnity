/**
 * $File: JCS_SimpleInvincibleTimeAction.cs $
 * $Date: 2017-03-16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action shows invincible time.
    /// </summary>
    public class JCS_SimpleInvincibleTimeAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_SimpleInvincibleTimeAction)")]

        [Tooltip("Time to do the invincible effect.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mInvicibleTime = 1.0f;

        // timer.
        private float mInvicibleTimer = 0;

        // trigger the invincible time action?
        private bool mTriggerAction = false;

        [Tooltip("Render target we are going to render.")]
        [SerializeField]
        private JCS_UnityObject[] mUnityObjects = null;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Flash Effect (JCS_SimpleInvincibleTimeAction) ")]

        [Tooltip("Color when is invincible.")]
        [SerializeField]
        private Color mInvincibleColor = new Color(141.0f / 255f, 141.0f / 255f, 141.0f / 255f);

        // record down the previoud color
        private Color[] mRecordColors = null;

        [Tooltip("How fast it flashes back and forth?")]
        [SerializeField]
        [Range(0.01f, 3.0f)]
        private float mFlashTime = 0.1f;

        // timer to do flashy effect.
        private float mFlashTimer = 0;

        // just a boolean record down what the
        // current color is.
        private bool mFlashToggle = false;

        [Header("- Sound")]

        [Tooltip("Play once while triggered.")]
        [SerializeField]
        private AudioClip mTriggerSound = null;

        /* Setter & Getter */

        public float InvicibleTime { get { return this.mInvicibleTime; } set { this.mInvicibleTime = value; } }
        // Use to check if this effect is active?
        public bool IsInvincible { get { return this.mTriggerAction; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mRecordColors = new Color[mUnityObjects.Length];
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            // check if effect?
            if (!mTriggerAction)
                return;


            mInvicibleTimer += JCS_Time.ItTime(mTimeType);

            // do flash algorithm.
            DoFlash();

            // check if the time reach?
            if (mInvicibleTime > mInvicibleTimer)
                return;

            // set back the local color to record color.
            for (int index = 0; index < mUnityObjects.Length; ++index)
            {
                mUnityObjects[index].LocalColor = mRecordColors[index];
            }

            // reset timer.
            mInvicibleTimer = 0;

            this.mFlashToggle = false;

            // cancel action
            mTriggerAction = false;
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.L))
            {
                TriggerAction();
            }
        }
#endif

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
            this.mInvicibleTimer = 0;

            // record down the current color.
            for (int index = 0; index < mUnityObjects.Length; ++index)
            {
                mRecordColors[index] = mUnityObjects[index].LocalColor;
            }

            // reset the flash toggle.
            this.mFlashToggle = false;

            // start the action.
            mTriggerAction = true;

            // play the sound once.
            JCS_SoundManager.FirstInstance().GlobalSoundPlayer().PlayOneShot(mTriggerSound);
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
                for (int index = 0; index < mUnityObjects.Length; ++index)
                {
                    mUnityObjects[index].LocalColor = this.mInvincibleColor;
                }

            }
            else
            {
                for (int index = 0; index < mUnityObjects.Length; ++index)
                {
                    mUnityObjects[index].LocalColor = this.mRecordColors[index];
                }
            }

            // toggle it.
            mFlashToggle = !mFlashToggle;

            // reset flash timer.
            mFlashTimer = 0;
        }
    }
}
