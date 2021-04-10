/**
 * $File: JCS_Canvas.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Default canvas class for JCSUnity.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class JCS_Canvas
        : MonoBehaviour
    {
        /* Variables */

        public static JCS_Canvas instance = null;

        private static string RESIZE_UI_PATH = "JCSUnity_Resources/LevelDesignUI/ResizeUI";

        [Header("** Check Variables (JCS_Canvas) **")]

        [Tooltip("Canvas object.")]
        [SerializeField]
        private Canvas mCanvas = null;

        [Tooltip("Resize UI object.")]
        [SerializeField]
        private JCS_ResizeUI mResizeUI = null;

        // Application Rect (Window)
        private RectTransform mAppRect = null;

        /* Setter & Getter */

        public RectTransform GetAppRect() { return this.mAppRect; }
        public Canvas GetCanvas() { return this.mCanvas; }
        public void SetResizeUI(JCS_ResizeUI ui) { this.mResizeUI = ui; }
        public JCS_ResizeUI GetResizeUI() { return this.mResizeUI; }

        /* Functions */

        private void Awake()
        {
            if (instance != null)
            {
                string black_screen_name = JCS_UISettings.BLACK_SCREEN_NAME;
                string white_screen_name = JCS_UISettings.WHITE_SCREEN_NAME;

                // cuz the transform list will change while we set the transform to
                // the transform,
                List<Transform> readyToSetList = new List<Transform>();

                Transform tempTrans = instance.transform;
                // so record all the transform
                for (int index = 0; index < tempTrans.childCount; ++index)
                {
                    Transform child = tempTrans.GetChild(index);
                    if (child.name == black_screen_name ||
                        child.name == (black_screen_name + "(Clone)"))
                        continue;

                    if (child.name == white_screen_name ||
                        child.name == (white_screen_name + "(Clone)"))
                        continue;

                    if (child.name == "JCS_IgnorePanel")
                        continue;

                    // TODO(JenChieh): optimize this?
                    if (child.GetComponent<JCS_IgnoreDialogueObject>() != null)
                        continue;

                    // add to set list ready to set to the new transform as parent
                    readyToSetList.Add(child);
                }

                // set to the new transform
                foreach (Transform trans in readyToSetList)
                {
                    // set parent to the new canvas in the new scene
                    trans.SetParent(this.transform);
                }

                // Delete the old one
                DestroyImmediate(instance.gameObject);
            }


            // attach the new one
            instance = this;

            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();

            if (JCS_UISettings.instance.RESIZE_UI)
            {
                // resizable UI in order to resize the UI correctly
                JCS_ResizeUI rui = JCS_Utility.SpawnGameObject(RESIZE_UI_PATH).GetComponent<JCS_ResizeUI>();
                rui.transform.SetParent(this.transform);
            }
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
        /// Add component to resize canvas.
        /// </summary>
        /// <param name="com"> Component add to canvas. </param>
        public void AddComponentToResizeCanvas(Component com)
        {
            Transform newParent = (mResizeUI != null) ? this.mResizeUI.transform : this.mCanvas.transform;
            if (newParent == null)
                JCS_Debug.LogError("Attach resize canvas exception: " + com);
            else
                com.transform.SetParent(newParent);

            // We will expect COM to be one of the UI component from built-in 
            // Unity. If this is true, we resize it's component to match
            // the current screen space.
            RectTransform rect = com.GetComponent<RectTransform>();
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

            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;
            JCS_ScreenSizef screenRaio = ss.ScreenRatio();

            Vector3 newScale = rect.localScale;
            newScale.x /= screenRaio.width;
            newScale.y /= screenRaio.height;
            rect.localScale = newScale;
        }
    }
}
