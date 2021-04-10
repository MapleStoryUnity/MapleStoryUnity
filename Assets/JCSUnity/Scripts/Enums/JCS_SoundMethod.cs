/**
 * $File: JCS_SoundMethod.cs $
 * $Date: 2017-10-23 12:36:03 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// All kind of methods to play sound.
    /// </summary>
    public enum JCS_SoundMethod
    {
        // just play the sound.
        PLAY_SOUND,

        // play the sound while nothing is playing.
        PLAY_SOUND_WHILE_NOT_PLAYING,

        // interrupt all the current playing sound, then play sound.
        PLAY_SOUND_INTERRUPT,
    }
}
