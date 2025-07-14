/**
 * $File: JCS_LogText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Log text for JCS_IGLogSystem to handle.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    [RequireComponent(typeof(JCS_SimpleTrackAction))]
    [RequireComponent(typeof(JCS_SlideEffect))]
    public class JCS_LogText : JCS_TextObject
    {
        /* Variables */

        private JCS_FadeObject mFadeObject = null;
        private JCS_SimpleTrackAction mSimpleTrackAction = null;
        private JCS_SlideEffect mSlideEffect = null;

        [Separator("Check Variables (JCS_LogText)")]

        [Tooltip("Which exactly the IGL controls this.")]
        [SerializeField]
        private JCS_IGLogSystem mIGLogSystem = null;

        [Tooltip("Is the log text active?")]
        [SerializeField]
        private bool mActive = false;

        /* Setter & Getter */

        public bool IsActive { get { return this.mActive; } }

        public JCS_SimpleTrackAction SimpleTrackAction { get { return this.mSimpleTrackAction; } }
        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }

        /* Functions */

        private void Awake()
        {
            mFadeObject = this.GetComponent<JCS_FadeObject>();
            mSimpleTrackAction = this.GetComponent<JCS_SimpleTrackAction>();
            mSlideEffect = this.GetComponent<JCS_SlideEffect>();

            // set the fade out call back,  so we active from pool, and check
            // to see if the object is fade out complete. if is complete set
            // the active to false (return to pool).
            mFadeObject.onAfterFadeOut = OnFadeOut;
        }

        /// <summary>
        /// Initialize the text.
        /// </summary>
        /// <param name="sys"> The parent log system. </param>
        public void Init(JCS_IGLogSystem sys)
        {
            this.mIGLogSystem = sys;

            ClearText();
        }

        /// <summary>
        /// Activate one log from pool.
        /// </summary>
        public void Execute(string message)
        {
            if (mActive)
            {
                Debug.LogError("Call this while the object is still active");
                return;
            }

            // set the message text
            SetText(message);

            // fade out
            mFadeObject.FadeOut();

            // slide out
            mSlideEffect.Active();

            mActive = true;
        }

        /// <summary>
        /// Set the message text.
        /// </summary>
        /// <param name="message"></param>
        public void SetText(string message)
        {
            text = message;
        }

        /// <summary>
        /// Clear the text message.
        /// </summary>
        public void ClearText()
        {
            text = "";
        }

        /// <summary>
        /// Function assgin to FadeObject in order to let the object know when
        /// the object to deactive, and prepare for next use.
        /// </summary>
        private void OnFadeOut()
        {
            mActive = false;

            // remove from render queue
            mIGLogSystem.RemoveFromRenderVec(this);

            mSlideEffect.Deactive();

            ClearText();
        }
    }
}
