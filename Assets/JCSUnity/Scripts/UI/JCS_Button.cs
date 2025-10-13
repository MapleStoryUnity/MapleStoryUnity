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

        public RectTransform GetRectTransfom() { return mRectTransform; }
        public Button buttonComp { get { return mButton; } }
        public Image image { get { return mImage; } }
        public int dialogueIndex { get { return mDialogueIndex; } set { mDialogueIndex = value; } }
        public bool autoListener { get { return mAutoListener; } set { mAutoListener = value; } }
        public bool interactable
        {
            get { return mInteractable; }
            set
            {
                mInteractable = value;

                // set this, in order to get the effect immdediatly.
                SetInteractable();
            }
        }
        public JCS_TextObject itText { get { return mItText; } }
        public JCS_ButtonSelection buttonSelection { get { return mButtonSelection; } set { mButtonSelection = value; } }
        public bool isSelectedInGroup { get { return mIsSelectedInGroup; } }

        /* Compatible with version 1.5.3 */
        public void SetSystemCallback(Action func) { sysOnClick += func; }
        public void SetSystemCallback(Action<JCS_Button> func) { sysOnClickBtn += func; }
        public void SetSystemCallback(Action<int> func, int selection)
        {
            sysOnClickSelection += func;
            mDialogueSelection = selection;
        }

        public int dialogueSelection { get { return mDialogueSelection; } }

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
                if (mInitialized)
                    return;
            }

            mRectTransform = GetComponent<RectTransform>();
            mButton = GetComponent<Button>();
            mImage = GetComponent<Image>();

            // try to get the text from the child.
            if (mItText == null)
                mItText = GetComponent<JCS_TextObject>();

            if (mItText != null)
            {
                if (mItText.textLegacy == null)
                    mItText.textLegacy = GetComponentInChildren<Text>();
                if (mItText.textTMP == null)
                    mItText.textTMP = GetComponentInChildren<TMP_Text>();
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

            mInitialized = true;
        }

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public virtual void ButtonClick()
        {
            mIsSelectedInGroup = IsSelected();

            if (!mIsSelectedInGroup)
                return;

            if (!mInteractable)
                return;

            /* System callback */
            sysOnClick?.Invoke();
            sysOnClickBtn?.Invoke(this);
            sysOnClickSelection?.Invoke(mDialogueSelection);

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
