/**
 * $File: JCS_SlideScreenGamePadButton.cs $
 * $Date: 2018-01-02 09:00:20 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button will do the slide screen. (Game Pad)
    /// </summary>
    public class JCS_SlideScreenGamePadButton : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Check Variables (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("This action are using `JCS_2DSlideScreenCamera`.")]
        [SerializeField]
        private JCS_2DSlideScreenCamera[] mSlideCameras = null;

        [Header("** Runtime Variables (JCS_SlideScreenGamePadButton) **")]

        [Tooltip("Direction you want to go.")]
        [SerializeField]
        private JCS_2D8Direction mDirection = JCS_2D8Direction.TOP;

        [Tooltip("Times to switch scene.")]
        [SerializeField]
        [Range(1, 5)]
        private int mCount = 1;

        [Header("- Sound")]

        [Tooltip("Sound when sliding screen.")]
        [SerializeField]
        private AudioClip mSlideScreenSound = null;

        [Header("- Delay")]

        [Tooltip("Time delay when slide screen.")]
        [SerializeField]
        private float mDelayTime = 0.0f;

        private float mDelayTimer = 0.0f;

        private bool mStartDelay = false;

        /* Setter & Getter */

        public void SetDirection(JCS_2D8Direction direction) { this.mDirection = direction; }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // try to get it from the scene by type.
            this.mSlideCameras = (JCS_2DSlideScreenCamera[])FindObjectsOfType(typeof(JCS_2DSlideScreenCamera));
        }

        protected override void Update()
        {
            base.Update();

            if (!mStartDelay)
                return;

            mDelayTimer += Time.deltaTime;

            if (mDelayTime < mDelayTimer)
            {
                // switch scene.
                SwitchScene();

                // play sound.
                mSoundPlayer.PlayOneShot(mSlideScreenSound);

                // reset timer (ready for next use)
                mDelayTimer = 0;

                // disable delay (ready for next use)
                mStartDelay = false;
            }
        }

        /// <summary>
        /// Button click function pointer override.
        /// </summary>
        public override void OnClick()
        {
            if (mSlideCameras.Length == 0)
            {
                JCS_Debug.LogReminder("Assign the button without camera is not allowed...");
                return;
            }

            // start counting delay
            mStartDelay = true;
        }

        /// <summary>
        /// Switch the scene.
        /// </summary>
        private void SwitchScene()
        {
            JCS_2DSlideScreenCamera slideCamera = null;

            for (int slideCount = 0; slideCount < mCount; ++slideCount)
            {
                for (int camIndex = 0; camIndex < mSlideCameras.Length; ++camIndex)
                {
                    slideCamera = this.mSlideCameras[camIndex];

                    // do the slide screen effect.
                    if (slideCamera != null)
                        slideCamera.SwitchScene(mDirection);
                }
            }
        }
    }
}
