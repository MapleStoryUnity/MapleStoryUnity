/**
 * $File: JCS_BGMPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JCSUnity
{
    /// <summary>
    /// Sound Player that plays BGM.
    /// </summary>
    public class JCS_BGMPlayer : JCS_SoundPlayer
    {
        /* Variables */

        public static JCS_BGMPlayer instance = null;

        /* Setter & Getter */

        /* Functions */

#if !UNITY_5_4_OR_NEWER
        private void OnLevelWasLoaded(int level)
        {
            LevelLoaded();
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            // NOTE(jenchieh): Only the first time will call this.
            // This game object is a unique game object. Meaning the
            // object itself uses 'DontDestroyOnLoad' function.
            if (instance == null)
            {
                instance = this;

#if UNITY_5_4_OR_NEWER
                // ==> OnLevelWasLoaded <==
                SceneManager.sceneLoaded += (scene, loadingMode) =>
                {
                    LevelLoaded();
                };
#endif
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        /// On level was loaded callback.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadingMode"></param>
        private void LevelLoaded()
        {
            // In 2023.2.19f1, the loop will be turned off for some reason.
            // Let's force BGM to run loop!
            mAudioSource.loop = true;

            var ss = JCS_SoundSettings.instance;

            if (!ss.KEEP_BGM_SWITCH_SCENE)
            {
                // Assign BGM from Sound Manager!
                mAudioSource.clip = ss.BACKGROUND_MUSIC;

                mAudioSource.Play();
            }
        }
    }
}
