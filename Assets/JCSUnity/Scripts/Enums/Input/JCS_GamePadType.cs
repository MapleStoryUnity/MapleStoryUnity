/**
 * $File: JCS_GamePadType.cs $
 * $Date: 2017-10-07 08:00:33 $
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
    /// List of all known game pad type...
    /// </summary>
    public enum JCS_GamePadType
    {
        ALL,

        /* Sony Play Station */
        PS,
        PS2,
        PS3,
        PS4,

        /* Microsoft XBox */
        XBOX,
        XBOX_360,
        XBOX_ONE,
    }
}
