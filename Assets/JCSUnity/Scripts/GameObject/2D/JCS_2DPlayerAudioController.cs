/**
 * $File: JCS_2DPlayerAudioController.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Audio player for specific player type.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_2DPlayerAudioController : MonoBehaviour
    {
        /* Variables */

        protected AudioListener mAudioListener = null;

        // Audio Source equals to Audio Player!
        protected JCS_SoundPlayer mSoundPlayer = null;

        /* Setter & Getter */

        public AudioListener GetAudioListener() { return this.mAudioListener; }

        /* Functions */

        protected virtual void Awake()
        {
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            mAudioListener = this.GetComponent<AudioListener>();
        }

        protected virtual void Start()
        {
            JCS_SoundManager sm = JCS_SoundManager.instance;

            sm.SetAudioListener(GetAudioListener());

            // add it to Sound Manager so it could manage
            // the volume and mute!
            sm.AssignSoundSource(JCS_SoundSettingType.SKILLS_SOUND, mSoundPlayer.GetAudioSource());
        }
    }
}
