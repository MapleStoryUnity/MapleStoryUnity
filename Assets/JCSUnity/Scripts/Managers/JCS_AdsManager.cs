/* NOTE: If you are using `Unity Ads` uncomment this line.
 */
//#define UNITY_ADS

#if (UNITY_ANDRIOD || UNITY_IOS || UNITY_EDITOR) && UNITY_ADS
/**
 * $File: JCS_AdsManager.cs $
 * $Date: 2017-04-28 21:48:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle Advertisment provide by Unity Technologies company.
    /// 
    /// NOTE(jenchieh): If using this manager, you need to do these 
    /// two thing.
    /// 
    ///     1) Work Online not offline on Unity.
    ///     2) Enable window->service->Ads service provided by Unity 
    ///       Technologies.
    /// 
    /// </summary>
    public class JCS_AdsManager : JCS_Manager<JCS_AdsManager>
        , IUnityAdsListener
    {
        public delegate void AdsDidErrorCallback(string message);
        public delegate void AdsDidFinishCallback(string placementId, ShowResult showResult);
        public delegate void AdsDidStartCallback(string placementId);
        public delegate void AdsReadyCallback(string placementId);

        /* Variables */

        [Separator("Initialize Variables (JCS_AdsManager)")]

#if (UNITY_ANDROID)
        [Tooltip("Andriod game id provided by Unity Server window.")]
        public string idAndroid = "";
#elif (UNITY_IOS)
        [Tooltip("iOS game id provided by Unity Server window.")]
        public string idiOS = "";
#endif

        private string mPlacementId = "rewardedVideo";

        [Tooltip("Test mode for advertisement module.")]
        private bool mTestMode = false;

        /* Setter & Getter */

        public bool testMode { get { return this.mTestMode; } set { this.mTestMode = value; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

#if UNITY_ANDROID
            Advertisement.Initialize(idAndroid, mTestMode);
#elif UNITY_IOS
            Advertisement.Initialize(idiOS, mTestMode);
#endif
        }

        /// <summary>
        /// Start the reward ads video. This will pop out the video screen.
        /// </summary>
        /// <param name="delayTime"> time delay. </param>
        /// <param name="result"> result callback </param>
        public void StartDelayRewardVideo(float delayTime, Action<ShowResult> result)
        {
            StartCoroutine(DelayedRewardVideo(delayTime, result));
        }

        /// <summary>
        /// Main function delay and show reward ads.
        /// </summary>
        /// <param name="delayTime"> time delay. </param>
        /// <param name="result"> result callback </param>
        /// <returns> result of the coroutine. </returns>
        private IEnumerator DelayedRewardVideo(float delayTime, Action<ShowResult> result)
        {
            yield return new WaitForSeconds(delayTime);

            ShowRewardedAd(result);
        }

        /// <summary>
        /// Show reward ads.
        /// </summary>
        /// <param name="callback"> function accept the result. </param>
        public void ShowRewardedAd(Action<ShowResult> callback)
        {
            if (!Advertisement.IsReady(mPlacementId))
                return;

            Advertisement.AddListener(this);
            Advertisement.Show(mPlacementId);
        }

        //----------------------------------------------------------------------

        public AdsDidErrorCallback adsDidError = null;
        public AdsDidFinishCallback adsDidFinish = null;
        public AdsDidStartCallback adsDidStart = null;
        public AdsReadyCallback adsReady = null;

        void IUnityAdsListener.OnUnityAdsDidError(string message)
        {
            if (adsDidError == null)
                return;

            adsDidError.Invoke(message);
        }

        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (adsDidFinish == null)
                return;

            adsDidFinish.Invoke(placementId, showResult);
        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
        {
            if (adsDidStart == null)
                return;

            adsDidStart.Invoke(placementId);
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId)
        {
            if (adsReady == null)
                return;

            adsReady.Invoke(placementId);
        }
    }
}
#endif
