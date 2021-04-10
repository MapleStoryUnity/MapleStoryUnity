/**
 * $File: JCS_TextButtonEffect.cs $
 * $Date: 2018-09-24 21:10:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JCSUnity
{
    /// <summary>
    /// Treat this button object just a text button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class JCS_TextButtonEffect
        : MonoBehaviour
    {
        /* Variables */

        private EventTrigger mEventTrigger = null;

        private Button mButton = null;

        private JCS_Button mJCS_Button = null;

        [Header("** Runtime Variables (JCS_TextButtonEffect) **")]

        [Tooltip("Text object to change color.")]
        [SerializeField]
        private Text mText = null;

        [Tooltip("Color text when the button is normal.")]
        [SerializeField]
        private Color mNormalColor = new Color(0.19f, 0.19f, 0.19f, 255.0f);

        [Tooltip("Color text when the button is highlighted.")]
        [SerializeField]
        private Color mHighlightedColor = new Color(0.1f, 0.1f, 0.1f, 255.0f);

        [Tooltip("Color text when the button is pressed.")]
        [SerializeField]
        private Color mPressedColor = Color.black;

        [Tooltip("Color text when the button is dsiabled.")]
        [SerializeField]
        private Color mDisabledColor = new Color(0.0f, 0.0f, 0.0f, 128.0f);

        /* Setter & Getter */

        public Text TextComp { get { return this.mText; } }
        public Color NormalColor { get { return this.mNormalColor; } set { this.mNormalColor = value; } }
        public Color HighlightedColor { get { return this.mHighlightedColor; } set { this.mHighlightedColor = value; } }
        public Color PressedColor { get { return this.mPressedColor; } set { this.mPressedColor = value; } }
        public Color DisabledColor { get { return this.mDisabledColor; } set { this.mDisabledColor = value; } }

        /* Functions */

        private void Awake()
        {
            this.mEventTrigger = this.GetComponent<EventTrigger>();
            this.mButton = this.GetComponent<Button>();
            this.mJCS_Button = this.GetComponent<JCS_Button>();

            // Try find it.
            if (this.mText == null)
                this.mText = this.GetComponentInChildren<Text>();

            JCS_Utility.AddEventTriggerEvent(mEventTrigger, 
                EventTriggerType.PointerEnter, 
                OnPointerEnter);
            JCS_Utility.AddEventTriggerEvent(mEventTrigger,
                EventTriggerType.PointerExit,
                OnPointerExit);
            JCS_Utility.AddEventTriggerEvent(mEventTrigger,
                EventTriggerType.PointerDown,
                OnPointerDown);
            JCS_Utility.AddEventTriggerEvent(mEventTrigger,
                EventTriggerType.PointerUp,
                OnPointerUp);

            // Initialize the first color.
            if (IsButtonInteractable())
                mText.color = mNormalColor;
            else
                mText.color = mDisabledColor;
        }

        /// <summary>
        /// Call it when is on mouse over.
        /// 
        /// Use in inspector for Event Trigger System. (Active)
        /// </summary>
        public void OnPointerEnter(PointerEventData data)
        {
            OnPointerEnter();
        }
        public void OnPointerEnter()
        {
            if (IsButtonInteractable())
                mText.color = mHighlightedColor;
            else
                mText.color = mDisabledColor;
        }

        /// <summary>
        /// Call it When is on mouse exit.
        /// 
        /// Use in inspector for Event Trigger System. (Deactive)
        /// </summary>
        /// <returns></returns>
        public void OnPointerExit(PointerEventData data)
        {
            OnPointerExit();
        }
        public void OnPointerExit()
        {
            if (IsButtonInteractable())
                mText.color = mNormalColor;
            else
                mText.color = mDisabledColor;
        }

        /// <summary>
        /// Call it when the pointer is down.
        /// </summary>
        /// <param name="data"></param>
        public void OnPointerDown(PointerEventData data)
        {
            OnPointerDown();
        }
        public void OnPointerDown()
        {
            if (IsButtonInteractable())
                mText.color = mPressedColor;
            else
                mText.color = mDisabledColor;
        }

        /// <summary>
        /// Call it when the pointer is up.
        /// </summary>
        /// <param name="data"></param>
        public void OnPointerUp(PointerEventData data)
        {
            OnPointerUp();
        }
        public void OnPointerUp()
        {
            // NOTE(jenchieh): this function is have the exactly 
            // the same code as OnPointerEnter. Consider remove this?
            if (IsButtonInteractable())
                mText.color = mHighlightedColor;
            else
                mText.color = mDisabledColor;
        }

        /// <summary>
        /// Get the button is current interactable or not.
        /// </summary>
        /// <returns></returns>
        private bool IsButtonInteractable()
        {
            // JCSUnity.JCS_Button have the higher priority then 
            // normal UnityEngine.UI.Button class.
            if (mJCS_Button != null)
                return mJCS_Button.Interactable;
            if (mButton != null)
                return mButton.interactable;

            return true;
        }
    }
}
