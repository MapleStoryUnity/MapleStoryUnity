/**
 * $File: JCS_Canvas.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
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

        // Execution when canvas is shown.
        public Action<JCS_Canvas> onShow = null;
        // Execution when canvas is hidden.
        public Action<JCS_Canvas> onHide = null;

        // Execution when canvas is shown by fading.
        public Action<JCS_Canvas> onShowFade = null;
        // Execution when canvas is hidden by fading.
        public Action<JCS_Canvas> onHideFade = null;

        private RectTransform mAppRect = null;  // Application Rect (Window)

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_Canvas)")]

        [Tooltip("Turn on this to test this behaviour.")]
        [SerializeField]
        private bool mTest = false;

        [Tooltip("Key to show canvas.")]
        [SerializeField]
        private KeyCode mKeyShow = KeyCode.A;

        [Tooltip("Key to hide canvas.")]
        [SerializeField]
        private KeyCode mKeyHide = KeyCode.S;
#endif

        [Separator("Check Variables (JCS_Canvas)")]

        [Tooltip("Canvas.")]
        [SerializeField]
        [ReadOnly]
        private Canvas mCanvas = null;

        [Tooltip("Canvas group.")]
        [SerializeField]
        [ReadOnly]
        private CanvasGroup mCanvasGroup = null;

        [Tooltip("Target fading alpha.")]
        [SerializeField]
        [ReadOnly]
        private float mFadeAlpa = 0.0f;

        [Tooltip("Type of fading currently used.")]
        [SerializeField]
        [ReadOnly]
        private JCS_FadeType mFading = JCS_FadeType.NONE;

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

        [Tooltip("Turn on when you want to fade the canvas by default.")]
        [SerializeField]
        private bool mFade = false;

        [Tooltip("How fast the canvas fades.")]
        [SerializeField]
        [Range(0.0001f, 30.0f)]
        private float mFadeFriction = 0.15f;

        [Tooltip("Full fade in amount.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeInAmount = 1.0f;

        [Tooltip("Full fade out amount.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeOutAmount = 0.0f;

        [Tooltip("The time type.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.UNSCALED_DELTA_TIME;

        [Tooltip("Play sound when active the canvas.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Play sound when deactive the canvas.")]
        [SerializeField]
        private AudioClip mDeactiveSound = null;

        /* Setter & Getter */

        public RectTransform AppRect { get { return this.mAppRect; } }
        public Canvas canvas { get { return this.mCanvas; } }
        public CanvasGroup canvasGroup { get { return this.mCanvasGroup; } }
        public JCS_ResizeUI ResizeUI { get { return this.mResizeUI; } }

        public bool DisplayOnAwake { get { return this.mDisplayOnAwake; } }
        public bool MainCanvas { get { return this.mMainCanvas; } }

        public bool Fade { get { return this.mFade; } set { this.mFade = value; } }
        public float FadeFriction { get { return this.mFadeFriction; } set { this.mFadeFriction = value; } }
        public float FadeInAmount { get { return this.mFadeInAmount; } set { this.mFadeInAmount = value; } }
        public float FadeOutAmount { get { return this.mFadeOutAmount; } set { this.mFadeOutAmount = value; } }
        public JCS_TimeType TimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }
        public AudioClip ActiveSound { get { return this.mActiveSound; } set { this.mActiveSound = value; } }
        public AudioClip DeactiveSound { get { return this.mDeactiveSound; } set { this.mDeactiveSound = value; } }

        /* Functions */

        private void Awake()
        {
            CheckMainCanvas();

            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();
            this.mCanvasGroup = this.GetComponent<CanvasGroup>();

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
            var uis = JCS_UISettings.instance;
            var screens = JCS_ScreenSettings.instance;

            if (uis.RESIZE_UI && !screens.IsNone())
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

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            DoFading();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTest)
                return;

            if (Input.GetKeyDown(mKeyShow))
                Show();
            else if (Input.GetKeyDown(mKeyHide))
                Hide();
        }
#endif

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
                Debug.LogError("Attach resize canvas exception: " + com);
            else
                com.transform.SetParent(newParent);

            // We will expect COM to be one of the UI component from built-in 
            // Unity. If this is true, we resize it's component to match
            // the current screen space.
            var rect = com.GetComponent<RectTransform>();
            FitScreenSize(rect);
        }

        /// <summary>
        /// Return true if the canvas is currently visible.
        /// </summary>
        public bool IsShown()
        {
            return mCanvas.enabled;
        }

        /// <summary>
        /// Show the canvas so it's visible.
        /// </summary>
        public void Show(bool mute = false)
        {
            Show(mFade, mute);
        }
        public void Show(bool fade, bool mute = false)
        {
            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mDeactiveSound, JCS_SoundMethod.PLAY_SOUND);

            mCanvas.enabled = true;

            if (fade)
            {
                mFading = JCS_FadeType.IN;

                mFadeAlpa = mFadeInAmount;
            }
            else
            {
                if (mCanvasGroup != null)
                    mCanvasGroup.alpha = mFadeInAmount;
            }

            onShow?.Invoke(this);
        }

        /// <summary>
        /// Hide the canvas so it's invisible.
        /// </summary>
        public void Hide(bool mute = false)
        {
            Hide(mFade, mute);
        }
        public void Hide(bool fade, bool mute = false)
        {
            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mActiveSound, JCS_SoundMethod.PLAY_SOUND);

            if (fade)
            {
                // Remains enabled since we're going to do fading.
                mCanvas.enabled = true;

                mFading = JCS_FadeType.OUT;

                mFadeAlpa = mFadeOutAmount;
            }
            else
            {
                mCanvas.enabled = false;

                if (mCanvasGroup != null)
                    mCanvasGroup.alpha = mFadeOutAmount;
            }

            onHide?.Invoke(this);
        }

        /// <summary>
        /// Toggle the canvas' visibility.
        /// </summary>
        /// <param name="mute"> True to mute the sound. </param>
        /// 
        public void ToggleVisibility(bool mute = false)
        {
            ToggleVisibility(mFade, mute);
        }
        public void ToggleVisibility(bool fade, bool mute = false)
        {
            if (IsShown())
                Hide(fade, mute);
            else
                Show(fade, mute);
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
                Debug.LogWarning("Having multiple main canvases is often not allowed: " + this.gameObject.name);
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
            Debug.LogWarning("No main canvas is detected");
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

        /// <summary>
        /// Do the fading effect.
        /// </summary>
        private void DoFading()
        {
            if (mFading == JCS_FadeType.NONE)
                return;

            if (mCanvasGroup == null)
            {
                Debug.Log($"Fade missing the canvas group: {name}");
                return;
            }

            float direction = mFadeAlpa - mCanvasGroup.alpha;

            mCanvasGroup.alpha += direction / mFadeFriction * JCS_Time.ItTime(mTimeType);

            float diff = Mathf.Abs(direction);

            // When close enough.
            if (diff < JCS_Constants.NEAR_THRESHOLD)
            {
                switch (mFading)
                {
                    case JCS_FadeType.IN:
                        {
                            mCanvasGroup.alpha = mFadeInAmount;

                            onShowFade?.Invoke(this);
                        }
                        break;
                    case JCS_FadeType.OUT:
                        {
                            mCanvasGroup.alpha = mFadeOutAmount;

                            mCanvas.enabled = false;

                            onHideFade?.Invoke(this);
                        }
                        break;
                }

                mFadeAlpa = mCanvasGroup.alpha;

                mFading = JCS_FadeType.NONE;
            }
        }
    }
}
