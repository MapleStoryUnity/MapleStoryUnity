/**
 * $File: JCS_2DFullScreenAtkAction.cs $
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
    /// Full screen attack.
    /// </summary>
    [RequireComponent(typeof(JCS_AnimPool))]
    public class JCS_2DFullScreenAtkAction
        : JCS_2DSkills
        , JCS_Action
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_2DFullScreenAtkAction) **")]

        [Tooltip("Supporting animation pool.")]
        [SerializeField]
        private JCS_AnimPool mAnimPoolSupAnim = null;

        [Tooltip("How many support animation going play on the scene.")]
        [SerializeField]
        [Range(0, 30)]
        private int mAnimDesity = 20;

        [Tooltip("How wide the skill effect?")]
        [SerializeField]
        [Range(0.0f, 1500.0f)]
        private float mSkillWide = 750.0f;

        [Tooltip("How hight the skill effect?")]
        [SerializeField]
        [Range(0.0f, 1500.0f)]
        private float mSkillHeight = 750.0f;

        [Tooltip("Maximum the enmey kill in the scene.")]
        [SerializeField]
        [Range(0, 15)]
        private int mSkillHit = 15;

        /* Setter & Getter */

        public int AnimDesity { get { return this.mAnimDesity; } set { this.mAnimDesity = value; } }
        public float SkillWide { get { return this.mSkillWide; } set { this.mSkillWide = value; } }
        public float SkillHeight { get { return this.mSkillHeight; } set { this.mSkillHeight = value; } }
        public int SkillHit { get { return this.mSkillHit; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mAnimPoolSupAnim = this.GetComponent<JCS_AnimPool>();
        }

        /// <summary>
        /// Active the skill.
        /// </summary>
        public override void ActiveSkill()
        {
            SpawnMainAnim1();
            SpawnMainAnim2();

            SpawnSupAnim();

            mSoundPlayer.PlayOneShot(mUseSound);
        }

        /// <summary>
        /// Spawn a support animation.
        /// </summary>
        private void SpawnSupAnim()
        {
            for (int index = 0;
                index < mAnimDesity;
                ++index)
            {
                // get a random animate from pool
                RuntimeAnimatorController anim = mAnimPoolSupAnim.GetRandomAnim();

                GameObject obj = JCS_Utility.SpawnAnimateObjectDeathEvent(anim, mOrderLayer);

#if (UNITY_EDITOR)
                obj.name = "JCS_2DFullScreenAtkAction";
#endif

                if (mSamePosition)
                    obj.transform.position = this.transform.position;
                if (mSameRotation)
                    obj.transform.rotation = this.transform.rotation;
                if (mSameScale)
                    obj.transform.localScale = this.transform.localScale;

                AddHitSound(ref obj);
                SetToRandomPos(ref obj);
            }
        }

        /// <summary>
        /// Set the gameobject to the random position.
        /// </summary>
        /// <param name="obj"></param>
        private void SetToRandomPos(ref GameObject obj)
        {
            float randPosX = JCS_Random.Range(-mSkillWide, mSkillWide);
            float randPosY = JCS_Random.Range(-mSkillHeight, mSkillHeight);

            Vector3 newPos = obj.transform.position;
            newPos.x += randPosX;
            newPos.y += randPosY;
            obj.transform.position = newPos;
        }

        /// <summary>
        /// Add the hit sound.
        /// </summary>
        /// <param name="obj"></param>
        private void AddHitSound(ref GameObject obj)
        {
            if (mHitSound == null)
                return;

            JCS_SoundProxyAction spa = obj.AddComponent<JCS_SoundProxyAction>();
            spa.audioClip = mHitSound;
            spa.SoundSettingType = mSoundPlayer.SoundSettingType;
        }
    }
}
