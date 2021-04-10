/**
 * $File: JCS_3DAnimator.cs $
 * $Date: 2016-09-28 13:42:28 $
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
    /// 3D Animator. Play the animation with multiple control 
    /// of JCS_3DAnimation objects.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_3DAnimator
        : MonoBehaviour
    {
        /* Variables */

        protected Animator mAnimator = null;

        /* Setter & Getter */

        public Animator GetAnimator() { return this.mAnimator; }

        /* Functions */

        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        /// <summary>
        /// Play the animation in current frame.
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
