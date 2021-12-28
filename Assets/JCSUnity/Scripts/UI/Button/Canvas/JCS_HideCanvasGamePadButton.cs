/**
 * $File: JCS_HideCanvasGamePadButton.cs $
 * $Date: 2021-12-29 01:23:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button to show canvas, so it's invisible on the screen. (Game Pad)
    /// </summary>
    public class JCS_HideCanvasGamePadButton : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_HideCanvasGamePadButton) **")]

        [Tooltip("Array of canvas to hide.")]
        public JCS_Canvas[] canvas = null;

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            JCS_UIUtil.HideCanvas(canvas);
        }
    }
}
