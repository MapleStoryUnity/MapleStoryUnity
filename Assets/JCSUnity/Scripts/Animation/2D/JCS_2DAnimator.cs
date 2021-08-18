/**
 * $File: JCS_2DAnimator.cs $
 * $Date: 2017-03-13 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Hold sequence of animations and play it by condition.
    /// </summary>
    public class JCS_2DAnimator : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables Variables (JCS_2DAnimator) **")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to play first animation.")]
        [SerializeField]
        private KeyCode mFirstAnimKey = KeyCode.A;

        [Tooltip("Key to play second animation.")]
        [SerializeField]
        private KeyCode mSecondAnimKey = KeyCode.S;

        [Tooltip("Key to play third animation.")]
        [SerializeField]
        private KeyCode mThirdAnimKey = KeyCode.D;

        [Tooltip("Key to play one shot on the current animation.")]
        [SerializeField]
        private KeyCode mPlayOneShotAnimKey = KeyCode.F;
#endif

        [Header("** Check Variables Variables (JCS_2DAnimator) **")]

        [Tooltip("Current animation id.")]
        [SerializeField]
        private int mCurrentAnimId = 0;

        [Tooltip("Current playing animation.")]
        [SerializeField]
        private JCS_2DAnimation mCurrentAnimation = null;

        [Tooltip("Maxinum frame in the animation.")]
        [SerializeField]
        private int mMaxAnimCount = 0;

        // Current animation stack.
        private JCS_2DAnimation mStackAnim = null;
        private int mStackAnimId = -1;

        private JCS_2DAnimation mOneShotAnim = null;

        [Header("** Runtime Variables (JCS_2DAnimator) **")]

        [Tooltip(@"How fast the animation plays.")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mAnimationTimeProduction = 1.0f;

        [Tooltip("All the animations this animator holds.")]
        [SerializeField]
        private List<JCS_2DAnimation> mAnimations = null;

        [Header("** Optional Variables (JCS_2DAnimator) **")]

        [Tooltip("Hold animation displayed frame event.")]
        [SerializeField]
        private JCS_2DAnimDisplayHolder m2DAnimDisplayHolder = null;

        /* Setter & Getter */

        public float AnimationTimeProduction { get { return this.mAnimationTimeProduction; } }
        public int CurrentAnimId { get { return this.mCurrentAnimId; } }
        public JCS_2DAnimation CurrentAnimation { get { return this.mCurrentAnimation; } }
        public List<JCS_2DAnimation> Animations { get { return this.mAnimations; } }
        public int AnimationsLength { get { return this.mAnimations.Count; } }

        public JCS_2DAnimDisplayHolder AnimDisplayHolder { get { return this.m2DAnimDisplayHolder; } }

        /* Functions */

        private void Awake()
        {
            /*
             * NOTE(jayces): consider this as a default value because
             * this component is really handy.
             */
            if (m2DAnimDisplayHolder == null)
                m2DAnimDisplayHolder = this.GetComponent<JCS_2DAnimDisplayHolder>();

            // update the max animation count.
            UpdateMaxAnimCount();
        }

        private void Start()
        {
            // active this animation.
            ActiveOneAnimation(0);

            // override the component in the animation array.
            foreach (JCS_2DAnimation anim in mAnimations)
            {
                if (anim == null)
                    continue;

                // let the animation know there are animator controlling 
                // the animation.
                anim.SetJCS2DAnimator(this);

                anim.PlayOnAwake = false;
            }
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            DoPlayOneShot();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(mFirstAnimKey))
                DoAnimation(0, true);

            if (JCS_Input.GetKey(mSecondAnimKey))
                DoAnimation(1, false);

            if (JCS_Input.GetKey(mThirdAnimKey))
                DoAnimation(2, true);

            if (JCS_Input.GetKey(mPlayOneShotAnimKey))
                PlayOneShot(0);
        }
#endif

        /// <summary>
        /// Play the animation base on the animation ID.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        /// <param name="over"> override the animation? </param>
        /// <param name="oneShot"> Is this animation playing one shot? </param>
        public void DoAnimation(
            int id, 
            bool over = false, 
            bool oneShot = false)
        {
            if (!over)
            {
                // if same id, return.
                if (mCurrentAnimId == id)
                    return;
            }

            mCurrentAnimId = id;

            PutAnimaIdInSequence();

            // get the current playing animation.
            mCurrentAnimation = mAnimations[mCurrentAnimId];

            if (mCurrentAnimation == null)
            {
#if (UNITY_EDITOR)
                if (JCS_GameSettings.instance.DEBUG_MODE)
                    JCS_Debug.LogError("Swtich animation failed cuz of null reference animation assigned...");
#endif
                return;
            }

            if (oneShot)
            {
                // restart the target animation.
                mCurrentAnimation.Play(0, true);
            }
            else
            {
                // play target animation.
                mCurrentAnimation.Play(-1, true);
            }

            // active this animation.
            ActiveOneAnimation(mCurrentAnimId);
        }

        /// <summary>
        /// Play one animation once and go back to 
        /// original animation.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        /// <param name="over"> override the play one shot action? </param>
        public void PlayOneShot(int id, bool over = false)
        {
            // 如果要蓋過, 就不檢查了.
            if (!over)
            {
                // check if play one shot still do the stuff.
                if (mStackAnim != null)
                    return;
            }

            // push the animation to stack.
            mStackAnim = mCurrentAnimation;
            mStackAnimId = mCurrentAnimId;

            /**
             * Record down the animation, in order to check if there is 
             * an animation changes during the one shot function.
             */
            mOneShotAnim = mAnimations[id];

            // play the newer animation.
            DoAnimation(id, over, true);
        }

        /// <summary>
        /// Check what animation is currently playing by this animator.
        /// </summary>
        /// <param name="inAnimId"> id to check </param>
        /// <returns> 
        /// true : the same animation.
        /// false : vice versa.
        /// </returns>
        public bool IsInState(int inAnimId)
        {
            return (inAnimId == mCurrentAnimId);
        }

        /// <summary>
        /// Play the animation with current frame.
        /// </summary>
        public void PlayAnimationInFrame()
        {
            if (mCurrentAnimation == null)
                return;

            mCurrentAnimation.Play();
        }

        /// <summary>
        /// Stop animation with current frame.
        /// </summary>
        public void StopAnimationInFrame()
        {
            if (mCurrentAnimation == null)
                return;

            mCurrentAnimation.Pause();
        }

        /// <summary>
        /// Update the maxinum frame count from the 
        /// anim frame sequence.
        /// </summary>
        public void UpdateMaxAnimCount()
        {
            this.mMaxAnimCount = this.mAnimations.Count;
        }

        /// <summary>
        /// Make sure the animation we are playing 
        /// are not null reference.
        /// </summary>
        private void PutAnimaIdInSequence()
        {
            if (mCurrentAnimId < 0)
                this.mCurrentAnimId = 0;
            else if (mCurrentAnimId >= mMaxAnimCount - 1)
                this.mCurrentAnimId = mMaxAnimCount - 1;
        }

        /// <summary>
        /// Active on animation by ID.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        private void ActiveOneAnimation(int id)
        {
            if (mAnimations[id] == null)
                return;

            foreach (JCS_2DAnimation anim in mAnimations)
            {
                if (anim == null)
                    continue;

                if (anim == mAnimations[id])
                    continue;

                // disable all
                anim.Active = false;
                anim.Stop();
            }

            // only active playing target animation.
            mAnimations[id].Active = true;
        }

        /// <summary>
        /// Check if play one shot valid.
        /// If valid, do the play one shot algorithm.
        /// </summary>
        private void DoPlayOneShot()
        {
            /**
             * Check stack empty, if not empty meaning there are PlayOneShot() 
             * animation going on.
             * 
             * ATTENTION(jenchieh): make sure to clean up the stack after the 
             * animatin is played.
             */
            if (mStackAnim == null)
                return;


            // clean up the stack after the animation is done playing.

            /* During the one shot animation, if there are animation change  */
            if (mOneShotAnim != mCurrentAnimation || 
                /* Of the animation done playing. */
                mCurrentAnimation.IsDonePlaying)
            {
                // switch the animation back to original animation by stack id.
                DoAnimation(mStackAnimId);

                // clean up the stack pointer.
                mStackAnim = null;
                mOneShotAnim = null;

                // clean up stack animation id.
                mStackAnimId = -1;
            }
        }
    }
}
