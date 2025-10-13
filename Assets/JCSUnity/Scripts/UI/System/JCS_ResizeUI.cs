/**
 * $File: JCS_ResizeUI.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// This responsible handle all the gui into correct proportion and 
    /// right scaling.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_ResizeUI : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRect = null;

#if UNITY_EDITOR
        private Image mImage = null;

        [Separator("Helper Variables (JCS_ResizeUI)")]

        [Tooltip("Show the resize panel during debug?")]
        [SerializeField]
        private bool mShowResizePanel = false;

        [Tooltip("Reize panel color.")]
        [SerializeField]
        private Color mResizePanelColor = new Color(171, 73, 73, 60);

        [Tooltip("Image sprite to showw the resize panel, should just be white.")]
        [SerializeField]
        private Sprite mImageSprite = null;
#endif

        [Separator("Check Variables (JCS_ResizeUI)")]

        [Tooltip("Width scale.")]
        [SerializeField]
        [ReadOnly]
        private float mWScale = 1.0f;

        [Tooltip("Height scale.")]
        [SerializeField]
        [ReadOnly]
        private float mHScale = 1.0f;

        [Tooltip("Target scale.")]
        [SerializeField]
        [ReadOnly]
        private float mTargetScale = 0.0f;

        /* Setter & Getter */

#if UNITY_EDITOR
        public bool showResizePanel
        {
            get { return this.mShowResizePanel; }
            set
            {
                this.mShowResizePanel = value;

                // Show or Hide?
                if (mShowResizePanel)
                    ShowResizePanel();
                else
                    HideResizePanel();
            }
        }
        public Color resizePanelColor { get { return this.mResizePanelColor; } set { this.mResizePanelColor = value; } }
#endif
        public RectTransform GetResizeRect() { return this.mRect; }
        public float wScale { get { return this.mWScale; } }
        public float hScale { get { return this.mHScale; } }
        public float targetScale { get { return this.mTargetScale; } }

        /* Functions */

        private void Awake()
        {
            mRect = GetComponent<RectTransform>();

            // if this is the root object set this as un destroyable
            gameObject.AddComponent<JCS_UniqueObject>();
        }

        private void Start()
        {
#if (UNITY_5_4_OR_NEWER)
            RectTransform appRect = JCS_Canvas.GuessCanvas().appRect;

            // TODO(jenchieh): unknown reason that something changes this to 
            // somewhere else. (since 5.4.0f3)
            Vector3 tempPos = appRect.localPosition;
            tempPos.z = 0;
            transform.localPosition = Vector3.zero;
#endif

            transform.localEulerAngles = Vector3.zero;

            transform.localScale = Vector3.one;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (mShowResizePanel)
                ShowResizePanel();
            else
                HideResizePanel();
#endif

            DoResizeUI();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Show the resize panel, for debugging usage.
        /// </summary>
        public void ShowResizePanel()
        {
            if (mImage == null)
                mImage = JCS_Util.ForceGetComponent<Image>(this);
            else
            {
                if (mShowResizePanel && mImage.enabled)
                    return;
            }

            // Show it.
            mImage.enabled = true;

            // Set image. Just a white sprite!
            mImage.sprite = mImageSprite;

            // Set color.
            mImage.color = mResizePanelColor;

            mShowResizePanel = true;
        }

        /// <summary>
        /// Hide the resize panel, for debugging usage.
        /// </summary>
        public void HideResizePanel()
        {
            if (mImage == null)
                return;

            mImage.enabled = false;

            mShowResizePanel = false;
        }
#endif

        /// <summary>
        /// Resize the UI if screen size changes.
        /// </summary>
        private void DoResizeUI()
        {
            var screenS = JCS_ScreenSettings.FirstInstance();

            float width = (float)JCS_Screen.width;
            float height = (float)JCS_Screen.height;

            JCS_ScreenSizef starting = screenS.StartingSize();

            mWScale = width / starting.width;
            mHScale = height / starting.height;

            mTargetScale = Mathf.Min(mWScale, mHScale);

            transform.localScale = Vector3.one * mTargetScale;
        }
    }
}
