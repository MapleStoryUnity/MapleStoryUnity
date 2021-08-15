/**
 * $File: JCS_CursorCustomizeType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Cursor state type.
    /// </summary>
    public enum JCS_CursorCustomizeType
    {
        NORMAL_SELECT = 0,
        HELP_SELECT = 1,
        WORKING_IN_BACKGROUND = 2,
        BUSY = 3,
        PRECISION_SELECT = 4,
        TEXT_SELECT = 5,
        HANDWRITING = 6,
        UNAVAILABLE = 7,
        VERTICAL_RESIZE = 8,
        HORIZONTAL_RESIZE = 9,
        DIAGONAL_RESIZE_1 = 10,
        DIAGONAL_RESIZE_2 = 11,
        MOVE = 12,
        ALTERNATE_SELECT = 13,
        LINK_SELECT = 14, 
    }
}
