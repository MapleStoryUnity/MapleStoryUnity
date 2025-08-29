/**
 * $File: JCS_ButtonSelectionGroupController.cs $
 * $Date: 2017-10-07 14:58:41 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Control the 'JCS_ButtonSelectionGroup' class with certain ket input, 
    /// without that class this class is meaningless.
    /// </summary>
    [RequireComponent(typeof(JCS_ButtonSelectionGroup))]
    public class JCS_ButtonSelectionGroupController : MonoBehaviour
    {
        /* Variables */

        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;

        [Separator("Runtime Variables (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Active key listener?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Key type of this controller.")]
        [SerializeField]
        private JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;

        [Header("Keyboard")]

        [Tooltip("Key for next selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMNext = KeyCode.None;

        [Tooltip("Key for previous selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMPrev = KeyCode.None;

        [Tooltip("Okay for this selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMOkay = KeyCode.None;

        [Header("Full Control (Keyboard)")]

        [Tooltip("Up select key. (Keyboard)")]
        [SerializeField]
        private KeyCode mMUp = KeyCode.None;

        [Tooltip("Down select key. (Keyboard)")]
        [SerializeField]
        private KeyCode mMDown = KeyCode.None;

        [Tooltip("Right select key. (Keyboard)")]
        [SerializeField]
        private KeyCode mMRight = KeyCode.None;

        [Tooltip("Left select key. (Keyboard)")]
        [SerializeField]
        private KeyCode mMLeft = KeyCode.None;

        [Header("Gamepad")]

        [Tooltip("Which device we are listening to?")]
        [SerializeField]
        private JCS_JoystickId mGamepadId = JCS_JoystickId.ANY;

        [Tooltip("Next selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJNext = JCS_JoystickButton.NONE;

        [Tooltip("Previous selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJPrev = JCS_JoystickButton.NONE;

        [Tooltip("Okay for this selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJOkay = JCS_JoystickButton.NONE;

        [Header("Full Control (Gamepad)")]

        [Tooltip("Up select key. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJUp = JCS_JoystickButton.NONE;

        [Tooltip("Down select key. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJDown = JCS_JoystickButton.NONE;

        [Tooltip("Right select key. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJRight = JCS_JoystickButton.NONE;

        [Tooltip("Left select key. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJLeft = JCS_JoystickButton.NONE;

        [Header("Audio")]

        [Tooltip("Sound when next button is pressed.")]
        [SerializeField]
        private AudioClip mNextSound = null;

        [Tooltip("Sound when prev button is pressed.")]
        [SerializeField]
        private AudioClip mPrevSound = null;

        [Tooltip("Sound when okay button is pressed.")]
        [SerializeField]
        private AudioClip mOkaySound = null;

        [Tooltip("Sound when up button is pressed.")]
        [SerializeField]
        private AudioClip mUpSound = null;

        [Tooltip("Sound when down button is pressed.")]
        [SerializeField]
        private AudioClip mDownSound = null;

        [Tooltip("Sound when right button is pressed.")]
        [SerializeField]
        private AudioClip mRightSound = null;

        [Tooltip("Sound when left button is pressed.")]
        [SerializeField]
        private AudioClip mLeftSound = null;

        [Tooltip("Method to play next sound.")]
        [SerializeField]
        private JCS_SoundMethod mNextSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play prev sound.")]
        [SerializeField]
        private JCS_SoundMethod mPrevSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play okay sound.")]
        [SerializeField]
        private JCS_SoundMethod mOkaySoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play up sound.")]
        [SerializeField]
        private JCS_SoundMethod mUpSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play down sound.")]
        [SerializeField]
        private JCS_SoundMethod mDownSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play right sound.")]
        [SerializeField]
        private JCS_SoundMethod mRightSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play left sound.")]
        [SerializeField]
        private JCS_SoundMethod mLeftSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        /* Setter & Getter */

        public JCS_ButtonSelectionGroup buttonSelectionGroup { get { return mButtonSelectionGroup; } }

        public bool active { get { return mActive; } set { mActive = value; } }

        public KeyCode mNext { get { return mMNext; } set { mMNext = value; } }
        public KeyCode mNPrev { get { return mMNext; } set { mMNext = value; } }
        public KeyCode mOkay { get { return mMNext; } set { mMNext = value; } }
        public KeyCode nUp { get { return mMNext; } set { mMNext = value; } }
        public KeyCode nDown { get { return mMNext; } set { mMNext = value; } }
        public KeyCode nRight { get { return mMNext; } set { mMNext = value; } }
        public KeyCode nLeft { get { return mMNext; } set { mMNext = value; } }

        public JCS_JoystickId gamepadId { get { return mGamepadId; } set { mGamepadId = value; } }

        public JCS_JoystickButton jNext { get { return mJNext; } set { mJNext = value; } }
        public JCS_JoystickButton jPrev { get { return mJPrev; } set { mJPrev = value; } }
        public JCS_JoystickButton jOkay { get { return mJOkay; } set { mJOkay = value; } }
        public JCS_JoystickButton jUp { get { return mJUp; } set { mJUp = value; } }
        public JCS_JoystickButton jDown { get { return mJDown; } set { mJDown = value; } }
        public JCS_JoystickButton jRight { get { return mJRight; } set { mJRight = value; } }
        public JCS_JoystickButton jLeft { get { return mJLeft; } set { mJLeft = value; } }

        public AudioClip nextSound { get { return mNextSound; } set { mNextSound = value; } }
        public AudioClip prevSound { get { return mPrevSound; } set { mPrevSound = value; } }
        public AudioClip okaySound { get { return mOkaySound; } set { mOkaySound = value; } }
        public AudioClip upSound { get { return mUpSound; } set { mUpSound = value; } }
        public AudioClip downSound { get { return mDownSound; } set { mDownSound = value; } }
        public AudioClip rightSound { get { return mRightSound; } set { mRightSound = value; } }
        public AudioClip leftSound { get { return mLeftSound; } set { mLeftSound = value; } }

        public JCS_SoundMethod nextSoundMethod { get { return mNextSoundMethod; } set { mNextSoundMethod = value; } }
        public JCS_SoundMethod prevSoundMethod { get { return mPrevSoundMethod; } set { mPrevSoundMethod = value; } }
        public JCS_SoundMethod okaySoundMethod { get { return mOkaySoundMethod; } set { mOkaySoundMethod = value; } }
        public JCS_SoundMethod upSoundMethod { get { return mUpSoundMethod; } set { mUpSoundMethod = value; } }
        public JCS_SoundMethod downSoundMethod { get { return mDownSoundMethod; } set { mDownSoundMethod = value; } }
        public JCS_SoundMethod rightSoundMethod { get { return mRightSoundMethod; } set { mRightSoundMethod = value; } }
        public JCS_SoundMethod leftSoundMethod { get { return mLeftSoundMethod; } set { mLeftSoundMethod = value; } }

        /* Functions */

        private void Awake()
        {
            mButtonSelectionGroup = GetComponent<JCS_ButtonSelectionGroup>();
        }

        private void Update()
        {
            if (!mActive)
                return;

            /* Basic Control (One Dimensional) */
            {
                if (ActiveNext())
                {
                    bool didAction = mButtonSelectionGroup.NextSelection();
                    if (didAction)
                        PlayNextSound();
                }
                if (ActivePrev())
                {
                    bool didAction = mButtonSelectionGroup.PrevSelection();
                    if (didAction)
                        PlayPrevSound();
                }
            }

            if (ActiveOkay())
            {
                // NOTE(jenchieh): no need to check this, because this 
                // will always be true.
                mButtonSelectionGroup.OkaySelection();
                PlayOkaySound();
            }

            /* Full Contol (Two Dimensional) */
            {
                if (ActiveUp())
                {
                    bool didAction = mButtonSelectionGroup.UpSelection();
                    if (didAction)
                        PlayUpSound();
                }
                if (ActiveDown())
                {
                    bool didAction = mButtonSelectionGroup.DownSelection();
                    if (didAction)
                        PlayDownSound();
                }
                if (ActiveRight())
                {
                    bool didAction = mButtonSelectionGroup.RightSelection();
                    if (didAction)
                        PlayRightSound();
                }
                if (ActiveLeft())
                {
                    bool didAction = mButtonSelectionGroup.LeftSelection();
                    if (didAction)
                        PlayLeftSound();
                }
            }
        }

        /// <summary>
        /// Active next selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveNext()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMNext) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJNext);
        }

        /// <summary>
        /// Active previous selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActivePrev()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMPrev) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJPrev);
        }

        /// <summary>
        /// Active okay selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveOkay()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMOkay) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJOkay);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveUp()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMUp) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJUp);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveDown()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMDown) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJDown);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveRight()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMRight) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJRight);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveLeft()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMLeft) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamepadId, mJLeft);
        }

        /* Play the next sound. */
        private void PlayNextSound()
        {
            if (mNextSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mNextSound, mNextSoundMethod);
        }

        /* Play the prev sound. */
        private void PlayPrevSound()
        {
            if (mPrevSound == null)
                return; 
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mPrevSound, mPrevSoundMethod);
        }

        /* Play the okay sound. */
        private void PlayOkaySound()
        {
            if (mOkaySound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mOkaySound, mOkaySoundMethod);
        }

        /* Play the up sound. */
        private void PlayUpSound()
        {
            if (mUpSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mUpSound, mUpSoundMethod);
        }

        /* Play the down sound. */
        private void PlayDownSound()
        {
            if (mDownSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mDownSound, mDownSoundMethod);
        }

        /* Play the right sound. */
        private void PlayRightSound()
        {
            if (mRightSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mRightSound, mRightSoundMethod);
        }

        /* Play the left sound. */
        private void PlayLeftSound()
        {
            if (mLeftSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mLeftSound, mLeftSoundMethod);
        }
    }
}
