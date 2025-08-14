/**
 * $File: JCS_ButtonSelection.cs $
 * $Date: 2017-10-07 14:41:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

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
        /* Variables */

        public Action selectionEnable = SelectionEnable;
        public Action selectionDisable = SelectionDisable;
        public Action<bool> selectionActive = SelectionActive;

        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;

        [Separator("Check Variables (JCS_ButtonSelection)")]

        [Tooltip("Is this selection got active?")]
        [SerializeField]
        [ReadOnly]
        private bool mActive = false;

        [Separator("Initialize Variables (JCS_ButtonSelection)")]

        [Tooltip("Deactive this button on Awake time?")]
        [SerializeField]
        private bool mDeactiveAtAwake = true;

        [Separator("Runtime Variables (JCS_ButtonSelection)")]

        [Tooltip("Skip this selection?")]
        [SerializeField]
        private bool mSkip = false;

        [Tooltip("Events when you enter this selection.")]
        [SerializeField]
        private UnityEvent mSelectedEvent = null;

        [Tooltip("List of effect when on this selection.")]
        [SerializeField]
        private JCS_UnityObject[] mEffects = null;

        [Header("- Button")]

        [Tooltip("Button for selection group to handle.")]
        [SerializeField]
        private JCS_Button mButton = null;

        [Tooltip("This game object itself is a button and use this button component.")]
        [SerializeField]
        private bool mSelfAsButton = true;

        [Header("- Full Control")]

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

        public bool deactiveAtAwake { get { return mDeactiveAtAwake; } set { mDeactiveAtAwake = value; } }
        public bool selfAsButton { get { return mSelfAsButton; } set { mSelfAsButton = value; } }
        public JCS_Button button { get { return mButton; } set { mButton = value; } }
        public UnityEvent selectedEvent { get { return mSelectedEvent; } }
        public bool active
        {
            get { return mActive; }
            set
            {
                mActive = value;
                DoActive();
            }
        }
        public JCS_ButtonSelectionGroup buttonSelectionGroup { get { return mButtonSelectionGroup; } set { mButtonSelectionGroup = value; } }
        public bool skip { get { return mSkip; } }

        public JCS_ButtonSelection upSelection { get { return mUpSelection; } set { mUpSelection = value; } }
        public JCS_ButtonSelection downSelection { get { return mDownSelection; } set { mDownSelection = value; } }
        public JCS_ButtonSelection rightSelection { get { return mRightSelection; } set { mRightSelection = value; } }
        public JCS_ButtonSelection leftSelection { get { return mLeftSelection; } set { mLeftSelection = value; } }

        /* Functions */

        private void Awake()
        {
            if (mDeactiveAtAwake)
            {
                // Deactive every at start
                active = false;
            }

            if (mSelfAsButton)
            {
                if (mButton == null)
                    mButton = GetComponent<JCS_Button>();
            }

            // let the button know this is going to be control in the group.
            if (mButton != null)
                mButton.buttonSelection = this;
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
                mButton.ButtonClick();

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
            for (int index = 0; index < mEffects.Length; ++index)
            {
                JCS_UnityObject effect = mEffects[index];

                if (effect != null)
                {
                    effect.UpdateUnityData();
                    effect.localEnabled = act;
                }
            }
        }
    }
}
