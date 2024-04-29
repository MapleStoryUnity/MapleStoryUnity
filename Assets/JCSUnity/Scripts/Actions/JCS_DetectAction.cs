/**
 * $File: JCS_DetectAction.cs $
 * $Date: 2016-11-18 12:45:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Do the behaviour on the detection.
    /// </summary>
    public class JCS_DetectAction : JCS_AIAction
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DetectAction)")]

        [Tooltip("Do this action?")]
        [SerializeField]
        private bool mDoAction = true;

        [Tooltip("Targets we are detecting.")]
        [SerializeField]
        private Transform[] mTargetTransforms = null;

        [Tooltip("Range to detect.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mDetectRange = 1.0f;

        // Detect one transform?
        private bool mDetectOne = false;

        // Detect all the transform?
        private bool mDetectAll = false;

        /* Setter & Getter */

        public bool DoAction { get { return this.mDoAction; } set { this.mDoAction = value; } }
        public float DetectRange { get { return this.mDetectRange; } set { this.mDetectRange = value; } }

        /* Functions */

        private void Update()
        {
            if (!mDoAction)
                return;

            DoDetect();
        }

        /// <summary>
        /// Detect one of the transform?
        /// </summary>
        /// <returns> 
        /// true: detect one of the transform.
        /// false: not transform detect.
        /// </returns>
        public bool DetectOneTransform()
        {
            return this.mDetectOne;
        }

        /// <summary>
        /// Detect all of the transform?
        /// </summary>
        /// <returns> 
        /// true: detect one of the transform.
        /// false: not transform detect.
        /// </returns>
        public bool DetectAllTransform()
        {
            return this.mDetectAll;
        }

        /// <summary>
        /// Do detect all the transforms.
        /// </summary>
        private void DoDetect()
        {
            // turn all trigger off.
            mDetectOne = false;
            mDetectAll = false;

            int count = 0;
            int actualCount = mTargetTransforms.Length;

            for (int index = 0; index < mTargetTransforms.Length; ++index)
            {
                Transform targetTrans = mTargetTransforms[index];

                // skip if null...
                if (targetTrans == null)
                {
                    // once is null, actual length is minus!!
                    --actualCount;
                    continue;
                }

                float distance = Vector3.Distance(targetTrans.position, this.transform.position);

                // check distance in the detect range.
                if (distance <= mDetectRange)
                {
                    // detect one target!!
                    mDetectOne = true;
                    ++count;
                }
            }

            // 1) actual transform count (does not include null transform)
            // 2) count : detected count.
            if (actualCount == count)
            {
                // detect all!!
                mDetectAll = true;
            }
        }
    }
}
