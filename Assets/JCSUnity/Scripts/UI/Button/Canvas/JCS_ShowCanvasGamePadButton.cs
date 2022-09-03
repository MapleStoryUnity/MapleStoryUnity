/**
 * $File: JCS_ShowCanvasGamepadButton.cs $
 * $Date: 2021-12-29 01:22:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button to show canvas, so it's visible on the screen. (Gamepad)
    /// </summary>
    public class JCS_ShowCanvasGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ShowCanvasGamepadButton) **")]

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
