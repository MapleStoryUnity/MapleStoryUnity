/**
 * $File: JCS_Webcam.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Webcam object. Must have the texture on it in order to render.
    /// </summary>
    public class JCS_Webcam : JCS_UnityObject
    {
        /* Variables */

        private bool mDetectDevice = true;

        private string mDeviceName = "";

        private WebCamTexture mWebCamTexture = null;

        private float mResumeTimer = 0.0f;

        private bool mResumeTrigger = false;

        [Separator("⚡️ Runtime Variables (JCS_Webcam)")]

        [Tooltip("Manually preserve the size in scene.")]
        [SerializeField]
        private bool mManuallySetSize = false;

        [Tooltip("Make webcam maximize to the widest edge.")]
        [SerializeField]
        private bool mMustBeFullScreen = false;

        [Tooltip("FPS for webcam.")]
        [SerializeField]
        [Range(1, 120)]
        private int mFPS = 12;

        [Tooltip("After the screenshot is taken, how fast to resume the webcam.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mResumeTime = 3.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("🔍 Effect")]

        [Tooltip("Do the splash effect?")]
        [SerializeField]
        private bool mSplash = true;

        // Is the splash effect triggered?
        private bool mSplashEffectTrigger = false;

        [Tooltip("Delay time to fade out the splash screen.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mDelayTime = 1.0f;

        private float mDelayTimer = 0.0f;

#if (UNITY_STANDALONE || UNITY_EDITOR)
        [Header("🔍 Keys")]

        [Tooltip("Key to take webcam image.")]
        [SerializeField]
        private KeyCode mTakePicKey = KeyCode.None;
#endif

        [Header("🔍 Sound")]

        [Tooltip("Sound player for this component.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when taking the screenshot.")]
        [SerializeField]
        private AudioClip mTakePhotoSound = null;

        /* Setter & Getter */

        public bool manuallySetSize { get { return mManuallySetSize; } set { mManuallySetSize = value; } }
        public bool mustBeFullScreen { get { return mMustBeFullScreen; } set { mMustBeFullScreen = value; } }
        public int fps { get { return mFPS; } set { mFPS = value; } }
        public float resumeTime { get { return mResumeTime; } set { mResumeTime = value; } }
        public float delayTime { get { return mDelayTime; } set { mDelayTime = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
#if (UNITY_STANDALONE || UNITY_EDITOR)
        public KeyCode takePicKey { get { return mTakePicKey; } set { mTakePicKey = value; } }
#endif
        public bool isPlaying { get { return mWebCamTexture.isPlaying; } }
        public AudioClip takePhotoSound { get { return mTakePhotoSound; } set { mTakePhotoSound = value; } }

        /* Functions */

        private void Start()
        {
            ActiveWebcam();
        }

        private void Update()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            ProcessInput();
#endif

            DoSplash();
            DoAspect();
        }

        private void OnApplicationQuit()
        {
            // force stop
            Stop();
        }

        private void OnDestroy()
        {
            Stop();
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Stop()
        {
            if (mWebCamTexture == null)
                return;

            mWebCamTexture.Stop();
            mWebCamTexture = null;

            SetWebcamTexture(null);
        }
        public void Play()
        {
            mWebCamTexture.Play();
        }
        public void Pause()
        {
            mWebCamTexture.Pause();
        }

        /// <summary>
        /// Active the webcam.
        /// </summary>
        public void ActiveWebcam()
        {
            WebCamDevice[] devices = WebCamTexture.devices;

            // check if there is webcam avaliable in the platform
            if (devices.Length == 0)
            {
                // no device detected!
                mDetectDevice = false;
                return;
            }

            var scs = JCS_ScreenSettings.FirstInstance();
            int screenWidth = scs.standardSize.width;
            int screenHeight = scs.standardSize.height;

            mDeviceName = devices[0].name;
            mWebCamTexture = new WebCamTexture(mDeviceName, screenWidth, screenHeight, mFPS);
            UpdateUnityData();

            Play();
        }

        /// <summary>
        /// Take a snapshot and store image to data path.
        /// </summary>
        public string TakeSnapshotWebcam()
        {
            // No device detected!!
            // cannot take snap shot without the device!!
            if (!mDetectDevice)
            {
                Debug.LogError("No webcam detected in the current devices");
                return null;
            }

            var gs = JCS_GameSettings.FirstInstance();
            var prefix = gs.webcamFilename;
            var ext = gs.webcamExt;

            string savePath = SavePath();

            JCS_IO.CreateDirectory(savePath);

            var snap = new Texture2D(mWebCamTexture.width, mWebCamTexture.height);
            snap.SetPixels(mWebCamTexture.GetPixels());
            snap.Apply();

            // get the last saved webcam image's index
            int last_saved_index = LastImageFileIndex() + 1;

            string fullPath = ImagePathByIndex(last_saved_index);

            File.WriteAllBytes(fullPath, snap.EncodeToPNG());


            if (mSplash)
            {
                var sm = JCS_SceneManager.FirstInstance();

                if (sm.GetWhiteScreen() == null)
                    JCS_UISettings.PopWhiteScreen();

                sm.GetWhiteScreen().FadeIn();

                // do the snap shot effect
                mSplashEffectTrigger = true;
            }

            // Stop the camera
            mWebCamTexture.Pause();

            // start the timer wait for resume
            mResumeTrigger = true;

            // play sound.
            {
                var soundm = JCS_SoundManager.FirstInstance();
                JCS_SoundPlayer sp = (mSoundPlayer) ? mSoundPlayer : soundm.GlobalSoundPlayer();
                sp.PlayOneShot(mTakePhotoSound);
            }

            return fullPath;
        }

        /// <summary>
        /// Get the webcam images' save path.
        /// </summary>
        public static string SavePath()
        {
            var gs = JCS_GameSettings.FirstInstance();
            string path = JCS_Path.Combine(Application.persistentDataPath, gs.webcamPath);
            return path;
        }

        /// <summary>
        /// Last webcam image's file index.
        /// </summary>
        public static int LastImageFileIndex()
        {
            var gs = JCS_GameSettings.FirstInstance();
            var prefix = gs.webcamFilename;
            var ext = gs.webcamExt;
            return JCS_IO.LastFileIndex(SavePath(), prefix, ext);
        }

        /// <summary>
        /// Form webcam image path by index.
        /// </summary>
        /// <param name="index"> Image file's index. </param>
        /// <returns> Image path form by index. </returns>
        public static string ImagePathByIndex(int index)
        {
            var gs = JCS_GameSettings.FirstInstance();
            string path = SavePath() + gs.webcamFilename + index + gs.webcamExt;
            return path;
        }

        /// <summary>
        /// Load webcam image by file index.
        /// </summary>
        /// <param name="index"> File's index. </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> Sprite object that loaded image file by index. </returns>
        public static Sprite LoadImageByIndex(int index, float pixelPerUnit = 100.0f)
        {
            string path = ImagePathByIndex(index);
            return JCS_ImageLoader.LoadImage(path, pixelPerUnit);
        }

        /// <summary>
        /// Load all webcam images.
        /// </summary>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns>
        /// Return a list of sprite with loaded webcam image data.
        /// </returns>
        public static List<JCS_LoadedSpriteData> LoadAllImages(float pixelPerUnit = 100.0f)
        {
            var images = new List<JCS_LoadedSpriteData>();
            int last = LastImageFileIndex() + 1;
            for (int index = 0; index < last; ++index)
            {
                Sprite sprite = LoadImageByIndex(index, pixelPerUnit);
                if (sprite == null)
                    continue;

                var data = new JCS_LoadedSpriteData(sprite, index);
                images.Add(data);
            }
            return images;
        }

        /// <summary>
        /// Delete webcam image by image file's index.
        /// </summary>
        public static void DeleteImageByIndex(int index)
        {
            string path = ImagePathByIndex(index);
            File.Delete(path);
        }

        /// <summary>
        /// Delete all webcam images from disk.
        /// </summary>
        public static void DeleteAllImages()
        {
            JCS_IO.DeleteAllFilesFromDir(SavePath());
        }

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
        public override void UpdateUnityData()
        {
            base.UpdateUnityData();

            SetWebcamTexture(mWebCamTexture);
        }

        /// <summary>
        /// Set the webcam texture.
        /// </summary>
        /// <param name="tex"> Texture you want to be set. </param>
        private void SetWebcamTexture(WebCamTexture tex)
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    mRenderer.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.UI:
                    mImage.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    mSpriteRenderer.material.mainTexture = tex;
                    break;
            }
        }

#if (UNITY_STANDALONE || UNITY_EDITOR)
        /// <summary>
        /// Process the input.
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetKeyDown(mTakePicKey))
            {
                TakeSnapshotWebcam();
            }
        }
#endif

        /// <summary>
        /// Do splash effect.
        /// </summary>
        private void DoSplash()
        {
            float dt = JCS_Time.ItTime(mTimeType);

            if (mResumeTrigger)
            {
                mResumeTimer += dt;

                if (mResumeTime < mResumeTimer)
                {
                    mResumeTimer = 0.0f;

                    if (mWebCamTexture != null)
                        mWebCamTexture.Play();

                    // done resume.
                    mResumeTrigger = false;
                }
            }

            if (mSplashEffectTrigger)
            {
                mDelayTimer += dt;

                if (mDelayTimer > mDelayTime)
                {
                    mDelayTimer = 0.0f;
                    JCS_SceneManager.FirstInstance().GetWhiteScreen().FadeOut();
                    mSplashEffectTrigger = false;
                }
            }
        }

        /// <summary>
        /// Do aspect ratio calculation to webcam.
        /// </summary>
        private void DoAspect()
        {
            if (mWebCamTexture == null)
                return;

            if (mManuallySetSize)
                return;

            if (!mDetectDevice)
            {
                Debug.LogError("No webcam detected in the current devices");
                return;
            }

            if (GetRectTransform() != null)
            {
                var scs = JCS_ScreenSettings.FirstInstance();

                float screenWidth = scs.standardSize.width;
                float screenHeight = scs.standardSize.height;

                float xRatio = screenWidth / (float)mWebCamTexture.width;
                float yRatio = screenHeight / (float)mWebCamTexture.height;

                float width = 0.0f;
                float height = 0.0f;

                bool mode = (mMustBeFullScreen) ? (screenWidth > screenHeight) : (screenWidth < screenHeight);

                if (mode)
                {
                    width = screenWidth;
                    height = (float)mWebCamTexture.height * xRatio;
                }
                else
                {
                    width = (float)mWebCamTexture.width * yRatio;
                    height = screenHeight;
                }

                GetRectTransform().sizeDelta = new Vector2(width, height);
            }

            float scaleY = mWebCamTexture.videoVerticallyMirrored ? -1f : 1f;
            {
                Vector3 newScale = localScale;
                newScale.y *= scaleY;
                localScale = newScale;
            }

            int orient = -mWebCamTexture.videoRotationAngle;
            localEulerAngles = new Vector3(0.0f, 0.0f, orient);
        }
    }
}
