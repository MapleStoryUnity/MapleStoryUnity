/**
 * $File: JCS_ButtonSoundEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.EventSystems;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Customize your own button sound base on different 
    /// circumstance.
    /// 
    /// Please use this class with Unity's "Event Trigger (Script)"!!!
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(EventTrigger))]
    public class JCS_ButtonSoundEffect : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;
        private EventTrigger mEventTrigger = null;

        [Separator("Optional Variables (JCS_ButtonSoundEffect)")]

        [Tooltip(@"Sound Player for this button, if this transform dose not 
have the 'JCS_Soundplayer' then it will grab the global sound player.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("Auto add to Unity's \"Event Trigger(Script)\" or not?")]

        [Tooltip("is true u dont have to add manully!")]
        [SerializeField]
        private bool mAutoAddEvent = true;

        [Header("*USAGE: Please use this component with Unity's \"Event Trigger(Script)\"!!!")]

        [SerializeField]
        private AudioClip mOnMouseOverSound = null;
        [SerializeField]
        private AudioClip mOnMouseExitSound = null;
        [SerializeField]
        private AudioClip mOnMouseDownSound = null;
        [SerializeField]
        private AudioClip mOnMouseUpSound = null;
        [SerializeField]
        private AudioClip mOnMouseClickSound = null;
        [SerializeField]
        private AudioClip mOnMouseDoubleClickSound = null;

        private bool mIsOver = false;

        [SerializeField]
        private JCS_SoundMethod mOnMouseOverSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseExitSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseDownSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseUpSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseClickSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseDoubleClickSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Header("- Optional")]

        [Tooltip("Use to detect to see if the button is interactable or not.")]
        [SerializeField]
        private JCS_Button mJCSButton = null;

        [Tooltip(@"When button is not interactable will active these when on mouse down.")]
        [SerializeField]
        private AudioClip mOnMouseOverRefuseSound = null;
        [SerializeField]
        private AudioClip mOnMouseExitRefuseSound = null;
        [SerializeField]
        private AudioClip mOnMouseDownRefuseSound = null;
        [SerializeField]
        private AudioClip mOnMouseUpRefuseSound = null;
        [SerializeField]
        private AudioClip mOnMouseClickRefuseSound = null;
        [SerializeField]
        private AudioClip mOnMouseDoubleClickRefuseSound = null;

        [SerializeField]
        private JCS_SoundMethod mOnMouseOverRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseExitRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseDownRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseUpRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseClickRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;
        [SerializeField]
        private JCS_SoundMethod mOnMouseDoubleClickRefuseSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        /* Setter & Getter */

        public bool AutoAddEvent { get { return this.mAutoAddEvent; } set { this.mAutoAddEvent = value; } }

        public AudioClip OnMouseOverSound { get { return this.mOnMouseOverSound; } set { this.mOnMouseOverSound = value; } }
        public AudioClip OnMouseExitSound { get { return this.mOnMouseExitSound; } set { this.mOnMouseExitSound = value; } }
        public AudioClip OnMouseDownSound { get { return this.mOnMouseDownSound; } set { this.mOnMouseDownSound = value; } }
        public AudioClip OnMouseUpSound { get { return this.mOnMouseUpSound; } set { this.mOnMouseUpSound = value; } }
        public AudioClip OnMouseClickSound { get { return this.mOnMouseClickSound; } set { this.mOnMouseClickSound = value; } }
        public AudioClip OnMouseDoubleClickSound { get { return this.mOnMouseDoubleClickSound; } set { this.mOnMouseDoubleClickSound = value; } }

        public JCS_SoundMethod OnMouseOverSoundMethod { get { return this.mOnMouseOverSoundMethod; } set { this.mOnMouseOverSoundMethod = value; } }
        public JCS_SoundMethod OnMouseExitSoundMethod { get { return this.mOnMouseExitSoundMethod; } set { this.mOnMouseExitSoundMethod = value; } }
        public JCS_SoundMethod OnMouseDownSoundMethod { get { return this.mOnMouseDownSoundMethod; } set { this.mOnMouseDownSoundMethod = value; } }
        public JCS_SoundMethod OnMouseUpSoundMethod { get { return this.mOnMouseUpSoundMethod; } set { this.mOnMouseUpSoundMethod = value; } }
        public JCS_SoundMethod OnMouseClickSoundMethod { get { return this.mOnMouseClickSoundMethod; } set { this.mOnMouseClickSoundMethod = value; } }
        public JCS_SoundMethod OnMouseDoubleClickSoundMethod { get { return this.mOnMouseDoubleClickSoundMethod; } set { this.mOnMouseDoubleClickSoundMethod = value; } }

        public AudioClip OnMouseOverRefuseSound { get { return this.mOnMouseOverRefuseSound; } set { this.mOnMouseOverRefuseSound = value; } }
        public AudioClip OnMouseExitRefuseSound { get { return this.mOnMouseExitRefuseSound; } set { this.mOnMouseExitRefuseSound = value; } }
        public AudioClip OnMouseDownRefuseSound { get { return this.mOnMouseDownRefuseSound; } set { this.mOnMouseDownRefuseSound = value; } }
        public AudioClip OnMouseUpRefuseSound { get { return this.mOnMouseUpRefuseSound; } set { this.mOnMouseUpRefuseSound = value; } }
        public AudioClip OnMouseClickRefuseSound { get { return this.mOnMouseClickRefuseSound; } set { this.mOnMouseClickRefuseSound = value; } }
        public AudioClip OnMouseDoubleClickRefuseSound { get { return this.mOnMouseDoubleClickRefuseSound; } set { this.mOnMouseDoubleClickRefuseSound = value; } }

        public JCS_SoundMethod OnMouseOverRefuseSoundMethod { get { return this.mOnMouseOverRefuseSoundMethod; } set { this.mOnMouseOverRefuseSoundMethod = value; } }
        public JCS_SoundMethod OnMouseExitRefuseSoundMethod { get { return this.mOnMouseExitRefuseSoundMethod; } set { this.mOnMouseExitRefuseSoundMethod = value; } }
        public JCS_SoundMethod OnMouseDownRefuseSoundMethod { get { return this.mOnMouseDownRefuseSoundMethod; } set { this.mOnMouseDownRefuseSoundMethod = value; } }
        public JCS_SoundMethod OnMouseUpRefuseSoundMethod { get { return this.mOnMouseUpRefuseSoundMethod; } set { this.mOnMouseUpRefuseSoundMethod = value; } }
        public JCS_SoundMethod OnMouseClickRefuseSoundMethod { get { return this.mOnMouseClickRefuseSoundMethod; } set { this.mOnMouseClickRefuseSoundMethod = value; } }
        public JCS_SoundMethod OnMouseDoubleClickRefuseSoundMethod { get { return this.mOnMouseDoubleClickRefuseSoundMethod; } set { this.mOnMouseDoubleClickRefuseSoundMethod = value; } }

        /* Functions */

        private void Awake()
        {
            if (mSoundPlayer == null)
                mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            mRectTransform = this.GetComponent<RectTransform>();
            mEventTrigger = this.GetComponent<EventTrigger>();

            if (mJCSButton == null)
                mJCSButton = this.GetComponent<JCS_Button>();
        }

        private void Start()
        {
            /* 
             * NOTE(jenchieh): First get the sound player from its own 
             * transform, if it still missing then grab the global sound 
             * player. 
             */
            if (mSoundPlayer == null)
                mSoundPlayer = JCS_SoundManager.instance.GlobalSoundPlayer();

            if (mAutoAddEvent)
            {
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseOver);
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseDoubleClick);
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerExit, JCS_OnMouseExit);
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerDown, JCS_OnMouseDown);
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerUp, JCS_OnMouseUp);
                JCS_Util.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerClick, JCS_OnMouseClick);
            }
        }

        private void Update()
        {
            // IMPORTANT(JenChieh): only double click need update
            if (mIsOver)
            {
                if (JCS_Input.OnMouseDoubleClick(0))
                {
                    // either time is out or double click, 
                    // both are all over the "double click event".
                    mIsOver = false;

                    if (mJCSButton != null)
                    {
                        if (!mJCSButton.Interactable)
                        {
                            // play not ineractable sound
                            mSoundPlayer.PlayOneShotByMethod(
                                mOnMouseDoubleClickRefuseSound,
                                mOnMouseDoubleClickRefuseSoundMethod);
                            return;
                        }
                        else
                        {
                            // play normal double click sound
                            mSoundPlayer.PlayOneShotByMethod(
                                mOnMouseDoubleClickSound,
                                mOnMouseDoubleClickSoundMethod);
                        }
                    }
                }

                // check if the mouse still over or not
                if (!JCS_Util.MouseOverGUI(this.mRectTransform))
                    mIsOver = false;
            }
        }

        public void JCS_OnMouseOver(PointerEventData data)
        {
            JCS_OnMouseOver();
        }
        public void JCS_OnMouseOver()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShotByMethod(
                        mOnMouseOverRefuseSound,
                        mOnMouseOverRefuseSoundMethod);
                    return;
                }
            }

            mSoundPlayer.PlayOneShotByMethod(
                mOnMouseOverSound,
                mOnMouseOverSoundMethod);
        }

        public void JCS_OnMouseExit(PointerEventData data)
        {
            JCS_OnMouseExit();
        }
        public void JCS_OnMouseExit()
        {
            if (mJCSButton == null)
                return;

            if (!mJCSButton.Interactable)
            {
                // play not ineractable sound
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseExitRefuseSound,
                    mOnMouseExitRefuseSoundMethod);
            }
            else
            {
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseExitSound,
                    mOnMouseExitSoundMethod);
            }
        }

        public void JCS_OnMouseDown(PointerEventData data)
        {
            JCS_OnMouseDown();
        }
        public void JCS_OnMouseDown()
        {
            if (mJCSButton == null)
                return;

            if (!mJCSButton.Interactable)
            {
                // play not ineractable sound
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseDownRefuseSound,
                    mOnMouseDownRefuseSoundMethod);
            }
            else
            {
                // play normal sound
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseDownSound,
                    mOnMouseDownSoundMethod);
            }
        }

        public void JCS_OnMouseUp(PointerEventData data)
        {
            JCS_OnMouseUp();
        }
        public void JCS_OnMouseUp()
        {
            if (mJCSButton == null)
                return;

            if (!mJCSButton.Interactable)
            {
                // play not ineractable sound
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseUpRefuseSound,
                    mOnMouseUpRefuseSoundMethod);
            }
            else
            {
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseUpSound,
                    mOnMouseUpSoundMethod);
            }
        }

        public void JCS_OnMouseClick(PointerEventData data)
        {
            JCS_OnMouseClick();
        }
        public void JCS_OnMouseClick()
        {
            if (mJCSButton == null)
                return;

            if (!mJCSButton.Interactable)
            {
                // play not ineractable sound
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseClickRefuseSound,
                    mOnMouseClickRefuseSoundMethod);
            }
            else
            {
                mSoundPlayer.PlayOneShotByMethod(
                    mOnMouseClickSound,
                    mOnMouseClickSoundMethod);
            }
        }

        // plz put this in Pointer Enter event
        public void JCS_OnMouseDoubleClick(PointerEventData data)
        {
            JCS_OnMouseDoubleClick();
        }
        public void JCS_OnMouseDoubleClick()
        {
            mIsOver = true;
        }
    }
}
