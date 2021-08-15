/**
 * $File: JCS_ScreenshotButton.cs $
 * $Date: 2020-07-03 01:05:24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Button that will take the screenshot from the current gameplay scene.
    /// </summary>
    public class JCS_ScreenshotButton : JCS_Button
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override void JCS_OnClickCallback()
        {
            JCS_Camera.main.TakeScreenshot();
        }
    }
}
