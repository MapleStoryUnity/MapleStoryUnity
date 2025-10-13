/**
 * $File: JCS_Bullet.cs $
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
    /// Bullet base class.
    /// </summary>
    [RequireComponent(typeof(JCS_AttackerInfo))]
    public abstract class JCS_Bullet : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Bullet)")]

        [Tooltip("How Fast the bullet moves.")]
        [SerializeField] 
        protected float mMoveSpeed = 10.0f;

        [Tooltip("Is this object reflectable?")]
        [SerializeField] 
        protected bool mReflectable = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        protected JCS_AttackerInfo mAttackerInfo = null;

        /* Setter & Getter */

        public bool reflectable { get { return mReflectable; } }
        public virtual float moveSpeed { get { return mMoveSpeed; } set { mMoveSpeed = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public JCS_AttackerInfo attackerInfo { get { return mAttackerInfo; } }

        /* Functions */

        protected virtual void Awake()
        {
            mAttackerInfo = GetComponent<JCS_AttackerInfo>();
        }
    }
}
