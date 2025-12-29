/**
 * $File: JCS_LiveObject.cs $
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
    /// Base class of live object in world.
    /// </summary>
    public abstract class JCS_LiveObject : MonoBehaviour
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_LiveObject)")]

        [Tooltip(@"Health, Auto add, for better design plz add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mHPInfo = null;

        [Tooltip(@"Mana, Auto add, for better design plz add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mMPInfo = null;

        [Tooltip(@"Experience, Auto add, for better design plz add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mEXPInfo = null;

        // HP before really apply the attack
        protected int mPreCalHP = 0;

        [Tooltip("How much damage could knock back?")]
        [SerializeField]
        [Range(0, 999999999)]
        protected int mKB = 0;

        // Should we set it to global?
        [Tooltip("How much force to knock back?")]
        [SerializeField]
        [Range(1, 200)]
        protected float mKnockBackForce = 1;

        [Tooltip("Do the damage text effect after damage?")]
        [SerializeField]
        protected bool mDamageTextEffect = true;

        [Tooltip("Can the live object be damage?")]
        [SerializeField]
        protected bool mCanDamage = true;

        [Separator("Optional Variables (JCS_LiveObject)")]

        [Tooltip("Do this live object provide invincible time action?")]
        [SerializeField]
        protected JCS_InvincibleTimeAction mInvincibleTimeAction = null;

        /* Setter & Getter */

        public JCS_LiquidBarInfo hpInfo { get { return mHPInfo; } }
        public JCS_LiquidBarInfo mpInfo { get { return mMPInfo; } }
        public JCS_LiquidBarInfo expInfo { get { return mEXPInfo; } }
        public int hp { get { return mHPInfo.currentValue; } set { mHPInfo.currentValue = value; } }
        public int mp { get { return mMPInfo.currentValue; } set { mMPInfo.currentValue = value; } }
        public int exp { get { return mEXPInfo.currentValue; } set { mEXPInfo.currentValue = value; } }
        public void SetHP(int val)
        {
            if (val < 0)
                val = 0;
            hp = val;
        }
        public void SetMP(int val)
        {
            if (val < 0)
                val = 0;
            mp = val;
        }
        public bool canDamage { get { return mCanDamage; } set { mCanDamage = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            // try to get the action component by itself.
            if (mInvincibleTimeAction == null)
                mInvincibleTimeAction = GetComponent<JCS_InvincibleTimeAction>();

            // Auto add, for better design plz 
            // add "JCS_LiquidBarInfo" manually.
            if (mHPInfo == null)
                mHPInfo = gameObject.AddComponent<JCS_LiquidBarInfo>();
            mHPInfo.tagName = "HP";

            if (mMPInfo == null)
                mMPInfo = gameObject.AddComponent<JCS_LiquidBarInfo>();
            mMPInfo.tagName = "MP";


            if (mEXPInfo == null)
            {
                mEXPInfo = gameObject.AddComponent<JCS_LiquidBarInfo>();
                mEXPInfo.currentValue = 0;
            }
            mEXPInfo.tagName = "EXP";
        }

        /// <summary>
        /// Call this when the live object get hits.
        /// </summary>
        public virtual void Hit()
        {
            if (mInvincibleTimeAction != null)
            {
                // trigger the action.
                mInvincibleTimeAction.TriggerAction();
            }
        }

        /// <summary>
        /// Do the knock back effect.
        /// 
        /// Knock Back effect is like pushing the object
        /// a little bit furthur from the attacker.
        /// </summary>
        /// <param name="attacker"> attacker in order to determine the direction 
        ///                     this object get know back. </param>
        public abstract void KnockBack(Transform attacker = null);

        /// <summary>
        /// Do the knock back effect.
        /// 
        /// Knock Back effect is like pushing the object
        /// a little bit furthur from the attacker.
        /// </summary>
        /// <param name="force"> force to knock back </param>
        /// <param name="attacker"> attacker in order to determine the direction 
        ///                     this object get know back. </param>
        public abstract void KnockBack(float force, Transform attacker = null);
    }
}
