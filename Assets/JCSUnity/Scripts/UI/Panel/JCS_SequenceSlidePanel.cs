/**
 * $File: JCS_SequenceSlidePanel.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Sequence of panel that can be set by using other
    /// JCSUnity framework class's action/event.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(JCS_SlideEffect))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SequenceSlidePanel : MonoBehaviour
    {
        /* Variables */

        private JCS_SlideEffect mSlideEffect = null;
        private JCS_SoundPlayer mSoundPlayer = null;
        private RectTransform mRectTransform = null;

        [Separator("Initialize Variables (JCS_SequenceSlidePanel)")]

        [Tooltip(@"Optional choice, instead of using the auto detection. 
(JCS_EmptyButton are the recommaned default class to use.)")]
        [SerializeField]
        private JCS_Button mToggleOrExitButton = null;

        [Separator("Runtime Variables (JCS_SequenceSlidePanel)")]

        [Tooltip("Sequence of buttons with slide effect component with in the tranform.")]
        [SerializeField]
        private JCS_SlideEffect[] mSlideButtons = null;

        [Tooltip("Area that also control with this, plz do it manully. (only in children)")]
        [SerializeField]
        private JCS_SlideEffect[] mAreaEffects = null;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Spacing")]

        [Tooltip("Time to active one button animation.")]
        [SerializeField]
        private float mSpaceTime = 0.2f;

        private float mSpaceTimer = 0.0f;

        private int mBtnCounter = 0;

        [Tooltip("Exit when mouse is not on this panel.")]
        [SerializeField]
        private bool mExitByNoOverGUI = true;

        [Header("Sound")]

        [Tooltip("Audio when active the animation the outer panel.")]
        [SerializeField]
        private AudioClip mActiveClip = null;

        [Tooltip("Audio when exit the animation the outer panel.")]
        [SerializeField]
        private AudioClip mDeactiveClip = null;

        [Tooltip("Audio when active the animation inside the panel.")]
        [SerializeField]
        private AudioClip mInsideActiveClip = null;

        [Tooltip("Audio when exit the animation inside the panel.")]
        [SerializeField]
        private AudioClip mInsideDeactiveClip = null;

        /* Setter & Getter */

        public float spaceTime { get { return mSpaceTime; } set { mSpaceTime = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public AudioClip activeClip { get { return mActiveClip; } set { mActiveClip = value; } }
        public AudioClip deactiveClip { get { return mDeactiveClip; } set { mDeactiveClip = value; } }
        public AudioClip insideActiveClip { get { return mInsideActiveClip; } set { mInsideActiveClip = value; } }
        public AudioClip insideDeactiveClip { get { return mInsideDeactiveClip; } set { mInsideDeactiveClip = value; } }

        /* Functions */

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
            mSlideEffect = GetComponent<JCS_SlideEffect>();
            mSoundPlayer = GetComponent<JCS_SoundPlayer>();

            InitSequenceButtons();

            // do not auto check this exit, 
            // so it wont exit itself.
            mSlideEffect.autoAddEvent = false;

            // if button not equals to null, 
            // then we can do the button to close the panel instead of 
            // on mouse stay. (Area Detection)
            if (mToggleOrExitButton != null)
            {
                JCS_ToggleButton tb = mToggleOrExitButton.GetComponent<JCS_ToggleButton>();
                if (tb != null)
                {
                    tb.onActive = mSlideEffect.Active;
                    tb.onDeactive = ClosePanel;
                }
                else
                {
                    mToggleOrExitButton.SetSystemCallback(ClosePanel);
                }
            }
        }

        private void LateUpdate()
        {
            // once is detect that is on the gui
            if (mSlideEffect.isActive)
            {
                // active the animation.
                ActiveAnim();

                // only when the panel reach to the position, 
                // so it wont go too fast.
                if (mSlideEffect.IsIdle())
                {
                    if (mExitByNoOverGUI)
                    {
                        // start check weather the mouse 
                        // is on this panel or not.
                        if (!CheckAllActivePanel())
                        {
                            ClosePanel();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initialize all the button under this panel.
        /// </summary>
        private void InitSequenceButtons()
        {
            JCS_SlideEffect se = null;

            for (int index = 0; index < mSlideButtons.Length; ++index)
            {
                se = mSlideButtons[index];

                if (se == null)
                {
                    Debug.LogError("Missing button assign in the inspector");
                    continue;
                }

                // 
                se.transform.SetParent(this.transform);

                se.autoAddEvent = false;
            }

            for (int index = 0; index < mAreaEffects.Length; ++index)
            {
                se = mAreaEffects[index];

                if (se == null)
                {
                    Debug.LogError("Missing button assign in the inspector");
                    continue;
                }

                // 
                se.transform.SetParent(this.transform);
            }
        }

        /// <summary>
        /// Active all the button animation under this panel.
        /// </summary>
        private void ActiveAnim()
        {
            if (mBtnCounter == mSlideButtons.Length)
                return;

            if (mBtnCounter != 0)
            {
                mSoundPlayer.PlayOneShot(mActiveClip);

                mSpaceTimer += JCS_Time.ItTime(mTimeType);

                if (mSpaceTime < mSpaceTimer)
                {
                    mSpaceTimer = 0;
                }
                else
                    return;
            }

            JCS_SlideEffect se = mSlideButtons[mBtnCounter];

            // stop checking if exits
            se.autoAddEvent = false;

            se.Active();

            ++mBtnCounter;

            mSoundPlayer.PlayOneShot(mInsideActiveClip);
        }

        /// <summary>
        /// Deactive all the button animation under this panel.
        /// </summary>
        private void DeactiveAnim()
        {
            JCS_SlideEffect se = null;

            for (int index = 0; index < mSlideButtons.Length; ++index)
            {
                se = mSlideButtons[index];

                se.Deactive();
            }

            mSpaceTimer = 0;
            mBtnCounter = 0;

            mSoundPlayer.PlayOneShot(mDeactiveClip);
            mSoundPlayer.PlayOneShot(mInsideDeactiveClip);
        }

        /// <summary>
        /// Check if the trigger still active within the area.
        /// </summary>
        /// <returns></returns>
        private bool CheckAllActivePanel()
        {
            foreach (JCS_SlideEffect se in mAreaEffects)
            {
                // since is the child of this transform,
                // pass in the root panel, 
                // so it can calcualte the correct area.
                if (se.IsOnThere(mRectTransform))
                    return true;
            }

            // check if mouse still on this panel
            if (mSlideEffect.IsOnThere())
                return true;

            return false;
        }

        /// <summary>
        /// Close the panel, include all the button under this panel.
        /// </summary>
        private void ClosePanel()
        {
            DeactiveAnim();
            mSlideEffect.Deactive();
        }
    }
}
