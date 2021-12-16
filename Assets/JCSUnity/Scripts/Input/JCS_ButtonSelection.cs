/**
 * $File: JCS_ButtonSelection.cs $
 * $Date: 2017-10-07 14:41:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Events;

namespace JCSUnity
{
    /// <summary>
    /// One of the button selection for button selection group.
    /// 
    /// NOTE(jenchieh): this must use with JCS_ButtonSelectionGroup
    /// class or else is useless.
    /// </summary>
    public class JCS_ButtonSelection : MonoBehaviour
    {
        public delegate void SelectionActiveEvent(bool act);

        /* Variables */

        public EmptyFunction selectionEnable = SelectionEnable;
        public EmptyFunction selectionDisable = SelectionDisable;
        public SelectionActiveEvent selectionActive = SelectionActive;

        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;

        [Header("** Check Variables (JCS_ButtonSelection) **")]

        [Tooltip("Is this selection got active?")]
        [SerializeField]
        private bool mActive = false;


        [Header("** Initialize Variables (JCS_ButtonSelection) **")]

        [Tooltip("Deactive this button on Awake time?")]
        [SerializeField]
        private bool mDeactiveAtAwake = true;


        [Header("** Runtime Variables (JCS_ButtonSelection) **")]

        [Tooltip("Skip this selection?")]
        [SerializeField]
        private bool mSkip = false;

        [Tooltip("Events when you enter this selection.")]
        [SerializeField]
        private UnityEvent mSelectedEvent = null;

        [Tooltip("List of effect when on this selection.")]
        [SerializeField]
        private JCS_UnityObject[] mEffects = null;


        [Header("- Button (JCS_ButtonSelection)")]

        [Tooltip("Button for selection group to handle.")]
        [SerializeField]
        private JCS_Button mButton = null;

        [Tooltip("This gameobject itself is a button and use this button component.")]
        [SerializeField]
        private bool mSelfAsButton = true;


        [Header("- Full Control (JCS_ButtonSelection)")]

        [Tooltip("What is the selection ontop of this selection? (Press Up)")]
        [SerializeField]
        private JCS_ButtonSelection mUpSelection = null;

        [Tooltip("What is the selection ontop of this selection? (Press Down)")]
        [SerializeField]
        private JCS_ButtonSelection mDownSelection = null;

        [Tooltip("What is the selection ontop of this selection? (Press Right)")]
        [SerializeField]
        private JCS_ButtonSelection mRightSelection = null;

        [Tooltip("What is the selection ontop of this selection? (Press Left)")]
        [SerializeField]
        private JCS_ButtonSelection mLeftSelection = null;


        /* Setter & Getter */

        public bool DeactiveAtAwake { get { return this.mDeactiveAtAwake; } set { this.mDeactiveAtAwake = value; } }
        public bool SelfAsButton { get { return this.mSelfAsButton; } set { this.mSelfAsButton = value; } }
        public JCS_Button Button { get { return this.mButton; } set { this.mButton = value; } }
        public UnityEvent SelectedEvent { get { return this.mSelectedEvent; } }
        public bool Active
        {
            get { return this.mActive; }
            set
            {
                this.mActive = value;
                DoActive();
            }
        }
        public JCS_ButtonSelectionGroup ButtonSelectionGroup { get { return this.mButtonSelectionGroup; } set { this.mButtonSelectionGroup = value; } }
        public bool Skip { get { return this.mSkip; } }

        public JCS_ButtonSelection UpSelection { get { return this.mUpSelection; } set { this.mUpSelection = value; } }
        public JCS_ButtonSelection DownSelection { get { return this.mDownSelection; } set { this.mDownSelection = value; } }
        public JCS_ButtonSelection RightSelection { get { return this.mRightSelection; } set { this.mRightSelection = value; } }
        public JCS_ButtonSelection LeftSelection { get { return this.mLeftSelection; } set { this.mLeftSelection = value; } }


        /* Functions */

        private void Awake()
        {
            if (mDeactiveAtAwake)
            {
                // Deactive every at start
                this.Active = false;
            }

            if (mSelfAsButton)
            {
                if (this.mButton == null)
                    this.mButton = this.GetComponent<JCS_Button>();
            }

            // let the button know this is going to be control in the group.
            if (mButton != null)
                mButton.ButtonSelection = this;
        }

        private void Start()
        {
            SetSkip(mSkip);
        }

        /// <summary>
        /// Do stuff if this selection been checked.
        /// </summary>
        public void DoSelection()
        {
            if (mButton != null)
                mButton.JCS_ButtonClick();

            if (mSelectedEvent != null)
                mSelectedEvent.Invoke();
        }

        /// <summary>
        /// Check if this selection get selected.
        /// </summary>
        /// <returns>
        /// true: selected.
        /// false: vice versa.
        /// </returns>
        public bool IsSelected()
        {
            return this.mActive;
        }

        /// <summary>
        /// Make this selection selected.
        /// </summary>
        public void MakeSelect()
        {
            mButtonSelectionGroup.SelectSelection(this);
        }

        /// <summary>
        /// Make skip variable connect to interactable with button.
        /// </summary>
        public void SetSkip(bool act, bool fromButton = false)
        {
            if (mButton == null)
                return;

            this.mSkip = act;

            if (!fromButton)
                mButton.SetInteractable(!mSkip, true);
        }

        /* Default Function Pointers. */
        private static void SelectionEnable() { }
        private static void SelectionDisable() { }
        private static void SelectionActive(bool act) { }

        /// <summary>
        /// Do active and deactive.
        /// </summary>
        private void DoActive()
        {
            this.selectionActive.Invoke(this.mActive);

            if (this.mActive)
            {
                // Do stuff when active..
                selectionEnable.Invoke();
            }
            else
            {
                // Do stuff when deactive..
                selectionDisable.Invoke();
            }

            ActiveEffects(this.mActive);
        }

        /// <summary>
        /// Active the effects?
        /// </summary>
        /// <param name="act">
        /// true: active effects.
        /// false: deactive effects.
        /// </param>
        private void ActiveEffects(bool act)
        {
            for (int index = 0;
                   index < mEffects.Length;
                   ++index)
            {
                JCS_UnityObject effect = mEffects[index];

                if (effect != null)
                {
                    effect.UpdateUnityData();
                    effect.LocalEnabled = act;
                }
            }
        }
    }
}
