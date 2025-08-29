#if (UNITY_EDITOR || UNITY_STANDALONE)
/**
 * $File: JCS_2DCursor.cs $
 * $Date: 2017-05-24 22:02:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// 2D animate cursor.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DCursor : JCS_Cursor
    {
        /* Variables */

        private JCS_2DAnimator m2DAnimator = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_2DCursor)")]

        [SerializeField]
        private bool mTestWithKey = false;
#endif

        [Separator("Runtime Variables (JCS_2DCursor)")]

        [Tooltip("Add on offset to the cursor.")]
        [SerializeField]
        private Vector3 mCursorOffset = Vector3.zero;

        [Header("Cursor State")]

        [Tooltip("Cursor custom type.")]
        [SerializeField]
        private JCS_CursorCustomizeType mCursorCustomizeType = JCS_CursorCustomizeType.NORMAL_SELECT;

        [Tooltip("Normal select")]
        [SerializeField]
        private JCS_2DAnimation mNormalSelect = null;

        [Tooltip("Help select")]
        [SerializeField]
        private JCS_2DAnimation mHelpSelect = null;

        [Tooltip("Working in background")]
        [SerializeField]
        private JCS_2DAnimation mWorkingInBackground = null;

        [Tooltip("Busy")]
        [SerializeField]
        private JCS_2DAnimation mBusy = null;

        [Tooltip("Precision select")]
        [SerializeField]
        private JCS_2DAnimation mPrecisionSelect = null;

        [Tooltip("Text select")]
        [SerializeField]
        private JCS_2DAnimation mTextSelect = null;

        [Tooltip("Handwriting")]
        [SerializeField]
        private JCS_2DAnimation mHandwriting = null;

        [Tooltip("Unavaliable")]
        [SerializeField]
        private JCS_2DAnimation mUnavaliable = null;

        [Tooltip("Vertical resize.")]
        [SerializeField]
        private JCS_2DAnimation mVerticalResize = null;

        [Tooltip("Horizontal resize")]
        [SerializeField]
        private JCS_2DAnimation mHorizontalResize = null;

        [Tooltip("Diagonal resize 1")]
        [SerializeField]
        private JCS_2DAnimation mDiagonalResize1 = null;

        [Tooltip("Diagonal resize 2")]
        [SerializeField]
        private JCS_2DAnimation mDiagonalResize2 = null;

        [Tooltip("Move")]
        [SerializeField]
        private JCS_2DAnimation mMove = null;

        [Tooltip("Alternate select")]
        [SerializeField]
        private JCS_2DAnimation mAlternateSelect = null;

        [Tooltip("Link select")]
        [SerializeField]
        private JCS_2DAnimation mLinkSelect = null;

        /* Setter & Getter */

        public JCS_CursorCustomizeType cursorCustomizeType { get { return this.mCursorCustomizeType; } }
        public Vector3 cursorOffset { get { return this.mCursorOffset; } set { this.mCursorOffset = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            m2DAnimator = GetComponent<JCS_2DAnimator>();

            m2DAnimator.animations.Add(mNormalSelect);
            m2DAnimator.animations.Add(mHelpSelect);
            m2DAnimator.animations.Add(mWorkingInBackground);
            m2DAnimator.animations.Add(mBusy);
            m2DAnimator.animations.Add(mPrecisionSelect);
            m2DAnimator.animations.Add(mTextSelect);
            m2DAnimator.animations.Add(mHandwriting);
            m2DAnimator.animations.Add(mUnavaliable);
            m2DAnimator.animations.Add(mVerticalResize);
            m2DAnimator.animations.Add(mHorizontalResize);
            m2DAnimator.animations.Add(mDiagonalResize1);
            m2DAnimator.animations.Add(mDiagonalResize2);
            m2DAnimator.animations.Add(mMove);
            m2DAnimator.animations.Add(mAlternateSelect);
            m2DAnimator.animations.Add(mLinkSelect);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif

            FollowMouse();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(KeyCode.F1))
                SwitchState(JCS_CursorCustomizeType.NORMAL_SELECT);
            if (JCS_Input.GetKey(KeyCode.F2))
                SwitchState(JCS_CursorCustomizeType.HELP_SELECT);
            if (JCS_Input.GetKey(KeyCode.F3))
                SwitchState(JCS_CursorCustomizeType.WORKING_IN_BACKGROUND);
            if (JCS_Input.GetKey(KeyCode.F4))
                SwitchState(JCS_CursorCustomizeType.BUSY);
            if (JCS_Input.GetKey(KeyCode.F5))
                SwitchState(JCS_CursorCustomizeType.PRECISION_SELECT);
            if (JCS_Input.GetKey(KeyCode.F6))
                SwitchState(JCS_CursorCustomizeType.TEXT_SELECT);
            if (JCS_Input.GetKey(KeyCode.F7))
                SwitchState(JCS_CursorCustomizeType.HANDWRITING);
            if (JCS_Input.GetKey(KeyCode.F8))
                SwitchState(JCS_CursorCustomizeType.UNAVAILABLE);
            if (JCS_Input.GetKey(KeyCode.F9))
                SwitchState(JCS_CursorCustomizeType.VERTICAL_RESIZE);
            if (JCS_Input.GetKey(KeyCode.F10))
                SwitchState(JCS_CursorCustomizeType.HORIZONTAL_RESIZE);
            if (JCS_Input.GetKey(KeyCode.F11))
                SwitchState(JCS_CursorCustomizeType.DIAGONAL_RESIZE_1);
            if (JCS_Input.GetKey(KeyCode.F12))
                SwitchState(JCS_CursorCustomizeType.DIAGONAL_RESIZE_2);
            if (JCS_Input.GetKey(KeyCode.F13))
                SwitchState(JCS_CursorCustomizeType.MOVE);
            if (JCS_Input.GetKey(KeyCode.F14))
                SwitchState(JCS_CursorCustomizeType.ALTERNATE_SELECT);
            if (JCS_Input.GetKey(KeyCode.F15))
                SwitchState(JCS_CursorCustomizeType.LINK_SELECT);
        }
#endif

        /// <summary>
        /// Switch the animation state.
        /// </summary>
        /// <param name="type"> type of the animation. </param>
        public void SwitchState(JCS_CursorCustomizeType type)
        {
            this.m2DAnimator.DoAnimation((int)type);

            this.mCursorCustomizeType = type;
        }

        /// <summary>
        /// Follow the cursor position.
        /// </summary>
        private void FollowMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 mousePos2d = new Vector3(mousePos.x, mousePos.y, 0);

            Camera cam = JCS_Camera.main.GetCamera();
            Vector3 mousePos3d = cam.ScreenToWorldPoint(mousePos2d);

            // add on offset
            mousePos3d += mCursorOffset;

            this.transform.position = mousePos3d;
        }
    }
}
#endif
