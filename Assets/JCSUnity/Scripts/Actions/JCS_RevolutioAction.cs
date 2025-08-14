/**
 * $File: JCS_RevolutioAction.cs $
 * $Date: 2017-09-19 12:27:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// AAction to make the revolution to an object.
    /// </summary>
    public class JCS_RevolutioAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_RevolutioAction)")]

        [Tooltip("Current angle.")]
        [SerializeField]
        private int mDegree = 0;

        [Tooltip("Radius to revolution.")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRadius = 10.0f;

        [Tooltip("Origin transform.")]
        [SerializeField]
        private Transform mOrigin = null;

        [Tooltip("Axis on revolution.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Tooltip("Revolute to local position instead of global position.")]
        [SerializeField]
        private bool mRevoluteAsLocalPosition = false;

        /* Setter & Getter */

        public JCS_Axis axis { get { return mAxis; } set { mAxis = value; } }
        public bool revoluteAsLocalPosition { get { return mRevoluteAsLocalPosition; } set { mRevoluteAsLocalPosition = value; } }
        public Transform origin { get { return mOrigin; } set { mOrigin = value; } }
        public float radius { get { return mRadius; } set { mRadius = value; } }
        public int degree { get { return mDegree; } set { mDegree = value; } }

        /* Functions */

        private void Update()
        {
            DoRevolution();
        }

        /// <summary>
        /// Do the revolution action.
        /// </summary>
        private void DoRevolution()
        {
            if (mOrigin == null)
                return;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    {
                        if (mRevoluteAsLocalPosition)
                        {
                            transform.localPosition
                                = JCS_Mathf.CirclePositionX(
                                    mOrigin.localPosition,
                                    transform.localPosition,
                                    mDegree,
                                    mRadius);
                        }
                        else
                        {
                            transform.position
                                = JCS_Mathf.CirclePositionX(
                                    mOrigin.position,
                                    transform.position,
                                    mDegree,
                                    mRadius);
                        }
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        if (mRevoluteAsLocalPosition)
                        {
                            transform.localPosition
                                = JCS_Mathf.CirclePositionY(
                                    mOrigin.localPosition,
                                    transform.localPosition,
                                    mDegree,
                                    mRadius);
                        }
                        else
                        {
                            transform.position
                                = JCS_Mathf.CirclePositionY(
                                    mOrigin.position,
                                    transform.position,
                                    mDegree,
                                    mRadius);
                        }
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        if (mRevoluteAsLocalPosition)
                        {
                            transform.localPosition
                                = JCS_Mathf.CirclePositionZ(
                                    mOrigin.localPosition,
                                    transform.localPosition,
                                    mDegree,
                                    mRadius);
                        }
                        else
                        {
                            transform.position
                                = JCS_Mathf.CirclePositionZ(
                                    mOrigin.position,
                                    transform.position,
                                    mDegree,
                                    mRadius);
                        }
                    }
                    break;
            }
        }
    }
}
