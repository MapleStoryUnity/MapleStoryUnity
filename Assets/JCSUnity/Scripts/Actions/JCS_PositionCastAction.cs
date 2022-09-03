/**
 * $File: JCS_PositionCastAction.cs $
 * $Date: 2016-12-01 02:55:56 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Cast the current transform to certain space.
    /// 
    /// NOTE(jenchieh): 
    /// Screen: from [0,0] to [Screen.width, Screen.height]
    /// Viewport: from [-1, -1] to [1, 1]
    /// </summary>
    public class JCS_PositionCastAction : JCS_UnityObject , JCS_IAction
    {
        /* Variables */

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_PositionCastAction) **")]

        [Tooltip("Test this component with key.")]
        public bool testWithKey = false;

        [Tooltip("Key to test if cast to screen space.")]
        [SerializeField]
        private KeyCode mCastToScreenKey = KeyCode.S;

        [Tooltip("Test position to cast to screen space.")]
        [SerializeField]
        private Vector3 mCastToScreenPosition = Vector3.zero;

        [Tooltip("Key to test if cast to world soace.")]
        [SerializeField]
        private KeyCode mCastToWorldKey = KeyCode.W;

        [Tooltip("Test position to cast to world space.")]
        [SerializeField]
        private Vector3 mCastToWorldPosition = Vector3.zero;
#endif

        [Header("** Check Variables (JCS_PositionCastAction) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Runtime Variables (JCS_PositionCastAction) **")]

        [Tooltip("Corresponding position offset.")]
        [SerializeField]
        private Vector3 mPositionOffset = Vector3.zero;

        /* Setter & Getter */

#if UNITY_EDITOR
        public Vector3 CastToScreenPosition { get { return this.mCastToScreenPosition; } }
        public Vector3 CastToWorldPosition { get { return this.mCastToWorldPosition; } }
#endif
        public Vector3 PositionOffset { get { return this.mPositionOffset; } set { this.mPositionOffset = value; } }

        /* Functions */

        private void Start()
        {
            // Only need it for the UI.
            if (GetObjectType() == JCS_UnityObjectType.UI ||
                GetObjectType() == JCS_UnityObjectType.TEXT)
            {
                // Get panel root, in order to calculate the 
                // correct distance base on the resolution.
                mPanelRoot = JCS_PanelRoot.GetFromParent(this.transform);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(mCastToScreenKey))
                CastToScreen(mCastToScreenPosition);
            if (JCS_Input.GetKeyDown(mCastToWorldKey))
                CastToWorld(mCastToWorldPosition);
        }
#endif

        /// <summary>
        /// Make a canvas space object to a world space position.
        /// 
        /// NOTE(jenchieh): Make UI object (canvas space) on top of the 
        /// world space game object.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 CastToScreen(Vector3 pos)
        {
            var camera = JCS_Camera.main;

            Vector3 positionOffset = mPositionOffset;

            if (mPanelRoot != null)
            {
                positionOffset.x *= mPanelRoot.PanelDeltaWidthRatio;
                positionOffset.y *= mPanelRoot.PanelDeltaHeightRatio;
            }

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.TEXT:
                case JCS_UnityObjectType.UI:
                    {
                        Vector2 worldToCanvasSpace = camera.WorldToCanvasSpace(pos);

                        var canvas = JCS_Canvas.GuessCanvas();
                        var resizeUI = canvas.ResizeUI;

                        float targetScale = resizeUI.TargetScale;

                        if (targetScale != 0.0f)
                        {
                            worldToCanvasSpace.x /= resizeUI.TargetScale;
                            worldToCanvasSpace.y /= resizeUI.TargetScale;
                        }

                        this.LocalPosition = worldToCanvasSpace + (Vector2)positionOffset;
                    }
                    break;
            }

            return this.LocalPosition;
        }

        /// <summary>
        /// Make a 3D game object to canvas space position.
        /// 
        /// NOTE(jenchieh): Make world space game object on top of 
        /// the UI object (canvas space).
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 CastToWorld(Vector2 pos)
        {
            var camera = JCS_Camera.main;

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                case JCS_UnityObjectType.SPRITE:
                    {
                        this.LocalPosition = camera.CanvasToWorldSpace(pos) + mPositionOffset;
                    }
                    break;
            }

            return this.LocalPosition;
        }
    }
}
