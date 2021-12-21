/**
 * $File: JCS_RollSelectorButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Use with JCS_RollBtnSelector.
    /// </summary>
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    public class JCS_RollSelectorButton : JCS_Button
    {
        /* Variables */

        private JCS_RollBtnSelector mRollBtnSelector = null;

        [Header("** Check Variables (JCS_RollSelectorButton) **")]

        [SerializeField]
        private JCS_Button[] btns = null;

        [SerializeField]
        private int mScrollIndex = 0;

        private JCS_SimpleTrackAction mTrackAction = null;

        private JCS_ScaleEffect mScaleEffect = null;

        /* Setter & Getter */

        public int ScrollIndex { get { return this.mScrollIndex; } set { this.mScrollIndex = value; } }
        public void SetRollSelector(JCS_RollBtnSelector rbs) { this.mRollBtnSelector = rbs; }
        public JCS_SimpleTrackAction SimpleTrackAction { get { return this.mTrackAction; } }
        public JCS_ScaleEffect GetScaleEffect() { return this.mScaleEffect; }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mTrackAction = this.GetComponent<JCS_SimpleTrackAction>();
            this.mScaleEffect = this.GetComponent<JCS_ScaleEffect>();

            // set system call back.
            // so when the player click it will call the function.
            SetSystemCallback(SetFocus);
        }

        private void Start()
        {
            OverwriteAllButton();
        }

        /// <summary>
        /// Override
        /// </summary>
        public override void ButtonClick()
        {
            if (mRollBtnSelector == null)
            {
                JCS_Debug.LogError("No Roll Button Selector attached");
                return;
            }

            // if is the focus one.
            if (mRollBtnSelector.IsFoucsed(this))
            {
                ActiveAllOtherButtons();
            }
            else
            {
                base.ButtonClick();
            }
        }

        public override void OnClick()
        {
            // empty.
        }

        /// <summary>
        /// Override the orignal one so the it won't stop the button 
        /// event listener detection.
        /// 
        /// Because we have other complete system to handle this
        /// occurs.
        /// </summary>
        /// <param name="act"></param>
        public override void SetInteractable(bool act)
        {
            //base.SetInteractable(act);

            if (act)
            {
                mImage.color = mInteractColor;
            }
            else
            {
                mImage.color = mNotInteractColor;
            }
        }

        /// <summary>
        /// Set the target moving toward position.
        /// </summary>
        public void SetTrackPosition()
        {
            mTrackAction.TargetPosition = this.transform.localPosition;
        }

        /// <summary>
        /// When button get focus, design the code here...
        /// </summary>
        private void SetFocus()
        {
            if (mRollBtnSelector == null)
            {
                JCS_Debug.LogError("This button has been set focus but without the handler");
                return;
            }

            mRollBtnSelector.SetFocusSelector(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OverwriteAllButton()
        {
            // remove all the listener.
            mButton.onClick.RemoveAllListeners();

            // only add this listener
            mButton.onClick.AddListener(ButtonClick);

            // get all the buttons on this transform
            btns = this.GetComponents<JCS_Button>();

            foreach (JCS_Button b in btns)
            {
                // don't call if is itself
                if (b == this)
                    continue;

                b.AutoListener = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActiveAllOtherButtons()
        {
            foreach (JCS_Button b in btns)
            {
                // don't call if is itself
                if (b == this)
                    continue;
                
                b.ButtonClick();
            }
        }
    }
}
