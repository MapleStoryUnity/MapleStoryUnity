/**
 * $File: JCS_2DAnimSequencePlayer.cs $
 * $Date: 2017-05-08 20:34:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Play a sequence of animation by time.
    /// </summary>
    public class JCS_2DAnimSequencePlayer
        : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DAnimSequencePlayer) **")]

        public bool testWithKey = false;

        [Tooltip("Play the animation sequence.")]
        public KeyCode playKey = KeyCode.H;

        [Tooltip("Stop the animation sequence.")]
        public KeyCode stopKey = KeyCode.J;
#endif

        [Header("** Check Variables (JCS_2DAnimSequencePlayer) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_2DAnimation mCurrentAnimation = null;

        [Tooltip("Is the animation done playin?")]
        [SerializeField]
        private bool mDonePlayingSequence = false;

        [Header("** Runtime Variables (JCS_2DAnimSequencePlayer) **")]

        [Tooltip("Active this component?")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("Loop the sequence?")]
        [SerializeField]
        private bool mLoop = false;

        // counter through the animation.
        private int mAnimCounter = 0;

        [Tooltip("How many time to play an animation.")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mTimePerAnim = 0.5f;

        // timer for each animation play!
        private float mTimer = 0.0f;

        [Tooltip("Sequence of animations that played.")]
        [SerializeField]
        private JCS_2DAnimation[] mAnimations = null;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool DonePlayingSequence { get { return this.mDonePlayingSequence; } }
        public bool Loop { get { return this.mLoop; } set { this.mLoop = value; } }
        public float TimePerAnim { get { return this.mTimePerAnim; } set { this.mTimePerAnim = value; } }

        /* Functions */

        private void Awake()
        {
            // select first animation.
            mCurrentAnimation = mAnimations[mAnimCounter];
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            if (!mActive)
                return;

            // check if done playing the sequence, and is not looping.
            if (mDonePlayingSequence && !Loop)
                return;

            // check if reach the animation count.
            if (mAnimations.Length <= mAnimCounter)
            {
                // done playing.
                mDonePlayingSequence = true;

                if (Loop)
                {
                    // start with zero.
                    mAnimCounter = 0;
                }

                return;
            }
            else
            {
                // not done playing.
                mDonePlayingSequence = false;
            }

            mTimer += Time.deltaTime;

            // check if reach the time.
            if (mTimer < mTimePerAnim)
                return;

            mTimer = 0.0f;

            // do the animation base on counter.
            mCurrentAnimation = mAnimations[mAnimCounter];

            mCurrentAnimation.Play(0, true);

            // add up the counter.
            ++mAnimCounter;
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(playKey))
                Play();

            if (JCS_Input.GetKeyDown(stopKey))
                Stop();
        }
#endif

        /// <summary>
        /// Start playing the animation sequence.
        /// </summary>
        public void Play()
        {
            // reset counter
            mAnimCounter = 0;

            mActive = true;

            mDonePlayingSequence = false;
        }

        /// <summary>
        /// Stop the animation sequence.
        /// </summary>
        public void Stop()
        {
            // reset counter
            mAnimCounter = 0;

            mActive = false;

            foreach (JCS_2DAnimation anim in mAnimations)
            {
                // stop the animation.
                anim.Stop();
            }
        }
    }
}
