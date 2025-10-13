/**
 * $File: JCS_I2DAnimator.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D Animator interface object.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public abstract class JCS_I2DAnimator : MonoBehaviour
    {
        /* Variables */

        protected Animator mAnimator = null;
        protected AnimatorStateInfo mAnimatorStateInfo;

        /* Setter & Getter */

        public Animator GetAnimator() { return mAnimator; }
        public AnimatorStateInfo GetAnimatorStateInfo() { return mAnimatorStateInfo; }

        /* Functions */

        protected virtual void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// Animation design here...
        /// </summary>
        /// <param name="state"></param>
        public abstract void DoAnimation(JCS_LiveObjectState state = JCS_LiveObjectState.STAND);

        /// <summary>
        /// play the animation in current frame.
        /// </summary>
        public virtual void PlayAnimationInFrame()
        {
            GetAnimator().enabled = true;
        }

        /// <summary>
        /// Stop animation in current frame.
        /// </summary>
        public virtual void StopAnimationInFrame()
        {
            GetAnimator().enabled = false;
        }
    }
}
