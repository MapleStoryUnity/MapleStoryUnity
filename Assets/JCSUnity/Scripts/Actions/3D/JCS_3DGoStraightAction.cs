/**
 * $File: JCS_3DGoStraightAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Move forward base on their own direction.
    /// </summary>
    public class JCS_3DGoStraightAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_3DGoStraightAction) **")]

        [Tooltip("How fast it moves.")]
        [SerializeField]
        [Range(-500.0f, 500.0f)]
        private float mMoveSpeed = 10.0f;

        [Tooltip("Which axis it moves.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Header("- Randomize Settings (JCS_3DGoStraightAction)")]

        [Tooltip(@"Randomize the speed depends on positive
move speed to negative move speed.")]
        [SerializeField]
        private bool mRandomizeSpeedAtStart = false;

        [Tooltip("Value randomize the move speed.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandomSpeedValue = 5.0f;

        /* Setter & Getter */

        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }
        public bool RandomizeSpeedAtStart { get { return this.mRandomizeSpeedAtStart; } set { this.mRandomizeSpeedAtStart = value; } }
        public float RandomSpeedValue { get { return this.mRandomSpeedValue; } set { this.mRandomSpeedValue = value; } }

        /* Functions */

        private void Start()
        {
            // randomize speed?
            if (mRandomizeSpeedAtStart)
                this.MoveSpeed += JCS_Random.Range(-mRandomSpeedValue, mRandomSpeedValue);
        }

        /**
         * FixedUpdate take so much of the performance. Consider use
         * regular Update instead of FixedUpdate.
         *
         * NOTE: if you want this to be accurate use FixedUpdate.
         */
        /* Comment either one of them. */
        //private void Update()
        private void FixedUpdate()
        {
            Vector3 newPos = Vector3.zero;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    {
                        newPos = Vector3.right * MoveSpeed * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        newPos = Vector3.forward * MoveSpeed * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        newPos = Vector3.up * MoveSpeed * Time.deltaTime;
                    }
                    break;
            }

            // if is valid, do action.
            //if (!JCS_Mathf.IsNaN(newPos))
            this.transform.Translate(newPos);
        }
    }
}
