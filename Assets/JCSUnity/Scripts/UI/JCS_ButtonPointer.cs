/**
 * $File: JCS_ButtonPointer.cs $
 * $Date: 2017-05-29 06:26:00 $
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
    /// GUI object that will goes toward a button, so this make the 
    /// player knows where is the current gui select option goes.
    /// 
    /// Like super mash brother selecting the character.
    /// </summary>
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    public class JCS_ButtonPointer : MonoBehaviour
    {
        /* Variables */

        private JCS_SimpleTrackAction mSimpleTrackAction = null;

        private JCS_PanelRoot mPanelRoot = null;

        [Separator("🌱 Initialize Variables (JCS_ButtonPointer)")]

        [Tooltip("Button that this pointer could point to.")]
        [SerializeField]
        private JCS_Button[] mButtons = null;

        [Separator("⚡️ Runtime Variables (JCS_ButtonPointer)")]

        [Tooltip("Do we point to the button that are not active?")]
        [SerializeField]
        private bool mDontPointIfButtonNotActive = false;

        [Tooltip("Position offset for the pointer.")]
        [SerializeField]
        private Vector3 mPointerOffset = Vector3.zero;

        /* Setter & Getter */

        public bool DontPointIfButtonNotActive { get { return mDontPointIfButtonNotActive; } set { mDontPointIfButtonNotActive = value; } }
        public Vector3 PointerOffset { get { return mPointerOffset; } set { mPointerOffset = value; } }

        /* Functions */

        private void Awake()
        {
            mSimpleTrackAction = GetComponent<JCS_SimpleTrackAction>();

            // start at the current position. 
            // so trick make the trackaction not moving at all at the 
            // beginning of the action.
            mSimpleTrackAction.targetPosition = transform.localPosition;

            for (int index = 0; index < mButtons.Length; ++index)
            {
                if (mButtons[index] == null)
                    continue;

                mButtons[index].SetSystemCallback(PointToButton);
            }
        }

        private void Start()
        {
            mPanelRoot = JCS_PanelRoot.GetFromParent(transform);

            if (mPanelRoot != null)
            {
                // Adjust the offset base on the screen size.
                mPointerOffset.x *= mPanelRoot.panelDeltaWidthRatio;
                mPointerOffset.y *= mPanelRoot.panelDeltaHeightRatio;
            }
        }

        /// <summary>
        /// Callback if the button get focus.
        /// </summary>
        private void PointToButton(JCS_Button btn)
        {
            if (mDontPointIfButtonNotActive)
            {
                if (!btn.interactable)
                    return;
            }

            Vector3 targetPoint = 
                btn.GetRectTransfom().localPosition +  // Target position. 
                mPointerOffset;  // Offset value.

            mSimpleTrackAction.targetPosition = targetPoint;
        }
    }
}
