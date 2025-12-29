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
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handles multiple tweener.
    /// </summary>
    public class JCS_TweenerHandler : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("🧪 Helper Variables (JCS_TweenerHandler)")]

        [Tooltip("Test component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to active tween to starting value.")]
        [SerializeField]
        private KeyCode mTweenToStart = KeyCode.J;

        [Tooltip("Key to active tween to target value.")]
        [SerializeField]
        private KeyCode mTweenToTarget = KeyCode.K;
#endif

        [Separator("📋 Check Variabless (JCS_TweenerHandler)")]

        [Tooltip("Optional panel root.")]
        [SerializeField]
        [ReadOnly]
        private JCS_PanelRoot mPanelRoot = null;

        [Separator("⚡️ Runtime Variables (JCS_TweenerHandler)")]

        [Tooltip("List of tweener informations.")]
        [SerializeField]
        private List<JCS_TweenInfo> mTweenInfos = null;

        /* Setter & Getter */

        public List<JCS_TweenInfo> tweenInfos { get { return mTweenInfos; } }

        /* Functions */

        private void Start()
        {
            // NOTE: Record down all the starting values.
            for (int index = 0; index < mTweenInfos.Count; ++index)
            {
                JCS_TweenInfo ti = mTweenInfos[index];

                RectTransform rt = ti.transformTweener.GetRectTransform();

                switch (ti.transformTweener.tweenType)
                {
                    /* Transform */
                    case JCS_TransformType.POSITION:
                        ti.startingValue = ti.transformTweener.localPosition;
                        break;
                    case JCS_TransformType.ROTATION:
                        ti.startingValue = ti.transformTweener.localEulerAngles;
                        break;
                    case JCS_TransformType.SCALE:
                        ti.startingValue = ti.transformTweener.localScale;
                        break;
                    /* RectTransform */
                    case JCS_TransformType.ANCHOR_MIN:
                        ti.startingValue = rt.anchorMin;
                        break;
                    case JCS_TransformType.ANCHOR_MAX:
                        ti.startingValue = rt.anchorMax;
                        break;
                    case JCS_TransformType.SIZE_DELTA:
                        ti.startingValue = rt.sizeDelta;
                        break;
                    case JCS_TransformType.PIVOT:
                        ti.startingValue = rt.pivot;
                        break;
                    case JCS_TransformType.ANCHOR_POSITION:
                        ti.startingValue = rt.anchoredPosition;
                        break;
                    case JCS_TransformType.ANCHOR_POSITION_3D:
                        ti.startingValue = rt.anchoredPosition3D;
                        break;
                    case JCS_TransformType.OFFSET_MIN:
                        ti.startingValue = rt.offsetMin;
                        break;
                    case JCS_TransformType.OFFSET_MAX:
                        ti.startingValue = rt.offsetMax;
                        break;
                }
            }

            // NOTE: Make compatible to resizable screen.
            {
                mPanelRoot = JCS_PanelRoot.GetFromParent(transform);

                if (mPanelRoot != null && mPanelRoot.transform == transform.parent)
                {
                    for (int index = 0; index < mTweenInfos.Count; ++index)
                    {
                        JCS_TweenInfo tt = mTweenInfos[index];
                        tt.targetValue.x *= mPanelRoot.panelDeltaWidthRatio;
                        tt.targetValue.y *= mPanelRoot.panelDeltaHeightRatio;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mTweenToStart))
                DoAllTweenToStartValue();

            if (Input.GetKeyDown(mTweenToTarget))
                DoAllTweenToTargetValue();
        }
#endif

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
                if (!ti.transformTweener.isDoneTweening)
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
