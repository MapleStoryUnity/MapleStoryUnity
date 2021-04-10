/**
 * $File: JCS_DisableWithAnimEndEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Disable the gameobject after the done playing the animation.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_DisableWithAnimEndEvent
        : MonoBehaviour
    {
        /* Variables */

        private Animator mAnimator = null;

        private float mAnimationTimer = 0.0f;

        [Header("** Runtime Variables (JCS_DisableWithAnimEndEvent) **")]

        [Tooltip("Times the animation need to loops to trigger this event.")]
        [SerializeField]
        private uint mLoopTimes = 1;

        /* Setter & Getter */

        public uint LoopTimes { get { return this.mLoopTimes; } set { this.mLoopTimes = value; } }

        /* Functions */

        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

            mAnimationTimer += Time.deltaTime;

            if (mAnimationTimer > animatorStateInfo.length * mLoopTimes)
            {
                mAnimationTimer = 0;
                this.gameObject.SetActive(false);
            }
        }
    }
}
