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
using MyBox;

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

        [Separator("📋 Check Variabless (JCS_AspectScreenPanel)")]

        [Tooltip("Type of the Aspect screen panel direction.")]
        [SerializeField]
        [ReadOnly]
        private JCS_2D4Direction mPlaceDirection = JCS_2D4Direction.TOP;

        /* Setter & Getter */

        public Image image { get { return mImage; } }
        public RectTransform sRectTransform { get { return mRectTransform; } }
        public JCS_2D4Direction placeDirection { get { return mPlaceDirection; } set { mPlaceDirection = value; } }

        /* Functions */

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
            mImage = GetComponent<Image>();
        }

        private void Start()
        {
            // Set the canvas to be our root.
            transform.SetParent(JCS_Canvas.GuessCanvas(transform).transform);

            // Add panel root without losing the original size.
            {
                Vector2 originalSize = mRectTransform.sizeDelta;

                mPanelRoot = gameObject.AddComponent<JCS_PanelRoot>();

                mRectTransform.sizeDelta = originalSize;
            }

            SetToScreenEdge();
        }

        private void LateUpdate()
        {
            JCS_Util.MoveToTheLastChild(mRectTransform);
        }

        /// <summary>
        /// Show the panel.
        /// </summary>
        public void ShowPanel()
        {
            mImage.enabled = true;
        }

        /// <summary>
        /// Hide the panel.
        /// </summary>
        public void HidePanel()
        {
            mImage.enabled = false;
        }

        /// <summary>
        /// Set the panel to the edge of the screen.
        /// </summary>
        private void SetToScreenEdge()
        {
            var ss = JCS_ScreenSettings.FirstInstance();

            Vector2 appRect = new Vector2(ss.startingSize.width, ss.startingSize.height);
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
                        name += " (Top)";
                    }
                    break;
                case JCS_2D4Direction.BOTTOM:
                    {
                        newPos.y -= halfScreenHeight;
                        name += " (Bottom)";
                    }
                    break;
                case JCS_2D4Direction.LEFT:
                    {
                        newPos.x -= halfScreenWidth;
                        name += " (Left)";
                    }
                    break;
                case JCS_2D4Direction.RIGHT:
                    {
                        newPos.x += halfScreenWidth;
                        name += " (Right)";
                    }
                    break;
            }

            mRectTransform.localPosition = newPos;
        }
    }
}
