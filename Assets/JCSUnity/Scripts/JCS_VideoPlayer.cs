#if !(UNITY_2018_2_OR_NEWER)
/**
 * $File: JCS_VideoPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using UnityEngine;
using MyBox;

/*
 * JCSUnity.JCS_VideoPlayer's MovieTexture is deprecated and no 
 * longer supported in Unity 2018.2 or newer versions.
 *  
 * TODO(jenchieh): make this compatible to the newest version of 
 * Unity Engine, or just leave it as legacy video player solution.
 */

namespace JCSUnity
{
    /* for PC only */
#if (UNITY_STANDALONE)      
    /// <summary>
    /// Play a video and jump into a scene.
    /// 
    /// PC version.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_VideoPlayer : JCS_UnityObject
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_VideoPlayer)")]

        [Tooltip("Moive playback.")]
        [SerializeField]
        [ReadOnly]
        private MovieTexture mMovieTexture = null;

        [Tooltip("")]
        [SerializeField]
        private AudioSource mAudioSource = null;

        [Tooltip("After play once the moive load the next scene?")]
        [SerializeField]
        private bool mLoadSceneAfterPlay = true;

        [Tooltip("Next scene will be loaded.")]
        [SerializeField]
        private string mSceneName = "JCS_AppCloseSimulate";

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Header("- Delay Playback")]

        [Tooltip("How long it delay before to play the clip.")]
        [SerializeField]
        [Range(0, 3000)]
        private float mDelayPlayTime = 0;

        // timer to delay to play the clip.
        private float mDelayPlayTimer = 0;

        // check if the clip played?
        private bool mClipPlayed = false;

        /* Setter & Getter */

        public bool loop
        {
            get
            {
                if (mMovieTexture == null)
                    return false;

                return (mMovieTexture.loop && mAudioSource.loop);
            }
            set
            {
                if (mMovieTexture == null)
                    return;

                mMovieTexture.loop = value;
                mAudioSource.loop = value;
            }
        }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mAudioSource = this.GetComponent<AudioSource>();

            LocalMainTexture = mMovieTexture;

            if (mMovieTexture != null)
                mAudioSource.clip = mMovieTexture.audioClip;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            DoDelayPlayClip();

            // check if done playing 
            // the movie and load the next scene.
            LoadNextScene();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.P))
                Play();
            if (JCS_Input.GetKeyDown(KeyCode.O))
                Pause();
        }
#endif

        /// <summary>
        /// Play the video.
        /// </summary>
        public void Play()
        {
            if (mMovieTexture == null)
                return;

            if (mMovieTexture.isPlaying)
                return;

            mMovieTexture.Play();
            mAudioSource.Play();
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void Pause()
        {
            if (mMovieTexture == null)
                return;

            mMovieTexture.Pause();
            mAudioSource.Pause();
        }

        /// <summary>
        /// Stop playing video.
        /// </summary>
        public void Stop()
        {
            if (mMovieTexture == null)
                return;

            mMovieTexture.Stop();
            mAudioSource.Stop();
        }

        /// <summary>
        /// Check if the moive dont playing, 
        /// load the next scene.
        /// </summary>
        private void LoadNextScene()
        {
            // if the clip have not played, skip.
            if (!mClipPlayed)
                return;

            if (!mLoadSceneAfterPlay)
                return;

            if (mMovieTexture == null)
                return;

            // if still playing
            if (mMovieTexture.isPlaying)
                return;

            // load the scene.
            JCS_SceneManager.instance.LoadScene(mSceneName);

            // unload the movie
            mMovieTexture = null;
        }

        /// <summary>
        /// Delay amount of time then play the video
        /// </summary>
        private void DoDelayPlayClip()
        {
            if (mClipPlayed)
                return;

            mDelayPlayTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            if (mDelayPlayTimer < mDelayPlayTime)
                return;

            // play the video
            Play();

            mClipPlayed = true;
        }

    }
#endif
    /* for phone and other device. */
#if (UNITY_IOS) || (UNITY_ANDROID)
    /// <summary>
    /// Play a video and jump into a scene.
    /// 
    /// Mobile version.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_VideoPlayer : JCS_UnityObject
    {
        /* Variables */

        [Separator("Check Variables (JCS_VideoPlayer)")]

        [Tooltip("Full path of the clip")]
        [SerializeField]
        [ReadOnly]
        private string mFullPath = "";

        [Tooltip("")]
        [SerializeField]
        [ReadOnly]
        private AudioSource mAudioSource = null;

        [Separator("Runtime Variables (JCS_VideoPlayer)")]

        [Tooltip("Moive playback.")]
        [SerializeField]
        private string mMovieName = "";

        [Tooltip("Extension of the movie. If there is one.")]
        [SerializeField]
        private string mMovieExtension = ".mp4";

        [Tooltip("Type of input handle during clip playing.")]
        [SerializeField]
        private FullScreenMovieControlMode mFullScreenMovieControlMode = FullScreenMovieControlMode.Full;

        [Tooltip("Background color.")]
        [SerializeField]
        private Color mBGColor = Color.black;

        [Tooltip("Next scene will be loaded.")]
        [SerializeField]
        private string mSceneName = "JCS_AppCloseSimulate";

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Header("- Delay Playback")]

        [Tooltip("How long it delay before to play the clip.")]
        [SerializeField]
        [Range(0, 3000)]
        private float mDelayPlayTime = 0;

        // timer to delay to play the clip.
        private float mDelayPlayTimer = 0;

        // check if the clip played?
        private bool mClipPlayed = false;

        [Header("*Usage: plz fill the clip time until next scene.")]

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField]
        [Range(0, 59)]
        private float mClipHour = 0;

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField]
        [Range(0, 59)]
        private float mClipMinute = 0;

        [Tooltip("Time to load next scene, in seconds.")]
        [SerializeField]
        [Range(0, 59)]
        private float mClipSecond = 0;

        // real time = mClipHour * 3600 + mClipMinute * 60 + mClipSecond
        private float mLoadNextSceneTime = 1;

        // timer to load next scene
        private float mLoadNextSceneTimer = 0;

        /* Setter & Getter */

        public bool loop
        {
            get
            {
                JCS_Debug.LogError("Loop does not work in Andriod Platform...");

                return false;
            }
            set
            {
                JCS_Debug.LogError("Loop does not work in Andriod Platform...");
            }
        }
        public AudioSource AudioSource { get { return this.mAudioSource; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mAudioSource = this.GetComponent<AudioSource>();

            // load the next scene.
            mLoadNextSceneTime =
                (mClipHour * 3600) +
                (mClipMinute * 60) +
                mClipSecond;
        }


        private void Update()
        {
            DoDelayPlayClip();

            LoadNextScene();
        }

        /// <summary>
        /// Play the video.
        /// </summary>
        public void Play()
        {
            // get the full path of the clip
            mFullPath =
                mMovieName +
                mMovieExtension;

            // play the screen movie.
            Handheld.PlayFullScreenMovie(
               mFullPath,
               mBGColor,
               mFullScreenMovieControlMode);
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void Pause()
        {
            JCS_Debug.LogError("Pause does not work in Andriod Platform...");
        }

        /// <summary>
        /// Stop playing video.
        /// </summary>
        public void Stop()
        {
            JCS_Debug.LogError("Stop does not work in Andriod Platform...");
        }

        /// <summary>
        /// Check if the moive dont playing, 
        /// load the next scene.
        /// </summary>
        private void LoadNextScene()
        {
            if (mClipPlayed)
                return;

            // start the load scene timer.
            mLoadNextSceneTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            // check if the clips ends.
            if (mLoadNextSceneTimer < mLoadNextSceneTime)
                return;

            // load the scene.
            JCS_SceneManager.instance.LoadScene(mSceneName);
        }

        /// <summary>
        /// Delay amount of time then play the video
        /// </summary>
        private void DoDelayPlayClip()
        {
            if (mClipPlayed)
                return;

            mDelayPlayTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            if (mDelayPlayTimer < mDelayPlayTime)
                return;

            // play the video
            Play();

            mClipPlayed = true;
        }

    }
#endif
    /* for browser WebGL */
#if (UNITY_WEBGL)
    /// <summary>
    /// Play a video and jump into a scene.
    /// 
    /// Browser/WebGL version.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_VideoPlayer : JCS_UnityObject
    {
        // TODO(jenchieh): complete this..
    }
#endif
}

#endif
