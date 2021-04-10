/**
 * $File: JCS_ScreenSizef.cs $
 * $Date: 2020-11-24 16:31:43 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Screen size definition. (Float)
    /// </summary>
    [System.Serializable]
    public class JCS_ScreenSizef
    {
        public static JCS_ScreenSizef zero { get { return new JCS_ScreenSizef(0.0f, 0.0f); } }

        [Tooltip("Width of the screen.")]
        public float width = 0;

        [Tooltip("Height of the screen.")]
        public float height = 0;

        public JCS_ScreenSizef(float width, float height)
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
