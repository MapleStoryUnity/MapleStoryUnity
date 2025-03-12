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

        // environment, ui, etc.
        private JCS_Vec<AudioSource> mSFXSounds = null;

        // personal stuff, personal skill, personal jump walk, etc.
        private JCS_Vec<AudioSource> mSkillsSounds = null;

        private JCS_SoundPlayer mGlobalSoundPlayer = null;

        private JCS_FadeSound mJCSFadeSound = null;

        // this only hold one audio clip on stack. I do not want
        // to get to messy about this.
        private AudioClip mOnStackAudioClip = null;

        private float mOnStackFadeInTime = 0;
        private float mOnStackFadeOutTime = 0;

        [Separator("Check Variables (JCS_SoundManager)")]

        [Tooltip("Current background music audio source.")]
        [SerializeField]
        [ReadOnly]
        private AudioSource mBGM = null;

        [Tooltip("Current background music is playing.")]
        [SerializeField]
        [ReadOnly]
        private AudioClip mCurrentBGM = null;

        // boolean check if the background music switching.
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
        public AudioSource GetBGMAudioSource()
        {
            if (this.mBGM == null)
                this.mBGM = JCS_BGMPlayer.instance.GetAudioSource();
            return this.mBGM;
        }
        public void SetBGM(AudioSource music)
        {
            this.mBGM = music;

            var ss = JCS_SoundSettings.instance;

            this.mBGM.volume = ss.GetBGM_Volume();
            this.mBGM.mute = ss.BGM_MUTE;
        }
        public JCS_Vec<AudioSource> GetEffectSounds() { return this.mSFXSounds; }
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

            mSFXSounds = new JCS_Vec<AudioSource>();
            mSkillsSounds = new JCS_Vec<AudioSource>();

            mGlobalSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        private void Start()
        {
            if (JCS_Camera.main == null)
            {
                JCS_Debug.LogError("There is no 'JCS_Camera' assign!");
                return;
            }

            var ss = JCS_SoundSettings.instance;

            // Reset the sound every scene
            SetEffectVolume(ss.GetEffect_Volume());
            SetSkillsSoundVolume(ss.GetSkill_Volume());
            SetSFXSoundMute(ss.EFFECT_MUTE);
            SetSkillsSoundMute(ss.PERFONAL_EFFECT_MUTE);
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
            AudioSource bgmAudioSource = GetBGMAudioSource();

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
        /// Push to the sound effect into array ready for use!
        /// </summary>
        /// <param name="sound"></param>
        public void PlayOneShotEffect(int index)
        {
            AudioSource aud = mSFXSounds.at(index);

            var ss = JCS_SoundSettings.instance;

            if (aud.clip != null)
                aud.PlayOneShot(aud.clip, ss.GetEffect_Volume());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="source"></param>
        public void AssignSoundSource(JCS_SoundSettingType type, AudioSource source)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM:
                    SetBGM(source);
                    break;
                case JCS_SoundSettingType.EFFECT:
                    AssignEffect(source);
                    break;
                case JCS_SoundSettingType.SKILL:
                    AssignSkill(source);
                    break;
            }
        }

        /// <summary>
        /// Set the sound volume base on type.
        /// </summary>
        /// <param name="type"> type of the sound you want to set. </param>
        /// <param name="volume"> volume of the sound. </param>
        public void SetVolume(JCS_SoundSettingType type, float volume)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM:
                    GetBGMAudioSource().volume = volume;
                    break;
                case JCS_SoundSettingType.EFFECT:
                    SetEffectVolume(volume);
                    break;
                case JCS_SoundSettingType.SKILL:
                    SetSkillsSoundVolume(volume);
                    break;
            }
        }

        /// <summary>
        /// Set weather the sound are mute or not by sound type.
        /// </summary>
        /// <param name="type"> type of the sound. </param>
        /// <param name="act"> action of the mute </param>
        public void SetMute(JCS_SoundSettingType type, bool act)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM:
                    GetBGMAudioSource().mute = act;
                    break;
                case JCS_SoundSettingType.EFFECT:
                    SetSFXSoundMute(act);
                    break;
                case JCS_SoundSettingType.SKILL:
                    SetSkillsSoundMute(act);
                    break;
            }
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
        /// Add a SFX in to the list in order to get manage.
        /// </summary>
        /// <param name="sound"> Unity's audio source class. </param>
        private void AssignEffect(AudioSource sound)
        {
            AssignSoundToList(mSFXSounds, sound);
        }

        /// <summary>
        /// Add a skill sound in to the list in order to get manage.
        /// </summary>
        /// <param name="sound"> Unity's audio source class. </param>
        private void AssignSkill(AudioSource sound)
        {
            AssignSoundToList(mSkillsSounds, sound);
        }

        /// <summary>
        /// Assgin the audio source to audio source list.
        /// </summary>
        /// <param name="list"> List of audio source. </param>
        /// <param name="sound"> audio source to add into list. </param>
        private void AssignSoundToList(JCS_Vec<AudioSource> list, AudioSource sound)
        {
            if (sound == null)
            {
                JCS_Debug.LogError("Assigning Source that is null...");
                return;
            }

            list.push(sound);
            sound.volume = JCS_SoundSettings.instance.GetEffect_Volume();
        }

        /// <summary>
        /// Set the SFX volume.
        /// </summary>
        /// <param name="vol"> volume to set. </param>
        private void SetEffectVolume(float vol)
        {
            SetVolume(mSFXSounds, vol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vol"></param>
        private void SetSkillsSoundVolume(float vol)
        {
            SetVolume(mSkillsSounds, vol);
        }

        /// <summary>
        /// Set the SFX mute or not mute.
        /// </summary>
        /// <param name="act"> target mute action. </param>
        private void SetSFXSoundMute(bool act)
        {
            SetMute(mSFXSounds, act);
        }

        /// <summary>
        /// Set the skill sound mute or not mute.
        /// </summary>
        /// <param name="act"> target mute action. </param>
        private void SetSkillsSoundMute(bool act)
        {
            SetMute(mSkillsSounds, act);
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
                AudioSource bgmAudioSource = GetBGMAudioSource();

                // set the audio source.
                mJCSFadeSound.SetAudioSource(bgmAudioSource);

                // set the bgm and play it
                bgmAudioSource.clip = this.mCurrentBGM;
                bgmAudioSource.Play();

                // active the fade sound in effect.
                mJCSFadeSound.FadeIn(
                    JCS_SoundSettings.instance.GetBGM_Volume(),
                    this.mRealSoundFadeOutTime);

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

            // do nothing if still playing
            if (GetBGMAudioSource().isPlaying)
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
