/**
 * $File: JCS_SoundProxyAction.cs $
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
    /// Action spawns a new game object and play the sound, after the 
    /// sound is played the game object will be destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_DestroyReminder))]
    public class JCS_SoundProxyAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_SoundProxyAction)")]

        [Tooltip("Sound to plays.")]
        [SerializeField]
        private AudioClip mAudioClip = null;

        /* Setter & Getter */

        public AudioClip audioClip { get { return this.mAudioClip; } set { this.mAudioClip = value; } }
        
        /* Functions */

        private void Start()
        {
            var obj = new GameObject();
            var dsee = obj.AddComponent<JCS_DestroySoundEndEvent>();

#if UNITY_EDITOR
            obj.name = "JCS_SoundProxyAction";
#endif

            if (mAudioClip == null)
            {
                Debug.LogError(
                    "You called a proxy action but without data in it");
                return;
            }

            dsee.SetAudioClipAndPlayOneShot(mAudioClip);
        }
    }
}
