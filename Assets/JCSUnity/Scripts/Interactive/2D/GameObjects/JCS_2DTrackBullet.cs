/**
 * $File: JCS_2DTrackBullet.cs $
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
    /// Bullet that will track the object.
    /// </summary>
    [RequireComponent(typeof(JCS_2DTrackAction))]
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    [RequireComponent(typeof(JCS_DestroyObjectWithTime))]
    [RequireComponent(typeof(JCS_HitCountEvent))]
    public class JCS_2DTrackBullet : JCS_Bullet
    {
        /* Variables */

        private bool mAct = false;
        private JCS_2DTrackAction mTrackAction = null;
        private JCS_3DGoStraightAction mGoStraightAction = null;

        [Separator("Initialize Varialbes (JCS_2DTrackBullet)")]

        [SerializeField]
        private float mDelayTimeToFollow = 0;

        private float mDelayTimeToFollowTimer = 0;

        [Separator("Runtime Variables (JCS_2DTrackBullet)")]

        [Tooltip("Do the action?")]
        [SerializeField]
        private bool mLookAtTarget = true;

        /* Setter & Getter */

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mTrackAction = GetComponent<JCS_2DTrackAction>();
            mTrackAction.following = false;


            mGoStraightAction = GetComponent<JCS_3DGoStraightAction>();
            mGoStraightAction.enabled = true;
        }

        private void Start()
        {
            // set all action to this move speed
            mGoStraightAction.moveSpeed = mMoveSpeed;

            // if tracking are using the smooth track.
            // move speed have to be positive in order to get to the
            // correct direction.
            mTrackAction.moveSpeed = JCS_Mathf.ToPositive(mMoveSpeed);
        }

        private void Update()
        {
            DoCounter();
        }

        /// <summary>
        /// Do delay when track.
        /// </summary>
        private void DoCounter()
        {
            // check effect trigger!
            if (!mLookAtTarget)
                return;

            if (mAct)
                return;

            if (mTrackAction.targetTransform == null)
                return;

            mDelayTimeToFollowTimer += JCS_Time.ItTime(mTimeType);

            if (mDelayTimeToFollow < mDelayTimeToFollowTimer)
            {
                // start follow!
                mTrackAction.following = true;

                mAct = true;

                mGoStraightAction.enabled = false;
                mDelayTimeToFollowTimer = 0;
            }
        }
    }
}
