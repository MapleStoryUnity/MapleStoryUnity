/**
 * $File: JCS_2DLiveObject.cs $
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
    /// Living thing related object.
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObjectAnimator))]
    public class JCS_2DLiveObject : JCS_LiveObject
    {
        /* Variables */

        private SpriteRenderer mSpriteRenderer = null;
        private JCS_2DAnimator m2DAnimator = null;

        [Separator("Check Variables (JCS_2DLiveObject)")]

        [Tooltip("This boolean to check what type of object is this.")]
        [SerializeField]
        [ReadOnly]
        private bool mIsPlayer = false;

        [Tooltip("Is the live object been target?")]
        [SerializeField]
        [ReadOnly]
        private bool mBeenTarget = false;

        [Separator("Optional Variables (JCS_2DLiveObject)")]

        [Tooltip("If the object is player from JCSUnity does not recommand using this object.")]
        [SerializeField]
        private JCS_VelocityInfo mVelocityInfo = null;

        [Tooltip(@"Apply Ability Format could let u design the ability scale to 
this transform as a living object")]
        [SerializeField]
        private JCS_AbilityFormat mAbilityFormat = null;

        [Tooltip(@"Record down all the information about the attacker on the same map. In order
to get the information from them.")]
        [SerializeField]
        private JCS_AttackerRecorder mAttackRecorder = null;

        /* Setter & Getter */

        public JCS_2DAnimator liveObjectAnimator { get { return m2DAnimator; } }
        public SpriteRenderer spriteRenderer { get { return mSpriteRenderer; } }
        public bool beenTarget { get { return mBeenTarget; } set { mBeenTarget = value; } }
        public bool damageTextEffect { get { return mDamageTextEffect; } set { mDamageTextEffect = value; } }
        public bool isPlayer { get { return mIsPlayer; } set { mIsPlayer = value; } }

        // Optional Variables
        public JCS_VelocityInfo velocityInfo { get { return mVelocityInfo; } }
        public JCS_AbilityFormat abilityFormat { get { return mAbilityFormat; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();

            // try to get this component in this transform.
            if (mVelocityInfo == null)
                mVelocityInfo = this.GetComponent<JCS_VelocityInfo>();
            if (mAbilityFormat == null)
                mAbilityFormat = this.GetComponent<JCS_AbilityFormat>();
            if (mAttackRecorder == null)
                mAttackRecorder = this.GetComponent<JCS_AttackerRecorder>();

            // auto detect to see if this game object player or not.
            var p = this.GetComponent<JCS_Player>();

            // if found it return true!
            if (p != null)
                mIsPlayer = true;
            else
                mIsPlayer = false;

            // set hp the same
            mPreCalHP = hp;
        }

        public bool IsDead()
        {
            return (hp <= 0);
        }

        /// <summary>
        /// Delta change the mana point.
        /// </summary>
        /// <param name="val"> value add on mana, could + or -. </param>
        public void DeltaMP(int val)
        {
            mp += val;
        }

        /// <summary>
        /// Call this event to active the dead effect.
        /// 
        /// Design the Die event here...
        /// </summary>
        public virtual void Die()
        {
            // if something still targeting this, 
            // return until have not been target.
            if (beenTarget)
                return;

            // force play animation.
            if (m2DAnimator != null)
                m2DAnimator.PlayAnimationInFrame();

            // force color back to white.
            if (mSpriteRenderer != null)
                mSpriteRenderer.color = Color.white;

            hp = 0;
            mp = 0;
            mCanDamage = false;

            // disable all the collision.
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
                boxCollider.enabled = false;

            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
                cc.enabled = false;

            if (mVelocityInfo != null)
            {
                // freeze the object
                mVelocityInfo.Freeze();
            }
        }

        /// <summary>
        /// Check if the object is dead.
        /// </summary>
        public void CheckDie()
        {
            if (hp <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreDead()
        {
            mCanDamage = false;
            mPreCalHP = 0;

            // freeze the object
            mVelocityInfo.Freeze();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="attacker"></param>
        public void ReceivePreCalDamage(int[] damages, Transform attacker = null)
        {
            foreach (int damage in damages)
            {
                if (!mCanDamage)
                    break;

                PreCalDamage(damage);
            }
        }

        /// <summary>
        /// Optional choice to apply Damage Text. Base
        /// function is under this function with the same name.
        /// </summary>
        public void ApplyDamageText(
            int minDamage, int maxDamage, 
            Vector2 pos, 
            int hit = 1, 
            int criticalChance = 0, 
            AudioClip hitSound = null)
        {
            ApplyDamageText(null, minDamage, maxDamage, pos, hit, criticalChance, hitSound);
        }
        /// <summary>
        /// Apply Damage text by the chosen effect and 
        /// algorithm. Lastly, apply it to Health Point.
        /// </summary>
        /// <param name="attacker"> Attacker </param>
        /// <param name="minDamage"> minimum damage could be </param>
        /// <param name="maxDamage"> maximum damage could be </param>
        /// <param name="pos"> position, usually this transform's position </param>
        /// <param name="hit"> how many hit in this sequence, could be one. </param>
        /// <param name="criticalChance"> critical chance could be? </param>
        public void ApplyDamageText(
            Transform attacker, 
            int minDamage, int maxDamage, 
            Vector2 pos, 
            int hit = 1, 
            int criticalChance = 0,
            AudioClip hitSound = null)
        {
            int[] damages = null;

            // set the last attack in the safe way.
            SetLastAttacker(attacker);

            int defenseValue = 0;

            // get the defense value.
            if (mAbilityFormat != null)
                defenseValue = mAbilityFormat.GetDefenseValue();
            else
            {
                Debug.Log(
                    "No Ability Format attached.");
            }

            // Damage text version
            if (mDamageTextEffect)
            {

                JCS_MixDamageTextPool mixTP = JCS_UtilManager.FirstInstance().GetMixDamageTextPool();
                if (mixTP == null)
                {
#if UNITY_EDITOR
                    if (JCS_GameSettings.FirstInstance().DEBUG_MODE)
                    {
                        Debug.LogError("There is no Mix Damage Text Pool in the scene. Consider to grab one?");
                    }
#endif
                    return;
                }


                // spawn the damage text
                damages = mixTP.DamageTextSpawnerSimple(
                    minDamage, maxDamage, 
                    pos, 
                    hit, 
                    criticalChance,
                    defenseValue,        // defense value
                    isPlayer, 
                    hitSound);
            }
            // Do damage without damage text.
            else {

                // since is not pre-calculate.
                // we have to calculate the damage before
                // actually apply the damage to 
                // this live object.
                for (int index = 0; index < hit; ++index)
                {
                    int dm = Random.Range(minDamage, maxDamage);

                    // 受到的傷害 = 傷害 - 防禦力
                    damages[index] = dm - defenseValue;
                }
            }

            ReceiveDamage(damages, attacker);
        }

        /// <summary>
        /// 使用於 如果已經計算好公式了!
        /// </summary>
        /// <param name="attacker"> Attacker </param>
        /// <param name="damages"> sequence of damage </param>
        /// <param name="pos"> damage text position </param>
        /// <param name="criticalChance"> chance of spawning red text </param>
        public void ApplyDamageText(
            Transform attacker, 
            int[] damages, 
            Vector2 pos, 
            int criticalChance, 
            AudioClip hitSound = null)
        {
            // set the last attack in the safe way.
            SetLastAttacker(attacker);

            // Damage text version
            if (mDamageTextEffect)
            {

                JCS_MixDamageTextPool mixTP = JCS_UtilManager.FirstInstance().GetMixDamageTextPool();
                if (mixTP == null)
                {
                    Debug.LogError(
                        "There is no Mix Damage Text Pool in the scene. Consider to grab one?");
                    return;
                }

                // spawn the damage text
                mixTP.DamageTextSpawnerSimple(
                    damages, 
                    pos, 
                    criticalChance, 
                    isPlayer,
                    hitSound);
            }
            // Do damage without damage text.
            else {
                // no need to do anything cuz is 
                // pre-calculate already.
            }

            ReceiveDamage(damages, attacker);
        }

        /// <summary>
        /// Do the knock back effect.
        /// 
        /// Knock Back effect is like pushing the object
        /// a little bit furthur from the attacker.
        /// </summary>
        /// <param name="attacker"> attacker in order to determine the direction 
        ///                     this object get know back. </param>
        public override void KnockBack(Transform attacker = null)
        {
            KnockBack(mKnockBackForce, attacker);
        }
        /// <summary>
        /// Do the knock back effect.
        /// 
        /// Knock Back effect is like pushing the object
        /// a little bit furthur from the attacker.
        /// </summary>
        /// <param name="force"> force to knock back </param>
        /// <param name="attacker"> attacker in order to determine the direction 
        ///                     this object get know back. </param>
        public override void KnockBack(float force, Transform attacker = null)
        {
            if (mVelocityInfo == null)
                return;

            if (attacker == null)
                return;

            if (attacker.position.x > this.transform.position.x)
                mVelocityInfo.moveSpeedX = -force;
            else
                mVelocityInfo.moveSpeedX = force;
                
        }

        /// <summary>
        /// 在這裡, 直接調用HP變量.
        /// 會變成即時性的攻擊. 所以 先在這裡設定一個新的變量(在這裡是mPreCalHP).
        /// 這個變量有跟hp一樣的量. 然後先用這個變量計算血量.
        /// 如果這個變量已經小於等於0,那我們就不讓其他的人可以鎖定他為攻擊目標!
        /// </summary>
        /// <param name="damage"> 需要計算的血量, damage </param>
        private void PreCalDamage(int damage)
        {
            mPreCalHP -= damage;

            if (mPreCalHP <= 0)
            {
                PreDead();
            }
        }

        /// <summary>
        /// Apply damage in the single damage value.
        /// </summary>
        /// <param name="damage"> damage value </param>
        private void ApplyDamage(int damage)
        {
            hp -= damage;
        }

        /// <summary>
        /// Receive Damages form the attackers sequence attack.
        /// </summary>
        /// <param name="damages"> sequence damages </param>
        /// <param name="attacker"> attacker's transform compoenet </param>
        private void ReceiveDamage(int[] damages, Transform attacker = null)
        {

            foreach (int damage in damages)
            {
                if (hp <= 0)
                {
                    if (m2DAnimator.animDisplayHolder != null)
                        m2DAnimator.animDisplayHolder.StopHolding();
                    break;
                }

                // if damage higher than knock back value, 
                // stop the monster
                if (damage >= mKB)
                {
                    KnockBack(attacker);

                    if (m2DAnimator.animDisplayHolder != null)
                        m2DAnimator.animDisplayHolder.HoldAnimation((int)JCS_LiveObjectState.HIT);
                }

                ApplyDamage(damage);
            }

            // call hit function, in order to
            // do some optional effect.
            Hit();
        }

        /// <summary>
        /// Safe way to set the last attacker.
        /// </summary>
        /// <param name="lastAttacker"> last attacker's transform component </param>
        private void SetLastAttacker(Transform lastAttacker)
        {
            if (mAttackRecorder == null)
                return;

            mAttackRecorder.lastAttacker = lastAttacker;
        }
    }
}
