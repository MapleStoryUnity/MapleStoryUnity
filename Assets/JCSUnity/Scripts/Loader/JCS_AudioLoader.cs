/**
 * $File: JCS_AudioLoader.cs $
 * $Date: 2018-07-30 18:56:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace JCSUnity
{
    /// <summary>
    /// Audiol loader, load an external audio file.
    /// </summary>
    public static class JCS_AudioLoader
    {
        /// <summary>
        /// Load the audio from path/url in runtime.
        /// </summary>
        /// <param name="path"> Path without file name. </param>
        /// <param name="filename"> Name of the file. </param>
        /// <param name="type"> Type of the audio clip. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            string path,
            string filename,
            AudioType type = AudioType.OGGVORBIS,
            Action<AudioClip> callback = null)
        {
            string url = Path.Join(path, filename);
            return LoadAudio(url, type, callback);
        }

        /// <summary>
        /// Load the audio from path/url in runtime.
        /// </summary>
        /// <param name="url"> Url to the target audio file. </param>
        /// <param name="type"> Type of the audio clip. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            string url,
            AudioType type = AudioType.OGGVORBIS,
            Action<AudioClip> callback = null)
        {
#if UNITY_2018_1_OR_NEWER
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, type);
            yield return request.SendWebRequest();
            AudioClip ac = DownloadHandlerAudioClip.GetContent(request);
#else
            WWW request = new WWW(url);
            yield return request;
            AudioClip ac = request.GetAudioClip(false, false);
#endif

            if (callback != null)
                callback.Invoke(ac);
        }
    }
}
