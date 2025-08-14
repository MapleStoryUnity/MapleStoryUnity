/**
 * $File: JCS_InputManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Take care of the cross platform input.
    /// </summary>
    public class JCS_InputManager : JCS_Manager<JCS_InputManager>
    {
        /* Variables */

        [Separator("Runtime Varaibles (JCS_InputManager)")]

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

        public bool support_OnMouseEnter { get { return mSupport_OnMouseEnter; } }
        public bool support_OnMouseExit { get { return mSupport_OnMouseExit; } }
        public bool support_OnMouseDown { get { return mSupport_OnMouseDown; } }
        public bool support_OnMouseUp { get { return mSupport_OnMouseUp; } }
        public bool support_OnMouseOver { get { return mSupport_OnMouseOver; } }
        public bool support_OnMouseDrag { get { return mSupport_OnMouseDrag; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);
        }
    }
}
