/**
 * $File: JCS_2DDeadAction.cs $
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
    /// Dead action for live object.
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_2DDeadAction : MonoBehaviour
    {
        /* Variables */

        private JCS_2DLiveObject mLiveObject = null;

        [Header("- Optional")]

        [Tooltip("Live object animation.")]
        [SerializeField]
        private JCS_2DLiveObjectAnimator mLiveObjectAnimator = null;

        [Header("- Sound")]

        [Tooltip("Play one shot when this action active.")]
        [SerializeField]
        private AudioClip mDieSound = null;

        // check to play the die sound.
        private bool mSoundPlayed = false;

        [Header("- Effect")]

        [Tooltip("Disable unnecessary componenet when died.")]
        [SerializeField]
        private bool mDisableOtherComponentWhileDie = true;

        [Tooltip("Freeze the object while displaying dead animation.")]
        [SerializeField]
        private bool mFreezeWhileDie = true;

        [Tooltip("Array of componenet you want to disable when died.")]
        [SerializeField]
        private MonoBehaviour[] mDisableComponents = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            mLiveObject = this.GetComponent<JCS_2DLiveObject>();

            if (mLiveObjectAnimator == null)
                mLiveObjectAnimator = this.GetComponent<JCS_2DLiveObjectAnimator>();
        }

        private void Update()
        {
            // if still could damage this live object, mean this object isn't
            // dead yet.
            if (0 < mLiveObject.HP)
                return;

            // if dies
            if (!mSoundPlayed)
            {
                // just play one time.
                JCS_SoundManager.FirstInstance().GlobalSoundPlayer().PlayOneShot(mDieSound);
                mSoundPlayed = true;
            }

            // do the dead animation
            if (mLiveObjectAnimator.Animator.AnimDisplayHolder != null)
                mLiveObjectAnimator.Animator.AnimDisplayHolder.StopHolding();
            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.DIE);

            if (mFreezeWhileDie)
                mLiveObject.VelocityInfo.Freeze();

            // disable other un-necessary components
            if (mDisableOtherComponentWhileDie)
            {
                for (int index = 0; index < mDisableComponents.Length; ++index)
                {
                    // disable all un-necessary component
                    if (mDisableComponents[index] == null)
                        continue;

                    // disable the component
                    mDisableComponents[index].enabled = false;
                }

                // after disable it dont do it agian in next frame.
                mDisableOtherComponentWhileDie = false;
            }

            // if the animation starts, start timer and check the
            // dead animation is end or not.
            if (mLiveObjectAnimator.IsInState(JCS_LiveObjectState.DIE))
            {
                if (mLiveObjectAnimator.Animator.CurrentAnimation.IsDonePlaying)
                {
                    // if is end destroy the object itself.
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
