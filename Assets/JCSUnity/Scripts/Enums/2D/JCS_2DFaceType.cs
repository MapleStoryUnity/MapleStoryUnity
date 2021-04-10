/**
 * $File: JCS_2DFaceType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Standard Sprite should always face left
    /// so left is +1
    /// and right is -1
    /// </summary>
    public enum JCS_2DFaceType
    {
        NONE = 0,

        FACE_LEFT = 1,
        FACE_RIGHT = -1
    }
}
