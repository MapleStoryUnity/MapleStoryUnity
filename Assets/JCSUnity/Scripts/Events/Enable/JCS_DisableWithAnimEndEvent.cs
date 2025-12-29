/**
 * $File: JCS_DisableWithAnimEndEvent.cs $
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
    /// Disable the game object after the done playing the animation.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_DisableWithAnimEndEvent : MonoBehaviour
    {
        /* Variables */

        private Animator mAnimator = null;

        private float mAnimationTimer = 0.0f;

        [Separator("⚡️ Runtime Variables (JCS_DisableWithAnimEndEvent)")]

        [Tooltip("Times the animation need to loops to trigger this event.")]
        [SerializeField]
        private uint mLoopTimes = 1;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public uint loopTimes { get { return mLoopTimes; } set { mLoopTimes = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

            mAnimationTimer += JCS_Time.ItTime(mTimeType);

            if (mAnimationTimer > animatorStateInfo.length * mLoopTimes)
            {
                mAnimationTimer = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
