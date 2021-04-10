/**
 * $File: JCS_ButtonPointer.cs $
 * $Date: 2017-05-29 06:26:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// GUI object that will goes toward a button, so this make the 
    /// player knows where is the current gui select option goes.
    /// 
    /// Like super mash brother selecting the character.
    /// </summary>
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    public class JCS_ButtonPointer
        : MonoBehaviour
    {
        /* Variables */

        private JCS_SimpleTrackAction mSimpleTrackAction = null;

        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Initialize Variables (JCS_ButtonPointer) **")]

        [Tooltip("Button that this pointer could point to.")]
        [SerializeField]
        private JCS_Button[] mButtons = null;

        [Header("** Runtime Variables (JCS_ButtonPointer) **")]

        [Tooltip("Do we point to the button that are not active?")]
        [SerializeField]
        private bool mDontPointIfButtonNotActive = false;

        [Tooltip("Position offset for the pointer.")]
        [SerializeField]
        private Vector3 mPointerOffset = Vector3.zero;

        /* Setter & Getter */

        public bool DontPointIfButtonNotActive { get { return this.mDontPointIfButtonNotActive; } set { this.mDontPointIfButtonNotActive = value; } }
        public Vector3 PointerOffset { get { return this.mPointerOffset; } set { this.mPointerOffset = value; } }

        /* Functions */

        private void Awake()
        {
            this.mSimpleTrackAction = this.GetComponent<JCS_SimpleTrackAction>();

            // start at the current position. 
            // so trick make the trackaction not moving at all at the 
            // beginning of the action.
            this.mSimpleTrackAction.TargetPosition = this.transform.localPosition;

            for (int index = 0; index < mButtons.Length; ++index)
            {
                if (mButtons[index] == null)
                    continue;

                mButtons[index].SetSystemCallback(PointToButton);
            }
        }

        private void Start()
        {
            this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();

            if (mPanelRoot != null)
            {
                // Adjust the offset base on the screen size.
                mPointerOffset.x /= mPanelRoot.PanelDeltaWidthRatio;
                mPointerOffset.y /= mPanelRoot.PanelDeltaHeightRatio;
            }
        }

        /// <summary>
        /// Callback if the button get focus.
        /// </summary>
        private void PointToButton(JCS_Button btn)
        {
            if (mDontPointIfButtonNotActive)
            {
                if (!btn.Interactable)
                    return;
            }

            Vector3 targetPoint = 
                btn.GetRectTransfom().localPosition +  // Target position. 
                mPointerOffset;  // Offset value.

            mSimpleTrackAction.TargetPosition = targetPoint;
        }
    }
}
