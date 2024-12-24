/**
 * $File: JCS_Canvas.cs $
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
    /// Control of the canvas component.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class JCS_Canvas : MonoBehaviour
    {
        /* Variables */

        public static JCS_Canvas main = null;

        private const string RESIZE_UI_PATH = "UI/ResizeUI";

        private Canvas mCanvas = null;

        private RectTransform mAppRect = null;  // Application Rect (Window)

        [Separator("Check Variables (JCS_Canvas)")]

        [Tooltip("Resize object.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizeUI mResizeUI = null;

        [Separator("Initialize Variables (JCS_Canvas)")]

        [Tooltip("If true, show on awake time; otherwise, hide it.")]
        [SerializeField]
        private bool mDisplayOnAwake = true;

        [Tooltip("Resizable screen will be attach to this canvas.")]
        [SerializeField]
        private bool mMainCanvas = true;

        [Separator("Runtime Variables (JCS_Canvas)")]

        [Tooltip("Play sound when active the canvas.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Play sound when deactive the canvas.")]
        [SerializeField]
        private AudioClip mDeactiveSound = null;

        /* Setter & Getter */

        public RectTransform AppRect { get { return this.mAppRect; } }
        public Canvas canvas { get { return this.mCanvas; } }
        public JCS_ResizeUI ResizeUI { get { return this.mResizeUI; } }

        public bool DisplayOnAwake { get { return this.mDisplayOnAwake; } }
        public bool MainCanvas { get { return this.mMainCanvas; } }

        public AudioClip ActiveSound { get { return this.mActiveSound; } set { this.mActiveSound = value; } }
        public AudioClip DeactiveSound { get { return this.mDeactiveSound; } set { this.mDeactiveSound = value; } }

        /* Functions */

        private void Awake()
        {
            CheckMainCanvas();

            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();

            if (JCS_UISettings.instance.RESIZE_UI && !JCS_ScreenSettings.instance.IsNone())
            {
                // resizable UI in order to resize the UI correctly
                mResizeUI = JCS_Util.Instantiate(RESIZE_UI_PATH).GetComponent<JCS_ResizeUI>();
                mResizeUI.transform.SetParent(this.transform);
            }

            JCS_UIManager.instance.AddCanvas(this);

            if (mDisplayOnAwake)
                Show();
            else
                Hide();
        }

        private void Start()
        {
            if (JCS_UISettings.instance.RESIZE_UI && !JCS_ScreenSettings.instance.IsNone())
            {
                if (mResizeUI == null)
                    return;

                // get the screen width and height
                Vector2 actualRect = this.AppRect.sizeDelta;

                // set it to the right resolution
                mResizeUI.GetResizeRect().sizeDelta = actualRect;
            }

            NoMainCanvas();
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

            return main;
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
        /// Show the canvas so it's visible.
        /// </summary>
        public void Show(bool mute = false)
        {
            mCanvas.enabled = true;
            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mDeactiveSound, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Hide the canvas so it's invisible.
        /// </summary>
        public void Hide(bool mute = false)
        {
            mCanvas.enabled = false;
            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mActiveSound, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Toggle the canvas' visibility.
        /// </summary>
        /// <param name="mute"> True to mute the sound. </param>
        public void ToggleVisibility(bool mute = false)
        {
            if (mCanvas.enabled)
                Hide(mute);
            else
                Show(mute);
        }

        /// <summary>
        /// Prompt warning if there are multiple main canvases in the scene.
        /// </summary>
        private void CheckMainCanvas()
        {
            if (!this.mMainCanvas)
                return;

            if (main != null)
            {
                JCS_Debug.LogWarning("Having multiple main canvases is often not allowed, " + this.gameObject.name);
                return;
            }

            main = this;
        }

        /// <summary>
        /// Prompt a warning if no main canvas is defined in the scene.
        /// </summary>
        private void NoMainCanvas()
        {
            if (main) return;
            JCS_Debug.LogWarning("No main canvas is detected");
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
