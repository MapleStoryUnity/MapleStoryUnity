/**
 * $File: JCS_Vector3Direction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Vector 3d type in enum use in Unity Engine.
    /// </summary>
    public enum JCS_Vector3Direction
    {
        CENTER             = 0,   // (0, 0, 0)
        UP                 = 1,   // (0, 1, 0)
        DOWN               = 2,   // (0, -1, 0)
        FORWARD            = 3,   // (0, 0, 1)
        BACK               = 4,   // (0, 0, -1)
        RIGHT              = 5,   // (1, 0, 0)
        LEFT               = 6,   // (-1, 0, 0)
        FORWARD_LEFT       = 7,   // (-1, 0, 1)
        FORWARD_RIGHT      = 8,   // (1, 0, 1)
        BACK_LEFT          = 9,   // (-1, 0, -1)
        BACK_RIGHT         = 10,  // (1, 0, -1)
        UP_LEFT            = 11,  // (-1, 1, 0)
        UP_RIGHT           = 12,  // (1, 1, 0)
        FORWARD_UP         = 13,  // (0, 1, 1)
        FORWARD_DOWN       = 14,  // (0, -1, 1)
        BACK_UP            = 15,  // (0, 1, -1)
        BACK_DOWN          = 16,  // (0, -1, -1)
        DOWN_LEFT          = 17,  // (-1, -1, 0)
        DOWN_RIGHT         = 18,  // (1, -1, 0)
        FORWARD_UP_LEFT    = 19,  // (-1, 1, 1)
        FORWARD_UP_RIGHT   = 20,  // (1, 1, 1)
        FORWARD_DOWN_LEFT  = 21,  // (-1, -1, 1)
        FORWARD_DOWN_RIGHT = 22,  // (1, -1, 1)
        BACK_UP_LEFT       = 23,  // (-1, 1, -1)
        BACK_UP_RIGHT      = 24,  // (1, 1, -1)
        BACK_DOWN_LEFT     = 25,  // (-1, -1, -1)
        BACK_DOWN_RIGHT    = 26,  // (1, -1, -1)
    }
}
