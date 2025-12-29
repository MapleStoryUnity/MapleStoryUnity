/**
 * $File: JCS_SceneSettings.cs $
 * $Date: 2016-10-28 13:59:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle the scene the better way.
    /// </summary>
    [RequireComponent(typeof(VideoPlayer))]
    public class JCS_SceneSettings : JCS_Settings<JCS_SceneSettings>
    {
        /* Variables */

        private VideoPlayer mVideoPlayer = null;

        [Separator("📋 Check Variabless (JCS_SceneSettings)")]

        [Tooltip("True when is switching scene.")]
        [ReadOnly]
        public bool switchingScene = false;

        [Tooltip("Next scene name to load.")]
        [ReadOnly]
        public string nextSceneName = "";

        [Tooltip("Previous scene mode.")]
        [ReadOnly]
        public Scene previousScene = default;

        [Tooltip("Load scene mode.")]
        [ReadOnly]
        public LoadSceneMode mode = LoadSceneMode.Single;

        [Separator("⚡️ Runtime Variables (JCS_SceneSettings)")]

        [Tooltip("General Scene fade in time. (For all scene)")]
        [Range(0.0f, 5.0f)]
        [SerializeField]
        private float mTimeIn = 1.5f;

        [Tooltip("General Scene fade out time. (For all scene)")]
        [Range(0.0f, 5.0f)]
        [SerializeField]
        private float mTimeOut = 1.5f;

        [Tooltip("Screen color to fade in/out the scene.")]
        public Color screenColor = Color.black;

        [Tooltip("The video clip to play for fade in.")]
        [SerializeField]
        private VideoClip mClipIn = null;

        [Tooltip("The video clip to play for fade out.")]
        [SerializeField]
        private VideoClip mClipOut = null;

        /* Setter & Getter */

        public VideoPlayer videoPlayer { get { return mVideoPlayer; } }

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            mVideoPlayer = GetComponent<VideoPlayer>();

            // 預設關閉的, Unity 不能同時撥放兩個以上的影片.
            mVideoPlayer.enabled = false;
        }

        /// <summary>
        /// Return the time for fade out the scene base on the settings.
        /// </summary>
        public float TimeOut()
        {
            var sm = JCS_SceneManager.FirstInstance();

            if (sm.overrideSetting)
                return sm.timeOut;

            return mTimeOut;
        }

        /// <summary>
        /// Return the time for fade in the scene base on the settings.
        /// </summary>
        public float TimeIn()
        {
            var sm = JCS_SceneManager.FirstInstance();

            if (sm.overrideSetting)
                return sm.timeIn;

            return mTimeIn;
        }

        /// <summary>
        /// Return the video clip for fading in.
        /// </summary>
        public VideoClip ClipIn()
        {
            var sm = JCS_SceneManager.FirstInstance();

            if (sm.overrideSetting)
                return sm.clipIn;

            return mClipIn;
        }

        /// <summary>
        /// Return the video clip for fading out.
        /// </summary>
        public VideoClip ClipOut()
        {
            var sm = JCS_SceneManager.FirstInstance();

            if (sm.overrideSetting)
                return sm.clipOut;

            return mClipOut;
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_SceneSettings _old, JCS_SceneSettings _new)
        {
            _new.switchingScene = _old.switchingScene;
            _new.nextSceneName = _old.nextSceneName;
            _new.previousScene = _old.previousScene;
            _new.mode = _old.mode;

            _new.mTimeIn = _old.mTimeIn;
            _new.mTimeOut = _old.mTimeOut;
            _new.screenColor = _old.screenColor;

            _new.mVideoPlayer = _old.mVideoPlayer;
            _new.mClipIn = _old.mClipIn;
            _new.mClipOut = _old.mClipOut;
        }
    }
}
