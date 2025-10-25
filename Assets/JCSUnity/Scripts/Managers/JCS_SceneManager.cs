/**
 * $File: JCS_SceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manage scenes changes.
    /// </summary>
    public class JCS_SceneManager : JCS_Manager<JCS_SceneManager>
    {
        /* Variables */

        public Action onSwitchSceneInit = null;
        public Action onSwitchSceneLoad = null;
        public Func<bool> onSwitchSceneIn = null;
        public Func<bool> onSwitchSceneOut = null;

        private bool mIsEnteringSwitchScene = false;

        [Separator("Check Variables (JCS_SceneManager)")]

        [Tooltip("Black screen object to be assigned by the system.")]
        [SerializeField]
        [ReadOnly]
        private JCS_BlackScreen mBlackScreen = null;

        [Tooltip("Black slide screen object to be assigned by the system.")]
        [SerializeField]
        [ReadOnly]
        private JCS_BlackSlideScreen mBlackSlideScreen = null;

        [Tooltip("The raw image used to play video transition.")]
        [SerializeField]
        [ReadOnly]
        private RawImage mRawImgVideoTransition = null;

        [Tooltip("Which direction to fade slide.")]
        [SerializeField]
        [ReadOnly]
        private JCS_Align mAlign = JCS_Align.ALIGN_LEFT;

        [Tooltip("Do u need the white screen in ur game?")]
        [SerializeField]
        [ReadOnly]
        private bool mPopWhiteScreen = false;

        [SerializeField]
        [ReadOnly]
        private JCS_WhiteScreen mWhiteScreen = null;

        [Tooltip("A list of loaded overlays.")]
        [SerializeField]
        [ReadOnly]
        private List<string> mLoadedOverlaySceneNames = null;

        // Executions after the overlay scene is loaded.
        private Dictionary<string, Action> mOnOverlaySceneLoaded = new();

        [Separator("Initialize Variables (JCS_SceneManager)")]

        [Tooltip("Type/Method to switch the scene.")]
        [SerializeField]
        private JCS_SwitchSceneType mSwitchSceneType = JCS_SwitchSceneType.FADE;

        [Header("Overlay")]

        [Tooltip("Load additive overlay scenes asynchronously.")]
        [SerializeField]
        private bool mOverlayUseAsync = false;

        [Tooltip("A list of addictive scene to load.")]
        [SerializeField]
        [Scene]
        private List<string> mOverlaySceneNames = null;

        [Separator("Runtime Variables (JCS_SceneManager)")]

        [Header("Transition")]

        [Tooltip("Do this scene using the specific setting.")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Fade in time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mTimeIn = 1.0f;

        [Tooltip("Fade out time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mTimeOut = 1.0f;

        [Tooltip("The video clip to play for fade in.")]
        [SerializeField]
        private VideoClip mClipIn = null;

        [Tooltip("The video clip to play for fade out.")]
        [SerializeField]
        private VideoClip mClipOut = null;

        // fade the sound while switching the scene.
        private JCS_FadeSound mFadeSound = null;

        // Scene in the game so is dynamic instead of Unity's scene system's scene
        // Unity 自帶就有Scene這個物件. 在這個Unity Scene裡面自己宣告Scene
        // 比起Unity的Scene比較動態, 因為自己宣告的操控比較多.
        private JCS_DynamicScene mDynamicScene = null;

        /* Setter & Getter */

        public JCS_SwitchSceneType switchSceneType { get { return mSwitchSceneType; } set { mSwitchSceneType = value; } }
        public List<string> overlaySceneNames { get { return mOverlaySceneNames; } set { mOverlaySceneNames = value; } }
        public bool overlayUseAsync { get { return mOverlayUseAsync; } set { mOverlayUseAsync = value; } }

        public JCS_DynamicScene GetDynamicScene() { return mDynamicScene; }
        public void SetDynamicScene(JCS_DynamicScene ds) { mDynamicScene = ds; }
        public JCS_WhiteScreen GetWhiteScreen() { return mWhiteScreen; }
        public JCS_BlackScreen GetBlackScreen() { return mBlackScreen; }

        public bool overrideSetting { get { return mOverrideSetting; } }
        public float timeIn { get { return mTimeIn; } set { mTimeIn = value; } }
        public float timeOut { get { return mTimeOut; } set { mTimeOut = value; } }
        public VideoClip clipIn { get { return mClipIn; } set { mClipIn = value; } }
        public VideoClip clipOut { get { return mClipOut; } set { mClipOut = value; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);

            HandleAdditive();

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.FADE:
                    {
                        mBlackScreen = JCS_UISettings.PopBlackScreen();
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE:
                    {
                        mBlackSlideScreen = JCS_UISettings.PopBlackSlideScreen();
                    }
                    break;

                case JCS_SwitchSceneType.VIDEO:
                    {
                        mRawImgVideoTransition = JCS_UISettings.PopVideoTransition();
                    }
                    break;
            }

            // Pop white screen depends on game needs.
            if (mPopWhiteScreen)
                mWhiteScreen = JCS_UISettings.PopWhiteScreen();

#if UNITY_EDITOR
            // add the tool in editor mode.
            gameObject.AddComponent<ReadSceneNames>();
#endif
        }

        private void Start()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // NOTE(jenchieh): get the fade out time base on  the scene setting
            // and scene manager specific.
            float fadeoutTime = ss.TimeIn();

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        onSwitchSceneInit?.Invoke();
                    }
                    break;

                case JCS_SwitchSceneType.FADE:
                    {
                        // get the current screen color.
                        mBlackScreen.localColor = ss.screenColor;

                        mBlackScreen.FadeOut(fadeoutTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE:
                    {
                        mBlackSlideScreen.StartSlideOut(mAlign, fadeoutTime);
                    }
                    break;

                case JCS_SwitchSceneType.VIDEO:
                    {
                        ss.videoPlayer.enabled = true;

                        ss.videoPlayer.clip = ss.ClipOut();

                        ss.videoPlayer.StepForward();

                        ss.videoPlayer.Play();
                    }
                    break;
            }

            // Only need to fade BGM when BGM is not switch between scene.
            var soundS = JCS_SoundSettings.FirstInstance();

            if (!soundS.keepBGMSwitchScene)
            {
                if (soundS.smoothSwithBetweenScene)
                {
                    // get the component.
                    if (mFadeSound == null)
                        mFadeSound = gameObject.AddComponent<JCS_FadeSound>();

                    AudioSource bgmAS = JCS_BGMPlayer.instance.audioSource;

                    // set the background audio source.
                    mFadeSound.SetAudioSource(bgmAS);

                    // active the fade sound in effect.
                    mFadeSound.FadeIn(
                        1.0f,
                        /* Fade in the sound base on the setting. */
                        soundS.TimeIn());
                }
            }
            else
            {
                // If the keep bgm is true, we disable it once  everytime a
                // scene is loaded.
                // 
                // ATTENTION(jenchieh): This should be place for the last use
                // of the variable 'KEEP_BGM_SWITCH_SCENE'.
                soundS.keepBGMSwitchScene = false;
            }

            // the game is loaded start the game agian
            JCS_GameManager.FirstInstance().gamePaused = false;
        }

        private void Update()
        {
            if (mIsEnteringSwitchScene)
                DoEnterSwitchScene();
            else
                DoExitSwitchScene();
        }

        /// <summary>
        /// Handle additive scene overlays.
        /// </summary>
        private void HandleAdditive()
        {
            foreach (string sceneName in mOverlaySceneNames)
            {
                if (mOverlayUseAsync)
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                else
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnOverlaySceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnOverlaySceneLoaded;
        }

        private void OnOverlaySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Overlay scene must be additive.
            if (mode != LoadSceneMode.Additive)
                return;

            string sceneName = scene.name;

            mLoadedOverlaySceneNames.Add(sceneName);

            // Execute event.
            if (mOnOverlaySceneLoaded.ContainsKey(sceneName))
                RegisterOverlaySceneLoaded(sceneName, mOnOverlaySceneLoaded[sceneName]);
        }

        /// <summary>
        /// Register an event call after the targeted overlay scene is loaded.
        /// </summary>
        /// <param name="sceneName"> The target overlay scene name. </param>
        /// <param name="evt"> The execution to call after the overlay scene is loaded. </param>
        public void RegisterOverlaySceneLoaded(string sceneName, Action evt)
        {
            // Already loaded, execute and return.
            if (mLoadedOverlaySceneNames.Contains(sceneName))
            {
                evt?.Invoke();

                return;
            }

            // Add one if not exists.
            if (!mOnOverlaySceneLoaded.ContainsKey(sceneName))
                mOnOverlaySceneLoaded.Add(sceneName, null);

            // Register event.
            mOnOverlaySceneLoaded[sceneName] += evt;
        }

        #region Load Scene

        /// <summary>
        /// Load the target scene.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            var ss = JCS_SceneSettings.FirstInstance();

            float fadeInTime = ss.TimeIn();
            Color screenColor = ss.screenColor;
            bool keepBGM = false;

            LoadScene(sceneName, mode, fadeInTime, screenColor, keepBGM);
        }
        public void LoadScene(string sceneName, LoadSceneMode mode,
            float fadeInTime, Color screenColor, bool keepBGM)
        {
#if UNITY_EDITOR
            // only do this in Editor Mode, 
            // this help level designer to do their job.
            if (!ReadSceneNames.CheckSceneAvailable(sceneName))
            {
                Debug.Log("Scene [" + sceneName + "] you want to load is not in the Build Setting");
                return;
            }
#endif

            var scs = JCS_SceneSettings.FirstInstance();

            // if is loading already, dont load it agian
            if (scs.switchingScene)
                return;

            // set the next scene name
            scs.nextSceneName = sceneName;

            scs.previousScene = SceneManager.GetActiveScene();
            scs.mode = mode;

            var apps = JCS_AppSettings.FirstInstance();

            if (apps.saveOnSwitchScene)
            {
                // do the saving.
                apps.onSaveAppData?.Invoke();
            }

            // Mark loading scene.
            mIsEnteringSwitchScene = true;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        onSwitchSceneLoad?.Invoke();
                    }
                    break;

                case JCS_SwitchSceneType.FADE:
                    {
                        // move to the last child in order
                        // to render the black screen in front of 
                        // any UI's GUI
                        mBlackScreen.MoveToTheLastChild();

                        // set the screen color.
                        // NOTE(jenchieh): always start with opacity the same 
                        // as previous.
                        screenColor.a = mBlackScreen.localColor.a;
                        mBlackScreen.localColor = screenColor;

                        // record down the screen color.
                        scs.screenColor = screenColor;

                        // start fading in (black screen)
                        mBlackScreen.FadeIn(fadeInTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE:
                    {
                        mBlackSlideScreen.MoveToTheLastChild();

                        mBlackSlideScreen.StartSlideIn(mAlign, fadeInTime);
                    }
                    break;

                case JCS_SwitchSceneType.VIDEO:
                    {
                        scs.videoPlayer.enabled = true;

                        scs.videoPlayer.clip = scs.ClipIn();

                        scs.videoPlayer.StepForward();

                        scs.videoPlayer.Play();
                    }
                    break;
            }

            var sos = JCS_SoundSettings.FirstInstance();

            sos.keepBGMSwitchScene = keepBGM;

            if (!keepBGM)
            {
                // start fading sound
                if (sos.smoothSwithBetweenScene)
                {
                    // get the component.
                    if (mFadeSound == null)
                        mFadeSound = gameObject.AddComponent<JCS_FadeSound>();

                    AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

                    mFadeSound.SetAudioSource(bgmAudioSource);

                    // fade out sound to zero
                    mFadeSound.FadeOut(0, fadeInTime);
                }
            }

            // start check to switch scene or not
            scs.switchingScene = true;
        }

        #endregion

        #region Reload Scene

        /// <summary>
        /// Reload the current scene.
        /// </summary>
        public void ReloadScene()
        {
            ReloadScene(LoadSceneMode.Single);
        }
        public void ReloadScene(LoadSceneMode mode)
        {
            var ss = JCS_SceneSettings.FirstInstance();

            float fadeInTime = ss.TimeIn();
            Color screenColor = ss.screenColor;
            bool keepBGM = false;

            // load scene and pass the value in.
            ReloadScene(mode, fadeInTime, screenColor, keepBGM);
        }
        public void ReloadScene(LoadSceneMode mode,
            float fadeInTime, Color screenColor, bool keepBGM)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            LoadScene(sceneName, mode, fadeInTime, screenColor, keepBGM);
        }

        #endregion

        #region Next Scene

        /// <summary>
        /// Return the next scene name.
        /// </summary>
        public static string NextSceneName(int offset = 1)
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + offset;

            string pathToScene = SceneUtility.GetScenePathByBuildIndex(nextIndex);
            string sceneName = Path.GetFileNameWithoutExtension(pathToScene);

            return sceneName;
        }

        /// <summary>
        /// Load the next scene.
        /// </summary>
        public void LoadNextScene()
        {
            LoadNextScene(LoadSceneMode.Single);
        }
        public void LoadNextScene(LoadSceneMode mode)
        {
            var ss = JCS_SceneSettings.FirstInstance();

            float fadeInTime = ss.TimeIn();
            Color screenColor = ss.screenColor;
            bool keepBGM = false;

            LoadNextScene(mode, fadeInTime, screenColor, keepBGM);
        }
        public void LoadNextScene(LoadSceneMode mode,
            float fadeInTime, Color screenColor, bool keepBGM)
        {
            string sceneName = NextSceneName();

            LoadScene(sceneName, mode, fadeInTime, screenColor, keepBGM);
        }

        #endregion

        /// <summary>
        /// Return the scene name by their options.
        /// </summary>
        /// <param name="sceneName"> The default scene name. </param>
        /// <param name="reload"> Reload it? </param>
        /// <param name="offset"> Offset for next scene's build index. </param>
        public static string GetSceneNameByOption(string sceneName, bool reload, int offset = 1)
        {
            if (reload)
            {
                return SceneManager.GetActiveScene().name;
            }
            else
            {
                if (sceneName.IsNullOrEmpty())
                    sceneName = NextSceneName(offset);
            }

            return sceneName;
        }

        /// <summary>
        /// Return a list of all scenes.
        /// </summary>
        public static List<Scene> GetAllScenes()
        {
            List<Scene> scenes = new();

            JCS_Loop.Times(SceneManager.sceneCount, (count) =>
            {
                Scene scene = SceneManager.GetSceneAt(count);

                scenes.Add(scene);
            });

            return scenes;
        }

        /// <summary>
        /// Check is loading the scene or not.
        /// </summary>
        /// <returns> return result. </returns>
        public bool IsSwitchingScene()
        {
            return JCS_SceneSettings.FirstInstance().switchingScene;
        }

        /// <summary>
        /// Do the async switch scene.
        /// </summary>
        private void DoEnterSwitchScene()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // check if during the switch scene?
            if (!ss.switchingScene)
                return;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        if (onSwitchSceneIn != null)
                        {
                            if (onSwitchSceneIn.Invoke())
                                EnterNextScene();
                        }
                    }
                    break;

                case JCS_SwitchSceneType.FADE:
                    {
                        if (mBlackScreen.IsFadeIn())
                            EnterNextScene();
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE:
                    {
                        if (mBlackSlideScreen.IsDoneSliding())
                            EnterNextScene();
                    }
                    break;

                case JCS_SwitchSceneType.VIDEO:
                    {
                        if (!ss.videoPlayer.isPlaying)
                        {
                            EnterNextScene();

                            ss.videoPlayer.enabled = false;
                        }
                    }
                    break;
            }
        }

        private void DoExitSwitchScene()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // check if during the switch scene?
            if (!ss.switchingScene)
                return;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        if (onSwitchSceneOut != null)
                        {
                            if (onSwitchSceneOut.Invoke())
                                ss.switchingScene = false;
                        }
                    }
                    break;

                case JCS_SwitchSceneType.FADE:
                    {
                        if (mBlackScreen.IsFadeOut())
                            ss.switchingScene = false;
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE:
                    {
                        if (mBlackSlideScreen.IsDoneSliding())
                            ss.switchingScene = false;
                    }
                    break;

                case JCS_SwitchSceneType.VIDEO:
                    {
                        if (!ss.videoPlayer.isPlaying)
                            ss.switchingScene = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Activate the next scene when it's ready.
        /// </summary>
        private void EnterNextScene()
        {
            // Delay a bit of time to make sure it's completely
            // fade out.
            Invoke(nameof(InvokeEnterNextScene), 0.01f);
        }
        private void InvokeEnterNextScene()
        {
            var scs = JCS_SceneSettings.FirstInstance();

            SceneManager.LoadScene(scs.nextSceneName, scs.mode);
        }
    }
}
