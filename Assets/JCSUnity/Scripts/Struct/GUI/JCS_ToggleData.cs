/**
 * $File: JCS_ToggleData.cs $
 * $Date: 2018-08-26 00:23:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Toggle data.
    /// </summary>
    [Serializable]
    public class JCS_ToggleData : JCS_IUIComponentData
    {
        public bool isOn = false;
    }
}
