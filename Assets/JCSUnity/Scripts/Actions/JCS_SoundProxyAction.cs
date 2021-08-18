/**
 * $File: JCS_SoundProxyAction.cs $
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
    /// Action spawns a new gameobject and play the sound, after the 
    /// sound is played the gameobject will be destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_DestroyReminder))]
    public class JCS_SoundProxyAction : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_SoundProxyAction) **")]

        [Tooltip("Sound to plays.")]
        [SerializeField]
        private AudioClip mAudioClip = null;

        [Tooltip("Sound settings type.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        /* Setter & Getter */

        public AudioClip audioClip { get { return this.mAudioClip; } set { this.mAudioClip = value; } }
        public JCS_SoundSettingType SoundSettingType { get { return this.mSoundSettingType; } set { this.mSoundSettingType = value; } }
        
        /* Functions */

        private void Start()
        {
            GameObject obj = new GameObject();
            JCS_DestroySoundEndEvent dsee = obj.AddComponent<JCS_DestroySoundEndEvent>();

#if (UNITY_EDITOR)
            obj.name = "JCS_SoundProxyAction";
#endif

            if (mAudioClip == null)
            {
                JCS_Debug.LogError(
                    "You called a proxy action but without data in it...");
                return;
            }

            dsee.SetAudioClipAndPlayOneShot(mAudioClip, mSoundSettingType);
        }
    }
}
