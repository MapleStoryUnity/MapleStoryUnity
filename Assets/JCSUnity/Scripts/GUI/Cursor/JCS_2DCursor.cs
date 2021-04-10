#if (UNITY_EDITOR || UNITY_STANDALONE)
/**
 * $File: JCS_2DCursor.cs $
 * $Date: 2017-05-24 22:02:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D animate cursor.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DCursor
        : MonoBehaviour
    {
        /* Variables */

        private JCS_2DAnimator m2DAnimator = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DCursor) **")]

        [SerializeField]
        private bool mTestWithKey = false;
#endif

        [Header("** Runtime Variables (JCS_2DCursor) **")]

        [Tooltip("Add on offset to the cursor.")]
        [SerializeField]
        private Vector3 mCursorOffset = Vector3.zero;

        [Header("** Cursor State (JCS_Cursor) **")]

        [SerializeField]
        private JCS_CursorCustomizeType mCursorCustomizeType = JCS_CursorCustomizeType.NORMAL_SELECT;
        [SerializeField] private JCS_2DAnimation mNormalSelect = null;
        [SerializeField] private JCS_2DAnimation mHelpSelect = null;
        [SerializeField] private JCS_2DAnimation mWorkingInBackground = null;
        [SerializeField] private JCS_2DAnimation mBusy = null;
        [SerializeField] private JCS_2DAnimation mPrecisionSelect = null;
        [SerializeField] private JCS_2DAnimation mTextSelect = null;
        [SerializeField] private JCS_2DAnimation mHandwriting = null;
        [SerializeField] private JCS_2DAnimation mUnavaliable = null;
        [SerializeField] private JCS_2DAnimation mVerticalResize = null;
        [SerializeField] private JCS_2DAnimation mHorizontalResize = null;
        [SerializeField] private JCS_2DAnimation mDiagonalResize1 = null;
        [SerializeField] private JCS_2DAnimation mDiagonalResize2 = null;
        [SerializeField] private JCS_2DAnimation mMove = null;
        [SerializeField] private JCS_2DAnimation mAlternateSelect = null;
        [SerializeField] private JCS_2DAnimation mLinkSelect = null;

        /* Setter & Getter */

        public JCS_CursorCustomizeType CursorCustomizeType { get { return this.mCursorCustomizeType; } }
        public Vector3 CursorOffset { get { return this.mCursorOffset; } set { this.mCursorOffset = value; } }

        /* Functions */

        private void Awake()
        {
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();

            m2DAnimator.Animations.Add(mNormalSelect);
            m2DAnimator.Animations.Add(mHelpSelect);
            m2DAnimator.Animations.Add(mWorkingInBackground);
            m2DAnimator.Animations.Add(mBusy);
            m2DAnimator.Animations.Add(mPrecisionSelect);
            m2DAnimator.Animations.Add(mTextSelect);
            m2DAnimator.Animations.Add(mHandwriting);
            m2DAnimator.Animations.Add(mUnavaliable);
            m2DAnimator.Animations.Add(mVerticalResize);
            m2DAnimator.Animations.Add(mHorizontalResize);
            m2DAnimator.Animations.Add(mDiagonalResize1);
            m2DAnimator.Animations.Add(mDiagonalResize2);
            m2DAnimator.Animations.Add(mMove);
            m2DAnimator.Animations.Add(mAlternateSelect);
            m2DAnimator.Animations.Add(mLinkSelect);
        }

        private void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            FollowMouse();
        }

#if (UNITY_EDITOR)
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
