/**
 * $File: JCS_2D8Direction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{

    /*
          0 | 1 | 2
        -------------
          7 | 8 | 3
        -------------
          6 | 5 | 4

        */

    /// <summary>
    /// Base on Graph above
    /// </summary>
    public enum JCS_2D8Direction
    {
        // 4 sides
        TOP = 1,
        BOTTOM = 5,
        RIGHT = 3,
        LEFT = 7,

        // 4 corners
        TOP_LEFT = 0,
        TOP_RIGHT = 2,
        BOTTOM_RIGHT = 4,
        BOTTOM_LEFT = 6,

        CENTER = 8
    }
}
