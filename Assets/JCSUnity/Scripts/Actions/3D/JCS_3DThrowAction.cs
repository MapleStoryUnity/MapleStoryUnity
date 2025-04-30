/**
 * $File: JCS_3DThrowAction.cs $
 * $Date: 2017-10-14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Thorws a game object toward another game object.
    /// </summary>
    public class JCS_3DThrowAction : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_3DThrowAction)")]

        [Tooltip("Target to test to throw to.")]
        [SerializeField]
        private Transform mTestTarget = null;

        [Tooltip("Key to use to test using the velocity/kenimatic function.")]
        [SerializeField]
        private KeyCode mTestWithVelKey = KeyCode.None;

        [Tooltip("Key to use to test using the time function.")]
        [SerializeField]
        private KeyCode mTestWithTimeKey = KeyCode.None;
#endif

        [Separator("Check Variables (JCS_3DThrowAction)")]

        [Tooltip("Velocity of the object.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mVelocity = Vector3.zero;

        [Separator("Runtime Variables (JCS_3DThrowAction)")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("Mulitply the gravity.")]
        [SerializeField]
        [Range(0.1f, 30.0f)]
        private float mGravityProduct = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Rotate to look at forward location.")]
        [SerializeField]
        private bool mFaceFoward = false;

        [Separator("By Force")]

        [Tooltip("Force to hit the target.")]
        [SerializeField]
        [Range(0.1f, 300.0f)]
        private float mForce = 20.0f;

        [Tooltip("Angle degree to hit the target.")]
        [SerializeField]
        private float mDegree = 0.0f;

        [Separator("By Time")]

        [Tooltip("Target time to hit the target.")]
        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float mTime = 1.0f;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public Vector3 Velocity { get { return this.mVelocity; } }
        public float GravityProduct { get { return this.mGravityProduct; } set { this.mGravityProduct = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }
        public bool FaceFoward { get { return this.mFaceFoward; } set { this.mFaceFoward = value; } }

        public float Force { get { return this.mForce; } set { this.mForce = value; } }
        public float Degree { get { return this.mDegree; } set { this.mDegree = value; } }
        public float Time { get { return this.mTime; } set { this.mTime = value; } }

        private float G
        {
            get { return JCS_Constants.GRAVITY * mGravityProduct; }
        }

        /* Functions */

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            if (!mActive)
                return;

            float dt = JCS_Time.ItTime(mTimeType);

            // make it effect by gravity.
            this.mVelocity.y += JCS_Constants.GRAVITY * mGravityProduct * dt;

            // add up velocity.
            this.transform.position += mVelocity * dt;

            if (mFaceFoward)
            {
                Vector3 direction = mVelocity;

                Vector3 point = transform.position + direction * JCS_Physics.LOOK_DISTANCE;

                this.transform.LookAt(point);
            }
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (Input.GetKeyDown(mTestWithVelKey))
                ThrowByForce(mTestTarget.position);

            if (Input.GetKeyDown(mTestWithTimeKey))
                ThrowByTime(mTestTarget.position);
        }
#endif

        /// <summary>
        /// Do the throw action by time.
        /// </summary>
        /// <param name="startPos"> Point to start this action. </param>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="time"> Certain time will reach the target position. </param>
        public void ThrowByTime(Vector3 targetPos)
        {
            ThrowByTime(targetPos, mTime);
        }
        public void ThrowByTime(Vector3 targetPos, float time)
        {
            Vector3 displacement = targetPos - this.transform.position;

            // Calculate initial velocity.
            mVelocity.x = displacement.x / time;
            mVelocity.z = displacement.z / time;

            mVelocity.y = (displacement.y - (0.5f * G * Mathf.Pow(time, 2))) / time;

            // start the action.
            this.mActive = true;
        }

        /// <summary>
        /// Do the throw action by calculate the kinematic.
        /// </summary>
        /// <param name="startPos"> Point to start this action. </param>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="vel"> velocity to hit the point. </param>
        public void ThrowByForce(Vector3 targetPos)
        {
            ThrowByForce(targetPos, mForce, mDegree);
        }
        public void ThrowByForce(Vector3 targetPos, float vel, float degree)
        {
            Vector3 displacement = targetPos - this.transform.position;
            var displacementXZ = new Vector3(displacement.x, 0, displacement.z);
            float horizontalDistance = displacementXZ.magnitude;

            float angle = degree * Mathf.Deg2Rad;
            float horizontalVelocity = vel * Mathf.Cos(angle);

            float time = horizontalDistance / horizontalVelocity;

            ThrowByTime(targetPos, time);
        }

        #region Simulation

        /// <summary>
        /// Return a list of arch positions.
        /// </summary>
        /// <param name="pointCount"> This decides how many points you want. </param>
        /// <param name="startPos"> Starting position. </param>
        /// <param name="targetPos"> End position </param>
        /// <param name="time"> Time to perform. </param>
        /// <param name="gravityProduct"> Arch height. </param>
        public static List<Vector3> GetArchByTime(
            int pointCount,
            Vector3 startPos, Vector3 targetPos,
            float time, float gravityProduct)
        {
            List<Vector3> points = new();

            Vector3 displacement = targetPos - startPos;

            Vector3 velocity = Vector3.zero;

            // Calculate initial velocity.
            velocity.x = displacement.x / time;
            velocity.z = displacement.z / time;
            velocity.y = (displacement.y - (JCS_Constants.GRAVITY * gravityProduct * time * time / 2.0f)) / time;

            /* 開始模擬 */

            float timer = 0.0f;

            float interval = time / pointCount;

            // Add first point.
            points.Add(startPos);

            while (timer < time)
            {
                float dt = interval;  // Interval is the delta time!

                // make it effect by gravity.
                velocity.y += JCS_Constants.GRAVITY * gravityProduct * dt;

                // add up velocity.
                startPos += velocity * dt;

                // Records the positions.
                points.Add(startPos);

                timer += interval;
            }

            return points;
        }

        /// <summary>
        /// Return a list of arch positions.
        /// </summary>
        /// <param name="pointCount"> This decides how many points you want. </param>
        /// <param name="startPos"> Starting position. </param>
        /// <param name="targetPos"> End position </param>
        /// <param name="vel"> Force velocity. </param>
        /// <param name="gravityProduct"> Arch height. </param>
        public static List<Vector3> GetArchByForce(
           int pointCount,
           Vector3 startPos, Vector3 targetPos,
           float vel, float degree, float gravityProduct)
        {
            Vector3 displacement = targetPos - startPos;
            var displacementXZ = new Vector3(displacement.x, 0, displacement.z);
            float horizontalDistance = displacementXZ.magnitude;

            float angle = degree * Mathf.Deg2Rad;
            float horizontalVelocity = vel * Mathf.Cos(angle);

            float time = horizontalDistance / horizontalVelocity;

            return GetArchByTime(pointCount, startPos, targetPos, time, gravityProduct);
        }

        #endregion
    }
}
