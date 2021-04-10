/**
 * $File: JCS_StaticLightning.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Lightning Splash effect.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPool))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_StaticLightning
    : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_StaticLightning) **")]

        [Tooltip("Lightning effect.")]
        [SerializeField]
        private JCS_WhiteScreen mLightning = null;

        [Tooltip("Possibility to occurs lightning effect.")]
        [SerializeField] [Range(0, 100)]
        private int mPossiblity = 50;

        [Tooltip("Random time to do the lightning effect.")]
        [SerializeField] [Range(1.0f, 5.0f)]
        private float mRandomTime = 2.5f;

        [Tooltip("This amount of time do chance lightning.")]
        [SerializeField]
        private float mLimitTime = 2.5f;

        private float mLimitTimer = 0.0f;

        private float mRecordTime = 0.0f;

        // Sound settings
        private JCS_SoundPool mSoundPool = null;
        private JCS_SoundPlayer mSoundPlayer = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            this.mSoundPool = this.GetComponent<JCS_SoundPool>();

            mRecordTime = this.mLimitTime;
        }

        private void Start()
        {
            mLightning = JCS_SceneManager.instance.GetWhiteScreen();
        }

        private void Update()
        {
            if (mLightning == null)
            {
                JCS_Debug.LogWarning("Lightning effect without white screen invalid");
                return;
            }

            DoEffect();
        }

        /// <summary>
        /// Do the lightning effect once.
        /// </summary>
        private void DoEffect()
        {
            mLimitTimer += Time.deltaTime;

            if (mLimitTime > mLimitTimer)
                return;

            mLimitTimer = 0.0F;

            int randNum = JCS_Random.Range(0, 100);

            if (randNum <= mPossiblity)
                DoLightning();

            DoRandTime();
        }

        /// <summary>
        /// Play the lightning sound once.
        /// </summary>
        private void PlayLightningSound()
        {
            AudioClip ac = this.mSoundPool.GetRandomSound();

            mSoundPlayer.PlayOneShotWhileNotPlaying(ac);
        }

        /// <summary>
        /// Do lightning once.
        /// </summary>
        private void DoLightning()
        {
            mLightning.FadeOut(0.5f);

            PlayLightningSound();
        }

        /// <summary>
        /// Chance to occurs trigger lightning effect.
        /// </summary>
        private void DoRandTime()
        {
            // set it back to original time
            mLimitTime = mRecordTime;

            // add new rand time.
            mLimitTime += JCS_Random.Range(-mRandomTime, mRandomTime);
        }
    }
}
