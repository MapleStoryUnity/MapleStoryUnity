/**
 * $File: JCS_Switch.cs $
 * $Date: 2018-08-21 22:22:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Better version of checkbox/switch GUI.
    /// </summary>
    [RequireComponent(typeof(JCS_ColorTweener))]
    public class JCS_Switch : JCS_Button
    {
        /* Variables */

        public Action onSwitchOn = null;   // Callback when the switch is on.
        public Action onSwitchOff = null;  // Callback when the switch is off.

        // Callback when the value changed.
        public Action onValueChanged = null;

        private JCS_ColorTweener mColorTweener = null;

#if UNITY_EDITOR
        [Separator("🧪 Helper Variables (JCS_Switch)")]

        [Tooltip("Test module with the key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to toggle this toggle component.")]
        [SerializeField]
        private KeyCode mToggleOnOffKey = KeyCode.A;

        [Tooltip("Key to toggle interactable.")]
        [SerializeField]
        private KeyCode mToggleInteractable = KeyCode.S;
#endif

        [Separator("🌱 Initialize Variables (JCS_Switch)")]

        [Tooltip("Set the on/off position the same as the current " +
        "sign's position.")]
        [SerializeField]
        private bool mOnOffStartingPosition = true;

        [Tooltip("Is the color same as the editing status?")]
        [SerializeField]
        private bool mColorSameAsEditingStatus = true;

        [Separator("⚡️ Runtime Variables (JCS_Switch)")]

        [Tooltip("Sign of this switch.")]
        [SerializeField]
        private JCS_SwitchSign mSwitchSign = null;

        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool mIsOn = true;

        [Tooltip("Position when is on.")]
        [SerializeField]
        private Vector3 mOnPos = new Vector3(20.0f, 0.0f, 0.0f);

        [Tooltip("Position when is off.")]
        [SerializeField]
        private Vector3 mOffPos = new Vector3(-20.0f, 0.0f, 0.0f);

        [Header("🔍 Color")]

        [Tooltip("Button color when is on.")]
        [SerializeField]
        private Color mOnButtonColor = Color.white;

        [Tooltip("Button color when is off.")]
        [SerializeField]
        private Color mOffButtonColor = Color.white;

        [Tooltip("Background color when is on.")]
        [SerializeField]
        private Color mOnBackgroundColor = Color.white;

        [Tooltip("Background color when is off.")]
        [SerializeField]
        private Color mOffBackgroundColor = Color.white;

        /* Setter & Getter */

        public bool IsOn
        {
            get { return mIsOn; }
            set
            {
                if (mIsOn != value)
                {
                    mIsOn = value;

                    if (onValueChanged != null)
                        onValueChanged.Invoke();
                }

                // Update toggle once.
                DoToggle();
            }
        }
        public bool OnOffStartingPosition { get { return mOnOffStartingPosition; } set { mOnOffStartingPosition = value; } }
        public bool ColorSameAsEditingStatus { get { return mColorSameAsEditingStatus; } set { mColorSameAsEditingStatus = value; } }
        public Vector3 OnPos { get { return mOnPos; } set { mOnPos = value; } }
        public Vector3 OffPos { get { return mOffPos; } set { mOffPos = value; } }


        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mSwitchSign == null)
                mSwitchSign = GetComponentInChildren<JCS_SwitchSign>();

            mColorTweener = GetComponent<JCS_ColorTweener>();

            // Add interactable callback.
            onInteractableStateChanged += OnInteractableStateChanged;
        }

        private void Start()
        {
            SetStartingPosition();
            SetCurrentColorStatus();

            // Update toggle GUI once.
            DoToggle();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mToggleOnOffKey))
                Toggle();

            if (Input.GetKeyDown(mToggleInteractable))
                interactable = !interactable;
        }
#endif

        /// <summary>
        /// On click event.
        /// </summary>
        public override void OnClick()
        {
            Toggle();
        }

        /// <summary>
        /// Do the toggle action once.
        /// </summary>
        /// <returns>
        /// true, after toggle once is on.
        /// false, after toggle once is off.
        /// 
        /// ATTENTION: If toggle is not interactable will 
        /// return false.
        /// </returns>
        public bool Toggle()
        {
            if (!interactable)
                return false;

            // Toggle it.
            IsOn = !IsOn;

            return mIsOn;
        }

        /// <summary>
        /// Set on/off position to the current transform position.
        /// </summary>
        private void SetStartingPosition()
        {
            if (!mOnOffStartingPosition)
                return;

            if (mIsOn)
            {
                mOnPos = mSwitchSign.transform.localPosition;
                mOffPos = JCS_Mathf.ToNegative(mSwitchSign.transform.localPosition);
            }
            else
            {
                mOffPos = mSwitchSign.transform.localPosition;
                mOnPos = JCS_Mathf.ToNegative(mSwitchSign.transform.localPosition);
            }
        }

        /// <summary>
        /// Set all the color info to the current color status.
        /// </summary>
        private void SetCurrentColorStatus()
        {
            if (mColorSameAsEditingStatus)
            {
                if (mIsOn)
                {
                    mOnButtonColor = mSwitchSign.colorTweener.localColor;
                    mOnBackgroundColor = mColorTweener.localColor;
                }
                else
                {
                    mOffButtonColor = mSwitchSign.colorTweener.localColor;
                    mOffBackgroundColor = mColorTweener.localColor;
                }
            }
            else
            {
                if (mIsOn)
                {
                    SetButtonColor(mOnButtonColor);
                    SetBackgroundColor(mOnBackgroundColor);
                }
                else
                {
                    SetButtonColor(mOffButtonColor);
                    SetBackgroundColor(mOffBackgroundColor);
                }
            }
        }

        /// <summary>
        /// Do the actual toggle action.
        /// </summary>
        private void DoToggle()
        {
            DoToggle(mIsOn);
        }

        /// <summary>
        /// Do the actual toggle action.
        /// </summary>
        /// <param name="act"> Is on or off? </param>
        private void DoToggle(bool act)
        {
            if (act)
            {
                mSwitchSign.transformTweener.DoTween(mOnPos);
                mSwitchSign.colorTweener.DoTween(mOnButtonColor);
                mColorTweener.DoTween(mOnBackgroundColor);

                onSwitchOn?.Invoke();
            }
            else
            {
                mSwitchSign.transformTweener.DoTween(mOffPos);
                mSwitchSign.colorTweener.DoTween(mOffButtonColor);
                mColorTweener.DoTween(mOffBackgroundColor);

                onSwitchOff?.Invoke();
            }
        }

        /// <summary>
        /// After set interactable callback.
        /// </summary>
        private void OnInteractableStateChanged()
        {
            Color targetBackgroundColor = mOnBackgroundColor;
            Color targetButtonColor = mOnButtonColor;

            if (!mIsOn)
            {
                targetBackgroundColor = mOffBackgroundColor;
                targetButtonColor = mOffButtonColor;
            }

            if (interactable)
            {
                targetBackgroundColor.a = mInteractColor.a;
                targetButtonColor.a = mInteractColor.a;
            }
            else
            {
                // Stop color tweener if between the process of tweener.
                mSwitchSign.colorTweener.ResetTweener();
                mColorTweener.ResetTweener();

                targetBackgroundColor.a = mNotInteractColor.a;
                targetButtonColor.a = mNotInteractColor.a;
            }

            SetBackgroundColor(targetBackgroundColor);
            SetButtonColor(targetButtonColor);
        }

        /// <summary>
        /// Set the color of the toggle button.
        /// </summary>
        /// <param name="col"></param>
        private void SetButtonColor(Color col)
        {
            mSwitchSign.colorTweener.localColor = col;
        }

        /// <summary>
        /// Set the color of the toggle background.
        /// </summary>
        /// <param name="col"></param>
        private void SetBackgroundColor(Color col)
        {
            mColorTweener.localColor = col;
        }
    }
}
