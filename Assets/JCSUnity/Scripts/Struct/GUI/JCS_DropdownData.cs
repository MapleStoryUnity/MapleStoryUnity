/**
 * $File: JCS_DropdownData.cs $
 * $Date: 2018-08-26 00:06:32 $
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
    /// Dropdown data to record it down.
    /// </summary>
    [System.Serializable]
    public class JCS_DropdownData
        : JCS_UIComponentData
    {
        public int value = 0;
    }
}
