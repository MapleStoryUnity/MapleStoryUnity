/**
 * $File: JCS_PanelResizeType.cs $
 * $Date: 2021-08-27 21:06:42 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of resize method for panel UI.
    /// </summary>
    public enum JCS_PanelResizeType
    {
        NONE,

        KEEP_RATIO,  /* Keep the ratio, without stretching it (Default) */
        FIT_ALL,     /* Fit the full screen completely */
    }
}
