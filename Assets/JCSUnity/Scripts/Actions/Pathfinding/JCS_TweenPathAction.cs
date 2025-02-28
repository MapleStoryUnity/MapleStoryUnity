/**
 * $File: JCS_TweenPathAction.cs $
 * $Date: 2020-04-02 19:06:04 $
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
    /// The point to point simple path action that uses tween to move.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_TweenPathAction : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;

        private JCS_AdjustTimeTrigger mAdjustTimerTrigger = null;

        [Separator("Check Variables (JCS_TweenPathAction)")]

        [Tooltip("Current target point index that this object to going approach.")]
        [SerializeField]
        private int mTargetPointIndex = -1;

        [Separator("Runtime Variables (JCS_TweenPathAction)")]

        [Tooltip("List of points for setting up the path.")]
        [SerializeField]
        private List<JCS_UnityObject> mPoints = null;

        [Tooltip("Random the path by randomizing the target point.")]
        [SerializeField]
        private bool mRandom = false;

        [Tooltip("Do continue tween instead of just tween.")]
        [SerializeField]
        private bool mContinueTween = false;

        /* Setter & Getter */

        public List<JCS_UnityObject> Points { get { return this.mPoints; } }
        public bool Random { get { return this.mRandom; } set { this.mRandom = value; } }
        public bool ContinueTween { get { return this.mContinueTween; } set { this.mContinueTween = value; } }

        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mAdjustTimerTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

#if UNITY_EDITOR
            if (mPoints.Count == 0)
                JCS_Debug.LogWarning("Path action with 0 path point is not valid");
#endif

            mAdjustTimerTrigger.onAction = DoPath;
        }

        /// <summary>
        /// Generate the next new point but not the same as last point.
        /// </summary>
        private void GetNextPoint()
        {
            if (mRandom)
            {
                int len = mPoints.Count;
                int newIndex = JCS_Random.RangeInclude(0, len - 1);

                if (newIndex == mTargetPointIndex && len != 1)
                    GetNextPoint();
                else
                    mTargetPointIndex = newIndex;
            }
            else
            {
                ++mTargetPointIndex;

                if (mTargetPointIndex >= mPoints.Count)
                    mTargetPointIndex = 0;
            }
        }

        private void DoPath()
        {
            GetNextPoint();

            if (mContinueTween)
                mTransformTweener.DoTweenContinue(mPoints[mTargetPointIndex]);
            else
                mTransformTweener.DoTween(mPoints[mTargetPointIndex].transform.position);
        }
    }
}
