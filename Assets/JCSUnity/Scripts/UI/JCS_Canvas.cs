/**
 * $File: JCS_Canvas.cs $
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
    /// Default canvas class for JCSUnity.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class JCS_Canvas : MonoBehaviour
    {
        /* Variables */

        private const string RESIZE_UI_PATH = "LevelDesignUI/ResizeUI";

        [Header("** Check Variables (JCS_Canvas) **")]

        [Tooltip("Canvas object.")]
        [SerializeField]
        private Canvas mCanvas = null;

        [Tooltip("Resize object.")]
        [SerializeField]
        private JCS_ResizeUI mResizeUI = null;

        // Application Rect (Window)
        private RectTransform mAppRect = null;

        /* Setter & Getter */

        public RectTransform GetAppRect() { return this.mAppRect; }
        public Canvas GetCanvas() { return this.mCanvas; }
        public JCS_ResizeUI ResizeUI { get { return this.mResizeUI; } }

        /* Functions */

        private void Awake()
        {
            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();

            if (JCS_UISettings.instance.RESIZE_UI)
            {
                // resizable UI in order to resize the UI correctly
                mResizeUI = JCS_Util.SpawnGameObject(RESIZE_UI_PATH).GetComponent<JCS_ResizeUI>();
                mResizeUI.transform.SetParent(this.transform);
            }

            JCS_UIManager.instance.AddCanvas(this);
        }

        private void Start()
        {
            if (JCS_UISettings.instance.RESIZE_UI)
            {
                if (mResizeUI == null)
                    return;

                // get the screen width and height
                Vector2 actualRect = this.GetAppRect().sizeDelta;

                // set it to the right resolution
                mResizeUI.GetResizeRect().sizeDelta = actualRect;
            }
        }

        /// <summary>
        /// Return the `canvas` that is the parent of the `trans` object.
        /// 
        /// If `trans` is not relate to any canvas object, we return
        /// the upmost canvas object by default.
        /// </summary>
        public static JCS_Canvas GuessCanvas(Transform trans = null)
        {
            if (trans != null)
            {
                var canvas = trans.GetComponentInParent<JCS_Canvas>();
                if (canvas != null)
                    return canvas;
            }

            // We return the upperest canvas on the screen.
            List<JCS_Canvas> canvases = JCS_UIManager.instance.Canvases;
            return canvases[canvases.Count - 1];
        }

        /// <summary>
        /// Add component to resize canvas.
        /// </summary>
        /// <param name="com"> Component add to canvas. </param>
        public void AddComponentToResizeCanvas(Component com)
        {
            Transform newParent = (mResizeUI != null) ? mResizeUI.transform : this.mCanvas.transform;
            if (newParent == null)
                JCS_Debug.LogError("Attach resize canvas exception: " + com);
            else
                com.transform.SetParent(newParent);

            // We will expect COM to be one of the UI component from built-in 
            // Unity. If this is true, we resize it's component to match
            // the current screen space.
            var rect = com.GetComponent<RectTransform>();
            FitScreenSize(rect);
        }

        /// <summary>
        /// Fit RECT into our screen space.
        /// </summary>
        /// <param name="rect"> The `RectTransform` object to resize fro screen. </param>
        private void FitScreenSize(RectTransform rect)
        {
            if (rect == null)
                return;

            var ss = JCS_ScreenSettings.instance;
            JCS_ScreenSizef screenRaio = ss.ScreenRatio();

            Vector3 newScale = rect.localScale;
            newScale.x /= screenRaio.width;
            newScale.y /= screenRaio.height;
            rect.localScale = newScale;
        }
    }
}
