/**
 * $File: JCS_TweenerHandler.cs $
 * $Date: 2020-04-06 14:48:12 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Handles multiple tweener.
    /// </summary>
    public class JCS_TweenerHandler : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_TweenPanel) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Runtime Variables (JCS_TweenerHandler) **")]

        [Tooltip("List of tweener informations.")]
        [SerializeField]
        private List<JCS_TweenInfo> mTweenInfos = null;

        /* Setter & Getter */

        public List<JCS_TweenInfo> TweenInfos { get { return this.mTweenInfos; } }

        /* Functions */

        private void Start()
        {
            // NOTE: Record down all the starting values.
            for (int index = 0; index < mTweenInfos.Count; ++index)
            {
                JCS_TweenInfo ti = mTweenInfos[index];
                switch (ti.transformTweener.TweenType)
                {
                    case JCS_TransformType.POSITION:
                        ti.startingValue = ti.transformTweener.LocalPosition;
                        break;
                    case JCS_TransformType.ROTATION:
                        ti.startingValue = ti.transformTweener.LocalEulerAngles;
                        break;
                    case JCS_TransformType.SCALE:
                        ti.startingValue = ti.transformTweener.LocalScale;
                        break;
                }
            }

            // NOTE: Make compatible to resizable screen.
            {
                this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
                if (mPanelRoot != null && this.mPanelRoot.transform == this.transform.parent)
                {
                    for (int index = 0; index < mTweenInfos.Count; ++index)
                    {
                        JCS_TweenInfo tt = mTweenInfos[index];
                        tt.targetValue.x *= mPanelRoot.PanelDeltaWidthRatio;
                        tt.targetValue.y *= mPanelRoot.PanelDeltaHeightRatio;
                    }
                }
            }
        }

        /// <summary>
        /// Check if done tweeening for all tweeners.
        /// </summary>
        /// <returns>
        /// Return true, when is done with tweening.
        /// Return false, when is NOT done with tweening.
        /// </returns>
        public bool IsAllDoneTweening()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
            {
                if (!ti.transformTweener.IsDoneTweening)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Make all transform tweeners tweene to the start value.
        /// </summary>
        public void DoAllTweenToStartValue()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
                ti.transformTweener.DoTween(ti.startingValue);
        }

        /// <summary>
        /// Make all transform tweeners tweene to the target value.
        /// </summary>
        public void DoAllTweenToTargetValue()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
                ti.transformTweener.DoTween(ti.targetValue);
        }
    }
}
