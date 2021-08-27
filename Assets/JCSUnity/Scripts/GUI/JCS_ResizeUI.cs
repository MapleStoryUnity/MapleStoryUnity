/**
 * $File: JCS_ResizeUI.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public static JCS_ResizeUI instance = null;

        private RectTransform mRect = null;

#if (UNITY_EDITOR)
        private Image mImage = null;

        [Header("** Helper Variables (JCS_ResizeUI) **")]

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

        [Header("** Check Variables (JCS_ResizeUI) **")]

        [Tooltip("Width scale.")]
        [SerializeField]
        private float mWScale = 1.0f;

        [Tooltip("Height scale.")]
        [SerializeField]
        private float mHScale = 1.0f;

        [Tooltip("Target scale.")]
        [SerializeField]
        private float mTargetScale = 0.0f;

        /* Setter & Getter */

#if (UNITY_EDITOR)
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
        public Color ResizePanelColor { get { return this.mResizePanelColor; } set { this.mResizePanelColor = value; } }
#endif
        public RectTransform GetResizeRect() { return this.mRect; }
        public float WScale { get { return this.mWScale; } }
        public float HScale { get { return this.mHScale; } }
        public float TargetScale { get { return this.mTargetScale; } }

        /* Functions */

        private void Awake()
        {
            this.mRect = this.GetComponent<RectTransform>();

            // if this is the root object set this as un destroyable
            this.gameObject.AddComponent<JCS_UniqueObject>();

            if (instance != null)
            {
                string black_screen_name = JCS_UISettings.BLACK_SCREEN_NAME;
                string white_screen_name = JCS_UISettings.WHITE_SCREEN_NAME;

                // Cuz the transform list will change while we set the 
                // transform to this transform. 
                List<Transform> readyToSetList = new List<Transform>();

                Transform tempTrans = instance.transform;
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


            instance = this;

            JCS_Canvas.instance.SetResizeUI(this);
        }

        private void Start()
        {
#if (UNITY_5_4_OR_NEWER)
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

            // TODO(jenchieh): unknown reason that something changes this to 
            // somewhere else. (since 5.4.0f3)
            Vector3 tempPos = appRect.localPosition;
            tempPos.z = 0;
            this.transform.localPosition = Vector3.zero;
#endif

            this.transform.localEulerAngles = Vector3.zero;

            this.transform.localScale = Vector3.one;
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            if (mShowResizePanel)
                ShowResizePanel();
            else
                HideResizePanel();
#endif

            DoResizeUI();
        }

#if (UNITY_EDITOR)
        /// <summary>
        /// Show the resize panel, for debugging usage.
        /// </summary>
        public void ShowResizePanel()
        {
            if (mImage == null)
                mImage = JCS_Utility.ForceGetComponent<Image>(this);
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
            var screenS = JCS_ScreenSettings.instance;

            float width = (float)Screen.width;
            float height = (float)Screen.height;

            JCS_ScreenSizef size = screenS.StartingScreenSize();

            mWScale = width / size.width;
            mHScale = height / size.height;

            mTargetScale = (mWScale > mHScale) ? mHScale : mWScale;

            transform.localScale = Vector3.one * mTargetScale;
        }
    }
}
