/**
 * $File: JCS_InputManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Take care of the cross platform input.
    /// </summary>
    public class JCS_InputManager
        : JCS_Managers<JCS_InputManager>
    {
        /* Variables */

        [Header("** Check Varaibles (JCS_InputManager) **")]

        [Tooltip("Slide input receiver.")]
        [SerializeField]
        private JCS_SlideInput mSlideInput = null;

        [Tooltip("Handle mobile event component.")]
        [SerializeField]
        private JCS_MobileMouseEvent mMobileMouseEvent = null;

        [Header("** Runtime Varaibles (JCS_InputManager) **")]

        [Tooltip("Support OnMouseEnter event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseEnter = false;

        [Tooltip("Support OnMouseExit event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseExit = false;

        [Tooltip("Support OnMouseDown event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseDown = true;

        [Tooltip("Support OnMouseUp event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseUp = true;

        [Tooltip("Support OnMouseOver event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseOver = false;

        [Tooltip("Support OnMouseDrag event from mobile.")]
        [SerializeField]
        private bool mSupport_OnMouseDrag = false;

        /* Setter & Getter */

        public void SetGlobalSlideInput(JCS_SlideInput si)
        {
            if (mSlideInput != null)
            {
                JCS_Debug.LogWarning("You are trying to override an existing `slide input`");
                return;
            }
            this.mSlideInput = si;
        }
        public void SetGlobalMobileMouseEvent(JCS_MobileMouseEvent mme)
        {
            if (mMobileMouseEvent != null)
            {
                JCS_Debug.LogWarning("You are trying to override an existing `mobile mouse event`");
                return;
            }
            this.mMobileMouseEvent = mme;
        }
        public JCS_SlideInput GetGlobalSlideInput()
        {
#if UNITY_EDITOR
            if (JCS_GameSettings.instance.DEBUG_MODE)
            {
                JCS_Debug.LogWarning("You tried to access global slide input but doesn't exists");
                return null;
            }
#endif
            return this.mSlideInput;
        }
        public JCS_MobileMouseEvent GetGlobalMobileMouseEvent()
        {
#if UNITY_EDITOR
            if (JCS_GameSettings.instance.DEBUG_MODE)
            {
                JCS_Debug.LogWarning("You tried to access global mobile mouse event but doesn't exists");
                return null;
            }
#endif
            return this.mMobileMouseEvent;
        }

        public bool Support_OnMouseEnter { get { return this.mSupport_OnMouseEnter; } }
        public bool Support_OnMouseExit { get { return this.mSupport_OnMouseExit; } }
        public bool Support_OnMouseDown { get { return this.mSupport_OnMouseDown; } }
        public bool Support_OnMouseUp { get { return this.mSupport_OnMouseUp; } }
        public bool Support_OnMouseOver { get { return this.mSupport_OnMouseOver; } }
        public bool Support_OnMouseDrag { get { return this.mSupport_OnMouseDrag; } }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            AddInputBaseOnPlatform();
        }

        /// <summary>
        /// Add the input type base on the platform type.
        /// </summary>
        private void AddInputBaseOnPlatform()
        {
            switch (JCS_ApplicationManager.instance.PlatformType)
            {
                case JCS_PlatformType.MOBILE:
                    {
                        if (this.mSlideInput == null)
                            this.mSlideInput = this.gameObject.AddComponent<JCS_SlideInput>();

                        if (this.mMobileMouseEvent == null)
                            this.mMobileMouseEvent = this.gameObject.AddComponent<JCS_MobileMouseEvent>();
                    }
                    break;
            }
        }
    }
}
