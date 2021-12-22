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

        [Header("** Runtime Variables (JCS_Webcam) **")]

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

        [Header("- Effect")]

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
        [Header("- Keys")]

        [Tooltip("Key to take webcam image.")]
        [SerializeField]
        private KeyCode mTakePicKey = KeyCode.None;
#endif

        [Header("- Sound")]

        [Tooltip("Sound when taking the screenshot.")]
        [SerializeField]
        private AudioClip mTakePhotoSound = null;

        /* Setter & Getter */

        public bool ManuallySetSize { get { return this.mManuallySetSize; } set { this.mManuallySetSize = value; } }
        public bool MustBeFullScreen { get { return this.mMustBeFullScreen; } set { this.mMustBeFullScreen = value; } }
        public int FPS { get { return this.mFPS; } set { this.mFPS = value; } }
        public float ResumeTime { get { return this.mResumeTime; } set { this.mResumeTime = value; } }
        public float DelayTime { get { return this.mDelayTime; } set { this.mDelayTime = value; } }
#if (UNITY_STANDALONE || UNITY_EDITOR)
        public KeyCode TakePicKey { get { return this.mTakePicKey; } set { this.mTakePicKey = value; } }
#endif
        public bool isPlaying { get { return this.mWebCamTexture.isPlaying; } }
        public AudioClip TakePhotoSound { get { return this.mTakePhotoSound; } set { this.mTakePhotoSound = value; } }

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

            this.mWebCamTexture.Stop();
            this.mWebCamTexture = null;

            SetWebcamTexture(null);
        }
        public void Play()
        {
            this.mWebCamTexture.Play();
        }
        public void Pause()
        {
            this.mWebCamTexture.Pause();
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

            var scs = JCS_ScreenSettings.instance;
            int screenWidth = scs.STANDARD_SCREEN_SIZE.width;
            int screenHeight = scs.STANDARD_SCREEN_SIZE.height;

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
                JCS_Debug.LogError("No webcam detected in the current devices");
                return null;
            }

            var gs = JCS_GameSettings.instance;
            var prefix = gs.WEBCAM_FILENAME;
            var ext = gs.WEBCAM_EXTENSION;

            string savePath = SavePath();

            JCS_IO.CreateDirectory(savePath);

            Texture2D snap = new Texture2D(mWebCamTexture.width, mWebCamTexture.height);
            snap.SetPixels(mWebCamTexture.GetPixels());
            snap.Apply();

            // get the last saved webcam image's index
            int last_saved_index = LastImageFileIndex() + 1;

            string fullPath = ImagePathByIndex(last_saved_index);

            File.WriteAllBytes(fullPath, snap.EncodeToPNG());


            if (mSplash)
            {
                JCS_SceneManager sm = JCS_SceneManager.instance;

                if (sm.GetWhiteScreen() == null)
                    JCS_UtilityFunctions.PopJCSWhiteScreen();

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
                var soundm = JCS_SoundManager.instance;
                JCS_SoundPlayer sp = soundm.GetGlobalSoundPlayer();
                sp.PlayOneShot(mTakePhotoSound);
            }

            return fullPath;
        }

        /// <summary>
        /// Get the webcam images' save path.
        /// </summary>
        public static string SavePath()
        {
            var gs = JCS_GameSettings.instance;
            string path = JCS_Path.Combine(Application.persistentDataPath, gs.WEBCAM_PATH);
            return path;
        }

        /// <summary>
        /// Last webcam image's file index.
        /// </summary>
        public static int LastImageFileIndex()
        {
            var gs = JCS_GameSettings.instance;
            var prefix = gs.WEBCAM_FILENAME;
            var ext = gs.WEBCAM_EXTENSION;
            return JCS_Util.LastFileIndex(SavePath(), prefix, ext);
        }

        /// <summary>
        /// Form webcam image path by index.
        /// </summary>
        /// <param name="index"> Image file's index. </param>
        /// <returns> Image path form by index. </returns>
        public static string ImagePathByIndex(int index)
        {
            var gs = JCS_GameSettings.instance;
            string path = SavePath() + gs.WEBCAM_FILENAME + index + gs.WEBCAM_EXTENSION;
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
            JCS_Util.DeleteAllFilesFromDir(SavePath());
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
                    this.mRenderer.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage.material.mainTexture = tex;
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer.material.mainTexture = tex;
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
            if (mResumeTrigger)
            {
                mResumeTimer += Time.deltaTime;

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
                mDelayTimer += Time.deltaTime;

                if (mDelayTimer > mDelayTime)
                {
                    mDelayTimer = 0.0f;
                    JCS_SceneManager.instance.GetWhiteScreen().FadeOut();
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
                JCS_Debug.LogError("No webcam detected in the current devices");
                return;
            }

            if (GetRectTransform() != null)
            {
                var scs = JCS_ScreenSettings.instance;

                float screenWidth = scs.STANDARD_SCREEN_SIZE.width;
                float screenHeight = scs.STANDARD_SCREEN_SIZE.height;

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

                this.GetRectTransform().sizeDelta = new Vector2(width, height);
            }

            float scaleY = mWebCamTexture.videoVerticallyMirrored ? -1f : 1f;
            {
                Vector3 newScale = this.LocalScale;
                newScale.y *= scaleY;
                this.LocalScale = newScale;
            }

            int orient = -this.mWebCamTexture.videoRotationAngle;
            this.LocalEulerAngles = new Vector3(0.0f, 0.0f, orient);
        }
    }
}
