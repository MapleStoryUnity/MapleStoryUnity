/**
 * $File: JCS_AppManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2015 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace JCSUnity
{
    public delegate void OnSystemLanguageChanged(SystemLanguage language);

    /// <summary>
    /// Manager manage application layer.
    /// </summary>
    public class JCS_AppManager : JCS_Manager<JCS_AppManager>
    {
        /* Variables */

        public static bool APP_PAUSE = false;
        public static bool APP_QUITTING = false;
        public static bool APP_INITIALIZING = true;

        // Execute after the system language has changed.
        public OnSystemLanguageChanged onSystemLanguageChanged = null;

        [Separator("Check Variables (JCS_AppManager)")]

        [Tooltip("Current systme language.")]
        [SerializeField]
        [ReadOnly]
        private SystemLanguage mSystemLanguage = SystemLanguage.Unknown;

        [Tooltip("List of language texts in game.")]
        [SerializeField]
        [ReadOnly]
        private List<JCS_LangText> mLangTexts = null;

        [Separator("Initialize Variables (JCS_AppManager)")]

        [Tooltip("Request permission for camera/webcam.")]
        [SerializeField]
        private bool mRequestCamera = false;

        [Tooltip("Request permission for microphone.")]
        [SerializeField]
        private bool mRequestMicrophone = false;

        [Tooltip("Request permission for location service.")]
        [SerializeField]
        private bool mRequestLocation = false;

        [Separator("Runtime Variables (JCS_AppManager)")]

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

        public bool SimulatePlatformType { get { return this.mSimulatePlatformType; } set { this.mSimulatePlatformType = value; } }
        public JCS_PlatformType PlatformType
        {
            get { return this.mPlatformType; }
            set
            {
                if (mSimulatePlatformType) return;
                this.mPlatformType = value;
            }
        }
        public bool SimulateSystemLanguage { get { return this.mSimulateSystemLanguage; } set { this.mSimulateSystemLanguage = value; } }
        public SystemLanguage systemLanguage
        {
            get { return this.mSystemLanguage; }
            set
            {
                this.mSystemLanguage = value;
                RefreshLangTexts();

                if (onSystemLanguageChanged != null)
                    onSystemLanguageChanged.Invoke(value);
            }
        }
        public bool RequestCamera { get { return this.mRequestCamera; } }
        public bool RequestMicrophone { get { return this.mRequestMicrophone; } }
        public bool RequestLocation { get { return this.mRequestLocation; } }

        /* Functions */

        private void OnApplicationQuit()
        {
            APP_QUITTING = true;

            var apps = JCS_AppSettings.instance;

            if (apps.SAVE_ON_EXIT_APP && apps.SAVE_APP_DATA_FUNC != null)
            {
                // save when exit app
                apps.SAVE_APP_DATA_FUNC.Invoke();
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

            StartRequestPermissions();

            // done initialize the application layer.
            APP_INITIALIZING = false;
        }

        public bool IsPC() { return (PlatformType == JCS_PlatformType.PC); }
        public bool IsMobile() { return (PlatformType == JCS_PlatformType.MOBILE); }

        /// <summary>
        /// Quit the application
        /// </summary>
        public void Quit(bool fade = true)
        {
            if (fade)
            {
                // load the quit scene.
                var scenem = JCS_SceneManager.instance;
                scenem.LoadScene("JCS_AppCloseSimulate");
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
            this.mLangTexts = JCS_Util.RemoveEmptySlotIncludeMissing(this.mLangTexts);
            foreach (JCS_LangText txt in mLangTexts)
                txt.Refresh();
        }

        public void StartRequestCamera() { StartCoroutine("DoRequestCamera"); }
        public void StartRequestMicrophone() { StartCoroutine("DoRequestMicrophone"); }
        public void StartRequestLocation() { StartCoroutine("DoRequestLocation"); }

        private IEnumerator DoRequestCamera()
        {
#if UNITY_EDITOR
            // Does nothing..
#elif UNITY_ANDROID
            yield return DoRequestUserPermission(Permission.Camera);
#elif UNITY_IOS
            yield return DoRequestUserAuthorization(UserAuthorization.WebCam);
#endif
            yield break;
        }

        private IEnumerator DoRequestMicrophone()
        {
#if UNITY_EDITOR
            // Does nothing..
#elif UNITY_ANDROID
            yield return DoRequestUserPermission(Permission.Microphone);
#elif UNITY_IOS
            yield return DoRequestUserAuthorization(UserAuthorization.Microphone);
#endif
            yield break;
        }

        private IEnumerator DoRequestLocation()
        {
#if UNITY_EDITOR
            // Does nothing..
#elif UNITY_ANDROID
            yield return DoRequestUserPermission(Permission.FineLocation);
#elif UNITY_IOS
            Input.location.Start();
#endif
            yield break;
        }

#if UNITY_EDITOR
        // Does nothing..
#elif UNITY_ANDROID
        private IEnumerator DoRequestUserPermission(string permission, PermissionCallbacks callback = null)
        {
            if (Permission.HasUserAuthorizedPermission(permission))
                yield break;

            Permission.RequestUserPermission(permission, callback);
        }
#elif UNITY_IOS
        private IEnumerator DoRequestUserAuthorization(UserAuthorization ua)
        {
            if (Application.HasUserAuthorization(ua))
                yield break;

            yield return Application.RequestUserAuthorization(ua);
        }
#endif

        /// <summary>
        /// Iterate through all the services, and enable it by settings.
        /// </summary>
        public void StartRequestPermissions()
        {
            StartCoroutine("DoRequestPermissions");
        }
        private IEnumerator DoRequestPermissions()
        {
            if (mRequestCamera)
                yield return DoRequestCamera();

            if (mRequestMicrophone)
                yield return DoRequestMicrophone();

            if (mRequestLocation)
                yield return DoRequestLocation();
        }
    }
}
