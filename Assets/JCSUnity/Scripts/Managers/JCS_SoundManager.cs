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

        private JCS_FadeSound mJCSFadeSound = null;

        // this only hold one audio clip on stack. I do not want
        // to get to messy about this.
        private AudioClip mOnStackAudioClip = null;

        private float mOnStackFadeInTime = 0;
        private float mOnStackFadeOutTime = 0;

        [Separator("Check Variables (JCS_SoundManager)")]

        [Tooltip("Current background music is playing.")]
        [SerializeField]
        [ReadOnly]
        private AudioClip mCurrentBGM = null;

        [Tooltip("Boolean check if the background music switching.")]
        [SerializeField]
        [ReadOnly]
        private bool mSwitchingBGM = false;

        [Separator("Runtime Variables (JCS_SoundManager)")]

        [Tooltip("Do this scene using the specific setting?")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Time to fade in the sound.")]
        [SerializeField]
        [Range(JCS_SoundSettings.MIN_FADEOUT_TIME, JCS_SoundSettings.MAX_FADEOUT_TIME)]
        private float mSoundFadeInTime = 1.5f;

        [Tooltip("Time to fade out the sound.")]
        [SerializeField]
        [Range(JCS_SoundSettings.MIN_FADEOUT_TIME, JCS_SoundSettings.MAX_FADEOUT_TIME)]
        private float mSoundFadeOutTime = 1.5f;

        [Tooltip("Disable the sound when window isn't focus.")]
        [SerializeField]
        private bool mDisableSoundWheWindowNotFocus = true;

        // real time that the bgm fade out.
        private float mRealSoundFadeOutTime = 0;

        private bool mDoneFadingOut = false;

        /* Setter & Getter */

        public void SetAudioListener(AudioListener al) { this.mAudioListener = al; }
        public AudioListener GetAudioListener() { return this.mAudioListener; }
        public JCS_SoundPlayer GlobalSoundPlayer() { return this.mGlobalSoundPlayer; }

        public bool OverrideSetting { get { return this.mOverrideSetting; } }
        public float SoundFadeInTime { get { return this.mSoundFadeInTime; } set { this.mSoundFadeInTime = value; } }
        public float SoundFadeOutTime { get { return this.mSoundFadeOutTime; } set { this.mSoundFadeOutTime = value; } }
        public bool DisableSoundWheWindowNotFocus { get { return this.mDisableSoundWheWindowNotFocus; } set { this.mDisableSoundWheWindowNotFocus = value; } }

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
            instance = this;

            // try to get component, this is not guarantee.
            this.mJCSFadeSound = this.GetComponent<JCS_FadeSound>();

            mGlobalSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
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
            var ss = JCS_SoundSettings.instance;

            SwitchBGM(
                clip,
                ss.GetSoundFadeOutTimeBaseOnSetting(),
                ss.GetSoundFadeInTimeBaseOnSetting());
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
            if (mJCSFadeSound == null)
            {
                mJCSFadeSound = this.GetComponent<JCS_FadeSound>();

                if (mJCSFadeSound == null)
                    mJCSFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();
            }

            // get the background music audio source.
            AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

            // check if loop
            bgmAudioSource.loop = loop;

            // set the audio source.
            mJCSFadeSound.SetAudioSource(bgmAudioSource);

            // active the fade sound in effect.
            mJCSFadeSound.FadeOut(
                0,
                /* Fade in the sound base on the setting. */
                fadeInTime);

            this.mRealSoundFadeOutTime = fadeOutTime;

            this.mSwitchingBGM = true;
            this.mDoneFadingOut = false;

            this.mCurrentBGM = soundClip;
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
            var ss = JCS_SoundSettings.instance;

            return PlayOneShotBGM(
                oneShotClip, 
                onStackClip,
                ss.GetSoundFadeOutTimeBaseOnSetting(),
                ss.GetSoundFadeInTimeBaseOnSetting());
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
            this.mOnStackAudioClip = onStackClip;

            this.mOnStackFadeInTime = fadeInTime;
            this.mOnStackFadeOutTime = fadeOutTime;

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
                if (!mJCSFadeSound.IsReachTargetVolume())
                    return;

                // get the background music audio source.
                AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

                // set the audio source.
                mJCSFadeSound.SetAudioSource(bgmAudioSource);

                // set the bgm and play it
                bgmAudioSource.clip = this.mCurrentBGM;
                bgmAudioSource.Play();

                // active the fade sound in effect.
                mJCSFadeSound.FadeIn(1.0f, this.mRealSoundFadeOutTime);

                mDoneFadingOut = true;
            }
            else
            {
                // check if the sound is fade in.
                if (!mJCSFadeSound.IsReachTargetVolume())
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
                this.mOnStackAudioClip, 
                this.mOnStackFadeInTime,
                this.mOnStackFadeOutTime);

            // clean stack
            mOnStackAudioClip = null;
        }
    }
}
