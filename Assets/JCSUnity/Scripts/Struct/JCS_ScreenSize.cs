/**
 * $File: JCS_ScreenSize.cs $
 * $Date: 2020-11-24 16:18:54 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Screen size definition. (Integer)
    /// </summary>
    [Serializable]
    public class JCS_ScreenSize
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_ScreenSize)")]

        [Tooltip("Width of the screen.")]
        [Range(1, 8192)]
        public int width = 0;

        [Tooltip("Height of the screen.")]
        [Range(1, 8192)]
        public int height = 0;

        /* Setter & Getter */

        public static JCS_ScreenSize zero { get { return new JCS_ScreenSize(0, 0); } }

        /* Functions */

        public JCS_ScreenSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return "(w: " + width + ", h: " + height + ")";
        }
    }
}
