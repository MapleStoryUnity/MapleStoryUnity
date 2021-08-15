/**
 * $File: JCS_OnDestroyPlaySoundEvent.cs $
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
    /// When object is destroy, play the sound.
    /// </summary>
    public class JCS_OnDestroyPlaySoundEvent : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_OnDestroyPlaySoundEvent) **")]

        [Tooltip("Audio clip to plays.")]
        [SerializeField]
        private AudioClip mAudioClip = null;

        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        /* Setter & Getter */

        public void SetSoundSettingType(JCS_SoundSettingType type) { mSoundSettingType = type; }
        public void SetAudioClip(AudioClip ac) { this.mAudioClip = ac; }

        /* Functions */

        /// <summary>
        /// When is on destroy play the sound.
        /// </summary>
        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_ApplicationManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.instance.IsSwitchingScene())
                return;

            // create object to do the event
            GameObject obj = new GameObject();

            // this object will play the sound, after the sound is played.
            // will destroy it self automatically.
            JCS_DestroySoundEndEvent dsee = obj.AddComponent<JCS_DestroySoundEndEvent>();

#if (UNITY_EDITOR)
            obj.name = "JCS_OnDestroyPlaySoundEvent";
#endif
            dsee.SetAudioClipAndPlayOneShot(mAudioClip, mSoundSettingType);
        }
    }
}
