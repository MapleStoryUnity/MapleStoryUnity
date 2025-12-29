/**
 * $File: JCS_SoundManager.cs $
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
    /// Manage of all the music, sound and sfx in the game.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SoundManager : JCS_Manager<JCS_SoundManager>
    {
        /* Variables */

        private AudioListener mAudioListener = null;

        private JCS_SoundPlayer mGlobalSoundPlayer = null;

        private JCS_FadeSound mFadeSound = null;

        // this only hold one audio clip on stack. I do not want
        // to get to messy about 
        private AudioClip mOnStackAudioClip = null;

        private float mOnStackFadeInTime = 0;
        private float mOnStackFadeOutTime = 0;

        [Separator("📋 Check Variabless (JCS_SoundManager)")]

        [Tooltip("Current background music is playing.")]
        [SerializeField]
        [ReadOnly]
        private AudioClip mCurrentBGM = null;

        [Tooltip("Boolean check if the background music switching.")]
        [SerializeField]
        [ReadOnly]
        private bool mSwitchingBGM = false;

        [Separator("⚡️ Runtime Variables (JCS_SoundManager)")]

        [Tooltip("Do this scene using the specific setting?")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Time to fade in the sound.")]
        [SerializeField]
        [Range(JCS_SoundSettings.MIN_FADEOUT_TIME, JCS_SoundSettings.MAX_FADEOUT_TIME)]
        private float mTimeIn = 1.5f;

        [Tooltip("Time to fade out the sound.")]
        [SerializeField]
        [Range(JCS_SoundSettings.MIN_FADEOUT_TIME, JCS_SoundSettings.MAX_FADEOUT_TIME)]
        private float mTimeOut = 1.5f;

        [Tooltip("Disable the sound when window isn't focus.")]
        [SerializeField]
        private bool mDisableSoundWheWindowNotFocus = true;

        // real time that the bgm fade out.
        private float mRealSoundFadeOutTime = 0;

        private bool mDoneFadingOut = false;

        /* Setter & Getter */

        public void SetAudioListener(AudioListener al) { mAudioListener = al; }
        public AudioListener GetAudioListener() { return mAudioListener; }
        public JCS_SoundPlayer GlobalSoundPlayer() { return mGlobalSoundPlayer; }

        public bool overrideSetting { get { return mOverrideSetting; } }
        public float timeIn { get { return mTimeIn; } set { mTimeIn = value; } }
        public float timeOut { get { return mTimeOut; } set { mTimeOut = value; } }
        public bool disableSoundWheWindowNotFocus { get { return mDisableSoundWheWindowNotFocus; } set { mDisableSoundWheWindowNotFocus = value; } }

        /* Functions */

        private void OnApplicationFocus(bool focusStatus)
        {
            if (!mDisableSoundWheWindowNotFocus)
                return;

            // turn off all the sound effect
            if (!focusStatus)
            {
                // mute all the sound
                AudioListener.volume = 0;
            }
            else
            {
                // enable all the sound
                AudioListener.volume = 1;
            }
        }

        private void Awake()
        {
            RegisterInstance(this);

            // try to get component, this is not guarantee.
            mFadeSound = GetComponent<JCS_FadeSound>();

            mGlobalSoundPlayer = GetComponent<JCS_SoundPlayer>();
        }

        private void Start()
        {
            if (JCS_Camera.main == null)
            {
                Debug.LogError("There is no 'JCS_Camera' assign!");
                return;
            }
        }

        private void Update()
        {
            DoSwitchBGM();

            DoStackBGM();
        }

        /// <summary>
        /// Switch the background music, fading in and out.
        /// </summary>
        /// <param name="soundClip"> clip to play. </param>
        /// <param name="fadeTime"> time to fade in and out. </param>
        /// <param name="loop"> loop music? </param>
        public void SwitchBGM(AudioClip clip, bool loop = true)
        {
            var ss = JCS_SoundSettings.FirstInstance();

            SwitchBGM(
                clip,
                ss.TimeOut(),
                ss.TimeIn());
        }

        /// /// <summary>
        /// Switch the background music, fading in and out.
        /// </summary>
        /// <param name="soundClip"> clip to play. </param>
        /// <param name="fadeInTime"> time to fade in. </param>
        /// <param name="fadeOutTime"> time to fade out. </param>
        /// <param name="loop"> loop music? </param>
        public void SwitchBGM(
            AudioClip soundClip,
            float fadeInTime,
            float fadeOutTime,
            bool loop = true)
        {
            if (mSwitchingBGM)
                return;

            /* Check if the audio clip good to play. */
            if (soundClip == null)
                return;

            /**
             *  Try to get the fade sound component.
             *  
             *  Since we have multiple component have 
             */
            if (mFadeSound == null)
            {
                mFadeSound = GetComponent<JCS_FadeSound>();

                if (mFadeSound == null)
                    mFadeSound = gameObject.AddComponent<JCS_FadeSound>();
            }

            // get the background music audio source.
            AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

            // check if loop
            bgmAudioSource.loop = loop;

            // set the audio source.
            mFadeSound.SetAudioSource(bgmAudioSource);

            // active the fade sound in effect.
            mFadeSound.FadeOut(
                0,
                /* Fade in the sound base on the setting. */
                fadeInTime);

            mRealSoundFadeOutTime = fadeOutTime;

            mSwitchingBGM = true;
            mDoneFadingOut = false;

            mCurrentBGM = soundClip;
        }

        /// <summary>
        /// Play one shot background music, after playing it.
        /// Play the on stack sound.
        /// </summary>
        /// <param name="oneShotClip"> One shot BGM </param>
        /// <param name="onStackClip"> Audio clip on stack </param>
        public bool PlayOneShotBGM(
            AudioClip oneShotClip,
            AudioClip onStackClip)
        {
            var ss = JCS_SoundSettings.FirstInstance();

            return PlayOneShotBGM(
                oneShotClip,
                onStackClip,
                ss.TimeOut(),
                ss.TimeIn());
        }

        /// <summary>
        /// Play one shot background music, after playing it.
        /// Play the on stack sound.
        /// </summary>
        /// <param name="oneShotClip"> One shot BGM </param>
        /// <param name="onStackClip"> Audio clip on stack </param>
        /// <param name="fadeInTime"> time to fade in the bgm </param>
        /// <param name="fadeOutTime"> time to fade out the bgm </param>
        public bool PlayOneShotBGM(
            AudioClip oneShotClip,
            AudioClip onStackClip,
            float fadeInTime,
            float fadeOutTime)
        {
            // stack must be empty before we play it.
            if (mOnStackAudioClip != null)
                return false;

            // store audio clip on stack.
            mOnStackAudioClip = onStackClip;

            mOnStackFadeInTime = fadeInTime;
            mOnStackFadeOutTime = fadeOutTime;

            // Play one shot the BGM
            SwitchBGM(
                oneShotClip,
                fadeInTime,
                fadeOutTime,
                /* Dont loop. */
                false);

            return true;
        }

        /// <summary>
        /// Set the sound volume in the list.
        /// </summary>
        /// <param name="list"> list of the audio source </param>
        /// <param name="vol"> target volume. </param>
        private void SetVolume(JCS_Vec<AudioSource> list, float vol)
        {
            for (int index = 0; index < list.length; ++index)
            {
                list.at(index).volume = vol;
            }
        }

        /// <summary>
        /// Set the sound mute or not with list needed.
        /// </summary>
        /// <param name="list"> list of the audio source. </param>
        /// <param name="act"> target mute action. </param>
        private void SetMute(JCS_Vec<AudioSource> list, bool act)
        {
            for (int index = 0; index < list.length; ++index)
            {
                list.at(index).mute = act;
            }
        }

        /// <summary>
        /// Do the switching bgm algorithm.
        /// </summary>
        private void DoSwitchBGM()
        {
            if (!mSwitchingBGM)
                return;

            /* Once if fade out we load next sound buffer. */

            if (!mDoneFadingOut)
            {
                // check if the sound is fade out.
                if (!mFadeSound.IsReachTargetVolume())
                    return;

                // get the background music audio source.
                AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

                // set the audio source.
                mFadeSound.SetAudioSource(bgmAudioSource);

                // set the bgm and play it
                bgmAudioSource.clip = mCurrentBGM;
                bgmAudioSource.Play();

                // active the fade sound in effect.
                mFadeSound.FadeIn(1.0f, mRealSoundFadeOutTime);

                mDoneFadingOut = true;
            }
            else
            {
                // check if the sound is fade in.
                if (!mFadeSound.IsReachTargetVolume())
                    return;

                // done switching the bgm.
                mSwitchingBGM = false;
            }
        }

        /// <summary>
        /// Check if there are audio clip on stack.
        /// If do, wait for the next BGM done playing, and 
        /// play the on stack bgm. Eventually clear the 
        /// stack. 
        /// 
        /// ATTENTION(jenchieh): This only work for not looping BGM.
        /// </summary>
        private void DoStackBGM()
        {
            // nothing to do, the stacks is clean!.
            if (mOnStackAudioClip == null)
                return;

            // do not check if switching bgm.
            if (mSwitchingBGM)
                return;

            AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

            // do nothing if still playing
            if (bgmAudioSource.isPlaying)
                return;

            // switch bgm
            SwitchBGM(
                mOnStackAudioClip,
                mOnStackFadeInTime,
                mOnStackFadeOutTime);

            // clean stack
            mOnStackAudioClip = null;
        }
    }
}
