/**
 * $File: JCS_2DSlideScreenCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Camera for GUI!! not in the game scene.
    ///
    /// use for switching scene panel.
    /// </summary>
    public class JCS_2DSlideScreenCamera
        : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DSlideScreenCamera) **")]

        public bool testWithKey = false;

        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.X;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        public KeyCode upLeftKey = KeyCode.Q;
        public KeyCode downLeftKey = KeyCode.Z;
        public KeyCode upRightKey = KeyCode.E;
        public KeyCode downRightKey = KeyCode.C;
#endif

        // Path that points to the panel.
        private string mPanelHolderPath = "JCSUnity_Framework_Resources/LevelDesignUI/JCS_SlideScreenPanelHolder";

        [Header("** Check Variables (JCS_2DSlideScreenCamera) **")]

        [Tooltip("Page start from center.")]
        [SerializeField]
        private Vector2 mCurrentPage = Vector2.zero;

        [Header("** Runtime Variables (JCS_2DSlideScreenCamera) **")]

        // Notice important that Designer should know what
        // Unity GUI type they are going to use!
        [Tooltip("GUI type.")]
        [SerializeField]
        private JCS_UnityGUIType mUnityGUIType = JCS_UnityGUIType.nGUI_3D;

        [Tooltip("Camera itself.")]
        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;

        [Tooltip("Slide screen panel holder for this camera.")]
        [SerializeField]
        private JCS_SlideScreenPanelHolder mPanelHolder = null;

        [Header("## Mobile")]

        [Tooltip("Area space to swipe for previous/next page.")]
        [SerializeField]
        private Vector2 mSwipeArea = new Vector2(0.3f, 0.3f);

        [Tooltip("Freeze the x axis sliding action.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze the y axis sliding action.")]
        [SerializeField]
        private bool mFreezeY = false;

        [Header("## Sound")]

        [Tooltip("Sound when trigger switch scene.")]
        [SerializeField]
        private AudioClip mSwitchSceneSound = null;

        [Header("## Boundary")]

        [Tooltip("Minimum page on x-axis.")]
        [SerializeField]
        [Range(-30, 30)]
        private int mMinPageX = -5;

        [Tooltip("Maximum page on x-axis.")]
        [SerializeField]
        [Range(-30, 30)]
        private int mMaxPageX = 5;

        [Tooltip("Minimum page on y-axis.")]
        [SerializeField]
        [Range(-30, 30)]
        private int mMinPageY = -5;

        [Tooltip("Maximum page on y-axis.")]
        [SerializeField]
        [Range(-30, 30)]
        private int mMaxPageY = 5;

        /* Setter & Getter */

        public JCS_SlideScreenPanelHolder PanelHolder { get { return this.mPanelHolder; } set { this.mPanelHolder = value; } }
        public void SetJCS2DCamera(JCS_2DCamera cam) { this.mJCS_2DCamera = cam; }
        public JCS_UnityGUIType UnityGUIType { get { return this.mUnityGUIType; } set { this.mUnityGUIType = value; } }
        public Vector2 SwipeArea { get { return this.mSwipeArea; } set { this.mSwipeArea = value; } }
        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public AudioClip SwitchSceneSound { get { return this.mSwitchSceneSound; } set { this.mSwitchSceneSound = value; } }
        public int MinPageX { get { return this.mMinPageX; } set { this.mMinPageX = value; } }
        public int MaxPageX { get { return this.mMaxPageX; } set { this.mMaxPageX = value; } }
        public int MinPageY { get { return this.mMinPageY; } set { this.mMinPageY = value; } }
        public int MaxPageY { get { return this.mMaxPageY; } set { this.mMaxPageY = value; } }

        /* Functions */

        private void Awake()
        {
            InitCamera();

            //
            if (mPanelHolder == null)
            {
                // spawn a default one!
                this.mPanelHolder = JCS_Utility.SpawnGameObject(
                    mPanelHolderPath,
                    transform.position,
                    transform.rotation).GetComponent<JCS_SlideScreenPanelHolder>();
            }
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            DoMobileSwipe();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(upKey))
                SwitchScene(JCS_2D4Direction.TOP);
            if (JCS_Input.GetKeyDown(rightKey))
                SwitchScene(JCS_2D4Direction.RIGHT);
            if (JCS_Input.GetKeyDown(downKey))
                SwitchScene(JCS_2D4Direction.BOTTOM);
            if (JCS_Input.GetKeyDown(leftKey))
                SwitchScene(JCS_2D4Direction.LEFT);

            if (JCS_Input.GetKeyDown(upLeftKey))
                SwitchScene(JCS_2D8Direction.TOP_LEFT);
            if (JCS_Input.GetKeyDown(upRightKey))
                SwitchScene(JCS_2D8Direction.TOP_RIGHT);
            if (JCS_Input.GetKeyDown(downRightKey))
                SwitchScene(JCS_2D8Direction.BOTTOM_RIGHT);
            if (JCS_Input.GetKeyDown(downLeftKey))
                SwitchScene(JCS_2D8Direction.BOTTOM_LEFT);
        }
#endif

        /// <summary>
        /// Swicth the scene by sliding its with direction.
        /// </summary>
        /// <param name="towardDirection"></param>
        public void SwitchScene(JCS_2D4Direction towardDirection)
        {
            bool valid = CalculatePage(towardDirection);
            if (!valid)
                return;

            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    UGUISwitchScene(towardDirection);
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    NGUISwitchScene(towardDirection);
                    break;
            }
            PlaySwitchSceneSound();
        }

        /// <summary>
        /// Swicth the scene by sliding its with direction.
        /// </summary>
        /// <param name="towardDirection"></param>
        public void SwitchScene(JCS_2D8Direction towardDirection)
        {
            bool valid = CalculatePage(towardDirection);
            if (!valid)
                return;

            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    UGUISwitchScene(towardDirection);
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    NGUISwitchScene(towardDirection);
                    break;
            }
            PlaySwitchSceneSound();
        }

        /// <summary>
        /// Check if the NEWPAGE is the valid page.
        /// </summary>
        /// <param name="newPage"> New page to test for min/max boundary. </param>
        /// <returns>
        /// Return true, if NEWPAGE is valid from current min/max boundary.
        /// </returns>
        private bool IsValidPage(Vector2 newPage)
        {
            bool canDo = true;

            if (newPage.x < mMinPageX)
                canDo = false;
            else if (newPage.x > mMaxPageX)
                canDo = false;

            if (newPage.y < mMinPageY)
                canDo = false;
            else if (newPage.y > mMaxPageY)
                canDo = false;

            return canDo;
        }

        /// <summary>
        /// Calculate the currnet page with DIRECTION.
        /// </summary>
        /// <param name="direction"> Page direction. </param>
        /// <returns>
        /// Return true, if is under the min/max boundary.
        /// </returns>
        private bool CalculatePage(JCS_2D4Direction direction)
        {
            Vector2 newPage = mCurrentPage;

            switch (direction)
            {
                case JCS_2D4Direction.TOP:
                    ++newPage.y;
                    break;
                case JCS_2D4Direction.BOTTOM:
                    --newPage.y;
                    break;
                case JCS_2D4Direction.RIGHT:
                    ++newPage.x;
                    break;
                case JCS_2D4Direction.LEFT:
                    --newPage.x;
                    break;
            }

            bool canDo = IsValidPage(newPage);
            if (canDo) mCurrentPage = newPage;
            return canDo;
        }

        /// <summary>
        /// Calculate the currnet page with DIRECTION.
        /// </summary>
        /// <param name="direction"> Page direction. </param>
        /// <returns>
        /// Return true, if is under the min/max boundary.
        /// </returns>
        private bool CalculatePage(JCS_2D8Direction direction)
        {
            Vector2 newPage = mCurrentPage;

            switch (direction)
            {
                case JCS_2D8Direction.TOP:
                    ++newPage.y;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    --newPage.y;
                    break;
                case JCS_2D8Direction.RIGHT:
                    ++newPage.x;
                    break;
                case JCS_2D8Direction.LEFT:
                    --newPage.x;
                    break;

                // 4 corners
                case JCS_2D8Direction.TOP_LEFT:
                    ++newPage.y;
                    --newPage.x;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    ++newPage.y;
                    ++newPage.x;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    --newPage.y;
                    ++newPage.x;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    --newPage.y;
                    --newPage.x;
                    break;
            }

            bool canDo = IsValidPage(newPage);
            if (canDo) mCurrentPage = newPage;
            return canDo;
        }

        /// <summary>
        /// Handle the mobile swipe input.
        /// </summary>
        private void DoMobileSwipe()
        {
            JCS_SlideInput si = JCS_InputManager.instance.GetGlobalSlideInput();
            if (si == null)
                return;

            bool enableSlidePanel = true;

            if (si.Touched)
            {
                Vector3 deltaPos = si.DeltaPos;

                bool cancelX = false;
                bool cancelY = false;

                if (mFreezeX) cancelX = true;
                if (mFreezeY) cancelY = true;

                /* Fix so you don't swipe over boundaries! */
                {
                    bool positiveX = JCS_Mathf.IsPositive(deltaPos.x);
                    bool positiveY = JCS_Mathf.IsPositive(deltaPos.y);

                    if (mCurrentPage.x <= mMinPageX && positiveX ||
                        mCurrentPage.x >= mMaxPageX && !positiveX)
                        cancelX = true;

                    if (mCurrentPage.y <= mMinPageY && positiveY ||
                        mCurrentPage.y >= mMaxPageY && !positiveY)
                        cancelY = true;
                }

                if (cancelX) deltaPos.x = 0.0f;
                if (cancelY) deltaPos.y = 0.0f;

                // If you can move at least one dimension,
                if (!cancelX || !cancelY)
                    enableSlidePanel = false;

                if (!enableSlidePanel)
                    mPanelHolder.DeltaMove(deltaPos);
            }

            mPanelHolder.EnableSlidePanels(enableSlidePanel);

            if (JCS_Input.GetMouseButtonUp(JCS_MouseButton.LEFT))
            {
                Vector3 posDiff = si.DragDistance;
                JCS_ScreenSizef vs = JCS_ScreenSettings.instance.VisibleScreenSize();
                JCS_ScreenSizef target_vs = new JCS_ScreenSizef(vs.width * mSwipeArea.x, vs.height * mSwipeArea.y);

                if (!mFreezeX && posDiff.x > target_vs.width)
                {
                    if (JCS_Mathf.IsPositive(si.DragDisplacement.x))
                        SwitchScene(JCS_2D4Direction.LEFT);
                    else
                        SwitchScene(JCS_2D4Direction.RIGHT);
                }

                if (!mFreezeY && posDiff.y > target_vs.height)
                {
                    if (JCS_Mathf.IsPositive(si.DragDisplacement.y))
                        SwitchScene(JCS_2D4Direction.BOTTOM);
                    else
                        SwitchScene(JCS_2D4Direction.TOP);
                }
            }
        }

        /// <summary>
        /// Play switch scene sound.
        /// </summary>
        private void PlaySwitchSceneSound()
        {
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GetGlobalSoundPlayer();
            sp.PlayOneShot(this.mSwitchSceneSound);
        }

        /// <summary>
        /// Return the screen size according to the GUI mode.
        /// </summary>
        /// <returns>
        /// Return a Vector2 with screen width and height.
        /// </returns>
        private Vector2 GetScreenSize()
        {
            float screenWidth = 0.0f;
            float screenHeight = 0.0f;

            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;
            JCS_Camera cam = JCS_Camera.main;

            JCS_PanelRoot panelRoot = mPanelHolder.slidePanels[0].GetComponent<JCS_PanelRoot>();
            if (panelRoot == null)
                panelRoot = mPanelHolder.slidePanels[0].GetComponentInParent<JCS_PanelRoot>();

            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    {
                        if (panelRoot != null)
                        {
                            screenWidth = ss.STARTING_SCREEN_SIZE.width;
                            screenHeight = ss.STARTING_SCREEN_SIZE.height;
                        }
                        else
                        {
                            screenWidth = ss.STANDARD_SCREEN_SIZE.width;
                            screenHeight = ss.STANDARD_SCREEN_SIZE.height;
                        }
                    }
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    {
                        screenWidth = cam.CamRectSize.x;
                        screenHeight = cam.CamRectSize.y;
                    }
                    break;
            }

            return new Vector2(screenWidth, screenHeight);
        }

        //////////// 2D //////////////////////////

        /// <summary>
        /// Iniialize the camera.
        /// </summary>
        private void InitCamera()
        {
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_Utility.SpawnGameObject(
                    JCS_2DCamera.JCS_2DCAMERA_PATH,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            // if still null, setting error!!
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("The object spawn does not have the `JCS_2DCamera` component");
                return;
            }

            // set target to follow!
            mJCS_2DCamera.SetFollowTarget(this.transform);
        }

        /// <summary>
        /// UGUI method switch the panel.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void UGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 screenSize = GetScreenSize();
            float screenWidth = screenSize.x;
            float screenHeight = screenSize.y;

            // make a copy of old position
            Vector3 newScenePosition = Vector3.zero;

            switch (towardDirection)
            {
                case JCS_2D4Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D4Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D4Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D4Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;
            }

            mPanelHolder.AddForce(newScenePosition);
        }

        /// <summary>
        /// UGUI method switch the panel.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void UGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 screenSize = GetScreenSize();
            float screenWidth = screenSize.x;
            float screenHeight = screenSize.y;

            // make a copy of old position
            Vector3 newScenePosition = Vector3.zero;

            switch (towardDirection)
            {
                case JCS_2D8Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D8Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;

                // 4 corners
                case JCS_2D8Direction.TOP_LEFT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
            }

            mPanelHolder.AddForce(newScenePosition);
        }

        //////////// 3D //////////////////////////

        /// <summary>
        /// NGUI method to switch panel/scene.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void NGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 screenSize = GetScreenSize();
            float screenWidth = screenSize.x;
            float screenHeight = screenSize.y;

            // make a copy of old position
            Vector3 newScenePosition = this.transform.position;

            // apply new position and set to its
            // position according to the direction
            // programmer pass in.
            switch (towardDirection)
            {
                case JCS_2D4Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D4Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D4Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D4Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;
            }

            // set this position to new position
            // so the camera will follow this object!
            this.transform.position = newScenePosition;
        }

        /// <summary>
        /// NGUI method to switch panel/scene.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void NGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 screenSize = GetScreenSize();
            float screenWidth = screenSize.x;
            float screenHeight = screenSize.y;

            // make a copy of old position
            Vector3 newScenePosition = this.transform.position;

            // apply new position and set to its
            // position according to the direction
            // programmer pass in.
            switch (towardDirection)
            {
                case JCS_2D8Direction.CENTER:
                    // do nothing
                    return;

                // 4 sides
                case JCS_2D8Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D8Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;

                // 4 corners
                case JCS_2D8Direction.TOP_LEFT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
            }

            // set this position to new position
            // so the camera will follow this object!
            this.transform.position = newScenePosition;
        }
    }
}
