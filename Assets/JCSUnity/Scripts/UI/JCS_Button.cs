/**
 * $File: JCS_Button.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using System;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Buttton Interface (uGUI)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public abstract class JCS_Button : MonoBehaviour
    {
        /* Variables */

        // framework only callback, do not override this callback
        public Action sysOnClick = null;
        public Action<JCS_Button> sysOnClickBtn = null;
        public Action<int> sysOnClickSelection = null;

        // user callback
        public Action onClick = null;
        public Action<JCS_Button> onClickBtn = null;

        public Action onInteractableStateChanged = null;

        [Separator("Check Variables (JCS_Button)")]

        [Tooltip("Record down the selection choice for dialogue system.")]
        [SerializeField]
        [ReadOnly]
        private int mDialogueSelection = -1;

        private bool mInitialized = false;

        [Separator("Optional Variables (JCS_Button)")]

        [Tooltip("text under the button, no necessary.")]
        [SerializeField]
        protected JCS_TextObject mItText = null;

        [Tooltip("Button Selection for if the button that are in the group.")]
        [SerializeField]
        protected JCS_ButtonSelection mButtonSelection = null;

        // record down if selected in the group. work with 
        // 'JCS_ButtonSelectionGroup' and 'JCS_ButtonSelection'.
        protected bool mIsSelectedInGroup = false;

        [Separator("Initialize Variables (JCS_Button)")]

        [Tooltip("Auto add listner to button click event?")]
        [SerializeField]
        protected bool mAutoListener = true;

        [Tooltip("Index pairing with Dialogue, in order to call the correct index.")]
        [SerializeField]
        protected int mDialogueIndex = -1;

        [Separator("Runtime Variables (JCS_Button)")]

        [Tooltip("Is the button interactable or not. (Default: true)")]
        [SerializeField]
        protected bool mInteractable = true;

        [Tooltip("Color tint when button is interactable.")]
        [SerializeField]
        protected Color mInteractColor = new Color(1, 1, 1, 1);

        [Tooltip("Color tint when button is not interactable.")]
        [SerializeField]
        protected Color mNotInteractColor = new Color(1, 1, 1, 0.5f);

        protected RectTransform mRectTransform = null;
        protected Button mButton = null;
        protected Image mImage = null;

        /* Setter & Getter */

        public RectTransform GetRectTransfom() { return this.mRectTransform; }
        public Button ButtonComp { get { return this.mButton; } }
        public Image Image { get { return this.mImage; } }
        public int DialogueIndex { get { return this.mDialogueIndex; } set { this.mDialogueIndex = value; } }
        public bool AutoListener { get { return this.mAutoListener; } set { this.mAutoListener = value; } }
        public bool Interactable
        {
            get { return this.mInteractable; }
            set
            {
                mInteractable = value;

                // set this, in order to get the effect immdediatly.
                SetInteractable();
            }
        }
        public JCS_TextObject ItText { get { return this.mItText; } }
        public JCS_ButtonSelection ButtonSelection { get { return this.mButtonSelection; } set { this.mButtonSelection = value; } }
        public bool IsSelectedInGroup { get { return this.mIsSelectedInGroup; } }

        /* Compatible with version 1.5.3 */
        public void SetSystemCallback(Action func) { this.sysOnClick += func; }
        public void SetSystemCallback(Action<JCS_Button> func) { this.sysOnClickBtn += func; }
        public void SetSystemCallback(Action<int> func, int selection)
        {
            this.sysOnClickSelection += func;
            this.mDialogueSelection = selection;
        }

        public int DialogueSelection { get { return this.mDialogueSelection; } }

        /* Functions */

        protected virtual void Awake()
        {
            Init();
        }

        /// <summary>
        /// Intialize the button once.
        /// </summary>
        public void Init(bool forceInit = false)
        {
            if (!forceInit)
            {
                if (this.mInitialized)
                    return;
            }

            this.mRectTransform = this.GetComponent<RectTransform>();
            this.mButton = this.GetComponent<Button>();
            this.mImage = this.GetComponent<Image>();

            // try to get the text from the child.
            if (mItText == null)
                mItText = this.GetComponent<JCS_TextObject>();

            if (mItText != null)
            {
                if (mItText.TextLegacy == null)
                    mItText.TextLegacy = this.GetComponentInChildren<Text>();
                if (mItText.TextTMP == null)
                    mItText.TextTMP = this.GetComponentInChildren<TMP_Text>();
            }

            if (mAutoListener)
            {
                // add listener itself, but it won't show in the inspector
                mButton.onClick.AddListener(ButtonClick);
            }

            // set the stating interactable.
            SetInteractable();

            // part of the on click callback
            SetSystemCallback(OnClick);

            this.mInitialized = true;
        }

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public virtual void ButtonClick()
        {
            this.mIsSelectedInGroup = IsSelected();

            if (!mIsSelectedInGroup)
                return;

            if (!mInteractable)
                return;

            /* System callback */
            sysOnClick?.Invoke();
            sysOnClickBtn?.Invoke(this);
            sysOnClickSelection?.Invoke(this.mDialogueSelection);

            /* User callback */

            onClick?.Invoke();
            onClickBtn?.Invoke(this);
        }

        /// <summary>
        /// This is the callback when the button get click.
        /// </summary>
        public abstract void OnClick();

        /// <summary>
        /// Use this to enable and disable the button.
        /// </summary>
        /// <param name="act"></param>
        public virtual void SetInteractable(bool act, bool fromSelection)
        {
            mInteractable = act;

            /* Make sure no error! */
            if (mButton == null)
                Init();

            mButton.enabled = mInteractable;

            if (mInteractable)
            {
                mImage.color = mInteractColor;
            }
            else
            {
                mImage.color = mNotInteractColor;
            }

            if (!fromSelection && mButtonSelection != null)
                mButtonSelection.SetSkip(!mInteractable, true);

            // interactable callback.
            onInteractableStateChanged?.Invoke();
        }
        public virtual void SetInteractable(bool act)
        {
            SetInteractable(act, false);
        }
        public virtual void SetInteractable()
        {
            SetInteractable(mInteractable, false);
        }

        /// <summary>
        /// Check if this button selected. If you are using with
        /// the 'JCS_ButtonSelectionGroup' and 'JCS_ButtonSelection'
        /// then you might need this check to call out the on click event.
        /// </summary>
        /// <returns>
        /// true: is selected in the group.
        /// false: vice versa.
        /// </returns>
        private bool IsSelected()
        {
            if (mButtonSelection == null)
                return true;

            if (!mButtonSelection.IsSelected())
            {
                // Make it selected.
                mButtonSelection.MakeSelect();
                return false;
            }

            return true;
        }
    }
}
