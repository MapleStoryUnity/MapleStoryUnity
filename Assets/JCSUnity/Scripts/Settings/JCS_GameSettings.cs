/**
 * $File: JCS_GameSettings.cs $
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
    /// Hold the general game setting.
    /// </summary>
    public class JCS_GameSettings : JCS_Settings<JCS_GameSettings>
    {
        /* Variables */

        [Separator("📋 Check Variabless (JCS_GameSettings)")]

        [SerializeField]
        [ReadOnly]
        private string fullDataPath = "";

        [SerializeField]
        [ReadOnly]
        private string fullScreenshotPath = "";

        [SerializeField]
        [ReadOnly]
        private string fullWebcamPath = "";

        [SerializeField]
        [ReadOnly]
        private string fullStreamingCachePath = "";

        [Separator("⚡️ Runtime Variables (JCS_GameSettings)")]

        [Tooltip("Debug mode flag.")]
        public bool debugMode = false;

        [Tooltip("Gravity production. (For game that have gravity in it)")]
        public float gravityProduct = 4.5f;

        [Header("🔍 Camera")]

        [Tooltip("Type of the camera.")]
        public JCS_CameraType cameraType = JCS_CameraType.NONE;

        [Header("🔍 Player")]

        [Tooltip("Game only allows control one player.")]
        public bool activeOnePlayer = true;

        [Header("🔍 Collision")]

        [Tooltip("Do collusion happen with eacth other. (Player)")]
        public bool playerIgnoreEachOther = true;

        [Tooltip("Can tribe damage each other?")]
        public bool tribeDamageEachOther = false;

        [Header("🔍 Resources")]

        [Tooltip("Base URL for streaming assets, please point to a directory.")]
        public string streamingBaseUrl = "https://wwww.example.com/";

        [Tooltip("Cache streaming assets' data path.")]
        public string streamingCachePath = "/Data_jcs/Cache_StreamingAssets/";

        [Header("🔍 Screenshot")]

        [Tooltip("Screenshot folder path.")]
        public string screenshotPath = "/Data_jcs/Screenshot/";

        [Tooltip("Screenshot file name.")]
        public string screenshotFilename = "Screenshot_";

        [Tooltip("Screenshot image extension.")]
        public string screenshotExt = ".png";

        [Header("🔍 Webcam")]

        [Tooltip("Webcam image save path.")]
        public string webcamPath = "/Data_jcs/WebcamShot/";

        [Tooltip("Webcam file name.")]
        public string webcamFilename = "";

        [Tooltip("Webcam image extension.")]
        public string webcamExt = ".png";

        [Header("🔍 Damage")]

        [Tooltip("Mininum damage can be in the game.")]
        public int minDamage = 1;

        [Tooltip("Maxinum damage can be in the game.")]
        public int maxDamage = 999999;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            fullDataPath = JCS_AppData.SavePath();
            fullScreenshotPath = JCS_Camera.SavePath();
            fullWebcamPath = JCS_Webcam.SavePath();
            fullStreamingCachePath = JCS_StreamingAssets.CachePath();

            JCS_IO.CreateDirectory(fullDataPath);
            JCS_IO.CreateDirectory(fullScreenshotPath);
            JCS_IO.CreateDirectory(fullWebcamPath);
            JCS_IO.CreateDirectory(fullStreamingCachePath);
        }

        private void Update()
        {
            JCS_Input.MouseDeltaPosition();
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_GameSettings _old, JCS_GameSettings _new)
        {
            // Debug check
            _new.debugMode = _old.debugMode;

            _new.streamingBaseUrl = _old.streamingBaseUrl;
            _new.streamingCachePath = _old.streamingCachePath;

            _new.screenshotPath = _old.screenshotPath;
            _new.screenshotFilename = _old.screenshotFilename;
            _new.screenshotExt = _old.screenshotExt;

            _new.webcamPath = _old.webcamPath;
            _new.webcamFilename = _old.webcamFilename;
            _new.webcamExt = _old.webcamExt;
        }
    }
}
