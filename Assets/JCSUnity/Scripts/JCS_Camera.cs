/**
 * $File: JCS_Camera.cs $
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
    /// Camera class for JCSUnity framework.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class JCS_Camera : MonoBehaviour
    {
        /* Variables */

        public static JCS_Camera main = null;

        // Unity's camera object to handle.
        protected Camera mCamera = null;

        // Record down the last position and current position, in order
        // to add the difference between the two frame.
        protected Vector3 mLastFramePos = Vector3.zero;

        [Separator("Check Variables (JCS_Camera)")]

        [Tooltip("Current camera's velocity.")]
        [SerializeField]
        [ReadOnly]
        protected Vector3 mVelocity = Vector3.zero;

        [Separator("Initialize Variables (JCS_Camera)")]

        [Tooltip("Display the camera depth.")]
        [SerializeField]
        protected bool mDisplayGameDepthCamera = false;

        [Tooltip("The color of the camera depth.")]
        [SerializeField]
        protected Color mGameCamColor = Color.white;

        [Separator("Runtime Variables (JCS_Camera)")]

        [Tooltip("Target transform information.")]
        [SerializeField]
        protected Transform mTargetTransform = null;

        [Tooltip("Flag to check if currently the camera following the target object.")]
        [SerializeField]
        protected bool mFollowing = true;

        // x = width, y = height, z = 0 (no involve in depth.)
        protected Vector3 mCamRectSize = Vector3.one;

        // x = left, y = top, width = right, height = bottom.
        // TopLeft -> BottomLeft
        protected Rect mCamRect = new Rect();

        [Tooltip("Offset the camera position from its' original position.")]
        [SerializeField]
        protected Vector3 mPositionOffset = Vector3.zero;

        // Record any necessary data, for resize screen event.
        protected bool mSceneJustLoad = true;

        // Record down the camera data, orthographic size.
        protected float mRecordOrthographicSize = 0.0f;

        // Record down the camera data, filed of view.
        protected float mRecordFieldOfView = 0.0f;

        // Record last vertical FOV, so the FOV from camera is still adjustable.
        private float mLastVerticalFOV = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Flag to check if using smooth track, otherwise hard track.")]
        [SerializeField]
        protected bool mSmoothTrack = true;

        /* Setter & Getter */

        public Vector3 PositionOffset { get { return this.mPositionOffset; } set { this.mPositionOffset = value; } }
        public Vector3 CamRectSize { get { return this.mCamRectSize; } set { this.mCamRectSize = value; } }
        public Rect CamRect { get { return this.mCamRect; } }
        public Camera GetCamera() { return this.mCamera; }
        public float fieldOfView { get { return this.mCamera.fieldOfView; } set { this.mCamera.fieldOfView = value; } }
        public Vector3 Velocity { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public bool Following { get { return this.mFollowing; } set { this.mFollowing = value; } }
        public bool SmoothTrack { get { return this.mSmoothTrack; } set { this.mSmoothTrack = value; } }

        public virtual void SetFollowTarget(Transform trans) { this.mTargetTransform = trans; }
        public virtual Transform GetFollowTarget() { return mTargetTransform; }

        public float ScreenAspect { get { return (float)mCamera.pixelWidth / (float)mCamera.pixelHeight; } }

        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            main = this;

            this.mCamera = this.GetComponent<Camera>();
        }

        protected virtual void Start()
        {
            // Record down the camera data.
            mRecordOrthographicSize = mCamera.orthographicSize;
            mRecordFieldOfView = mCamera.fieldOfView;

            // add to on screen resize callback.
            var screens = JCS_ScreenSettings.instance;
            screens.onScreenResize += OnScreenResize;
            screens.onScreenIdle += OnScreenIdle;
        }

        protected virtual void Update()
        {
            // empty
        }

        protected virtual void LateUpdate()
        {
#if UNITY_EDITOR
            DisplayGameDepthCamera();
#endif
        }

        /// <summary>
        /// Weather if we want to take a screen shot, call
        /// this function will do the trick!
        /// </summary>
        public virtual string TakeScreenshot()
        {
            string savePath = "";
#if (UNITY_STANDALONE || UNITY_EDITOR)
#if (UNITY_2017_1_OR_NEWER)
            /**
             * Data path info:
             *      - http://docs.unity3d.com/ScriptReference/Application-dataPath.html
             */

            // get the last saved screenshot image's index
            int last_saved_index = LastImageFileIndex() + 1;

            savePath = ImagePathByIndex(last_saved_index);

            ScreenCapture.CaptureScreenshot(savePath);
#endif
#endif
            return savePath;
        }

        /// <summary>
        /// Get the screenshot images' save path.
        /// </summary>
        /// <returns></returns>
        public static string SavePath()
        {
            var gs = JCS_GameSettings.instance;
            string path = JCS_Path.Combine(Application.persistentDataPath, gs.SCREENSHOT_PATH);
            return path;
        }

        /// <summary>
        /// Last screenshot image's file index.
        /// </summary>
        public static int LastImageFileIndex()
        {
            var gs = JCS_GameSettings.instance;
            var prefix = gs.SCREENSHOT_FILENAME;
            var ext = gs.SCREENSHOT_EXTENSION;
            return JCS_IO.LastFileIndex(SavePath(), prefix, ext);
        }

        /// <summary>
        /// Form screenshot image path by index.
        /// </summary>
        /// <param name="index"> Image file's index. </param>
        /// <returns> Image path form by index. </returns>
        public static string ImagePathByIndex(int index)
        {
            var gs = JCS_GameSettings.instance;
            string path = SavePath() + gs.SCREENSHOT_FILENAME + index + gs.SCREENSHOT_EXTENSION;
            return path;
        }

        /// <summary>
        /// Load screenshot image by file index.
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
        /// Load all screenshot images.
        /// </summary>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns>
        /// Return a list of sprite with loaded screenshot image data.
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
        /// Delete screenshot image by image file's index.
        /// </summary>
        public static void DeleteImageByIndex(int index)
        {
            string path = ImagePathByIndex(index);
            File.Delete(path);
        }

        /// <summary>
        /// Delete all screenshot images from disk.
        /// </summary>
        public static void DeleteAllImages()
        {
            JCS_IO.DeleteAllFilesFromDir(SavePath());
        }

        private Vector3 GameDepthRect(Vector3 depth)
        {
            // Next step: find camera 4 bounds.
            Camera cam = main.GetCamera();

            var canvas = JCS_Canvas.GuessCanvas();

            Vector3 camPos = cam.transform.position;
            // only need to know the depth.
            {
                camPos.x = 0.0f;
                camPos.y = 0.0f;
            }
            Vector3 canvasPos = canvas.transform.position;
            // only need to know the depth.
            {
                canvasPos.x = 0.0f;
                canvasPos.y = 0.0f;
            }
            float camToCanvasDistance = Vector3.Distance(camPos, canvasPos);

            float camToGameDepthDistance = Vector3.Distance(camPos, depth);

            RectTransform appRect = canvas.AppRect;
            Vector2 canvasRect = appRect.sizeDelta;
            // transfer rect from screen space to world space
            {
                canvasRect.x *= appRect.localScale.x;
                canvasRect.y *= appRect.localScale.y;
            }

            return new Vector3(
                camToGameDepthDistance * canvasRect.x / camToCanvasDistance,
                camToGameDepthDistance * canvasRect.y / camToCanvasDistance,
                0);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Square inside the game editor screen. Display 
        /// the screen width and height in current game depth.
        /// 2D Game probably need this. 3D Game is optional.
        /// </summary>
        private void DisplayGameDepthCamera()
        {
            if (!mDisplayGameDepthCamera)
                return;

            mCamRectSize = GameDepthRect(Vector3.zero);
            Camera cam = main.GetCamera();

            Vector3 camPos = cam.transform.position;

            float camTopBound = camPos.y + mCamRectSize.y / JCS_Mathf.D_HALF;
            float camBotBound = camPos.y - mCamRectSize.y / JCS_Mathf.D_HALF;
            float camRightBound = camPos.x + mCamRectSize.x / JCS_Mathf.D_HALF;
            float camLeftBound = camPos.x - mCamRectSize.x / JCS_Mathf.D_HALF;

            // top left -> bot right
            mCamRect.x = camLeftBound;
            mCamRect.y = camTopBound;
            mCamRect.width = camRightBound;
            mCamRect.height = camBotBound;

            Vector3 topLeft = cam.transform.position;
            topLeft.x -= mCamRectSize.x / JCS_Mathf.D_HALF;
            topLeft.y += mCamRectSize.y / JCS_Mathf.D_HALF;

            Vector3 topRight = cam.transform.position;
            topRight.x += mCamRectSize.x / JCS_Mathf.D_HALF;
            topRight.y += mCamRectSize.y / JCS_Mathf.D_HALF;

            Vector3 botLeft = cam.transform.position;
            botLeft.x -= mCamRectSize.x / JCS_Mathf.D_HALF;
            botLeft.y -= mCamRectSize.y / JCS_Mathf.D_HALF;

            Vector3 botRight = cam.transform.position;
            botRight.x += mCamRectSize.x / JCS_Mathf.D_HALF;
            botRight.y -= mCamRectSize.y / JCS_Mathf.D_HALF;

            // set depth to the same
            topLeft.z = 0;
            topRight.z = 0;
            botLeft.z = 0;
            botRight.z = 0;

            // Draw the box
            JCS_Debug.DrawRect(topLeft, topRight, botRight, botLeft, mGameCamColor);

        }
#endif

        /// <summary>
        /// Check weather the "type" in the screen space. (Character Controller)
        /// </summary>
        /// <param name="cap"> character controller to check. (Collider) </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(CharacterController cap)
        {
            // First Step: Find CharacterController's 4 bounds.
            Vector3 capScale = cap.transform.localScale;

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * JCS_Mathf.D_HALF)) * capScale.y;

            if (cH < 0.0f)
                cH = 0.0f;

            float cTopBound = cap.transform.position.y + cCenter.y + (cH / JCS_Mathf.D_HALF) + cR;
            float cBotBound = cap.transform.position.y + cCenter.y - (cH / JCS_Mathf.D_HALF) - cR;
            float cRightBound = cap.transform.position.x + cCenter.x + cR;
            float cLeftBound = cap.transform.position.x + cCenter.x - cR;

#if UNITY_EDITOR
            Vector3 cOrigin = cap.transform.position + cCenter;
            Debug.DrawLine(cOrigin,
                new Vector3(cOrigin.x, cTopBound, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cOrigin.x, cBotBound, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cRightBound, cOrigin.y, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cLeftBound, cOrigin.y, cOrigin.z));
#endif

            float depth = cap.transform.position.z;
            Vector3 gameRect = GameDepthRect(new Vector3(0.0f, depth, 0.0f));
            Camera cam = main.GetCamera();

            Vector3 camPos = cam.transform.position;

            float camTopBound = camPos.y + gameRect.y / JCS_Mathf.D_HALF;
            float camBotBound = camPos.y - gameRect.y / JCS_Mathf.D_HALF;
            float camRightBound = camPos.x + gameRect.x / JCS_Mathf.D_HALF;
            float camLeftBound = camPos.x - gameRect.x / JCS_Mathf.D_HALF;

#if UNITY_EDITOR
            Vector3 topLeft = cam.transform.position;
            topLeft.x -= gameRect.x / JCS_Mathf.D_HALF;
            topLeft.y += gameRect.y / JCS_Mathf.D_HALF;

            Vector3 topRight = cam.transform.position;
            topRight.x += gameRect.x / JCS_Mathf.D_HALF;
            topRight.y += gameRect.y / JCS_Mathf.D_HALF;

            Vector3 botLeft = cam.transform.position;
            botLeft.x -= gameRect.x / JCS_Mathf.D_HALF;
            botLeft.y -= gameRect.y / JCS_Mathf.D_HALF;

            Vector3 botRight = cam.transform.position;
            botRight.x += gameRect.x / JCS_Mathf.D_HALF;
            botRight.y -= gameRect.y / JCS_Mathf.D_HALF;

            // set depth to the same
            topLeft.z = depth;
            topRight.z = depth;
            botLeft.z = depth;
            botRight.z = depth;

            // Draw the box
            JCS_Debug.DrawRect(topLeft, topRight, botRight, botLeft);
#endif

            if (cRightBound < camLeftBound ||
                camRightBound < cLeftBound ||
                cTopBound < camBotBound ||
                camTopBound < cBotBound)
            {
                // no in the screen
                return false;
            }

            // in screen
            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (Sprite Renderer)
        /// </summary>
        /// <param name="checkTrans"> sprite renderer to check. (Sprite width & height) </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(SpriteRenderer checkTrans)
        {
            Vector2 objectRect = JCS_UIUtil.GetSpriteRendererRect(checkTrans);

            Camera cam = main.GetCamera();
            Vector2 objPos = cam.WorldToViewportPoint(checkTrans.transform.position);
            Vector2 camPos = cam.WorldToViewportPoint(cam.transform.position);

            float objLeft = objPos.x - (objectRect.x / JCS_Mathf.D_HALF);
            float objRight = objPos.x + (objectRect.x / JCS_Mathf.D_HALF);
            float objTop = objPos.y + (objectRect.y / JCS_Mathf.D_HALF);
            float objBot = objPos.y - (objectRect.y / JCS_Mathf.D_HALF);

            RectTransform appRect = JCS_Canvas.GuessCanvas().AppRect;

            float camWidth = appRect.sizeDelta.x;
            float camHeight = appRect.sizeDelta.y;

            float camLeft = camPos.x - (camWidth / JCS_Mathf.D_HALF);
            float camRight = camPos.x + (camWidth / JCS_Mathf.D_HALF);
            float camTop = camPos.y + (camHeight / JCS_Mathf.D_HALF);
            float camBot = camPos.y - (camHeight / JCS_Mathf.D_HALF);

#if UNITY_EDITOR
            var topLeft = new Vector3(objLeft, objTop, 0.0f);
            var topRight = new Vector3(objRight, objTop, 0.0f);
            var botRight = new Vector3(objRight, objBot, 0.0f);
            var botLeft = new Vector3(objLeft, objBot, 0.0f);

            Debug.DrawLine(topLeft, topRight);
            Debug.DrawLine(topLeft, botLeft);
            Debug.DrawLine(botRight, botLeft);
            Debug.DrawLine(topRight, botRight);
#endif

            // TODO(JenChieh): Not done.

            if ((objRight < camLeft || objLeft > camRight) &&
                (objTop < camBot || objBot > camTop))
            {
                // out of screen.
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (Transform)
        /// </summary>
        /// <param name="checkTrans"></param>
        /// <returns></returns>
        public bool CheckInScreenSpace(Transform checkTrans)
        {
            // TODO(JenChieh): continue finish the function?

            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (RectTransform)
        /// </summary>
        /// <param name="checkTrans"> rect transform's size, etc. </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(RectTransform checkTrans)
        {
            Vector2 rectSize = checkTrans.sizeDelta;
            Vector3 panelPos = checkTrans.localPosition;

            float halfSlotWidth = rectSize.x / JCS_Mathf.D_HALF * checkTrans.localScale.x;
            float halfSlotHeight = rectSize.y / JCS_Mathf.D_HALF * checkTrans.localScale.y;

            float panelLeftBorder = panelPos.x - halfSlotWidth;
            float panelRightBorder = panelPos.x + halfSlotWidth;
            float panelTopBorder = panelPos.y + halfSlotHeight;
            float panelBottomBorder = panelPos.y - halfSlotHeight;

            Camera cam = main.GetCamera();
            Vector3 camPos = cam.transform.position;
            // Transfer 3D space to 2D space
            Vector2 camPosToScreen = cam.WorldToScreenPoint(camPos);

            // Get application rect
            RectTransform appRect = JCS_Canvas.GuessCanvas().AppRect;
            Vector2 screenRect = appRect.sizeDelta;

            float camLeftBorder = camPosToScreen.x - screenRect.x / JCS_Mathf.D_HALF;
            float camRightBorder = camPosToScreen.x + screenRect.x / JCS_Mathf.D_HALF;
            float camTopBorder = camPosToScreen.y + screenRect.y / JCS_Mathf.D_HALF;
            float camBottomBorder = camPosToScreen.y - screenRect.y / JCS_Mathf.D_HALF;

            if (panelRightBorder - rectSize.x > camRightBorder ||
                panelLeftBorder + rectSize.x < camLeftBorder ||
                panelTopBorder - rectSize.y > camTopBorder ||
                panelBottomBorder + rectSize.y < camBottomBorder)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Transfer world space to canvas space.
        /// Source -> http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
        /// </summary>
        /// <param name="targetWorldPos"> world position to transfer. </param>
        /// <returns> Canvas space position </returns>
        public Vector2 WorldToCanvasSpace(Vector3 targetWorldPos)
        {
            Camera cam = GetCamera();

            //first you need the RectTransform component of your canvas
            RectTransform canvasRect = JCS_Canvas.GuessCanvas().AppRect;

            //then you calculate the position of the UI element

            // 0,0 for the canvas is at the center of the screen, 
            // whereas WorldToViewPortPoint treats 
            // the lower left corner as 0,0. Because of 
            // this, you need to subtract the height / width of 
            // the canvas * 0.5 to get the correct position.

            Vector3 viewportPos = cam.WorldToViewportPoint(targetWorldPos);

            Vector2 worldObject_ScreenPosition = new Vector2(
                ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * JCS_Mathf.T_HALF)),
                ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * JCS_Mathf.T_HALF)));

            //now you can set the position to the ui element
            return worldObject_ScreenPosition;
        }

        /// <summary>
        /// Transfer canvas space to world space.
        /// 
        /// NOTE(jenchieh): this dont often use, cuz world space element
        /// dont often follow the ui element.
        /// </summary>
        /// <param name="targetCanvasPos"> canvas position to transfer. </param>
        /// <returns> world space position </returns>
        public Vector3 CanvasToWorldSpace(Vector2 targetCanvasPos)
        {
            Camera cam = GetCamera();

            //first you need the RectTransform component of your canvas
            RectTransform canvasRect = JCS_Canvas.GuessCanvas().AppRect;

            Vector2 canvasObject_WorldPosition = new Vector2(
                ((targetCanvasPos.x + (canvasRect.sizeDelta.x * JCS_Mathf.T_HALF)) / canvasRect.sizeDelta.x),
                ((targetCanvasPos.y + (canvasRect.sizeDelta.y * JCS_Mathf.T_HALF)) / canvasRect.sizeDelta.y));

            Vector3 worldPos = cam.ViewportToWorldPoint(canvasObject_WorldPosition);

            //now you can set the position to the world element
            return worldPos;
        }

        /// <summary>
        /// Set the position (float x, y)
        /// Position offset added already.
        /// </summary>
        /// <param name="xPos"> x position </param>
        /// <param name="yPos"> y position </param>
        public void SetPosition(float xPos, float yPos)
        {
            Vector3 newPos = this.transform.position;
            newPos.x = xPos + mPositionOffset.x;
            newPos.y = yPos + mPositionOffset.y;
            this.transform.position = newPos;
        }

        /// <summary>
        /// Set the position (Vector2)
        /// Position offset added already.
        /// </summary>
        /// <param name="vec"> position x axis </param>
        public void SetPosition(Vector2 vec)
        {
            SetPosition(vec.x, vec.y);
        }

        /// <summary>
        /// Set the position (float x, y, z)
        /// Position offset added already.
        /// </summary>
        /// <param name="xPos"> x position </param>
        /// <param name="yPos"> y position </param>
        /// <param name="zPos"> z position </param>
        public void SetPosition(float xPos, float yPos, float zPos)
        {
            Vector3 newPos = this.transform.position;
            newPos.x = xPos + mPositionOffset.x;
            newPos.y = yPos + mPositionOffset.y;
            newPos.z = zPos + mPositionOffset.z;
            this.transform.position = newPos;
        }

        /// <summary>
        /// Set the position (Vector3)
        /// Position offset added already.
        /// </summary>
        /// <param name="vec"> position 3 axis </param>
        public void SetPosition(Vector3 vec)
        {
            SetPosition(vec.x, vec.y, vec.z);
        }

        /// <summary>
        /// Callback when screen not resizing.
        /// </summary>
        protected virtual void OnScreenIdle()
        {
            if (mLastVerticalFOV != mCamera.fieldOfView)
            {
                mRecordFieldOfView = RevertAngleConversionByRatio(mCamera.fieldOfView, GetAspectRatio());
            }
        }

        /// <summary>
        /// Resize the game if screen size changes.
        /// </summary>
        protected virtual void OnScreenResize()
        {
            if (mCamera.orthographic)
                OnResizeOrthographic();
            else
                OnResizePerspective();
        }

        /// <summary>
        /// Resize for perspective camera.
        /// </summary>
        private void OnResizePerspective()
        {
            var ss = JCS_ScreenSettings.instance;

            float verticalFOV = mRecordFieldOfView;
            mCamera.fieldOfView = AngleConversionByRatio(verticalFOV, GetAspectRatio());
            mLastVerticalFOV = mCamera.fieldOfView;  // record it

            /* Store it to screen settings. */
            {
                ss.FIELD_OF_VIEW = mCamera.fieldOfView;
            }
        }

        /// <summary>
        /// Resize for orthographic camera.
        /// </summary>
        private void OnResizeOrthographic()
        {
            var ss = JCS_ScreenSettings.instance;

            JCS_ScreenSizef starting = ss.StartingScreenSize();
            JCS_ScreenSizef current = ss.CURRENT_SCREEN_SIZE;

            float currentScreenRatio = current.width / current.height;
            float startingScreenRatio = starting.width / starting.height;

            if (currentScreenRatio > startingScreenRatio)
            {
                // Set the limit if reach the starting screen ratio.
                current.width = starting.width;
                current.height = starting.height;
            }

            float prevRatio = ss.PREV_SCREEN_SIZE.width / ss.PREV_SCREEN_SIZE.height;
            float newRatio = current.width / current.height;

            float divRatio = prevRatio / newRatio;

            mCamera.orthographicSize *= divRatio;

            if (mSceneJustLoad)
            {
                JCS_ScreenSizef bs = ss.BlackspaceSize();

                float bw = bs.width;
                float bh = bs.height;

                // Width does not need to be calculate, but need to set back
                // to the original value. Hence, the `mRecordOrthographicSize`
                // and `mRecordFieldOfView` variables.
                if (bw > bh)
                {
                    mCamera.orthographicSize = mRecordOrthographicSize;
                }
                // Calculating the proper hight.
                else
                {
                    // Calculate what the height suppose to be!
                    float supposeHeight = ((float)JCS_Screen.width * starting.height) / starting.width;

                    // Use the 'suppose height' to find the proper 
                    // height ratio.
                    float heightRatio = ((float)JCS_Screen.height / supposeHeight);

                    mCamera.orthographicSize = heightRatio * mRecordOrthographicSize;
                }

                mSceneJustLoad = false;
            }

            /* Store it to screen settings. */
            {
                ss.ORTHOGRAPHIC_SIZE = mCamera.orthographicSize;
            }
        }

        /// <summary>
        /// Return the aspect ratio from the standard screen size.
        /// </summary>
        private float GetAspectRatio()
        {
            var ss = JCS_ScreenSettings.instance;
            JCS_ScreenSize standard = ss.STANDARD_SCREEN_SIZE;
            float mainAreaAspectRatio = (float)standard.width / (float)standard.height;
            float aspectDifference = Mathf.Min(1f, ScreenAspect / mainAreaAspectRatio);
            return 1 / aspectDifference;
        }

        /// <summary>
        /// Convert DEGREE to largest width FOV.
        /// </summary>
        private float AngleConversionByRatio(float degree, float ratio)
        {
            float tan = Mathf.Tan(0.5f * degree * Mathf.Deg2Rad);
            float atan = Mathf.Atan(tan * ratio);
            float fov = 2f * atan * Mathf.Rad2Deg;
            return fov;
        }

        /// <summary>
        /// Inverse function of function `AngleConversionByRatio`.
        /// </summary>
        private float RevertAngleConversionByRatio(float fov, float ratio)
        {
            float atan = fov * 0.5f * Mathf.Deg2Rad;
            float tan = Mathf.Tan(atan) * (1f / ratio);
            float degree = Mathf.Atan(tan) * Mathf.Rad2Deg * 2f;
            return degree;
        }
    }
}
