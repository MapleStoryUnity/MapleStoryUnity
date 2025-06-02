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

        [Header("- Keyboard")]

        [Tooltip("Key for next selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMNext = KeyCode.None;

        [Tooltip("Key for previous selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMPrev = KeyCode.None;

        [Tooltip("Okay for this selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMOkay = KeyCode.None;

        [Header("- Full Control (Keyboard)")]

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

        [Header("- Gamepad")]

        [Tooltip("Which device we are listening to?")]
        [SerializeField]
        private JCS_JoystickId mGamePadId = JCS_JoystickId.ANY;

        [Tooltip("Next selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJNext = JCS_JoystickButton.NONE;

        [Tooltip("Previous selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJPrev = JCS_JoystickButton.NONE;

        [Tooltip("Okay for this selection. (Gamepad)")]
        [SerializeField]
        private JCS_JoystickButton mJOkay = JCS_JoystickButton.NONE;

        [Header("- Full Control (Gamepad)")]

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

        [Header("- Audio")]

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

        public JCS_ButtonSelectionGroup ButtonSelectionGroup { get { return this.mButtonSelectionGroup; } }

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        public KeyCode MNext { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MMPrev { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MOkay { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MUp { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MDown { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MRight { get { return this.mMNext; } set { this.mMNext = value; } }
        public KeyCode MLeft { get { return this.mMNext; } set { this.mMNext = value; } }

        public JCS_JoystickId GamePadId { get { return this.mGamePadId; } set { this.mGamePadId = value; } }

        public JCS_JoystickButton JNext { get { return this.mJNext; } set { this.mJNext = value; } }
        public JCS_JoystickButton JPrev { get { return this.mJPrev; } set { this.mJPrev = value; } }
        public JCS_JoystickButton JOkay { get { return this.mJOkay; } set { this.mJOkay = value; } }
        public JCS_JoystickButton JUp { get { return this.mJUp; } set { this.mJUp = value; } }
        public JCS_JoystickButton JDown { get { return this.mJDown; } set { this.mJDown = value; } }
        public JCS_JoystickButton JRight { get { return this.mJRight; } set { this.mJRight = value; } }
        public JCS_JoystickButton JLeft { get { return this.mJLeft; } set { this.mJLeft = value; } }

        public AudioClip NextSound { get { return this.mNextSound; } set { this.mNextSound = value; } }
        public AudioClip PrevSound { get { return this.mPrevSound; } set { this.mPrevSound = value; } }
        public AudioClip OkaySound { get { return this.mOkaySound; } set { this.mOkaySound = value; } }
        public AudioClip UpSound { get { return this.mUpSound; } set { this.mUpSound = value; } }
        public AudioClip DownSound { get { return this.mDownSound; } set { this.mDownSound = value; } }
        public AudioClip RightSound { get { return this.mRightSound; } set { this.mRightSound = value; } }
        public AudioClip LeftSound { get { return this.mLeftSound; } set { this.mLeftSound = value; } }

        public JCS_SoundMethod NextSoundMethod { get { return this.mNextSoundMethod; } set { this.mNextSoundMethod = value; } }
        public JCS_SoundMethod PrevSoundMethod { get { return this.mPrevSoundMethod; } set { this.mPrevSoundMethod = value; } }
        public JCS_SoundMethod OkaySoundMethod { get { return this.mOkaySoundMethod; } set { this.mOkaySoundMethod = value; } }
        public JCS_SoundMethod UpSoundMethod { get { return this.mUpSoundMethod; } set { this.mUpSoundMethod = value; } }
        public JCS_SoundMethod DownSoundMethod { get { return this.mDownSoundMethod; } set { this.mDownSoundMethod = value; } }
        public JCS_SoundMethod RightSoundMethod { get { return this.mRightSoundMethod; } set { this.mRightSoundMethod = value; } }
        public JCS_SoundMethod LeftSoundMethod { get { return this.mLeftSoundMethod; } set { this.mLeftSoundMethod = value; } }

        /* Functions */

        private void Awake()
        {
            this.mButtonSelectionGroup = this.GetComponent<JCS_ButtonSelectionGroup>();
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
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJNext);
        }

        /// <summary>
        /// Active previous selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActivePrev()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMPrev) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJPrev);
        }

        /// <summary>
        /// Active okay selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveOkay()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMOkay) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJOkay);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveUp()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMUp) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJUp);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveDown()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMDown) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJDown);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveRight()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMRight) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJRight);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveLeft()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMLeft) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJLeft);
        }

        /* Play the next sound. */
        private void PlayNextSound()
        {
            if (mNextSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mNextSound, mNextSoundMethod);
        }

        /* Play the prev sound. */
        private void PlayPrevSound()
        {
            if (mPrevSound == null)
                return; 
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mPrevSound, mPrevSoundMethod);
        }

        /* Play the okay sound. */
        private void PlayOkaySound()
        {
            if (mOkaySound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mOkaySound, mOkaySoundMethod);
        }

        /* Play the up sound. */
        private void PlayUpSound()
        {
            if (mUpSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mUpSound, mUpSoundMethod);
        }

        /* Play the down sound. */
        private void PlayDownSound()
        {
            if (mDownSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mDownSound, mDownSoundMethod);
        }

        /* Play the right sound. */
        private void PlayRightSound()
        {
            if (mRightSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mRightSound, mRightSoundMethod);
        }

        /* Play the left sound. */
        private void PlayLeftSound()
        {
            if (mLeftSound == null)
                return;
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GlobalSoundPlayer();
            sp.PlayOneShotByMethod(mLeftSound, mLeftSoundMethod);
        }
    }
}
