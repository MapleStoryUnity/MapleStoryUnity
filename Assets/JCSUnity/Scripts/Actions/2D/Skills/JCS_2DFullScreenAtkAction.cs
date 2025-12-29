/**
 * $File: JCS_2DFullScreenAtkAction.cs $
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
    /// Full screen attack.
    /// </summary>
    [RequireComponent(typeof(JCS_AnimPool))]
    public class JCS_2DFullScreenAtkAction : JCS_2DSkills, JCS_IAction
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_2DFullScreenAtkAction)")]

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

        public int animDesity { get { return mAnimDesity; } set { mAnimDesity = value; } }
        public float skillWide { get { return mSkillWide; } set { mSkillWide = value; } }
        public float skillHeight { get { return mSkillHeight; } set { mSkillHeight = value; } }
        public int skillHit { get { return mSkillHit; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mAnimPoolSupAnim = GetComponent<JCS_AnimPool>();
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
            for (int index = 0; index < mAnimDesity; ++index)
            {
                // get a random animate from pool
                RuntimeAnimatorController anim = mAnimPoolSupAnim.GetRandomAnim();

                GameObject obj = JCS_Util.SpawnAnimateObjectDeathEvent(anim, mOrderLayer);

#if UNITY_EDITOR
                obj.name = "JCS_2DFullScreenAtkAction";
#endif

                if (mSamePosition)
                    obj.transform.position = transform.position;
                if (mSameRotation)
                    obj.transform.rotation = transform.rotation;
                if (mSameScale)
                    obj.transform.localScale = transform.localScale;

                AddHitSound(ref obj);
                SetToRandomPos(ref obj);
            }
        }

        /// <summary>
        /// Set the game object to the random position.
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

            var spa = obj.AddComponent<JCS_SoundProxyAction>();
            spa.audioClip = mHitSound;
        }
    }
}
