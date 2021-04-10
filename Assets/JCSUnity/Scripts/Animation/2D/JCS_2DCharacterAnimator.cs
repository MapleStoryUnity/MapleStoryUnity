/**
 * $File: JCS_2DCharacterAnimator.cs $
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
    /// Player must have the animation implemented.
    /// </summary>
    [Obsolete(@"Old and will be remove in newer version... 
Use JCS_2DAnimator instead.", true)]
    public class JCS_2DCharacterAnimator
        : JCS_I2DAnimator
    {
        /* Variables */

        [Header("** Check Variables (JCS_2DCharacterAnimator) **")]

        [Tooltip("")]
        [SerializeField]
        protected string mCurrentStateName = "";

        [Tooltip("")]
        [SerializeField]
        protected JCS_LiveObjectState mCurrentState = JCS_LiveObjectState.STAND;

        [Tooltip("")]
        [SerializeField]
        private JCS_AttackState mAttackState = JCS_AttackState.NONE;

        [Tooltip("")]
        [SerializeField]
        protected bool mEndAttackStage = true;

        [Tooltip("")]
        [SerializeField]
        protected float mAnimationTimer = 0.0f;

        [Header("** Initialize Variables (JCS_2DCharacterAnimator) **")]

        [Tooltip(@"Override the current animation, start from the beginning.")]
        [SerializeField]
        protected bool mOverrideAnim = false;

        [Tooltip(@"Please set the int inside the 'Animator' window in Unity, 
Control the animation state with code are much easier to programmer.")]
        [SerializeField]
        protected string mAnimationState = "State";

        [Tooltip("")]
        [SerializeField] [Range(-10.0f, 10.0f)]
        protected float mAnimationOffset = 0.05f;

        [Tooltip("Plz use %jcs as the state variable name.")]
        [SerializeField]
        protected string mFullClipStateName = "Player_01_%jcs";

        [Tooltip(@"U can implement multiple attack animation here by naming the 
animation `attack01/attack02/attack03`, etc.")]
        [SerializeField] [Range(1, 5)]
        protected int mAttackAnimationCount = 1;

        /* Setter & Getter */

        public string GetAnimationState() { return this.mAnimationState; }
        public JCS_LiveObjectState GetCurrentAnimationState() { return this.mCurrentState; }
        public bool GetEndAttackStage() { return this.mEndAttackStage; }
        public JCS_AttackState GetAttackState() { return this.mAttackState; }

        /* Functions */

        private void Update()
        {
            if (mEndAttackStage)
                return;

            if (IsInState(JCS_LiveObjectState.RAND_ATTACK))
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer >= mAnimatorStateInfo.length + mAnimationOffset)
                {
                    mAnimationTimer = 0.0f;
                    mEndAttackStage = true;
                }
            }
        }

        /// <summary>
        /// Animation design here...
        /// </summary>
        /// <param name="state"></param>
        public override void DoAnimation(JCS_LiveObjectState state)
        {
            // return if the animation are already playing
            if (mCurrentState == state)
            {
                if (mOverrideAnim)
                    PlayAtBeginning();
                return;
            }

            // state in Unity Animator System.
            int stateIndex = (int)state;

            if (state == JCS_LiveObjectState.RAND_ATTACK)
            {
                // get the correct attack animation.
                stateIndex = (int)GetRandomAttackState();

                mAttackState = ((JCS_AttackState)stateIndex);

                // update state name by attack index/animation!
                mCurrentStateName = GetStateName(state, (JCS_AttackState)stateIndex);

                // ready to check attack ends.
                mEndAttackStage = false;
                mAnimationTimer = 0.0f;
            }
            else
                mCurrentStateName = GetStateName(state);

            // Set the state machine into Unity Engine's animator system!
            GetAnimator().SetInteger(GetAnimationState(), stateIndex);

            // record down the current state we are in.
            this.mCurrentState = state;
        }

        /// <summary>
        /// Check this player in that state!
        /// </summary>
        /// <param name="state"> state to check </param>
        /// <returns> true: in the state, false: not in the state </returns>
        public bool IsInState(JCS_LiveObjectState state)
        {
            // first check the self-define one.
            if (mCurrentState != state)
                return false;

            // then check Unity's Animator state.
            mAnimatorStateInfo = GetAnimator().GetCurrentAnimatorStateInfo(0);

            return mAnimatorStateInfo.IsName(mCurrentStateName);
        }

        /// <summary>
        /// Play the animation start of the 
        /// current aniamtion.
        /// </summary>
        public void PlayAtBeginning()
        {
            mAnimator.Play(mCurrentStateName, -1, 0);
        }

        /// <summary>
        /// Find the clip by clip name.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="animClipName"></param>
        /// <returns></returns>
        private AnimationClip FindClip(AnimationClip[] arr, string animClipName)
        {

            for (int index = 0;
                index < arr.Length;
                ++index)
            {
                if (arr[index].name == animClipName)
                    return arr[index];
            }

            JCS_Debug.LogError("No animation's clip found");
            return null;
        }

        /// <summary>
        /// Get the specific state name from animation state.
        /// </summary>
        /// <param name="state"> state to get </param>
        /// <returns> name of the state supose to be. </returns>
        private string GetStateName(JCS_LiveObjectState state, JCS_AttackState attackState = JCS_AttackState.NONE)
        {
            string stateName = mFullClipStateName;
            string swapName = "";

            switch (state)
            {
                case JCS_LiveObjectState.STAND:
                    swapName = "stand";
                    break;
                case JCS_LiveObjectState.WALK:
                    swapName = "walk";
                    break;
                case JCS_LiveObjectState.RAND_ATTACK:
                    {
                        switch (attackState)
                        {
                            case JCS_AttackState.ATTACK_01:
                                swapName = "attack01";
                                break;
                            case JCS_AttackState.ATTACK_02:
                                swapName = "attack02";
                                break;
                            case JCS_AttackState.ATTACK_03:
                                swapName = "attack03";
                                break;
                            case JCS_AttackState.ATTACK_04:
                                swapName = "attack04";
                                break;
                            case JCS_AttackState.ATTACK_05:
                                swapName = "attack05";
                                break;
                        }
                    }
                    break;
                case JCS_LiveObjectState.JUMP:
                    swapName = "jump";
                    break;
                case JCS_LiveObjectState.PRONE:
                    swapName = "prone";
                    break;
                case JCS_LiveObjectState.ALERT:
                    swapName = "alert";
                    break;
                case JCS_LiveObjectState.FLY:
                    swapName = "fly";
                    break;
                case JCS_LiveObjectState.LADDER:
                    swapName = "ladder";
                    break;
                case JCS_LiveObjectState.ROPE:
                    swapName = "rope";
                    break;
                case JCS_LiveObjectState.SIT:
                    swapName = "sit";
                    break;
                case JCS_LiveObjectState.HIT:
                    swapName = "hit";
                    break;
                case JCS_LiveObjectState.DANCE:
                    swapName = "dance";
                    break;
                case JCS_LiveObjectState.SWIM:
                    swapName = "swim";
                    break;
                case JCS_LiveObjectState.DIE:
                    swapName = "die";
                    break;
                case JCS_LiveObjectState.GHOST:
                    swapName = "ghost";
                    break;
            }

            string[] words;

            words = stateName.Split(new[] { "%jcs" }, StringSplitOptions.None);

            stateName = words[0] + swapName + words[1];

            return stateName;
        }

        /// <summary>
        /// Randomly pick one attack animation.
        /// </summary>
        /// <returns> animation index to attack state. </returns>
        private JCS_AttackState GetRandomAttackState()
        {
            // apply a random attack animation.
            int attackAnimIndex = JCS_Random.Range(1, mAttackAnimationCount + 1);

            switch (attackAnimIndex)
            {
                case 1:
                    return JCS_AttackState.ATTACK_01;
                case 2:
                    return JCS_AttackState.ATTACK_02;
                case 3:
                    return JCS_AttackState.ATTACK_03;
                case 4:
                    return JCS_AttackState.ATTACK_04;
                case 5:
                    return JCS_AttackState.ATTACK_05;
            }

            JCS_Debug.LogError("This shouldn't happens");

            // this sould not happens.
            return JCS_AttackState.ATTACK_01;
        }
    }
}
