/**
 * $File: JCS_ApplicationManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Manager manage application layer.
    /// </summary>
    public class JCS_ApplicationManager
        : JCS_Managers<JCS_ApplicationManager>
    {
        /* Variables */

        public static bool APP_PAUSE = false;
        public static bool APP_QUITTING = false;
        public static bool APP_INITIALIZING = true;

        [Header("** Check Variables (JCS_ApplicationManager) **")]

        [Tooltip("Current systme language.")]
        [SerializeField]
        private SystemLanguage mSystemLanguage = SystemLanguage.Unknown;

        [Tooltip("List of language texts in game.")]
        [SerializeField]
        private List<JCS_LangText> mLangTexts = null;

        [Header("** Runtime Variables (JCS_ApplicationManager) **")]

        [Tooltip("This will override platform Type.")]
        [SerializeField]
        private bool mSimulatePlatformType = true;

        [Tooltip("Target platform type to simulate.")]
        [SerializeField]
        private JCS_PlatformType mPlatformType = JCS_PlatformType.PC;

        [Tooltip("If true, override current system language.")]
        [SerializeField]
        private bool mSimulateSystemLanguage = false;

        [Tooltip("Target language to simulate.")]
        [SerializeField]
        private SystemLanguage mSimulateLanguage = SystemLanguage.Unknown;

        /* Setter & Getter */

        public SystemLanguage systemLanguage
        {
            get { return this.mSystemLanguage; }
            set
            {
                this.mSystemLanguage = value;
                RefreshLangTexts();
            }
        }
        public JCS_PlatformType PlatformType
        {
            get { return this.mPlatformType; }
            set
            {
                if (mSimulatePlatformType) return;
                this.mPlatformType = value;
            }
        }

        /* Functions */

        private void OnApplicationQuit()
        {
            APP_QUITTING = true;

            JCS_GameSettings gs = JCS_GameSettings.instance;
            if (gs.SAVE_ON_EXIT_APP &&
                gs.SAVE_GAME_DATA_FUNC != null)
            {
                // save when exit app
                gs.SAVE_GAME_DATA_FUNC.Invoke();
            }
        }

        private void Awake()
        {
            instance = this;

            mSystemLanguage = (mSimulateSystemLanguage) ? mSimulateLanguage : Application.systemLanguage;

#if (UNITY_EDITOR || UNITY_STANDALONE)
            // set platform type
            PlatformType = JCS_PlatformType.PC;

#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            // set platform type
            PlatformType = JCS_PlatformType.MOBILE;

            // set the sleep time to never
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

            // done initialize the application layer.
            APP_INITIALIZING = false;
        }

        /// <summary>
        /// Return true if current platform is personal computer.
        /// </summary>
        public bool IsPC()
        {
            return (PlatformType == JCS_PlatformType.PC);
        }

        /// <summary>
        /// Return true if current platform is mobile phone.
        /// </summary>
        public bool IsMobile()
        {
            return (PlatformType == JCS_PlatformType.MOBILE);
        }

        /// <summary>
        /// Quit the application
        /// </summary>
        public void Quit(bool fade = true)
        {
            if (fade)
            {
                // load the quit scene.
                JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulate");
            }
            else
            {
                // quit the app directly,
                Application.Quit();
            }
        }

        /// <summary>
        /// Register a new language text.
        /// </summary>
        public void AddLangText(JCS_LangText txt)
        {
            this.mLangTexts.Add(txt);
        }

        /// <summary>
        /// Refresh all languages text in game.
        /// </summary>
        public void RefreshLangTexts()
        {
            this.mLangTexts = JCS_Utility.RemoveEmptySlotIncludeMissing(this.mLangTexts);
            foreach (JCS_LangText txt in mLangTexts)
                txt.Refresh();
        }
    }
}
