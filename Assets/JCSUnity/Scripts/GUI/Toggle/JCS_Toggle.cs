/**
 * $File: JCS_Toggle.cs $
 * $Date: 2018-08-21 22:22:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Better version of checkbox/toggle GUI.
    /// </summary>
    [RequireComponent(typeof(JCS_ColorTweener))]
    public class JCS_Toggle : JCS_Button
    {
        public delegate void ToggleOnCallback();
        public delegate void ToggleOffCallback();
        public delegate void OnValueChanged();

        /* Variables */

        public ToggleOnCallback toggleOnCallback = null;
        public ToggleOffCallback toggleOffCallback = null;

        public OnValueChanged onValueChanged = null;

        private JCS_ColorTweener mColorTweener = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_Toggle) **")]

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

        [Header("** Initialize Variables (JCS_Toggle) **")]

        [Tooltip("Set the on/off position the same as the current " +
        "sign's position.")]
        [SerializeField]
        private bool mOnOffStartingPosition = true;

        [Tooltip("Is the color same as the editing status?")]
        [SerializeField]
        private bool mColorSameAsEditingStatus = true;


        [Header("** Runtime Variables (JCS_Toggle) **")]

        [Tooltip("Sign of this toggle.")]
        [SerializeField]
        private JCS_ToggleSign mToggleSign = null;

        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool mIsOn = true;

        [Tooltip("Position when is on.")]
        [SerializeField]
        private Vector3 mOnPos = new Vector3(20.0f, 0.0f, 0.0f);

        [Tooltip("Position when is off.")]
        [SerializeField]
        private Vector3 mOffPos = new Vector3(-20.0f, 0.0f, 0.0f);


        [Header("- Color (JCS_Toggle) ")]

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
            get { return this.mIsOn; }
            set {
                if (this.mIsOn != value)
                {
                    this.mIsOn = value;

                    if (onValueChanged != null)
                        onValueChanged.Invoke();
                }

                // Update toggle once.
                DoToggle();
            }
        }
        public bool OnOffStartingPosition { get { return this.mOnOffStartingPosition; } set { this.mOnOffStartingPosition = value; } }
        public bool ColorSameAsEditingStatus { get { return this.mColorSameAsEditingStatus; } set { this.mColorSameAsEditingStatus = value; } }
        public Vector3 OnPos { get { return this.mOnPos; } set { this.mOnPos = value; } }
        public Vector3 OffPos { get { return this.mOffPos; } set { this.mOffPos = value; } }


        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mToggleSign == null)
                this.mToggleSign = this.GetComponentInChildren<JCS_ToggleSign>();

            this.mColorTweener = this.GetComponent<JCS_ColorTweener>();

            // Add interactable callback.
            this.interactableCallback += InteractableCallback;
        }

        private void Start()
        {
            SetStartingPosition();
            SetCurrentColorStatus();

            // Update toggle GUI once.
            DoToggle();
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mToggleOnOffKey))
                Toggle();

            if (Input.GetKeyDown(mToggleInteractable))
                Interactable = !Interactable;
        }
#endif

        /// <summary>
        /// On click event.
        /// </summary>
        public override void JCS_OnClickCallback()
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
            if (!Interactable)
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
                this.mOnPos = mToggleSign.transform.localPosition;
                this.mOffPos = JCS_Mathf.ToNegative(mToggleSign.transform.localPosition);
            }
            else
            {
                this.mOffPos = mToggleSign.transform.localPosition;
                this.mOnPos = JCS_Mathf.ToNegative(mToggleSign.transform.localPosition);
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
                    mOnButtonColor = mToggleSign.ColorTweener.LocalColor;
                    mOnBackgroundColor = mColorTweener.LocalColor;
                }
                else
                {
                    mOffButtonColor = mToggleSign.ColorTweener.LocalColor;
                    mOffBackgroundColor = mColorTweener.LocalColor;
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
            DoToggle(this.mIsOn);
        }

        /// <summary>
        /// Do the actual toggle action.
        /// </summary>
        /// <param name="act"> Is on or off? </param>
        private void DoToggle(bool act)
        {
            if (act)
            {
                mToggleSign.TransformTweener.DoTween(mOnPos);
                mToggleSign.ColorTweener.DoTween(mOnButtonColor);
                mColorTweener.DoTween(mOnBackgroundColor);

                if (toggleOnCallback != null)
                    toggleOnCallback.Invoke();
            }
            else
            {
                mToggleSign.TransformTweener.DoTween(mOffPos);
                mToggleSign.ColorTweener.DoTween(mOffButtonColor);
                mColorTweener.DoTween(mOffBackgroundColor);

                if (toggleOffCallback != null)
                    toggleOffCallback.Invoke();
            }
        }

        /// <summary>
        /// After set interactable callback.
        /// </summary>
        private void InteractableCallback()
        {
            Color targetBackgroundColor = mOnBackgroundColor;
            Color targetButtonColor = mOnButtonColor;

            if (!mIsOn)
            {
                targetBackgroundColor = mOffBackgroundColor;
                targetButtonColor = mOffButtonColor;
            }

            if (Interactable)
            {
                targetBackgroundColor.a = mInteractColor.a;
                targetButtonColor.a = mInteractColor.a;
            }
            else
            {
                // Stop color tweener if between the process of tweener.
                mToggleSign.ColorTweener.ResetTweener();
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
            mToggleSign.ColorTweener.LocalColor = col;
        }

        /// <summary>
        /// Set the color of the toggle background.
        /// </summary>
        /// <param name="col"></param>
        private void SetBackgroundColor(Color col)
        {
            mColorTweener.LocalColor = col;
        }
    }
}
