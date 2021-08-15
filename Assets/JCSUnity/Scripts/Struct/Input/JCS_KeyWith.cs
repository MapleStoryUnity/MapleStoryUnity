/**
 * $File: JCS_KeyWith.cs $
 * $Date: 2018-08-28 23:26:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Data struct hold combination key info.
    /// </summary>
    [System.Serializable]
    public struct JCS_KeyWith
    {
        // Combination key.
        public JCS_KeyCombination comb;

        // Major key code.
        public KeyCode key;
    }
}
