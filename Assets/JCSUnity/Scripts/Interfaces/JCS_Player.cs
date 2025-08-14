/**
 * $File: JCS_Player.cs $
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
    /// Player base class.
    /// </summary>
    [RequireComponent(typeof(JCS_CharacterControllerInfo))]
    public abstract class JCS_Player : MonoBehaviour
    {
        /* Variables */

        protected CharacterController mCharacterController = null;
        protected JCS_CharacterControllerInfo mCharacterControllerInfo = null;

        [Separator("Runtime Variables (JCS_Player)")]

        [Tooltip("How fast this player moves.")]
        [SerializeField]
        protected Vector3 mVelocity = Vector3.zero;

        [Tooltip("Move speed of this player.")]
        [SerializeField]
        [Range(0.0f, 3000.0f)]
        protected float mMoveSpeed = 10.0f;

        [Tooltip("Is the player controllable?")]
        [SerializeField]
        protected bool mIsControllable = true;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool GetIsControllable() { return mIsControllable; }
        public void SetIsControllable(bool act) { mIsControllable = act; }
        public Vector3 velocity { get { return mVelocity; } set { mVelocity = value; } }
        public float velX { get { return mVelocity.x; } set { mVelocity.x = value; } }
        public float velY { get { return mVelocity.y; } set { mVelocity.y = value; } }
        public float velZ { get { return mVelocity.z; } set { mVelocity.z = value; } }
        public CharacterController GetCharacterController() { return mCharacterController; }
        public JCS_CharacterControllerInfo characterControllerInfo { get { return mCharacterControllerInfo; } }
        public float moveSpeed { get { return mMoveSpeed; } set { mMoveSpeed = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            mCharacterController = GetComponent<CharacterController>();
            mCharacterControllerInfo = GetComponent<JCS_CharacterControllerInfo>();
        }

        protected virtual void Start()
        {
            // set Execute order lower than "JCS_GameManager"
            JCS_GameManager.FirstInstance().Player = this;

            // Player Manager will take care of all the player
            JCS_PlayerManager.FirstInstance().AddPlayerToManage(this);
        }

        protected virtual void Update()
        {
            // empty
        }

        protected virtual void LateUpdate()
        {
            // empty
        }

        protected virtual void FixedUpdate()
        {
            if (mCharacterController.enabled)
            {
                // apply force
                mCharacterController.Move(mVelocity * JCS_Time.ItTime(mTimeType));
            }
        }

        /// <summary>
        /// Control this player or not base on boolean pass in.
        /// </summary>
        /// <param name="act"> 
        /// true : control. 
        /// false : not control. 
        /// </param>
        public abstract void ControlEnable(bool act);

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stand() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Attack() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Jump() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Prone() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Alert() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Fly() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Ladder() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Rope() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dance() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Swim() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Sit() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Hit() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Die() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Ghost() { }
    }
}
