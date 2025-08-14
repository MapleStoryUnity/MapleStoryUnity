/**
 * $File: JCS_SimplePathAction.cs $
 * $Date: 2020-04-03 08:03:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// The point to point simple path action.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    [RequireComponent(typeof(JCS_3DLookAtAction))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_SimplePathAction : MonoBehaviour
    {
        /* Variables */

        private JCS_3DGoStraightAction mGoStraightAction = null;

        private JCS_3DLookAtAction mLookAtAction = null;

        private JCS_AdjustTimeTrigger mAdjustTimerTrigger = null;

        [Separator("Check Variables (JCS_SimplePathAction)")]

        [Tooltip("Current target point index that this object to going approach.")]
        [SerializeField]
        [ReadOnly]
        private int mTargetPointIndex = -1;

        [Separator("Runtime Variables (JCS_SimplePathAction)")]

        [Tooltip("List of points for setting up the path.")]
        [SerializeField]
        private List<Transform> mPoints = null;

        [Tooltip("Type of the path action.")]
        [SerializeField]
        private JCS_PathActionType mPathActionType = JCS_PathActionType.INCREMENT;

        [Tooltip("Range that will stop the movement.")]
        [SerializeField]
        [Range(0.0f, 3000.0f)]
        private float mAcceptRange = 0.3f;

        /* Setter & Getter */

        public List<Transform> points { get { return mPoints; } }
        public JCS_PathActionType pathActionType { get { return mPathActionType; } set { mPathActionType = value; } }
        public float acceptRange { get { return mAcceptRange; } set { mAcceptRange = value; } }

        /* Functions */

        private void Awake()
        {
            mGoStraightAction = GetComponent<JCS_3DGoStraightAction>();
            mLookAtAction = GetComponent<JCS_3DLookAtAction>();
            mAdjustTimerTrigger = GetComponent<JCS_AdjustTimeTrigger>();

            mAdjustTimerTrigger.onAction = DoPath;

#if UNITY_EDITOR
            if (mPoints.Count == 0)
                Debug.LogWarning("Path action with 0 path point is not valid");
#endif

            GetNextPoint();
        }

        private void Update()
        {
            if (!mGoStraightAction.enabled)
                return;

            Transform target = mPoints[mTargetPointIndex];
            float distance = Vector3.Distance(target.position, transform.position);

            if (mAcceptRange >= distance)
                mGoStraightAction.enabled = false;
        }

        /// <summary>
        /// Generate the next new point but not the same as last point.
        /// </summary>
        private void GetNextPoint()
        {
            switch (mPathActionType)
            {
                case JCS_PathActionType.INCREMENT:
                    {
                        ++mTargetPointIndex;

                        if (mTargetPointIndex >= mPoints.Count)
                            mTargetPointIndex = 0;
                    }
                    break;
                case JCS_PathActionType.DECREMENT:
                    {
                        --mTargetPointIndex;

                        if (mTargetPointIndex < 0)
                            mTargetPointIndex = mPoints.Count - 1;
                    }
                    break;
                case JCS_PathActionType.INC_OR_DEC:
                    {
                        int incOrDec = JCS_Random.RangeInclude(0, 1);

                        if (incOrDec == 0)
                            mPathActionType = JCS_PathActionType.INCREMENT;
                        else
                            mPathActionType = JCS_PathActionType.DECREMENT;

                        GetNextPoint();

                        mPathActionType = JCS_PathActionType.INC_OR_DEC;
                    }
                    break;
                case JCS_PathActionType.RANDOM_ALL:
                    {
                        int len = mPoints.Count;
                        int newIndex = JCS_Random.RangeInclude(0, len - 1);

                        if (newIndex == mTargetPointIndex && len != 1)
                            GetNextPoint();
                        else
                            mTargetPointIndex = newIndex;
                    }
                    break;
            }
        }

        private void DoPath()
        {
            GetNextPoint();

            mGoStraightAction.enabled = true;

            mLookAtAction.SetTargetTransform(mPoints[mTargetPointIndex]);
        }
    }
}
