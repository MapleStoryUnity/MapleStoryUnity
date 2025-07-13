/**
 * $File: JCS_2DMoveTile.cs $
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
    /// Moving tile usually as a background.
    /// </summary>
    public class JCS_2DMoveTile : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_2DMoveTile)")]

        [SerializeField]
        [ReadOnly]
        private float mWidth = 0.0f;

        [SerializeField]
        [ReadOnly]
        private float mHeight = 0.0f;

        [Separator("Runtime Variables (JCS_2DMoveTile)")]

        [Tooltip("How fast this tile moves?")]
        [SerializeField]
        private float mMoveSpeed = 10.0f;

        private Vector3 mVelocity = Vector3.zero;

        // Record down the starting position.
        private Vector3 mOriginPosition = Vector3.zero;

        [Tooltip("Move in y axis.")]
        [SerializeField]
        private bool mIsYAxis = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            var sp = this.GetComponent<SpriteRenderer>();

            Vector2 spriteRect = JCS_UIUtil.GetSpriteRendererRectWithNoScale(sp);

            mWidth = spriteRect.x;
            mHeight = spriteRect.y;

            mOriginPosition = this.transform.position;
        }

        private void Update()
        {
            if (!mIsYAxis)
            {
                this.mVelocity.x = mMoveSpeed;

                float distance = this.transform.position.x - mOriginPosition.x;
                distance = JCS_Mathf.AbsoluteValue(distance);
                if (distance > mWidth)
                    this.transform.position = mOriginPosition;
            }
            else
            {
                this.mVelocity.y = mMoveSpeed;

                float distance = this.transform.position.y - mOriginPosition.y;
                distance = JCS_Mathf.AbsoluteValue(distance);
                if (distance > mHeight)
                    this.transform.position = mOriginPosition;
            }

            this.transform.position += mVelocity * JCS_Time.ItTime(mTimeType);
        }
    }
}