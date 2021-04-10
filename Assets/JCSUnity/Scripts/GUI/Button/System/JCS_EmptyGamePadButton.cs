/**
 * $File: JCS_EmptyGamePadButton.cs $
 * $Date: 2017-10-21 13:09:38 $
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
    /// Empty gamepad button.
    /// </summary>
    public class JCS_EmptyGamePadButton
        : JCS_GamePadButton
    {
        public override void JCS_OnClickCallback()
        {
            // empty.
        }
    }
}
