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

        public AudioListener GetAudioListener() { return mAudioListener; }

        /* Functions */

        protected virtual void Awake()
        {
            mSoundPlayer = GetComponent<JCS_SoundPlayer>();
            mAudioListener = GetComponent<AudioListener>();
        }

        protected virtual void Start()
        {
            var sm = JCS_SoundManager.FirstInstance();

            sm.SetAudioListener(GetAudioListener());
        }
    }
}
