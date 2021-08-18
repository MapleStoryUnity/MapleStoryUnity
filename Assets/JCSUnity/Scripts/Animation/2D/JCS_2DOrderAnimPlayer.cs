/**
 * $File: JCS_2DOrderAnimPlayer.cs $
 * $Date: 2017-05-10 18:23:15 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Play animator's animations in order base on if the animation are 
    /// done playing it.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DOrderAnimPlayer : MonoBehaviour
    {
        /* Variables */

        private JCS_2DAnimator mAnimator = null;

        [Header("** Check Variables (JCS_2DOrderAnimPlayer) **")]

        [Tooltip("Current playing animation's id.")]
        [SerializeField]
        private int mCurrentAnimationId = 0;

        [Header("** Runtime Variables (JCS_2DOrderAnimPlayer) **")]

        [Tooltip("Is the animation working?")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("Play the animation when the component is awake?")]
        [SerializeField]
        private bool mPlayOnAwake = false;

        [Tooltip("Loop the animations?")]
        [SerializeField]
        private bool mLoop = false;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool PlayOnAwake { get { return this.mPlayOnAwake; } set { this.mPlayOnAwake = value; } }
        public bool Loop { get { return this.mLoop; } set { this.mLoop = value; } }
        
        /* Functions */

        private void Start()
        {
            this.mAnimator = this.GetComponent<JCS_2DAnimator>();

            if (mPlayOnAwake)
            {
                // start the animation from the first id.
                mAnimator.DoAnimation(0, true);

                mActive = true;
            }
        }

        private void Update()
        {
            if (!mActive)
                return;

            // check if animation done playing.
            if (!mAnimator.CurrentAnimation.IsDonePlaying)
                return;

            // add up for next animation.
            ++mCurrentAnimationId;

            // reset anim id if loop.
            if (mCurrentAnimationId >= mAnimator.AnimationsLength)
            {
                if (mLoop)
                    mCurrentAnimationId = 0;
                else
                {
                    // set to the last animation in the array.
                    mCurrentAnimationId = mAnimator.AnimationsLength - 1;

                    // If not loop, just disable the cycle trigger.
                    mActive = false;
                }
            }

            // play animaiton.
            mAnimator.DoAnimation(mCurrentAnimationId, false, true);
        }
    }
}
