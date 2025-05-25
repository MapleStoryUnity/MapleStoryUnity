/**
 * $File: JCS_CanvasComp.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2024 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Component that requires the component `JCS_Canvas` without
    /// actually inherit the class `JCS_Canvas`.
    /// </summary>
    [RequireComponent(typeof(JCS_Canvas))]
    public class JCS_CanvasComp<T> : MonoBehaviour
    {
        /* Variables */

        // The canvas this component to control.
        protected JCS_Canvas mCanvas = null;

        /* Setter & Getter */

        public JCS_Canvas canvas { get { return this.mCanvas; } }

        public Action<JCS_Canvas> onShow
        {
            get => mCanvas.onShow;
            set => mCanvas.onShow = value;
        }
        public Action<JCS_Canvas> onHide
        {
            get => mCanvas.onHide;
            set => mCanvas.onHide = value;
        }
        public Action<JCS_Canvas> onShowFade
        {
            get => mCanvas.onShowFade;
            set => mCanvas.onShowFade = value;
        }
        public Action<JCS_Canvas> onHideFade
        {
            get => mCanvas.onHideFade;
            set => mCanvas.onHideFade = value;
        }

        /* Functions */

        protected virtual void Awake()
        {
            this.mCanvas = this.GetComponent<JCS_Canvas>();
        }

        public virtual bool IsShown() => mCanvas.IsShown();
        public virtual void Show(bool mute = false) => mCanvas.Show(mute);
        public virtual void Show(bool fade, bool mute = false) => mCanvas.Show(fade, mute);
        public virtual void Hide(bool mute = false) => mCanvas.Hide(mute);
        public virtual void Hide(bool fade, bool mute = false) => mCanvas.Hide(fade, mute);
        public virtual void ToggleVisibility(bool mute = false) => mCanvas.ToggleVisibility(mute);
        public virtual void ToggleVisibility(bool fade, bool mute = false) => mCanvas.ToggleVisibility(fade, mute);
    }
}
