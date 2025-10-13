/**
 * $File: JCS_ScreenManager.cs $
 * $Date: 2018-09-12 02:50:44 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manages screen action.
    /// </summary>
    public class JCS_ScreenManager : JCS_Manager<JCS_ScreenManager>
    {
        /* Variables */

        private const string RESIZEABLE_SCREEN_PANEL_PATH = "UI/JCS_ResizableScreenPanel";

        [Separator("Check Variables (JCS_ScreenManager)")]

        [Tooltip("Top resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mTopASP = null;

        [Tooltip("Bottom resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mBottomASP = null;

        [Tooltip("Left resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mLeftASP = null;

        [Tooltip("Right resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mRightASP = null;

        [Separator("Initialize Variables (JCS_ScreenManager)")]

        [Tooltip("Resize screen once on this scene?")]
        public bool resizeScreen = false;

        [Tooltip("Aspect ratio screen width for this scene you want to resize.")]
        public int aspectRatioWidth = 16;

        [Tooltip("Aspect ratio screen height for this scene you want to resize.")]
        public int aspectRatioHeight = 9;

        /* Setter & Getter */

        public JCS_ResizableScreenPanel topASP { get { return mTopASP; } }
        public JCS_ResizableScreenPanel bottomASP { get { return mBottomASP; } }
        public JCS_ResizableScreenPanel leftASP { get { return mLeftASP; } }
        public JCS_ResizableScreenPanel rightASP { get { return mRightASP; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);

            if (ShouldSpawnResizablePanels())
            {
                // Spawn the four aspect screen panels.
                mTopASP = JCS_Util.Instantiate(RESIZEABLE_SCREEN_PANEL_PATH).GetComponent<JCS_ResizableScreenPanel>();
                mBottomASP = JCS_Util.Instantiate(RESIZEABLE_SCREEN_PANEL_PATH).GetComponent<JCS_ResizableScreenPanel>();
                mLeftASP = JCS_Util.Instantiate(RESIZEABLE_SCREEN_PANEL_PATH).GetComponent<JCS_ResizableScreenPanel>();
                mRightASP = JCS_Util.Instantiate(RESIZEABLE_SCREEN_PANEL_PATH).GetComponent<JCS_ResizableScreenPanel>();

                // Set the ASP direction.
                mTopASP.placeDirection = JCS_2D4Direction.TOP;
                mBottomASP.placeDirection = JCS_2D4Direction.BOTTOM;
                mLeftASP.placeDirection = JCS_2D4Direction.LEFT;
                mRightASP.placeDirection = JCS_2D4Direction.RIGHT;
            }
        }

        private void Start()
        {
            var ss = JCS_ScreenSettings.FirstInstance();

            if (resizeScreen)
            {
                // Apply new screen aspect ratio.
                ss.aspectRatioSize.width = aspectRatioWidth;
                ss.aspectRatioSize.height = aspectRatioHeight;

                // Resize the screen base on the new screen aspect ratio.
                ss.ForceAspectScreenOnce();
            }

            // Set the panels' color
            SetResizablePanelsColor(ss.RESIZABLE_PANELS_COLOR);
        }

#if UNITY_EDITOR
        private void Update()
        {
            ControlResizablePanels();
        }

        private void ControlResizablePanels()
        {
            var ss = JCS_ScreenSettings.FirstInstance();

            /* Handle color. */
            {
                // Make color editable in runtime.
                SetResizablePanelsColor(ss.RESIZABLE_PANELS_COLOR);
            }

            /* Show hide the panels. */
            {
                if (ss.showResizablePanels)
                    ShowResizablePanels();
                else
                    HideResizablePanels();
            }
        }
#endif

        /// <summary>
        /// Show the resizable panels.
        /// </summary>
        public void ShowResizablePanels()
        {
            if (!ShouldSpawnResizablePanels())
                return;

            mTopASP.ShowPanel();
            mBottomASP.ShowPanel();
            mLeftASP.ShowPanel();
            mRightASP.ShowPanel();
        }

        /// <summary>
        /// Hide the resizable panels.
        /// </summary>
        public void HideResizablePanels()
        {
            if (!ShouldSpawnResizablePanels())
                return;

            mTopASP.HidePanel();
            mBottomASP.HidePanel();
            mLeftASP.HidePanel();
            mRightASP.HidePanel();
        }

        /// <summary>
        /// Set the color to all resizable panels.
        /// </summary>
        /// <param name="newColor"></param>
        public void SetResizablePanelsColor(Color newColor)
        {
            if (!ShouldSpawnResizablePanels())
                return;

            mTopASP.image.color = newColor;
            mBottomASP.image.color = newColor;
            mLeftASP.image.color = newColor;
            mRightASP.image.color = newColor;
        }

        /// <summary>
        /// Return true, if we should use resizalbe panels.
        /// </summary>
        private bool ShouldSpawnResizablePanels()
        {
            var screenS = JCS_ScreenSettings.FirstInstance();
            return screenS.ShouldSpawnResizablePanels();
        }
    }
}
