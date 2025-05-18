/* NOTE: If you are using `Unity Ads` uncomment this line.
 */
//#define UNITY_ADS

#if (UNITY_ANDRIOD || UNITY_IOS || UNITY_EDITOR) && UNITY_ADS
/**
 * $File: JCS_RewardAdsButton.cs $
 * $Date: 2017-04-28 21:59:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Advertisements;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Reward type of Ads button.
    /// </summary>
    public class JCS_RewardAdsButton : 
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_RewardAdsButton)")]

        [Tooltip("Time to delay the ads shows.")]
        [SerializeField]
        private float mDelayTime = 2.5f;

        // call back function.
        private Action<ShowResult> mRewardCallback = null;

        /* Setter & Getter */

        public void SetRewardCallback(Action<ShowResult> func) { this.mRewardCallback = func; }

        /* Functions */

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public override void OnClick()
        {
            if (mRewardCallback == null)
            {
                Debug.LogWarning("Active default reward function, please fill the reward callback!");
                return;
            }

            // start reward video.
            JCS_AdsManager.instance.StartDelayRewardVideo(
                mDelayTime,
                mRewardCallback);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Reward example function. Your function should be 
        /// similar to this.
        /// </summary>
        /// <param name="result"> result for the reward video? </param>
        private void OnRewardAdWatched(ShowResult result)
        {
            if (result == ShowResult.Failed)
            {
                Debug.Log(this, "Reward Ads video get Failed.");
            }
            else if (result == ShowResult.Finished)
            {
                Debug.Log(this, "Reward Ads video get Finished.");
            }
            else if (result == ShowResult.Skipped)
            {
                Debug.Log(this, "Reward Ads video get Skipped.");
            }
        }
#endif
    }
}
#endif
