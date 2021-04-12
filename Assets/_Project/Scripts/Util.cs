/**
 * $File: Util.cs $
 * $Date: 2021-04-13 00:08:58 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MSU
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Util
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
