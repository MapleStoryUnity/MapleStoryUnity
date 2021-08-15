/**
 * $File: JCS_AnchorPresetsType.cs $
 * $Date: 2018-09-04 21:15:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Anchore Presets type.
    /// </summary>
    public enum JCS_AnchorPresetsType
    {
        NONE, 

        // First column.
        LEFT_TOP,
        LEFT_MIDDLE,
        LEFT_BOTTOM,
        LEFT_STRETCH,

        // Second column.
        CENTER_TOP,
        CENTER_MIDDLE,
        CENTER_BOTTOM,
        CENTER_STRETCH,

        // Third column.
        RIGHT_TOP,
        RIGHT_MIDDLE,
        RIGHT_BOTTOM,
        RIGHT_STRETCH,

        // Fourth column.
        STRETCH_TOP,
        STRETCH_MIDDLE,
        STRETCH_BOTTOM, 
        STRETCH_STRETCH,
    }
}
