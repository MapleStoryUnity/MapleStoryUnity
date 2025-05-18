/**
 * $File: JCS_TowardTarget.cs $
 * $Date: 2016-11-12 21:27:25 $
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
    /// Particle will lerp to the target position.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_DisableWithCertainRangeEvent))]
    public class JCS_TowardTarget : JCS_Particle
    {
        /* Variables */

        // tweener effect to the object.
        private JCS_TransformTweener mTweener = null;

        // when reach the certain range disable it.
        private JCS_DisableWithCertainRangeEvent mDisableWidthCertainRangeEvent = null;

        [Separator("Runtime Variables (JCS_TowardTarget)")]

        [Tooltip("Reverse the particle direction?")]
        [SerializeField]
        private bool mReverseDirection = false;

        [Tooltip("Move toward this target.")]
        [SerializeField]
        private JCS_UnityObject mTarget = null;

        [Tooltip("Randomly move particle to a certain range.")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRange = 10.0f;

        [Tooltip("Value adjust the range value, so it will make it more random.")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mAdjustRange = 0.0f;

        [Tooltip("Is a 3D particle?")]
        [SerializeField]
        private bool mIncludeDepth = false;

        /* Setter & Getter */

        public bool ReverseDirection { get { return this.mReverseDirection; } set { this.mReverseDirection = value; } }
        public void SetTarget(JCS_UnityObject trans)
        {
            // update target position.
            this.mTarget = trans;
        }
        public float Range { get { return this.mRange; } set { this.mRange = value; } }
        public float AdjustRange { get { return this.mAdjustRange; } set { this.mAdjustRange = value; } }
        public bool IncludeDepth { get { return this.mIncludeDepth; } set { this.mIncludeDepth = value; } }

        /* Functions */

        private void Awake()
        {
            this.mTweener = this.GetComponent<JCS_TransformTweener>();
            this.mDisableWidthCertainRangeEvent = this.GetComponent<JCS_DisableWithCertainRangeEvent>();

            // set destination callback.
            mTweener.onDone = DestinationCallback;
        }

        private void OnEnable()
        {
            if (mTarget == null)
            {
                Debug.LogError("Can't set calculated circle position with null target transform");
                return;
            }

            // on enable set the random position
            // with in the circle range.

            // get the position.
            Vector3 newPos = CalculateCirclePosition();

            // set to the position.
            if (mReverseDirection)
            {
                // set the target transform.
                this.mDisableWidthCertainRangeEvent.SetTarget(null);
                this.mDisableWidthCertainRangeEvent.TargetPosition = newPos;

                // starting position.
                SetPosition(this.mTarget.transform.position);
            }
            else
            {
                // set the target transform.
                this.mTweener.SetTarget(this.mTarget);
                this.mDisableWidthCertainRangeEvent.SetTarget(this.mTarget);

                // starting position.
                SetPosition(newPos);
            }

            mTweener.UpdateUnityData();

            // reset alpha change.
            mTweener.LocalAlpha = 1.0f;

            // enable the sprite renderer component.
            mTweener.LocalEnabled = true;

            // reset tweener
            mTweener.ResetTweener();

            // update the unity data first.
            if (mReverseDirection)
            {
                /* 
                 * Reverse could only use DoTween, cannot 
                 * use DoTweenContinue. 
                 */
                mTweener.DoTween(newPos);
            }
            else
                mTweener.DoTweenContinue(this.mTarget);
        }

        /// <summary>
        /// Get the random position within the 
        /// certain range. (Circle)
        /// </summary>
        private Vector3 CalculateCirclePosition()
        {
            return CalculateCirclePosition(
                mTarget.transform.position,
                mRange,
                mAdjustRange);
        }

        /// <summary>
        /// Get the random position within the 
        /// certain range. (Circle)
        /// </summary>
        /// <param name="targetPos"> target position </param>
        /// <param name="range"> radius of circle </param>
        /// <param name="adjRange"> adjustable value? </param>
        /// <returns></returns>
        private Vector3 CalculateCirclePosition(Vector3 targetPos, float range, float adjRange = 0)
        {
            Vector3 newPos = mTarget.transform.position;

            // set up the unknown angle
            // ÀH¾÷"¤º¨¤"
            float angle = JCS_Random.Range(0, 360);

            // define offset
            float hypOffset = JCS_Random.Range(
                    -adjRange,
                    adjRange);

            // add offset to current distance (hyp)
            float hyp = range + hypOffset;

            float opp = Mathf.Sin(angle) * hyp;

            float adj = JCS_Mathf.PythagoreanTheorem(hyp, opp, JCS_Mathf.TriSides.adj);


            bool flipX = JCS_Mathf.IsPossible(50);
            if (flipX)
                newPos.x -= adj;
            else
                newPos.x += adj;


            bool flipY = JCS_Mathf.IsPossible(50);
            if (flipY)
                newPos.y -= opp;
            else
                newPos.y += opp;


            if (mIncludeDepth)
            {
                bool flipZ = JCS_Mathf.IsPossible(50);

                if (flipZ)
                    newPos.z -= opp;
                else
                    newPos.z += opp;
            }

            return newPos;
        }

        /// <summary>
        /// Callback when reach the destination.
        /// </summary>
        private void DestinationCallback()
        {
            this.gameObject.SetActive(false);
        }
    }
}
