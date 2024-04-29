/**
 * $File: JCS_Bubble.cs $
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
    /// The component make the bubble movement.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    public class JCS_Bubble : JCS_WeatherParticle
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Bubble)")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        private bool mDoAction = true;

        [Tooltip("How intense it shakes?")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mShakeMargin = 2.0f;

        [Tooltip("How fast it moves?")]
        [SerializeField]
        [Range(0.1f, 1000.0f)]
        private float mShakeSpeed = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool DoAction { get { return this.mDoAction; } set { this.mDoAction = value; } }
        public float ShakeSpeed { get { return this.mShakeSpeed; } set { this.mShakeSpeed = value; } }
        public float ShakeMargin { get { return this.mShakeMargin; } set { this.mShakeMargin = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
            if (!mDoAction)
                return;

            Vector3 newPos = this.transform.position;
            newPos.x += JCS_Random.Range(-mShakeMargin, mShakeMargin) * ShakeSpeed * JCS_Time.DeltaTime(mDeltaTimeType);
            this.transform.position = newPos;
        }
    }
}
