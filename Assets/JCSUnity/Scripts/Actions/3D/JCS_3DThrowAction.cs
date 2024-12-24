/**
 * $File: JCS_3DThrowAction.cs $
 * $Date: 2017-10-14 $
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
    /// Thorws a gameobject toward another gameobject.
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

        [Tooltip("Force to hit the target.")]
        [SerializeField]
        [Range(0.1f, 300.0f)]
        private float mForce = 20.0f;

        [Tooltip("Key to use to test using the time function.")]
        [SerializeField]
        private KeyCode mTestWithTimeKey = KeyCode.None;

        [Tooltip("Target time to hit the target.")]
        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float mTime = 1.0f;
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
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public Vector3 Velocity { get { return this.mVelocity; } }
        public float GravityProduct { get { return this.mGravityProduct; } set { this.mGravityProduct = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            if (!mActive)
                return;

            float dt = JCS_Time.DeltaTime(mDeltaTimeType);

            // make it effect by gravity.
            this.mVelocity.y += JCS_Constants.GRAVITY * mGravityProduct * dt;

            // add up velocity.
            this.transform.position += mVelocity * dt;
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (Input.GetKeyDown(mTestWithVelKey))
                ThrowByForce(mTestTarget.position, mForce);
            if (Input.GetKeyDown(mTestWithTimeKey))
                ThrowByTime(mTestTarget.position, mTime);
        }
#endif

        /// <summary>
        /// Do the throw action by time.
        /// </summary>
        /// <param name="startPos"> Point to start this action. </param>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="time"> Certain time will reach the target position. </param>
        public void ThrowByTime(Vector3 startPos, Vector3 targetPos, float time)
        {
            this.transform.position = startPos;
            ThrowByTime(targetPos, time);
        }

        /// <summary>
        /// Do the throw action by time.
        /// </summary>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="time"> Time. </param>
        public void ThrowByTime(Vector3 targetPos, float time)
        {
            Vector3 displacement = targetPos - this.transform.position;

            mVelocity.x = displacement.x / time;
            mVelocity.z = displacement.z / time;
            mVelocity.y = (displacement.y - (JCS_Constants.GRAVITY * mGravityProduct * time * time / 2)) / time;

            // start dropping.
            this.mActive = true;
        }

        /// <summary>
        /// Do the throw action by calculate the kinematic.
        /// </summary>
        /// <param name="startPos"> Point to start this action. </param>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="vel"> velocity to hit the point. </param>
        public void ThrowByForce(Vector3 startPos, Vector3 targetPos, float vel)
        {
            this.transform.position = startPos;
            ThrowByForce(targetPos, vel);
        }

        /// <summary>
        /// Do the throw action by calculate the kinematic.
        /// </summary>
        /// <param name="targetPos"> Point you want to hit. </param>
        /// <param name="vel"> velocity to hit the point. </param>
        public void ThrowByForce(Vector3 targetPos, float vel)
        {
            float distance = Vector3.Distance(targetPos, this.transform.position);
            float time = distance / vel;

            ThrowByTime(targetPos, time);
        }
    }
}
