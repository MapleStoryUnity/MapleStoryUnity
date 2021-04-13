/**
 * $File: JCS_AudioLoader.cs $
 * $Date: 2018-07-30 18:56:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace JCSUnity
{
    public delegate void AudioLoaded(AudioClip ac);

    /// <summary>
    /// Audiol loader, load an external audio file.
    /// </summary>
    public static class JCS_AudioLoader
    {
        /// <summary>
        /// Load the music from path in runetime.
        /// </summary>
        /// <param name="ac"> A container for audio data. </param>
        /// <param name="path"> file path, not include filename. </param>
        /// <param name="filename"> file name of the ogg file. </param>
        /// <param name="type"> Audio clip type. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            AudioClip ac,
            string path,
            string filename,
            AudioType type = AudioType.OGGVORBIS,
            AudioLoaded callback = null)
        {
            string fullFilePath = path + filename;
            return LoadAudio(ac, fullFilePath, type, callback);
        }

        /// <summary>
        /// Load the music from path in runtime.
        /// </summary>
        /// <param name="ac"> A container for audio data. </param>
        /// <param name="fullFilePath"> Filpath to the target audio file. </param>
        /// <param name="type"> Audio clip type. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            AudioClip ac,
            string fullFilePath,
            AudioType type = AudioType.OGGVORBIS, 
            AudioLoaded callback = null)
        {
            string formatFullFilePath = string.Format("file://{0}", fullFilePath);

#if UNITY_2018_1_OR_NEWER
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(formatFullFilePath, type);
            yield return request.SendWebRequest();
            ac = DownloadHandlerAudioClip.GetContent(request);
#else
            WWW request = new WWW(formatFullFilePath);
            yield return request;
            ac = request.GetAudioClip(false, false);
#endif

            if (callback != null)
                callback.Invoke(ac);
        }
    }
}
