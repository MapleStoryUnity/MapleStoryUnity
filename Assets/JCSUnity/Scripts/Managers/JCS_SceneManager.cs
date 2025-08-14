/**
 * $File: JCS_SceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        // Async loading scene operation. (thread)
        private AsyncOperation mAsyncOperation = null;

        [Separator("Check Variables (JCS_SceneManager)")]

        [Tooltip("Black screen object to be assigned by the system.")]
        [SerializeField]
        [ReadOnly]
        private JCS_BlackScreen mBlackScreen = null;

        [Tooltip("Black slide screen object to be assigned by the system.")]
        [SerializeField]
        [ReadOnly]
        private JCS_BlackSlideScreen mBlackSlideScreen = null;

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

        [Separator("Initialize Variables (JCS_SceneManager)")]

        [Tooltip("Type/Method to switch the scene.")]
        [SerializeField]
        private JCS_SwitchSceneType mSwitchSceneType = JCS_SwitchSceneType.BLACK_SCREEN;

        [Separator("Runtime Variables (JCS_SceneManager)")]

        [Header("- Screen")]

        [Tooltip("Do this scene using the specific setting.")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Fade in time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mSceneFadeInTime = 1.0f;

        [Tooltip("Fade out time. (For this scene)")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mSceneFadeOutTime = 1.0f;

        // fade the sound while switching the scene.
        private JCS_FadeSound mFadeSound = null;

        // Scene in the game so is dynamic instead of Unity's scene system's scene
        // Unity 自帶就有Scene這個物件. 在這個Unity Scene裡面自己宣告Scene
        // 比起Unity的Scene比較動態, 因為自己宣告的操控比較多.
        private JCS_DynamicScene mDynamicScene = null;

        /* Setter & Getter */

        public JCS_SwitchSceneType switchSceneType { get { return this.mSwitchSceneType; } set { this.mSwitchSceneType = value; } }
        public JCS_DynamicScene GetDynamicScene() { return this.mDynamicScene; }
        public void SetDynamicScene(JCS_DynamicScene ds) { this.mDynamicScene = ds; }
        public void SetBlackScreen(JCS_BlackScreen bs) { this.mBlackScreen = bs; }
        public void SetBlackSlideScreen(JCS_BlackSlideScreen bs) { this.mBlackSlideScreen = bs; }
        public void SetWhiteScreen(JCS_WhiteScreen ws) { this.mWhiteScreen = ws; }
        public JCS_WhiteScreen GetWhiteScreen() { return this.mWhiteScreen; }
        public JCS_BlackScreen GetBlackScreen() { return this.mBlackScreen; }

        public bool overrideSetting { get { return this.mOverrideSetting; } }
        public float sceneFadeInTime { get { return this.mSceneFadeInTime; } set { this.mSceneFadeInTime = value; } }
        public float sceneFadeOutTime { get { return this.mSceneFadeOutTime; } set { this.mSceneFadeOutTime = value; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        JCS_UtilFunctions.PopBlackScreen();
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        JCS_UtilFunctions.PopBlackSlideScreen();
                    }
                    break;
            }

            // Pop white screen depends on game needs.
            if (mPopWhiteScreen)
                JCS_UtilFunctions.PopWhiteScreen();

#if UNITY_EDITOR
            // add the tool in editor mode.
            this.gameObject.AddComponent<ReadSceneNames>();
#endif
        }

        private void Start()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // NOTE(jenchieh): get the fade out time base on  the scene setting
            // and scene manager specific.
            float fadeoutTime = ss.SceneFadeInTimeBaseOnSetting();

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        onSwitchSceneInit?.Invoke();
                    }
                    break;
                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        // get the current screen color.
                        mBlackScreen.localColor = ss.SCREEN_COLOR;

                        mBlackScreen.FadeOut(fadeoutTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        mBlackSlideScreen.StartSlideOut(mAlign, fadeoutTime);
                    }
                    break;
            }

            // Only need to fade BGM when BGM is not switch between scene.
            var soundS = JCS_SoundSettings.FirstInstance();

            if (!soundS.KEEP_BGM_SWITCH_SCENE)
            {
                if (soundS.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
                {
                    // get the component.
                    if (mFadeSound == null)
                        mFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();

                    AudioSource bgmAS = JCS_BGMPlayer.instance.audioSource;

                    // set the background audio source.
                    mFadeSound.SetAudioSource(bgmAS);

                    // active the fade sound in effect.
                    mFadeSound.FadeIn(
                        1.0f,
                        /* Fade in the sound base on the setting. */
                        soundS.GetSoundFadeInTimeBaseOnSetting());
                }
            }
            else
            {
                // If the keep bgm is true, we disable it once  everytime a
                // scene is loaded.
                // 
                // ATTENTION(jenchieh): This should be place for the last use
                // of the variable 'KEEP_BGM_SWITCH_SCENE'.
                soundS.KEEP_BGM_SWITCH_SCENE = false;
            }

            // the game is loaded start the game agian
            JCS_GameManager.FirstInstance().gamePaused = false;
        }

        private void Update()
        {
            if (IsEnteringSwitchScene())
                DoEnterSwitchScene();
            else
                DoExitSwitchScene();
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

            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();
            Color screenColor = ss.SCREEN_COLOR;
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
            if (scs.SWITCHING_SCENE)
                return;

            // set the next scene name
            scs.NEXT_SCENE_NAME = sceneName;

            scs.PREVIOUS_SCENE = SceneManager.GetActiveScene();
            scs.MODE = mode;

            var apps = JCS_AppSettings.FirstInstance();

            if (apps.SAVE_ON_SWITCH_SCENE)
            {
                // do the saving.
                apps.SAVE_APP_DATA_FUNC?.Invoke();
            }

            // preload the scene
            mAsyncOperation = SceneManager.LoadSceneAsync(scs.NEXT_SCENE_NAME, mode);
            mAsyncOperation.allowSceneActivation = false;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        onSwitchSceneLoad?.Invoke();
                    }
                    break;

                case JCS_SwitchSceneType.BLACK_SCREEN:
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
                        JCS_SceneSettings.FirstInstance().SCREEN_COLOR = screenColor;

                        // start fading in (black screen)
                        mBlackScreen.FadeIn(fadeInTime);
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        mBlackSlideScreen.MoveToTheLastChild();

                        mBlackSlideScreen.StartSlideIn(mAlign, fadeInTime);
                    }
                    break;
            }

            var sos = JCS_SoundSettings.FirstInstance();

            sos.KEEP_BGM_SWITCH_SCENE = keepBGM;

            if (!keepBGM)
            {
                // start fading sound
                if (sos.SMOOTH_SWITCH_SOUND_BETWEEN_SCENE)
                {
                    // get the component.
                    if (mFadeSound == null)
                        mFadeSound = this.gameObject.AddComponent<JCS_FadeSound>();

                    AudioSource bgmAudioSource = JCS_BGMPlayer.instance.audioSource;

                    mFadeSound.SetAudioSource(bgmAudioSource);

                    // fade out sound to zero
                    mFadeSound.FadeOut(0, fadeInTime);
                }
            }

            // start check to switch scene or not
            scs.SWITCHING_SCENE = true;
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

            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();
            Color screenColor = ss.SCREEN_COLOR;
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

            float fadeInTime = ss.SceneFadeInTimeBaseOnSetting();
            Color screenColor = ss.SCREEN_COLOR;
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
            return JCS_SceneSettings.FirstInstance().SWITCHING_SCENE;
        }

        /// <summary>
        /// Do the async switch scene.
        /// </summary>
        private void DoEnterSwitchScene()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // check if during the switch scene?
            if (!ss.SWITCHING_SCENE)
                return;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        if (onSwitchSceneIn != null)
                        {
                            if (onSwitchSceneIn.Invoke())
                            {
                                AllowSceneActivation();
                            }
                        }
                    }
                    break;

                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        if (mBlackScreen.IsFadeIn())
                        {
                            AllowSceneActivation();
                        }
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        if (mBlackSlideScreen.IsDoneSliding())
                        {
                            AllowSceneActivation();
                        }
                    }
                    break;
            }
        }

        private void DoExitSwitchScene()
        {
            var ss = JCS_SceneSettings.FirstInstance();

            // check if during the switch scene?
            if (!ss.SWITCHING_SCENE)
                return;

            switch (mSwitchSceneType)
            {
                case JCS_SwitchSceneType.CUSTOM:
                    {
                        if (onSwitchSceneOut != null)
                        {
                            if (onSwitchSceneOut.Invoke())
                                ss.SWITCHING_SCENE = false;
                        }
                    }
                    break;

                case JCS_SwitchSceneType.BLACK_SCREEN:
                    {
                        if (mBlackScreen.IsFadeOut())
                        {
                            ss.SWITCHING_SCENE = false;
                        }
                    }
                    break;

                case JCS_SwitchSceneType.SLIDE_SCREEN:
                    {
                        if (mBlackSlideScreen.IsDoneSliding())
                        {
                            ss.SWITCHING_SCENE = false;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Activate the next scene when it's ready.
        /// </summary>
        private void AllowSceneActivation()
        {
            // load the scene if is ready
            mAsyncOperation.allowSceneActivation = true;
        }

        /// <summary>
        /// Return true if we are still in the entering switch scene state.
        /// </summary>
        private bool IsEnteringSwitchScene()
        {
            return mAsyncOperation != null && !mAsyncOperation.allowSceneActivation;
        }
    }
}
