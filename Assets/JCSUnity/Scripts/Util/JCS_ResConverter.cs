/**
 * $File: JCS_ResConverter.cs $
 * $Date: 2020-08-04 15:22:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Utility that converts raw data to Engine usable resource.
    /// </summary>
    public static class JCS_ResConverter
    {
        /// <summary>
        /// Turn byte array to texture data.
        /// </summary>
        /// <param name="data"> Byte array ready to convert to texture data. </param>
        /// <returns> Texture that had been converted. </returns>
        public static Texture2D AsTexture2D(byte[] data)
        {
            return JCS_ImageLoader.ConvertToTexture(data);
        }

        /// <summary>
        /// Turn byte array to sprite data.
        /// </summary>
        /// <param name="data"> Byte array ready to convert to sprite data. </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> Sprite that had been converted. </returns>
        public static Sprite AsSprite(byte[] data, float pixelPerUnit = 100.0f)
        {
            Texture2D tex = AsTexture2D(data);
            return JCS_ImageLoader.Create(tex, pixelPerUnit);
        }

        /// <summary>
        /// Turn byte array to text data.
        /// </summary>
        /// <param name="data"> Byte array will be use to convert to text data. </param>
        /// <param name="charset"> Target charset type. </param>
        /// <returns> String data that had been converted. </returns>
        public static string AsText(byte[] data, JCS_CharsetType charset)
        {
            return JCS_Util.BytesToString(data, charset);
        }
    }
}
