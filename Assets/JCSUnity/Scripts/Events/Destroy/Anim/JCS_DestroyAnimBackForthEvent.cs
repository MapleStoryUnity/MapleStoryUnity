/**
 * $File: JCS_DestroyAnimBackForthEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Destroy the gameobject after plays the animation back 
    /// and forth once.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_DestroyAnimBackForthEvent
        : MonoBehaviour
    {
        /* Variables */

        private Animator mAnimator = null;
        private float mAnimationTimer = 0.0f;

        private AnimatorStateInfo mAnimtorStateInfo;

        private bool mPlayBack = false;
        private bool mPlayForth = false;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_DestroyAnimBackForthEvent) **")]

        [SerializeField]
        private bool mTestWithKey = false;

        [SerializeField]
        private KeyCode mPlayForthKey = KeyCode.I;

        [SerializeField]
        private KeyCode mPlayBackKey = KeyCode.O;
#endif


        [Header("** Runtime Variables (JCS_DestroyAnimBackForthEvent) **")]

        [Tooltip("How many times to plays back and forth before destorying.")]
        [SerializeField] [Range(1, 30)]
        private int mPlayTimes = 1;

        // Count the play times.
        private int mPlayCount = 0;


        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            mAnimtorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            if (mTestWithKey)
            {
                if (JCS_Input.GetKeyDown(mPlayForthKey))
                    PlayForth();
                if (JCS_Input.GetKeyDown(mPlayBackKey))
                    PlayBack();
            }
#endif


            // play forth first
            if (mPlayForth)
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer > mAnimtorStateInfo.length)
                {
                    mPlayForth = false;

                    // since we are using the same timer, 
                    // we simply set the timer to zero in order to 
                    // calculate the back animation time.
                    mAnimationTimer = 0;

                    // disable the animator
                    mAnimator.enabled = false;
                }
            }

            // play back second
            if (mPlayBack)
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer > mAnimtorStateInfo.length)
                {
                    mPlayBack = false;

                    mAnimationTimer = 0;

                    // Add up the play count.
                    ++mPlayCount;

                    // If reach the play times, destroy it.
                    if (mPlayTimes <= mPlayCount)
                        Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        /// Play the animation forth once.
        /// </summary>
        public void PlayForth()
        {
            mPlayForth = true;
            mPlayBack = false;

            mAnimator.enabled = true;

            mAnimationTimer = 0;

            // in Animator Window create a float variable call 
            // as the following name.
            mAnimator.SetFloat("AnimSpeed", 1);

            // get the name of this clip
            string tempClipName = mAnimator.runtimeAnimatorController.animationClips[0].ToString();
            int index = tempClipName.IndexOf(" ");
            string clipName = tempClipName.Substring(0, index);


            mAnimator.Play(clipName,   0);
        }

        /// <summary>
        /// Play the animation back once.
        /// </summary>
        public void PlayBack()
        {
            mPlayBack = true;
            mPlayForth = false;

            mAnimator.enabled = true;

            mAnimationTimer = 0;

            // in Animator Window create a float variable call 
            // as the following name.
            mAnimator.SetFloat("AnimSpeed", -1);

            // get the name of this clip
            string tempClipName = mAnimator.runtimeAnimatorController.animationClips[0].ToString();
            int index = tempClipName.IndexOf(" ");
            string clipName = tempClipName.Substring(0, index);


            mAnimator.Play(clipName, -1, mAnimtorStateInfo.normalizedTime);
        }
    }
}
