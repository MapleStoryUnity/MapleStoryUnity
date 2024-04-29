/**
 * $File: JCS_RandomTweenerAction.cs $
 * $Date: 2017-09-11 06:45:39 $
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
    /// Randomize the tweener in a certain time.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_RandomTweenerAction : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;
        private JCS_AdjustTimeTrigger mAdjustTimeTrigger = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_RandomTweenerAction)")]

        public Vector3 targetValue = Vector3.zero;
#endif

        [Separator("Runtime Variables (JCS_RandomTweenerAction)")]

        [Tooltip("Mininum vector value.")]
        [SerializeField]
        private Vector3 mMinVectorRange = Vector3.zero;

        [Tooltip("Maxinum vector value.")]
        [SerializeField]
        private Vector3 mMaxVectorRange = new Vector3(360, 360, 360);

        [Tooltip("Freeze in a certain vector? (x, y, z)")]
        [SerializeField]
        private JCS_Bool3 mFreeze = JCS_Bool3.allFalse;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mAdjustTimeTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            // set effect function pointer.
            this.mAdjustTimeTrigger.onAction = TargetNewVectorValue;
        }

        /// <summary>
        /// After a cetain time we target a new vector value to do
        /// tweener effect.
        /// </summary>
        private void TargetNewVectorValue()
        {
            // Do nothing if not done tweening.
            //if (!mTransformTweener.IsDoneTweening)
            //    return;

            Vector3 newVal = mTransformTweener.GetSelfTransformTypeVector3();

            // x
            if (!mFreeze.check1)
                newVal.x = JCS_Random.Range(mMinVectorRange.x, mMaxVectorRange.x);
            // y
            if (!mFreeze.check2)
                newVal.y = JCS_Random.Range(mMinVectorRange.y, mMaxVectorRange.y);
            // z
            if (!mFreeze.check3)
                newVal.z = JCS_Random.Range(mMinVectorRange.z, mMaxVectorRange.z);

            mTransformTweener.DoTween(newVal);

#if UNITY_EDITOR
            targetValue = newVal;
#endif
        }
    }
}
