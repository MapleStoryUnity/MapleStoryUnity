/**
 * $File: JCS_ColliderType.cs $
 * $Date: 2020-05-08 14:15:45 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// List of collider type.
    /// </summary>
    public enum JCS_ColliderType
    {
        NONE,

        /* 3D */
        CHARACTER_CONTROLLER,
        BOX,
        SPHERE,
        CAPSULE,

        /* 2D */
        BOX_2D,
        CIRCLE_2D,
        CAPSULE_2D,
    }
}
