/**
 * $File: JCS_CheckableDialogue.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// When cursor or some trigger active, will show the object 
    /// information on it. This is what this dialogue do.
    /// </summary>
    public class JCS_CheckableDialogue
        : JCS_PanelRoot
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_CheckableDialogue) **")]

        [Tooltip("Fit the dialogue in the screen.")]
        [SerializeField]
        private bool mFitPushScreen = true;

        // Script of displaying the sprite!
        [Tooltip("Item image to display.")]
        [SerializeField]
        private Image mItemImage = null;

        [Tooltip("Panel's rect transform.")]
        [SerializeField]
        private RectTransform mPanelRectTransform = null;
        
        /* Setter & Getter */

        public bool fitPushScreen { get { return this.mFitPushScreen; } set { this.mFitPushScreen = value; } }
        public void SetItemSprite(Sprite sp) { this.mItemImage.sprite = sp; }

        /* Functions */

#if (UNITY_EDITOR)
        private void Update()
        {
            //this.mPanelRectTransform.localPosition = new Vector3(0, -228, 0);
        }
#endif

        /// <summary>
        /// Make dialogue follow the mouse.
        /// </summary>
        /// <param name="point"></param>
        public void FollowMouse(JCS_2D8Direction point)
        {
            if (mPanelRectTransform == null)
                return;

            // point we going to pop this dialogue
            Vector3 showPoint = JCS_Input.MousePositionOnGUILayer();

            Vector2 dialogueRect = mPanelRectTransform.sizeDelta;

            float halfPanelWidth = dialogueRect.x / 2.0f * mPanelRectTransform.localScale.x;
            float halfPanelHeight = dialogueRect.y / 2.0f * mPanelRectTransform.localScale.x; ;

            switch (point)
            {
                case JCS_2D8Direction.TOP:
                    showPoint.y -= halfPanelHeight;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    showPoint.y += halfPanelHeight;
                    break;
                case JCS_2D8Direction.RIGHT:
                    showPoint.x -= halfPanelWidth;
                    break;
                case JCS_2D8Direction.LEFT:
                    showPoint.x += halfPanelWidth;
                    break;

                case JCS_2D8Direction.TOP_LEFT:
                    showPoint.y -= halfPanelHeight;
                    showPoint.x += halfPanelWidth;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    showPoint.y -= halfPanelHeight;
                    showPoint.x -= halfPanelWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    showPoint.y += halfPanelHeight;
                    showPoint.x += halfPanelWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    showPoint.y += halfPanelHeight;
                    showPoint.x -= halfPanelWidth;
                    break;
            }

            this.mPanelRectTransform.localPosition = showPoint;

            if (mFitPushScreen)
                FitPushScreen();
        }

        /// <summary>
        /// Prevent the dialogue go out of screen
        /// </summary>
        private void FitPushScreen()
        {
            Vector2 rectSize = mPanelRectTransform.sizeDelta;
            Vector3 panelPos = mPanelRectTransform.localPosition;

            float halfSlotWidth = rectSize.x / 2.0f * mPanelRectTransform.localScale.x;
            float halfSlotHeight = rectSize.y / 2.0f * mPanelRectTransform.localScale.y;

            float panelLeftBorder = panelPos.x - halfSlotWidth;
            float panelRightBorder = panelPos.x + halfSlotWidth;
            float panelTopBorder = panelPos.y + halfSlotHeight;
            float panelBottomBorder = panelPos.y - halfSlotHeight;

            Camera cam = JCS_Camera.main.GetCamera();
            Vector3 camPos = cam.transform.position;
            // Transfer 3D space to 2D space
            Vector2 camPosToScreen = cam.WorldToScreenPoint(camPos);

            // Get application rect
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();
            Vector2 screenRect = appRect.sizeDelta;

            float camLeftBorder = camPosToScreen.x - screenRect.x / 2.0f;
            float camRightBorder = camPosToScreen.x + screenRect.x / 2.0f;
            float camTopBorder = camPosToScreen.y + screenRect.y / 2.0f;
            float camBottomBorder = camPosToScreen.y - screenRect.y / 2.0f;

            Vector3 newShowPoint = this.mPanelRectTransform.localPosition;

            if (panelRightBorder > camRightBorder)
            {
                newShowPoint.x -= panelRightBorder - camRightBorder;
            }
            else if (panelLeftBorder < camLeftBorder)
            {
                newShowPoint.x -= panelLeftBorder - camLeftBorder;
            }

            if (panelTopBorder > camTopBorder)
            {
                newShowPoint.y -= panelTopBorder - camTopBorder;
            }
            else if (panelBottomBorder < camBottomBorder)
            {
                newShowPoint.y -= panelBottomBorder - camBottomBorder;
            }

            this.mPanelRectTransform.localPosition = newShowPoint;   
        }
    }
}
