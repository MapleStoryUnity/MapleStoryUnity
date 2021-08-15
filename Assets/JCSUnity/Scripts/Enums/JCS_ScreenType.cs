/**
 * $File: JCS_ScreenType.cs $
 * $Date: 2018-09-08 15:34:34 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Type of the screen handle.
    /// </summary>
    public enum JCS_ScreenType
    {
        NONE,

        // Always make the screen the same as standards.
        // SEE(jenchieht): 
        //   => JCS_ScreenSettings => STANDARD_SCREEN_WIDTH
        //   => JCS_ScreenSettings => STANDARD_SCREEN_HEIGHT
        ALWAYS_STANDARD, 

        // Always force the screen same aspect ratio.
        FORCE_ASPECT,

        // Resizable whatever.
        RESIZABLE,
    }
}
