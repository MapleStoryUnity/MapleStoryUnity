/**
 * $File: JCS_TextButtonEffect.cs $
 * $Date: 2018-09-24 21:10:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Treat this button object just a text button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class JCS_TextButtonEffect : MonoBehaviour
    {
        /* Variables */

        private EventTrigger mEventTrigger = null;

        private Button mButton = null;

        private JCS_Button mBtn = null;

        [Separator("⚡️ Runtime Variables (JCS_TextButtonEffect)")]

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

        public Text textComp { get { return mText; } }
        public Color normalColor { get { return mNormalColor; } set { mNormalColor = value; } }
        public Color highlightedColor { get { return mHighlightedColor; } set { mHighlightedColor = value; } }
        public Color pressedColor { get { return mPressedColor; } set { mPressedColor = value; } }
        public Color disabledColor { get { return mDisabledColor; } set { mDisabledColor = value; } }

        /* Functions */

        private void Awake()
        {
            mEventTrigger = GetComponent<EventTrigger>();
            mButton = GetComponent<Button>();
            mBtn = GetComponent<JCS_Button>();

            // Try find it.
            if (mText == null)
                mText = GetComponentInChildren<Text>();

            JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, 
                EventTriggerType.PointerEnter, 
                OnPointerEnter);
            JCS_UIUtil.AddEventTriggerEvent(mEventTrigger,
                EventTriggerType.PointerExit,
                OnPointerExit);
            JCS_UIUtil.AddEventTriggerEvent(mEventTrigger,
                EventTriggerType.PointerDown,
                OnPointerDown);
            JCS_UIUtil.AddEventTriggerEvent(mEventTrigger,
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
            if (mBtn != null)
                return mBtn.interactable;
            if (mButton != null)
                return mButton.interactable;

            return true;
        }
    }
}
