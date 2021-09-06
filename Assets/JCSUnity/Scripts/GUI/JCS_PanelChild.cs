/**
 * $File: JCS_PanelChild.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// This do the same thing as JCS_PanelRoot, but instead of all the child
    /// have to check JCS_PanelRoot is vague.The solution from this, we decide
    /// to have another component by name it differently and loop through the
    /// component and check if the panel has the correct proportion and
    /// scaling. Notice this class already been set by other same component,
    /// this will not be active.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_PanelChild : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        [Header("** Check Variables (JCS_PanelChild) **")]

        [Tooltip("Panel root object cache.")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Tooltip("Is this component the Unity defined UI component?")]
        [SerializeField]
        private bool mIsUnityDefinedUI = false;

        /* Setter & Getter */

        public JCS_PanelRoot PanelRoot { get { return this.mPanelRoot; } set { this.mPanelRoot = value; } }

        /* Functions */

        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            if (mPanelRoot == null)
                mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();

            this.mIsUnityDefinedUI = JCS_GUIUtil.IsUnityDefinedUI(this);

            // Rely on "Script Execution Order"
            {
                /* Check 'jpr' null for spawn GUI objects. */
                if (mPanelRoot != null)
                {
                    FitPerfectSize(
                        mPanelRoot.PanelDeltaWidthRatio,
                        mPanelRoot.PanelDeltaHeightRatio);
                }

                if (!mIsUnityDefinedUI || IsResponsive())
                {
                    // since we add this script assuming we are  int the fit
                    // perfect size mode
                    //
                    // see "JCS_PanelRoot" -> mFitScreenSize variables
                    AddPanelChild();
                }
            }
        }

        /// <summary>
        /// Fit screen size base on Unity Engine architecture.
        /// </summary>
        /// <param name="xRatio"></param>
        /// <param name="yRatio"></param>
        public void FitPerfectSize(float xRatio, float yRatio)
        {
            /* Do the scale. */
            {
                List<RectTransform> childs = null;
                if (!mIsUnityDefinedUI || IsResponsive())
                {
                    // NOTE: If not the Unity define UI, we need to  dettach all
                    // the child transform before we can resize it. If we resize
                    // it without dettach all child transforms, the children
                    // transform will also be scaled/changed.
                    // 
                    // 這個有點暴力解法... 不知道為什麼Unity沒有辦法
                    // 在初始化階段一次清乾淨.
                    childs = JCS_Utility.ForceDetachChildren(this.mRectTransform);
                }

                Vector3 newScale = mRectTransform.localScale;

                if (IsResponsive())
                {
                    float minRatio = Mathf.Min(xRatio, yRatio);
                    newScale.x *= minRatio;
                    newScale.y *= minRatio;
                }
                else
                {
                    newScale.x *= xRatio;
                    newScale.y *= yRatio;
                }

                mRectTransform.localScale = newScale;

                if (!mIsUnityDefinedUI || IsResponsive())
                {
                    // NOTE: Reattach all the previous child.
                    JCS_Utility.AttachChildren(this.mRectTransform, childs);
                }
            }

            /* Do the position. */
            {
                Vector3 newPosition = mRectTransform.localPosition;
                newPosition.x *= xRatio;
                newPosition.y *= yRatio;

                // set to the new position
                mRectTransform.localPosition = newPosition;
            }
        }

        /// <summary>
        /// Add all child with same effect.
        /// </summary>
        private void AddPanelChild()
        {
            Transform tempTrans = this.transform;

            for (int index = 0; index < transform.childCount; ++index)
            {
                Transform child = tempTrans.GetChild(index);

                // Only added once.
                if (child.GetComponent<JCS_PanelChild>() != null)
                    continue;

                var panelChild = child.gameObject.AddComponent<JCS_PanelChild>();
                panelChild.PanelRoot = mPanelRoot;
            }
        }

        /// <summary>
        /// Wrapper for function `JCS_ScreenSettings.instance.IsResponsive`.
        /// </summary>
        private bool IsResponsive()
        {
            var screenS = JCS_ScreenSettings.instance;
            return screenS.IsResponsive();
        }
    }
}
