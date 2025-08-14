/**
 * $File: JCS_2DSlideScreenCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Camera for GUI!! not in the game scene.
    ///
    /// use for switching scene panel.
    /// </summary>
    public class JCS_2DSlideScreenCamera : MonoBehaviour
    {
        /* Variables */

        // Function call after the scene changed
        public Action<Vector2> afterSceneSwitched = null;

        // Function call after the user has swiped
        public Action<Vector2> afterSwiped = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_2DSlideScreenCamera)")]

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
        private string mPanelHolderPath = "UI/System/JCS_SlideScreenPanelHolder";

        [Separator("Check Variables (JCS_2DSlideScreenCamera)")]

        [Tooltip("Page start from center.")]
        [SerializeField]
        [ReadOnly]
        private Vector2 mCurrentPage = Vector2.zero;

        [Separator("Runtime Variables (JCS_2DSlideScreenCamera)")]

        // Notice important that Designer should know what Unity GUI type they
        // are going to use!
        [Tooltip("GUI type.")]
        [SerializeField]
        private JCS_UnityGUIType mUnityGUIType = JCS_UnityGUIType.nGUI_3D;

        [Tooltip("Slide screen panel holder for this camera.")]
        [SerializeField]
        private JCS_SlideScreenPanelHolder mPanelHolder = null;

        [Header("- Mobile")]

        [Tooltip("If true, allow the mobile swipe action.")]
        [SerializeField]
        private bool mInteractableSwipe = true;

        [Tooltip("Area space to swipe for previous/next page.")]
        [SerializeField]
        private Vector2 mSwipeArea = new Vector2(0.5f, 0.5f);

        [Tooltip("If user swipe above this speed, switch to previous/next page automatically. (x-axis)")]
        [Range(0.0f, 5000.0f)]
        [SerializeField]
        private float mSwipeSpeedX = 800.0f;

        [Tooltip("If user swipe above this speed, switch to previous/next page automatically. (y-axis)")]
        [Range(0.0f, 5000.0f)]
        [SerializeField]
        private float mSwipeSpeedY = 800.0f;

        [Tooltip("Freeze the x axis sliding action.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze the y axis sliding action.")]
        [SerializeField]
        private bool mFreezeY = false;

        [Header("- Sound")]

        [Tooltip("Sound when trigger switch scene.")]
        [SerializeField]
        private AudioClip mSwitchSceneSound = null;

        [Header("- Boundary")]

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

        public JCS_SlideScreenPanelHolder panelHolder { get { return this.mPanelHolder; } set { this.mPanelHolder = value; } }
        public JCS_UnityGUIType unityGUIType { get { return this.mUnityGUIType; } set { this.mUnityGUIType = value; } }
        public bool interactableSwipe { get { return this.mInteractableSwipe; } set { this.mInteractableSwipe = value; } }
        public Vector2 swipeArea { get { return this.mSwipeArea; } set { this.mSwipeArea = value; } }
        public float swipeSpeedX { get { return this.mSwipeSpeedX; } set { this.mSwipeSpeedX = value; } }
        public float swipeSpeedY { get { return this.mSwipeSpeedY; } set { this.mSwipeSpeedY = value; } }
        public bool freezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool freezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public AudioClip switchSceneSound { get { return this.mSwitchSceneSound; } set { this.mSwitchSceneSound = value; } }
        public Vector2 currentPage { get { return this.mCurrentPage; } }
        public int minPageX { get { return this.mMinPageX; } set { this.mMinPageX = value; } }
        public int maxPageX { get { return this.mMaxPageX; } set { this.mMaxPageX = value; } }
        public int minPageY { get { return this.mMinPageY; } set { this.mMinPageY = value; } }
        public int maxPageY { get { return this.mMaxPageY; } set { this.mMaxPageY = value; } }

        /* Functions */

        private void Awake()
        {
            if (mPanelHolder == null)
            {
                // spawn a default one!
                this.mPanelHolder = JCS_Util.Instantiate(
                    mPanelHolderPath,
                    transform.position,
                    transform.rotation).GetComponent<JCS_SlideScreenPanelHolder>();
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif
            DoMobileSwipe();
        }

#if UNITY_EDITOR
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
        /// Set the page using page index. (Vector 2)
        /// </summary>
        /// <param name="page"> Index of the page. </param>
        public void SetPage(Vector2 page)
        {
            SetPageX((int)page.x);
            SetPageY((int)page.y);
        }

        /// <summary>
        /// Set the page using page index. (x-axis)
        /// </summary>
        /// <param name="page"> Index of the page. </param>
        public void SetPageX(int page)
        {
            int delta = page - (int)this.mCurrentPage.x;

            if (delta == 0)
                return;

            int count = JCS_Mathf.AbsoluteValue(delta);

            for (int countX = 0; countX < count; ++countX)
            {
                if (JCS_Mathf.IsPositive(delta))
                    SwitchScene(JCS_2D4Direction.RIGHT);
                else
                    SwitchScene(JCS_2D4Direction.LEFT);
            }
        }

        /// <summary>
        /// Set the page using page index. (y-axis)
        /// </summary>
        /// <param name="page"> Index of the page. </param>
        public void SetPageY(int page)
        {
            int delta = page - (int)this.mCurrentPage.y;

            if (delta == 0)
                return;

            int count = JCS_Mathf.AbsoluteValue(delta);

            for (int countY = 0; countY < count; ++countY)
            {
                if (JCS_Mathf.IsPositive(delta))
                    SwitchScene(JCS_2D4Direction.TOP);
                else
                    SwitchScene(JCS_2D4Direction.BOTTOM);
            }
        }

        /// <summary>
        /// Swicth the scene by sliding its with direction.
        /// </summary>
        /// <param name="towardDirection"></param>
        public void SwitchScene(JCS_2D4Direction towardDirection)
        {
            var direction8 = (JCS_2D8Direction)towardDirection;
            SwitchScene(direction8);
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

            if (afterSceneSwitched != null)
                afterSceneSwitched.Invoke(mCurrentPage);

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
            var ti = JCS_TouchInput.FirstInstance();
            if (ti == null)
                return;

            bool enableSlidePanel = true;

            if (mInteractableSwipe && ti.touched)
            {
                Vector3 deltaPos = ti.deltaPos;

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

            if (mInteractableSwipe && JCS_Input.GetMouseButtonUp(JCS_MouseButton.LEFT))
            {
                Vector3 posDiff = ti.dragDistance;
                JCS_ScreenSizef vs = JCS_ScreenSettings.FirstInstance().VisibleScreenSize();
                var target_vs = new JCS_ScreenSizef(vs.width * mSwipeArea.x, vs.height * mSwipeArea.y);

                var speedX = ti.dragDistance.x / ti.touchTime;
                var speedY = ti.dragDistance.y / ti.touchTime;

                bool reachedX = posDiff.x > target_vs.width;  // distance
                bool speedExceedX = speedX > mSwipeSpeedX;    // speed

                if (!mFreezeX && (reachedX || speedExceedX))
                {
                    if (JCS_Mathf.IsPositive(ti.dragDisplacement.x))
                        SwitchScene(JCS_2D4Direction.LEFT);
                    else
                        SwitchScene(JCS_2D4Direction.RIGHT);

                    if (afterSwiped != null)
                        afterSwiped.Invoke(mCurrentPage);
                }

                bool reachedY = posDiff.y > target_vs.height;  // distance
                bool speedExceedY = speedY > mSwipeSpeedY;     // speed

                if (!mFreezeY && (reachedY || speedExceedY))
                {
                    if (JCS_Mathf.IsPositive(ti.dragDisplacement.y))
                        SwitchScene(JCS_2D4Direction.BOTTOM);
                    else
                        SwitchScene(JCS_2D4Direction.TOP);

                    if (afterSwiped != null)
                        afterSwiped.Invoke(mCurrentPage);
                }
            }
        }

        /// <summary>
        /// Play switch scene sound.
        /// </summary>
        private void PlaySwitchSceneSound()
        {
            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();
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

            var screenS = JCS_ScreenSettings.FirstInstance();
            var cam = JCS_Camera.main;

            RectTransform slidePanel = mPanelHolder.slidePanels[0];

            var panelRoot = slidePanel.GetComponent<JCS_PanelRoot>();
            if (panelRoot == null)
                panelRoot = slidePanel.parent.GetComponent<JCS_PanelRoot>();

            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    {
                        if (panelRoot != null)
                        {
                            JCS_ScreenSizef size = screenS.StartingScreenSize();

                            screenWidth = size.width;
                            screenHeight = size.height;
                        }
                        else
                        {
                            screenWidth = screenS.STANDARD_SCREEN_SIZE.width;
                            screenHeight = screenS.STANDARD_SCREEN_SIZE.height;
                        }
                    }
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    {
                        screenWidth = cam.camRectSize.x;
                        screenHeight = cam.camRectSize.y;
                    }
                    break;
            }

            return new Vector2(screenWidth, screenHeight);
        }

        //////////// 2D //////////////////////////

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
        private void NGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 screenSize = GetScreenSize();
            float screenWidth = screenSize.x;
            float screenHeight = screenSize.y;

            // make a copy of old position
            Vector3 newScenePosition = this.transform.position;

            // Apply new position and set to its position according to the
            // direction programmer pass in.
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

            // Set this position to new position, so the camera will follow
            // this object!
            this.transform.position = newScenePosition;
        }
    }
}
