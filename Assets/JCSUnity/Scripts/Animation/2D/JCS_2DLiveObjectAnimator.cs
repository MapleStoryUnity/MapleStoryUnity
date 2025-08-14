/**
 * $File: JCS_2DLiveObjectAnimator.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Animator specific for live object.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DLiveObjectAnimator : JCS_I2DAnimator
    {
        /* Variables */

        private JCS_2DAnimator m2DAnimator = null;

        [Separator("Check Variables (JCS_2DLiveObjectAnimator)")]

        [SerializeField]
        [ReadOnly]
        protected string mCurrentStateName = "";

        [SerializeField]
        [ReadOnly]
        protected JCS_LiveObjectState mCurrentState = JCS_LiveObjectState.STAND;

        [Separator("Initialize Variables (JCS_2DLiveObjectAnimator)")]

        [Tooltip(@"Override the current animation, start from the beginning.")]
        [SerializeField]
        protected bool mOverrideAnim = false;

        /* Setter & Getter */

        public JCS_2DAnimator animator { get { return m2DAnimator; } }
        public JCS_LiveObjectState GetCurrentAnimationState() { return mCurrentState; }

        /* Functions */
        protected override void Awake()
        {
            base.Awake();

            m2DAnimator = GetComponent<JCS_2DAnimator>();
        }

        /// <summary>
        /// Do the animation.
        /// </summary>
        /// <param name="state"></param>
        public override void DoAnimation(JCS_LiveObjectState state = JCS_LiveObjectState.STAND)
        {
            m2DAnimator.DoAnimation((int)state);
            mCurrentState = state;
        }

        /// <summary>
        /// Check if the animation in the same state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsInState(JCS_LiveObjectState state)
        {
            return m2DAnimator.IsInState((int)state);
        }

        /// <summary>
        /// Play the animation in current frame.
        /// </summary>
        public override void PlayAnimationInFrame()
        {
            // empty..
        }

        /// <summary>
        /// Stop animation in current frame.
        /// </summary>
        public override void StopAnimationInFrame()
        {
            // empty..
        }
    }
}
