/**
 * $File: JCS_ResizableScreenPanel.cs $
 * $Date: 2018-09-07 22:05:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Resizable screen panel, the invisible area of the screen.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class JCS_ResizableScreenPanel : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;
        private Image mImage = null;

        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Check Variables (JCS_AspectScreenPanel) **")]

        [Tooltip("Type of the Aspect screen panel direction.")]
        [SerializeField]
        private JCS_2D4Direction mPlaceDirection = JCS_2D4Direction.TOP;

        /* Setter & Getter */

        public Image image { get { return this.mImage; } }
        public RectTransform sRectTransform { get { return this.mRectTransform; } }
        public JCS_2D4Direction PlaceDirection { get { return this.mPlaceDirection; } set { this.mPlaceDirection = value; } }

        /* Functions */

        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();
            this.mImage = this.GetComponent<Image>();
        }

        private void Start()
        {
            // Set the canvas to be our root.
            this.transform.SetParent(JCS_Canvas.GuessCanvas(transform).transform);

            // Add panel root without losing the original size.
            {
                Vector2 originalSize = mRectTransform.sizeDelta;

                this.mPanelRoot = this.gameObject.AddComponent<JCS_PanelRoot>();

                mRectTransform.sizeDelta = originalSize;
            }

            SetToScreenEdge();
        }

        private void LateUpdate()
        {
            JCS_Utility.MoveToTheLastChild(this.mRectTransform);
        }

        /// <summary>
        /// Show the panel.
        /// </summary>
        public void ShowPanel()
        {
            this.mImage.enabled = true;
        }

        /// <summary>
        /// Hide the panel.
        /// </summary>
        public void HidePanel()
        {
            this.mImage.enabled = false;
        }

        /// <summary>
        /// Set the panel to the edge of the screen.
        /// </summary>
        private void SetToScreenEdge()
        {
            var ss = JCS_ScreenSettings.instance;

            Vector2 appRect = new Vector2(ss.STARTING_SCREEN_SIZE.width, ss.STARTING_SCREEN_SIZE.height);
            Vector2 halfAppRect = appRect / 2.0f;

            Vector2 panelSize = mRectTransform.sizeDelta;

            float halfScreenWidth = (panelSize.x / 2.0f) + halfAppRect.x;
            float halfScreenHeight = (panelSize.y / 2.0f) + halfAppRect.y;

            Vector3 newPos = mRectTransform.localPosition;

            switch (mPlaceDirection)
            {
                case JCS_2D4Direction.TOP:
                    {
                        newPos.y += halfScreenHeight;
                        this.name += " (Top)";
                    }
                    break;
                case JCS_2D4Direction.BOTTOM:
                    {
                        newPos.y -= halfScreenHeight;
                        this.name += " (Bottom)";
                    }
                    break;
                case JCS_2D4Direction.LEFT:
                    {
                        newPos.x -= halfScreenWidth;
                        this.name += " (Left)";
                    }
                    break;
                case JCS_2D4Direction.RIGHT:
                    {
                        newPos.x += halfScreenWidth;
                        this.name += " (Right)";
                    }
                    break;
            }

            mRectTransform.localPosition = newPos;
        }
    }
}
