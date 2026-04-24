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
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class JCS_ButtonSoundEffect : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler
        , IPointerDownHandler
        , IPointerUpHandler
        , IPointerClickHandler
    {
        /* Variables */

        [Separator("📋 Check Variables (JCS_ButtonSoundEffect)")]

        [Tooltip("Is true when mouse is over this button.")]
        [SerializeField]
        [ReadOnly]
        private bool mIsEntered = false;

        [Separator("Optional Variables (JCS_ButtonSoundEffect)")]

        [Tooltip(@"Sound Player for this button, if this transform dose not 
have the 'JCS_Soundplayer' then it will grab the global sound player.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("🔍 *USAGE: Please use this component with Unity's \"Event Trigger(Script)\"!!!")]

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

        [Header("🔍 Optional")]

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
                mSoundPlayer = JCS_Glob.soundPlayer;
        }

        private void Update()
        {
            HandleDoubleClick();
        }

        private void HandleDoubleClick()
        {
            // IMPORTANT(JenChieh): only double click need update
            if (mBtn == null || !mIsEntered)
                return;

            if (JCS_Input.OnMouseDoubleClick(0))
            {
                if (!mBtn.interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShotByMethod(
                        mOnMouseDoubleClickRefuseSound,
                        mOnMouseDoubleClickRefuseSoundMethod);
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

        #region IPointer interfaces

        public void OnPointerEnter(PointerEventData eventData)
        {
            mIsEntered = true;

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

        public void OnPointerExit(PointerEventData eventData)
        {
            mIsEntered = false;

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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (mBtn == null || !mIsEntered)
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

        public void OnPointerUp(PointerEventData eventData)
        {
            if (mBtn == null || !mIsEntered)
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

        public void OnPointerClick(PointerEventData eventData)
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

        #endregion
    }
}
