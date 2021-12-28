/**
 * $File: JCS_ShowCanvasGamePadButton.cs $
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
    /// Button to show canvas, so it's visible on the screen. (Game Pad)
    /// </summary>
    public class JCS_ShowCanvasGamePadButton : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ShowCanvasGamePadButton) **")]

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
