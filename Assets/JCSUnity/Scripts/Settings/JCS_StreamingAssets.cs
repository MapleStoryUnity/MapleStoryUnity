/**
 * $File: JCS_StreamingAssets.cs $
 * $Date: 2020-08-03 22:47:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Generic streaming assets interface.
    /// </summary>
    public class JCS_StreamingAssets : JCS_Settings<JCS_StreamingAssets>
    {
        public delegate void RequestCallback(string path, bool success);

        /* Variables */

        private byte[] REQ_KEY = Encoding.ASCII.GetBytes("WAIT-090218");

        private byte[] mResultData = null;

        private RequestCallback requestCallback = null;

        [Separator("Check Variables (JCS_StreamingAssets)")]

        [Tooltip("List of file that we are going to download as target.")]
        [ReadOnly]
        public List<string> downloadList = null;

        [Tooltip("Flag to see if currently downloading the file.")]
        [ReadOnly]
        public bool requesting = false;

        [Tooltip("Full request URL.")]
        [ReadOnly]
        public string requestURL = "";

        [Tooltip("Request data path.")]
        [ReadOnly]
        public string requestPath = "";

        [Separator("Initialize Variables (JCS_StreamingAssets)")]

        [Tooltip("Preload streaming assets path.")]
        public List<string> preloadPath = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);

            downloadList = preloadPath;
        }

        private void Update()
        {
            ProcessDownload();
        }

        /// <summary>
        /// Return streaming assets path.
        /// </summary>
        public static string StreamingAssetsPath()
        {
            return Application.streamingAssetsPath;
        }

        /// <summary>
        /// Return the streaming assets cache path.
        /// </summary>
        public static string CachePath()
        {
            var gs = JCS_GameSettings.instance;
            string path = JCS_Path.Combine(Application.persistentDataPath, gs.STREAMING_CACHE_PATH);
            return path;
        }

        /// <summary>
        /// Return static URL path.
        /// </summary>
        public static string UrlPath()
        {
            var gs = JCS_GameSettings.instance;
            return gs.STREAMING_BASE_URL;
        }

        /// <summary>
        /// Check if current data are requesting data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool NeedRequestData(byte[] data)
        {
            return REQ_KEY == data;
        }

        /// <summary>
        /// Get all the bytes by PATH.
        /// </summary>
        public byte[] ReadAllBytes(string path, RequestCallback callback = null)
        {
            byte[] data = TryCacheData(path);
            if (data == null) data = TryStreamingAssets(path);
            if (data == null)
            {
                if (callback != null)
                {
                    if (requestCallback == null)
                        requestCallback = callback;
                    else
                    {
                        JCS_Debug.LogWarning("Override request callback is denied");
                    }
                }
                AddDownloadTarget(path);
                data = REQ_KEY;  // Set to `wait` key!
            }
            return data;
        }

        /// <summary>
        /// Add a download target by streaming assets PATH.
        /// </summary>
        public void AddDownloadTarget(string path)
        {
            downloadList.Add(path);
        }

        protected override void TransferData(JCS_StreamingAssets _old, JCS_StreamingAssets _new)
        {
            _old.requestCallback = _new.requestCallback;

            _old.downloadList = _new.downloadList;
            _old.requesting = _new.requesting;
            _old.requestURL = _new.requestURL;
            _old.requestPath = _new.requestPath;

            _old.preloadPath = _new.preloadPath;
        }

        private bool ValidCacheData(string path)
        {
            string filePath = CachePath() + path;
            return JCS_IO.IsFile(filePath);
        }

        private bool ValidStreamingAssets(string path)
        {
            string filePath = StreamingAssetsPath() + path;
            return JCS_IO.IsFile(filePath);
        }

        private byte[] TryCacheData(string path)
        {
            if (!ValidCacheData(path))
                return null;
            string filePath = CachePath() + path;
            return File.ReadAllBytes(filePath);
        }

        private byte[] TryStreamingAssets(string path)
        {
            if (!ValidStreamingAssets(path))
                return null;
            string filePath = StreamingAssetsPath() + path;
            return File.ReadAllBytes(filePath);
        }

        private void TryUrlData(string path)
        {
            mResultData = null;
            requestPath = path;
            requestURL = UrlPath() + requestPath;

            requesting = true;

            StartCoroutine(GetData());
        }

        private IEnumerator GetData()
        {
            UnityWebRequest www = UnityWebRequest.Get(requestURL);
            yield return www.SendWebRequest();

            bool success = false;

#if UNITY_2021_1_OR_NEWER
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                if (JCS_GameSettings.instance.DEBUG_MODE)
                    JCS_Debug.LogWarning(www.error);
            }
            else
            {
                mResultData = www.downloadHandler.data;
                WriteFileAsCache(requestPath, mResultData);

                success = true;
            }

            if (requestCallback != null)
                requestCallback.Invoke(requestPath, success);
            requestCallback = null;

            downloadList.Remove(requestPath);
            requesting = false;
        }

        /// <summary>
        /// Write a file and make it as cache data file.
        /// </summary>
        /// <param name="path"> Path to write to. </param>
        /// <param name="data"> Bytes data to write with. </param>
        private static void WriteFileAsCache(string path, byte[] data)
        {
            string filePath = CachePath() + path;
            string dirPath = Path.GetDirectoryName(filePath);
            JCS_IO.CreateDirectory(dirPath);
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Process the download list and try to download all target files.
        /// </summary>
        private void ProcessDownload()
        {
            if (downloadList.Count == 0 || requesting)
                return;

            string path = downloadList[0];

            if (NeedRequestData(ReadAllBytes(path)))
                TryUrlData(path);
            else
            {
                downloadList.RemoveAt(0);
            }
        }
    }
}
