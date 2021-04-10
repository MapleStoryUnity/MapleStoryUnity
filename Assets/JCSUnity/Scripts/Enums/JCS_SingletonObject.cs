/**
 * $File: JCS_SingletonObject.cs $
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
    /// List of all the singleton object.
    /// </summary>
    public enum JCS_SingletonObject
    {
        NONE,

        JCS_MANAGERS,
        JCS_GAMESETTINGS,
        JCS_CANVAS,
        JCS_CAMERA,
        JCS_PLAYER
    }
}
