/**
 * $File: JCS_SlideScreenButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button will do the slide screen.
    /// </summary>
    public class JCS_SlideScreenButton : JCS_Button
    {
        /* Variables */

        [Separator("Check Variables (JCS_SlideScreenButton)")]

        [Tooltip("This action are using `JCS_2DSlideScreenCamera`.")]
        [SerializeField]
        [ReadOnly]
        private JCS_2DSlideScreenCamera[] mSlideCameras = null;

        [Separator("Runtime Variables (JCS_SlideScreenButton)")]

        [Tooltip("Direction you want to move.")]
        [SerializeField] private JCS_2D8Direction mDirection = JCS_2D8Direction.TOP;

        [Tooltip("Times to switch scene.")]
        [SerializeField]
        [Range(1, 5)]
        private int mCount = 1;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

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
        public JCS_2D8Direction Direction { get { return this.mDirection; } }
        public int Count { get { return this.mCount; } }
        public float DelayTime { get { return this.mDelayTime; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mSoundPlayer == null)
                mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // try to get it from the scene by type.
            this.mSlideCameras = JCS_Util.FindObjectsByType(typeof(JCS_2DSlideScreenCamera)) as JCS_2DSlideScreenCamera[];
        }

        private void Update()
        {
            if (!mStartDelay)
                return;

            mDelayTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            if (mDelayTime < mDelayTimer)
            {
                // switch scene.
                SwitchScene();

                // play sound.
                JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mSlideScreenSound);

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
                JCS_Debug.LogReminder("Assign the button without camera is not allowed");
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
