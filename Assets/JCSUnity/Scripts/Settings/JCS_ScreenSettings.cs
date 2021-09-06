/**
 * $File: JCS_ScreenSettings.cs $
 * $Date: 2018-09-08 15:15:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    public delegate void OnScreenResize();

    /// <summary>
    /// Screen related settings.
    /// </summary>
    public class JCS_ScreenSettings : JCS_Settings<JCS_ScreenSettings>
    {
        /* Variables */

        public OnScreenResize onScreenResize = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_ScreenManager) **")]

        [Tooltip("Show the resizable screen panel in game?")]
        public bool SHOW_RESIZABLE_PANELS = true;
#endif

        [Header("** Check Variables (JCS_ScreenSettings) **")]

        [Tooltip("Screen size when the application starts.")]
        public JCS_ScreenSize STARTING_SCREEN_SIZE = JCS_ScreenSize.zero;

        [Tooltip("Store the camera orthographic size value over scene.")]
        public float ORTHOGRAPHIC_SIZE = 0.0f;

        [Tooltip("Store the camera filed of view value over scene.")]
        public float FIELD_OF_VIEW = 0.0f;

        [Tooltip("Previous screen size.")]
        public JCS_ScreenSizef PREV_SCREEN_SIZE = JCS_ScreenSizef.zero;

        [Tooltip("Current screen size.")]
        public JCS_ScreenSizef CURRENT_SCREEN_SIZE = JCS_ScreenSizef.zero;

        [Tooltip("Target aspect ratio screen size.")]
        public JCS_ScreenSize ASPECT_RATIO_SCREEN_SIZE = JCS_ScreenSize.zero;

        [Header("** Initialize Variables (JCS_ScreenSettings) **")]

        [Tooltip("Resize the screen/window to certain aspect when " +
            "the application starts. Aspect ratio can be set at 'JCS_ScreenManager'.")]
        public bool RESIZE_TO_ASPECT_WHEN_APP_STARTS = true;

        [Tooltip("Resize the screen/window to standard resoltuion when application starts.")]
        public bool RESIZE_TO_STANDARD_WHEN_APP_STARTS = false;

        [Tooltip("Resize the screen/window everytime a scene are loaded.")]
        public bool RESIZE_TO_ASPECT_EVERYTIME_SCENE_LOADED = false;

        [Tooltip("When resize, resize to the smaller edge, if not true will resize to larger edge.")]
        public bool RESIZE_TO_SMALLER_EDGE = true;

        [Tooltip("Defualt color to aspect panels.")]
        [SerializeField]
        private Color mResizablePanelsColor = Color.black;

        [Header("** Runtime Variables (JCS_ScreenSettings) **")]

        [Tooltip("Standard screen size to calculate the worldspace obejct's camera view.")]
        public JCS_ScreenSize STANDARD_SCREEN_SIZE = new JCS_ScreenSize(1920, 1080);

        [Tooltip("Type of the screen handle.")]
        public JCS_ScreenType SCREEN_TYPE = JCS_ScreenType.RESIZABLE;

        /* Setter & Getter */

        public Color RESIZABLE_PANELS_COLOR
        {
            get { return this.mResizablePanelsColor; }
            set
            {
                this.mResizablePanelsColor = value;

                JCS_ScreenManager.instance.SetResizablePanelsColor(this.mResizablePanelsColor);
            }
        }

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);

            // This will only run once at the time when 
            // the application is starts.
            if (!JCS_ApplicationSettings.instance.APPLICATION_STARTS)
            {
                // Calculate standard screen width and screen height.
                {
                    float gcd = JCS_Mathf.GCD(STANDARD_SCREEN_SIZE.width, STANDARD_SCREEN_SIZE.height);

                    ASPECT_RATIO_SCREEN_SIZE.width = (int)((float)STANDARD_SCREEN_SIZE.width / gcd);
                    ASPECT_RATIO_SCREEN_SIZE.height = (int)((float)STANDARD_SCREEN_SIZE.height / gcd);
                }

                if (RESIZE_TO_ASPECT_WHEN_APP_STARTS)
                {
                    // Force resize screen/window to certain aspect
                    // ratio once.
                    ForceAspectScreenOnce(true);
                }

                if (RESIZE_TO_STANDARD_WHEN_APP_STARTS)
                {
                    // Force resize screen/window to standard 
                    // resolution once.
                    ForceStandardScreenOnce(true);
                }

                /*
                 * NOTE(jenchieh): This is really weird, that even we 
                 * use 'Screen.SetResolution' function, the 'Screen.width'
                 * and 'Screen.height' will not change immediately.
                 * We just have to get it ourselves in all resize event
                 * function like above these functions.
                 * 
                 *   -> ForceAspectScreenOnce
                 *   -> ForceStandardScreenOnce
                 *   
                 */
                // Record down the starting screen width and screen height.
                //STARTING_SCREEN_SIZE.width = JCS_Screen.width;
                //STARTING_SCREEN_SIZE.height = JCS_Screen.height;
            }
            else
            {
                // Othereise, check if new scene loaded resize the 
                // screen once?
                if (RESIZE_TO_ASPECT_EVERYTIME_SCENE_LOADED)
                    ForceAspectScreenOnce();
            }
        }

        private void Start()
        {
            // When first run in the application...
            if (!JCS_ApplicationSettings.instance.APPLICATION_STARTS)
            {
                Camera cam = JCS_Camera.main.GetCamera();

                // Update the data just to see better.
                ORTHOGRAPHIC_SIZE = cam.orthographicSize;
                FIELD_OF_VIEW = cam.fieldOfView;
            }
        }

        private void LateUpdate()
        {
            DoScreenType();
        }

        /// <summary>
        /// Return true, if we should use resizalbe panels.
        /// </summary>
        public bool ShouldSpawnResizablePanels()
        {
            switch (SCREEN_TYPE)
            {
                case JCS_ScreenType.FIT_ALL:
                case JCS_ScreenType.MIXED:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Return true, if current screen type is responsive.
        /// </summary>
        public bool IsResponsive()
        {
            switch (SCREEN_TYPE)
            {
                case JCS_ScreenType.MIXED:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return the starting screen size by the current screen type.
        /// </summary>
        public JCS_ScreenSizef StartingScreenSize()
        {
            float newWidth;
            float newHeight;

            if (ShouldSpawnResizablePanels())
            {
                newWidth = STARTING_SCREEN_SIZE.width;
                newHeight = STARTING_SCREEN_SIZE.height;
            }
            else
            {
                newWidth = Screen.width;
                newHeight = Screen.height;
            }

            return new JCS_ScreenSizef(newWidth, newHeight);
        }

        /// <summary>
        /// Return the ratio from expected screen size to actual screen size.
        /// </summary>
        public JCS_ScreenSizef ScreenRatio()
        {
            return new JCS_ScreenSizef(
                STANDARD_SCREEN_SIZE.width / STARTING_SCREEN_SIZE.width,
                STANDARD_SCREEN_SIZE.height / STARTING_SCREEN_SIZE.height);
        }

        /// <summary>
        /// Return screen size of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns>
        /// Screen size information contain `width` and `height` value.
        /// </returns>
        public JCS_ScreenSizef BlackspaceSize()
        {
            return new JCS_ScreenSizef(
                JCS_Screen.width - STARTING_SCREEN_SIZE.width,
                JCS_Screen.height - STARTING_SCREEN_SIZE.height);
        }

        /// <summary>
        /// Get the screen size of the visible area.
        /// </summary>
        /// <returns>
        /// Screen size information contain `width` and `height` value.
        /// </returns>
        public JCS_ScreenSizef VisibleScreenSize()
        {
            JCS_ScreenSizef blackScreenSize = BlackspaceSize();
            return new JCS_ScreenSizef(
                JCS_Screen.width - blackScreenSize.width,
                JCS_Screen.height - blackScreenSize.height);
        }

        /// <summary>
        /// Make the screen in certain aspect ratio.
        /// </summary>
        /// <param name="starting"> Change the starting screen as well? </param>
        public void ForceAspectScreenOnce(bool starting = false)
        {
            int width = JCS_Screen.width;
            int height = JCS_Screen.height;

            bool smaller = ASPECT_RATIO_SCREEN_SIZE.width > ASPECT_RATIO_SCREEN_SIZE.height;

            // Reverse it if resize to larger edge.
            if (!RESIZE_TO_SMALLER_EDGE)
                smaller = !smaller;

            if (smaller)
            {
                // update the height
                float heightAccordingToWidth = width / ASPECT_RATIO_SCREEN_SIZE.width * ASPECT_RATIO_SCREEN_SIZE.height;
                JCS_Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);

                if (starting)
                {
                    STARTING_SCREEN_SIZE.width = width;
                    STARTING_SCREEN_SIZE.height = (int)heightAccordingToWidth;
                }
            }
            else
            {
                // update the width
                float widthAccordingToHeight = height / ASPECT_RATIO_SCREEN_SIZE.height * ASPECT_RATIO_SCREEN_SIZE.width;
                JCS_Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);

                if (starting)
                {
                    STARTING_SCREEN_SIZE.width = (int)widthAccordingToHeight;
                    STARTING_SCREEN_SIZE.height = height;
                }
            }
        }

        /// <summary>
        /// Resize the screen resolution to standard resolution once.
        /// </summary>
        /// <param name="starting"> Change the starting screen as well? </param>
        public void ForceStandardScreenOnce(bool starting = false)
        {
            JCS_Screen.SetResolution(STANDARD_SCREEN_SIZE.width, STANDARD_SCREEN_SIZE.height, false, 0);

            if (starting)
            {
                STARTING_SCREEN_SIZE.width = STANDARD_SCREEN_SIZE.width;
                STARTING_SCREEN_SIZE.height = STANDARD_SCREEN_SIZE.height;
            }
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_ScreenSettings _old, JCS_ScreenSettings _new)
        {
            _new.RESIZE_TO_ASPECT_WHEN_APP_STARTS = _old.RESIZE_TO_ASPECT_WHEN_APP_STARTS;
            _new.RESIZE_TO_STANDARD_WHEN_APP_STARTS = _old.RESIZE_TO_STANDARD_WHEN_APP_STARTS;
            _new.RESIZE_TO_ASPECT_EVERYTIME_SCENE_LOADED = _old.RESIZE_TO_ASPECT_EVERYTIME_SCENE_LOADED;

            _new.ORTHOGRAPHIC_SIZE = _old.ORTHOGRAPHIC_SIZE;
            _new.FIELD_OF_VIEW = _old.FIELD_OF_VIEW;

            _new.STARTING_SCREEN_SIZE = _old.STARTING_SCREEN_SIZE;
            _new.ASPECT_RATIO_SCREEN_SIZE = _old.ASPECT_RATIO_SCREEN_SIZE;
            _new.STANDARD_SCREEN_SIZE = _old.STANDARD_SCREEN_SIZE;

            _new.RESIZABLE_PANELS_COLOR = _old.RESIZABLE_PANELS_COLOR;

            _new.SCREEN_TYPE = _old.SCREEN_TYPE;
        }

        /// <summary>
        /// Do the task base on the screen type handle.
        /// </summary>
        private void DoScreenType()
        {
            if (!ShouldSpawnResizablePanels())
            {
                // These types do not expect resize!
                return;
            }    

            switch (SCREEN_TYPE)
            {
                case JCS_ScreenType.ALWAYS_STANDARD:
                    DoAlwaysStandard();
                    break;
                case JCS_ScreenType.FORCE_ASPECT:
                    DoFoceAspectScreen();
                    break;
                case JCS_ScreenType.RESIZABLE:
                    DoResizableScreen();
                    break;
            }
        }

        /// <summary>
        /// Always make the screen the same as standards screen width and 
        /// standard screen height.
        /// </summary>
        private void DoAlwaysStandard()
        {
            int width = JCS_Screen.width;
            int height = JCS_Screen.height;

            if (width != STANDARD_SCREEN_SIZE.width || height != STANDARD_SCREEN_SIZE.height)
            {
                JCS_Screen.SetResolution(STANDARD_SCREEN_SIZE.width, STANDARD_SCREEN_SIZE.height, false, 0);
            }

            this.PREV_SCREEN_SIZE.width = width;
            this.PREV_SCREEN_SIZE.height = height;
        }

        /// <summary>
        /// Force aspect window.
        /// 
        /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
        /// AUTHOR: Entity in JavaScript.
        /// Modefied: Jen-Chieh Shen to C#.
        /// </summary>
        private void DoFoceAspectScreen()
        {
            if (Screen.fullScreen)
                return;

            int width = JCS_Screen.width;
            int height = JCS_Screen.height;

            // if the user is changing the width
            if (PREV_SCREEN_SIZE.width != width)
            {
                // update the height
                float heightAccordingToWidth = width / ASPECT_RATIO_SCREEN_SIZE.width * ASPECT_RATIO_SCREEN_SIZE.height;
                JCS_Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);
            }

            // if the user is changing the height
            if (PREV_SCREEN_SIZE.height != height)
            {
                // update the width
                float widthAccordingToHeight = height / ASPECT_RATIO_SCREEN_SIZE.height * ASPECT_RATIO_SCREEN_SIZE.width;
                JCS_Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
            }

            this.PREV_SCREEN_SIZE.width = width;
            this.PREV_SCREEN_SIZE.height = height;
        }

        /// <summary>
        /// Do the resizable window.
        /// </summary>
        private void DoResizableScreen()
        {
            int width = JCS_Screen.width;
            int height = JCS_Screen.height;

            if (CURRENT_SCREEN_SIZE.width == width && CURRENT_SCREEN_SIZE.height == height)
                return;

            if (PREV_SCREEN_SIZE.width == 0.0f || PREV_SCREEN_SIZE.height == 0.0f)
            {
                // If zero, set to the same value.
                PREV_SCREEN_SIZE.width = width;
                PREV_SCREEN_SIZE.height = height;
            }
            else
            {
                // Record previous screen info.
                PREV_SCREEN_SIZE.width = CURRENT_SCREEN_SIZE.width;
                PREV_SCREEN_SIZE.height = CURRENT_SCREEN_SIZE.height;
            }

            // Update current screen info.
            CURRENT_SCREEN_SIZE.width = width;
            CURRENT_SCREEN_SIZE.height = height;

            // Do callback.
            if (onScreenResize != null)
                onScreenResize.Invoke();
        }
    }
}
