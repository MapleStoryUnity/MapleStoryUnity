/**
 * $File: JCS_Bullet.cs $
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
    /// Bullet base class.
    /// </summary>
    [RequireComponent(typeof(JCS_AttackerInfo))]
    public abstract class JCS_Bullet
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_Bullet) **")]

        [Tooltip("How Fast the bullet moves.")]
        [SerializeField] 
        protected float mMoveSpeed = 10.0f;

        [Tooltip("Is this object reflectable? (JCS_2DReflectBulletAction)")]
        [SerializeField] 
        protected bool mReflectable = false;

        protected JCS_AttackerInfo mAttackerInfo = null;

        /* Setter & Getter */

        public bool Reflectable { get { return this.mReflectable; } }
        public virtual float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public JCS_AttackerInfo AttackerInfo { get { return this.mAttackerInfo; } }

        /* Functions */

        protected virtual void Awake()
        {
            mAttackerInfo = this.GetComponent<JCS_AttackerInfo>();
        }
    }
}
