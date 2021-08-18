/**
 * $File: JCS_2DAnimDisplayHolder.cs $
 * $Date: 2017-06-18 21:15:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Hold an animation state for few seconds.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DAnimDisplayHolder : MonoBehaviour
    {
        /* Variables */

        private JCS_2DAnimator m2DAnimator = null;

        [Header("** Check Variables (JCS_2DAnimDisplayHolder) **")]

        [Tooltip(@"Record down what is the current animation playing, so after 
holding we could play the animation back in time.")]
        [SerializeField]
        private int mStoreAnimIndex = 0;

        private int mHoldAnimIndex = 0;

        // trigger holding?
        private bool mHolding = false;

        [Header("** Runtime Variables (JCS_2DAnimDisplayHolder) **")]

        [Tooltip("How long to hold this animation.")]
        [SerializeField] [Range(0.0f, 10.0f)]
        private float mHoldTime = 0.5f;

        private float mHoldTimer = 0.0f;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();
        }

        private void LateUpdate()
        {
            HoldAnim();
        }

        /// <summary>
        /// Hold an animation for few seconds.
        /// </summary>
        /// <param name="animIndex"> anim index. </param>
        public void HoldAnimation(int animIndex)
        {
            HoldAnimation(animIndex, this.mHoldTime);
        }

        /// <summary>
        /// Hold an animation for few seconds.
        /// </summary>
        /// <param name="animIndex"> anim index. </param>
        /// <param name="time"> time for animation to play. </param>
        public void HoldAnimation(int animIndex, float time)
        {
            this.mHoldAnimIndex = animIndex;

            if (this.m2DAnimator.CurrentAnimId != mHoldAnimIndex)
                this.mStoreAnimIndex = this.m2DAnimator.CurrentAnimId;

            this.m2DAnimator.DoAnimation(this.mHoldAnimIndex);

            this.mHoldTime = time;
            this.mHoldTimer = 0.0f;

            mHolding = true;
        }

        /// <summary>
        /// Stop holding the animation.
        /// </summary>
        public void StopHolding()
        {
            mHolding = false;

            this.mHoldTimer = 0.0f;
        }

        /// <summary>
        /// Do the holding animation algorithm here...
        /// </summary>
        private void HoldAnim()
        {
            if (!mHolding)
                return;

            // start timer.
            mHoldTimer += Time.deltaTime;

            // record down whats the current animation playing.
            if (this.m2DAnimator.CurrentAnimId != mHoldAnimIndex)
                this.mStoreAnimIndex = this.m2DAnimator.CurrentAnimId;

            // start holding the animation.
            m2DAnimator.DoAnimation(mHoldAnimIndex);

            // check if done holding the animation.
            if (mHoldTime > mHoldTimer)
                return;

            // reset timer and trigger.
            mHoldTimer = 0.0f;
            mHolding = false;

            // start the previous animation once.
            m2DAnimator.DoAnimation(mStoreAnimIndex);
        }
    }
}
