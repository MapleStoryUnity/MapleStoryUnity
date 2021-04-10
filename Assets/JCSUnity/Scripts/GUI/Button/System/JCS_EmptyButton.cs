/**
 * $File: JCS_EmptyButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Empty button specific for system call back usage.
    /// </summary>
    public class JCS_EmptyButton
        : JCS_Button
    {
        public override void JCS_OnClickCallback()
        {
            // empty.
        }
    }
}
