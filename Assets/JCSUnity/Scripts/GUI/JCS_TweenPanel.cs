/**
 * $File: JCS_TweenPanel.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    public delegate void ActiveCallback();
    public delegate void DeactiveCallback();

    /// <summary>
    /// Panel with active and deactive in order 
    /// to do back and forth effect.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_TweenerHandler))]
    public class JCS_TweenPanel
        : MonoBehaviour
    {
        /* Variables */

        private JCS_TweenerHandler mTweenerHandler = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_TweenPanel) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [SerializeField]
        private KeyCode mActiveKey = KeyCode.K;

        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.L;
#endif

        [Header("** Check Variables (JCS_TweenPanel) **")]

        [Tooltip("Is panel active/tweened?")]
        [SerializeField]
        private bool mIsActive = false;

        [Tooltip("Find root panel layer.")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Runtime Variables (JCS_TweenPanel) **")]

        [Tooltip("Override the tween animation while is still playing.")]
        [SerializeField]
        private bool mOverrideTween = false;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound plays when active this panel.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Sound plays when this panel is deactive.")]
        [SerializeField]
        private AudioClip mDeactiveSound = null;

        // call backs
        private ActiveCallback mActiveCallbackFunc = null;
        private DeactiveCallback mDeactiveCallbackFunc = null;

        /* Setter & Getter */

        public bool IsActive { get { return this.mIsActive; } }
        public JCS_TweenerHandler TweenerHandler { get { return this.mTweenerHandler; } }
        public bool OverrideTween { get { return this.mOverrideTween; } set { this.mOverrideTween = value; } }

        public ActiveCallback ActiveCallbackFunc { get { return this.mActiveCallbackFunc; } set { this.mActiveCallbackFunc = value; } }
        public DeactiveCallback DeactiveCallbackFunc { get { return this.mDeactiveCallbackFunc; } set { this.mDeactiveCallbackFunc = value; } }

        /* Functions */

        private void Awake()
        {
            this.mTweenerHandler = this.GetComponent<JCS_TweenerHandler>();
            if (mSoundPlayer == null)
                this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();
            if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

        /// <summary>
        /// Tween to the taget position and play sound effect.
        /// </summary>
        public void Active()
        {
            if (!mOverrideTween)
            {
                if (!mTweenerHandler.IsAllDoneTweening())
                    return;
            }

            if (this.mIsActive)
                return;

            mTweenerHandler.DoAllTweenToTargetValue();
            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mActiveSound, JCS_SoundMethod.PLAY_SOUND_WHILE_NOT_PLAYING);

            if (mPanelRoot != null)
                mPanelRoot.ShowDialogueWithoutSound();

            if (mActiveCallbackFunc != null)
                mActiveCallbackFunc.Invoke();

            this.mIsActive = true;
        }
        /// <summary>
        /// Tween back to the starting position and play sound effect.
        /// </summary>
        public void Deactive()
        {
            if (!mOverrideTween)
            {
                if (!mTweenerHandler.IsAllDoneTweening())
                    return;
            }

            if (!this.mIsActive)
                return;

            mTweenerHandler.DoAllTweenToStartValue();
            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mDeactiveSound, JCS_SoundMethod.PLAY_SOUND_WHILE_NOT_PLAYING);

            if (mDeactiveCallbackFunc != null)
                mDeactiveCallbackFunc.Invoke();

            this.mIsActive = false;
        }
    }
}
