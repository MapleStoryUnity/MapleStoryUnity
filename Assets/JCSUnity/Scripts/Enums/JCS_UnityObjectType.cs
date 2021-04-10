/**
 * $File: JCS_UnityObjectType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Unity Engine's type.
    /// </summary>
    public enum JCS_UnityObjectType
    {
        GAME_OBJECT,
        UI,
        SPRITE,
        TEXT,
#if TMP_PRO
        TMP,  // Text Mesh Pro
#endif
    }
}
