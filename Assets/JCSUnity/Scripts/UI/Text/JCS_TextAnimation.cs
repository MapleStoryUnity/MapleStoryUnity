/**
 * $File: JCS_TextAnimation.cs $
 * $Date: 2019-07-18 14:34:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Text animation that will display text accordingly.
    /// </summary>
    public class JCS_TextAnimation : JCS_TextObject
    {
        /* Variables */

        [Separator("Check Variables (JCS_TextAnimation)")]

        [Tooltip("Frame this animation is currently displayed.")]
        [SerializeField]
        [ReadOnly]
        private int mCurrentFrame = 0;

        [Separator("Runtime Variables (JCS_TextAnimation)")]

        [Tooltip("Animation active or not active.")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Hold all text animation's frame.")]
        [TextArea]
        public List<string> textFrame = null;

        [Tooltip("Seconds per frame.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mSPF = 0.5f;

        // Base timer to display frame.
        private float mFrameTimer = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public int CurrentFrame { get { return this.mCurrentFrame; } }
        public float SPF { get { return this.mSPF; } set { this.mSPF = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            // Initialize the first frame.
            UpdateTextFrame();
        }

        private void Update()
        {
            if (!mActive)
                return;

            DoTextAnimation();
        }

        /// <summary>
        /// Update the frame text.
        /// </summary>
        public void UpdateTextFrame()
        {
            UpdateTextFrame(this.mCurrentFrame);
        }

        /// <summary>
        /// Update the frame text.
        /// </summary>
        /// <param name="frameIndex"> Frame index to displayed. </param>
        public void UpdateTextFrame(int frameIndex)
        {
            if (this.mCurrentFrame < 0 ||
                this.mCurrentFrame >= textFrame.Count)
                return;

            this.mCurrentFrame = frameIndex;

            /* Ensure in display range. */
            if (this.mCurrentFrame >= textFrame.Count)
                this.mCurrentFrame = textFrame.Count - 1;
            else if (this.mCurrentFrame < 0)
                this.mCurrentFrame = 0;

            text = textFrame[this.mCurrentFrame];
        }

        /// <summary>
        /// Do the actual text animation here.
        /// </summary>
        private void DoTextAnimation()
        {
            if (textFrame == null)
                return;

            mFrameTimer += JCS_Time.ItTime(mTimeType);

            if (mFrameTimer < mSPF)
                return;

            ++this.mCurrentFrame;

            if (this.mCurrentFrame >= textFrame.Count)
                this.mCurrentFrame = 0;

            UpdateTextFrame();

            mFrameTimer = 0.0f;  // Reset timer.
        }
    }
}
