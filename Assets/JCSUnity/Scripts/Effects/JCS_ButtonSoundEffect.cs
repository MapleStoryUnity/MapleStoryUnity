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

        [Header("Optional")]

        [Tooltip("Use to detect to see if the button is interactable or not.")]
        [SerializeField]
        private JCS_Button mBtn = null;

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

        public bool autoAddEvent { get { return mAutoAddEvent; } set { mAutoAddEvent = value; } }

        public AudioClip onMouseOverSound { get { return mOnMouseOverSound; } set { mOnMouseOverSound = value; } }
        public AudioClip onMouseExitSound { get { return mOnMouseExitSound; } set { mOnMouseExitSound = value; } }
        public AudioClip onMouseDownSound { get { return mOnMouseDownSound; } set { mOnMouseDownSound = value; } }
        public AudioClip onMouseUpSound { get { return mOnMouseUpSound; } set { mOnMouseUpSound = value; } }
        public AudioClip onMouseClickSound { get { return mOnMouseClickSound; } set { mOnMouseClickSound = value; } }
        public AudioClip onMouseDoubleClickSound { get { return mOnMouseDoubleClickSound; } set { mOnMouseDoubleClickSound = value; } }

        public JCS_SoundMethod onMouseOverSoundMethod { get { return mOnMouseOverSoundMethod; } set { mOnMouseOverSoundMethod = value; } }
        public JCS_SoundMethod onMouseExitSoundMethod { get { return mOnMouseExitSoundMethod; } set { mOnMouseExitSoundMethod = value; } }
        public JCS_SoundMethod onMouseDownSoundMethod { get { return mOnMouseDownSoundMethod; } set { mOnMouseDownSoundMethod = value; } }
        public JCS_SoundMethod onMouseUpSoundMethod { get { return mOnMouseUpSoundMethod; } set { mOnMouseUpSoundMethod = value; } }
        public JCS_SoundMethod onMouseClickSoundMethod { get { return mOnMouseClickSoundMethod; } set { mOnMouseClickSoundMethod = value; } }
        public JCS_SoundMethod onMouseDoubleClickSoundMethod { get { return mOnMouseDoubleClickSoundMethod; } set { mOnMouseDoubleClickSoundMethod = value; } }

        public AudioClip onMouseOverRefuseSound { get { return mOnMouseOverRefuseSound; } set { mOnMouseOverRefuseSound = value; } }
        public AudioClip onMouseExitRefuseSound { get { return mOnMouseExitRefuseSound; } set { mOnMouseExitRefuseSound = value; } }
        public AudioClip onMouseDownRefuseSound { get { return mOnMouseDownRefuseSound; } set { mOnMouseDownRefuseSound = value; } }
        public AudioClip onMouseUpRefuseSound { get { return mOnMouseUpRefuseSound; } set { mOnMouseUpRefuseSound = value; } }
        public AudioClip onMouseClickRefuseSound { get { return mOnMouseClickRefuseSound; } set { mOnMouseClickRefuseSound = value; } }
        public AudioClip onMouseDoubleClickRefuseSound { get { return mOnMouseDoubleClickRefuseSound; } set { mOnMouseDoubleClickRefuseSound = value; } }

        public JCS_SoundMethod onMouseOverRefuseSoundMethod { get { return mOnMouseOverRefuseSoundMethod; } set { mOnMouseOverRefuseSoundMethod = value; } }
        public JCS_SoundMethod onMouseExitRefuseSoundMethod { get { return mOnMouseExitRefuseSoundMethod; } set { mOnMouseExitRefuseSoundMethod = value; } }
        public JCS_SoundMethod onMouseDownRefuseSoundMethod { get { return mOnMouseDownRefuseSoundMethod; } set { mOnMouseDownRefuseSoundMethod = value; } }
        public JCS_SoundMethod onMouseUpRefuseSoundMethod { get { return mOnMouseUpRefuseSoundMethod; } set { mOnMouseUpRefuseSoundMethod = value; } }
        public JCS_SoundMethod onMouseClickRefuseSoundMethod { get { return mOnMouseClickRefuseSoundMethod; } set { mOnMouseClickRefuseSoundMethod = value; } }
        public JCS_SoundMethod onMouseDoubleClickRefuseSoundMethod { get { return mOnMouseDoubleClickRefuseSoundMethod; } set { mOnMouseDoubleClickRefuseSoundMethod = value; } }

        /* Functions */

        private void Awake()
        {
            if (mSoundPlayer == null)
                mSoundPlayer = GetComponent<JCS_SoundPlayer>();
            mRectTransform = GetComponent<RectTransform>();
            mEventTrigger = GetComponent<EventTrigger>();

            if (mBtn == null)
                mBtn = GetComponent<JCS_Button>();
        }

        private void Start()
        {
            /* 
             * NOTE(jenchieh): First get the sound player from its own 
             * transform, if it still missing then grab the global sound 
             * player. 
             */
            if (mSoundPlayer == null)
                mSoundPlayer = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();

            if (mAutoAddEvent)
            {
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseOver);
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseDoubleClick);
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerExit, JCS_OnMouseExit);
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerDown, JCS_OnMouseDown);
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerUp, JCS_OnMouseUp);
                JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerClick, JCS_OnMouseClick);
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

                    if (mBtn != null)
                    {
                        if (!mBtn.interactable)
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
                if (!JCS_UIUtil.MouseOverGUI(this.mRectTransform))
                    mIsOver = false;
            }
        }

        public void JCS_OnMouseOver(PointerEventData data)
        {
            JCS_OnMouseOver();
        }
        public void JCS_OnMouseOver()
        {
            if (mBtn != null)
            {
                if (!mBtn.interactable)
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
            if (mBtn == null)
                return;

            if (!mBtn.interactable)
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
            if (mBtn == null)
                return;

            if (!mBtn.interactable)
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
            if (mBtn == null)
                return;

            if (!mBtn.interactable)
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
            if (mBtn == null)
                return;

            if (!mBtn.interactable)
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
