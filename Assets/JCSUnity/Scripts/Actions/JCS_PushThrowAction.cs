/**
 * $File: JCS_PushThrowAction.cs $
 * $Date: 2016-11-04 23:28:10 $
 * $Revision: $
 * $Creator: Tony Wei & Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action that throw the object to a position.
    /// </summary>
    public class JCS_PushThrowAction : MonoBehaviour
    {
        /* Variables */

        private Vector3 mVelocity = Vector3.zero;

        [Separator("Runtime Variables (JCS_PushThrowAction)")]

        [Tooltip("Do effect?")]
        [SerializeField]
        private bool mEffect = false;

        [Tooltip("Angle you want to throw to.")]
        [SerializeField]
        private float mAngle = 0.0f;

        [Tooltip("Speed of this projectile. ")]
        [SerializeField]
        private float mG = 2.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public float Angle { get { return this.mAngle; } set { this.mAngle = value; } }
        public float G { get { return this.mG; } set { this.mG = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Update()
        {
            // check do the effect or not?
            if (!mEffect)
                return;

            float dt = JCS_Time.ItTime(mTimeType);

            this.mVelocity.y -= mG * dt;
            this.transform.position += this.mVelocity * dt;
        }

        /// <summary>
        /// Call this function to start the projectile.
        /// </summary>
        /// <param name="start"> start position </param>
        /// <param name="target"> target position </param>
        public void SetStart(Vector3 start, Vector3 target)
        {
            SetStart(start, target, mG);
        }

        /// <summary>
        /// Call this function to start the projectile.
        /// </summary>
        /// <param name="start"> start position </param>
        /// <param name="target"> target position </param>
        /// <param name="force"> speed to the target </param>
        public void SetStart(Vector3 start, Vector3 target, float force)
        {
            SetStart(start, target, mAngle, force);
        }

        /// <summary>
        /// Call this function to start the projectile.
        /// </summary>
        /// <param name="start"> start position </param>
        /// <param name="target"> target position </param>
        /// <param name="angle"> angle u want to shoot </param>
        /// <param name="force"> speed to the target </param>
        public void SetStart(Vector3 start, Vector3 target, float angle, float force)
        {
            // set the starting position.
            this.transform.position = start;

            // get the force.
            this.mG = force;

            // calculate start velocity
            float tan = Mathf.Tan(angle * Mathf.Deg2Rad);
            float h = Mathf.Abs(start.y - target.y);
            float w = Mathf.Abs(target.x - start.x);

            float vX = Mathf.Sqrt((force * w * w) / (2 * (h + w * tan)));
            float vY = tan * vX;

            vX = (target.x - start.x) < 0 ? vX : -vX;
            this.mVelocity = new Vector2(-vX, vY);
        }
    }
}
