/**
 * $File: JCS_ShowCanvasButton.cs $
 * $Date: 2021-12-28 23:43:48 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button to show canvas, so it's visible on the screen.
    /// </summary>
    public class JCS_ShowCanvasButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_ShowCanvasButton)")]

        [Tooltip("Array of canvas to show.")]
        public JCS_Canvas[] canvas = null;

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            JCS_UIUtil.ShowCanvas(canvas);
        }
    }
}
