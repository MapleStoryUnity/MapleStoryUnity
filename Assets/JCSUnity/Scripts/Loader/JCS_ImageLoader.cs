/**
 * $File: JCS_ImageLoader.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace JCSUnity
{
    /// <summary>
    /// Callback after the image is loaded.
    /// </summary>
    /// <param name="tex"> The loaded image in texture. </param>
    public delegate void ImageLoaded(Texture2D tex);

    /// <summary>
    /// Image loader, load image to sprite from resource.
    /// </summary>
    public static class JCS_ImageLoader
    {
        /// <summary>
        /// Convert byte array to texture object.
        /// </summary>
        /// <param name="data"> Image byte array. </param>
        /// <returns> Texture object that fills with image DATA. </returns>
        public static Texture2D ConvertToTexture(byte[] data)
        {
            var tex = new Texture2D(2, 2);
            tex.LoadImage(data);
            return tex;
        }

        /// <summary>
        /// Load image file as texture.
        /// </summary>
        /// <param name="filePath"> Image file path. </param>
        /// <returns> Image data. </returns>
        public static Texture2D LoadTexture(string filePath)
        {
            byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);
            var tex = ConvertToTexture(pngBytes);
            return tex;
        }

        /// <summary>
        /// Create sprite from byte array.
        /// </summary>
        /// <param name="data"> Image byte data. </param>
        /// <returns> sprite object. </returns>
        public static Sprite Create(byte[] data)
        {
            var tex = ConvertToTexture(data);
            return Create(tex);
        }

        /// <summary>
        /// Create sprite object.
        /// </summary>
        /// <param name="tex"> image data </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> sprite object. </returns>
        public static Sprite Create(Texture2D tex, float pixelPerUnit = 100.0f)
        {
            return Create(tex, 0.0f, 0.0f, tex.width, tex.height, pixelPerUnit);
        }

        /// <summary>
        /// Create sprite object.
        /// </summary>
        /// <param name="tex"> image data </param>
        /// <param name="x"> position x </param>
        /// <param name="y"> position y </param>
        /// <param name="width"> width of the image </param>
        /// <param name="height"> height of the image </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> sprite object. </returns>
        public static Sprite Create(
            Texture2D tex,
            float x, float y,
            float width, float height,
            float pixelPerUnit = 100.0f)
        {
            return Sprite.Create(
                tex,
                new Rect(x, y, width, height),
                new Vector2(0.5f, 0.5f),
                pixelPerUnit);
        }

        /// <summary>
        /// Load Image by file path.
        /// </summary>
        /// <param name="filePath"> Image file path. </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> Sprite object. </returns>
        public static Sprite LoadImage(string filePath, float pixelPerUnit = 100.0f)
        {
            Sprite img = null;
            if (JCS_IO.IsFileOrDirectoryExists(filePath))
            {
                Texture2D tex = LoadTexture(filePath);
                img = Create(tex, 0.0f, 0.0f, tex.width, tex.height, pixelPerUnit);
            }
            return img;
        }

        /// <summary>
        /// Load Image by file path.
        /// </summary>
        /// <param name="filePath"> Image file path. </param>
        /// <param name="x"> position x </param>
        /// <param name="y"> position y </param>
        /// <param name="width"> width of the image </param>
        /// <param name="height"> height of the image </param>
        /// <param name="pixelPerUnit"> Pixel per unit conversion to world space. </param>
        /// <returns> Sprite object. </returns>
        public static Sprite LoadImage(
            string filePath,
            float x,
            float y,
            float width,
            float height,
            float pixelPerUnit = 100.0f)
        {
            Texture2D tex = LoadTexture(filePath);

            // Use custom x/y/width/height values.
            Sprite img = Create(tex, x, y, width, height, pixelPerUnit);

            return img;
        }

        /// <summary>
        /// Load the image from path/url in runtime.
        /// </summary>
        /// <param name="url"> Url to the target image file. </param>
        /// <param name="callback"> Callback after the image is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadImage(string url, ImageLoaded callback)
        {
#if UNITY_2018_1_OR_NEWER
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
#else
            WWW request = new WWW(url);
            yield return request;
            Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            request.LoadImageIntoTexture(tex);
#endif

            if (callback != null)
                callback.Invoke(tex);
        }
    }
}
