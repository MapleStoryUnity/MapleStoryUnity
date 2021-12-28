/**
 * $File: JCS_InputManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Take care of the cross platform input.
    /// </summary>
    public class JCS_InputManager : JCS_Manager<JCS_InputManager>
    {
        /* Variables */

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
    }
}
