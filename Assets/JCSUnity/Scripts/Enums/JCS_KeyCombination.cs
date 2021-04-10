/**
 * $File: JCS_KeyCombination.cs $
 * $Date: 2018-08-28 19:32:10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Key combination.
    /// </summary>
    public enum JCS_KeyCombination
    {
        NONE, 

        /* 1 key, total of 2 keys combination. */
        ALT, 
        CTRL,
        SHIFT,

        /* 2 keys, total of 3 keys combination. */
        ALT_CTRL, 
        ALT_SHIFT,
        CTRL_SHIFT,

        /* All 3 keys, total of 4 keys combination. */
        ALT_CTRL_SHIFT,
    }
}
