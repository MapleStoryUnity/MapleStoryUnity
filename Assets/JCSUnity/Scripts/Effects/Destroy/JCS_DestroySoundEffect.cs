/**
 * $File: JCS_DestroySoundEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Effect that play the sound after the gameobject is destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_SoundPool))]
    public class JCS_DestroySoundEffect : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_DestroySoundEffect) **")]

        [Tooltip("Sound to play when this occurs.")]
        [SerializeField]
        private JCS_SoundPool mRandomSoundAction = null;

        //-- Hit List
        [Tooltip("Active the effect by hitting the certain object.")]
        [SerializeField]
        private bool mActiveWithHitList = true;

        private JCS_HitListEvent mHitList = null;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;

        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        /* Setter & Getter */

        public bool ActiveWithHitList { get { return this.mActiveWithHitList; } set { this.mActiveWithHitList = value; } }
        public bool ActiveWithDestroyTime { get { return this.mActiveWithDestroyTime; } set { this.mActiveWithDestroyTime = value; } }

        /* Functions */

        private void Awake()
        {
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mRandomSoundAction = this.GetComponent<JCS_SoundPool>();

            mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_ApplicationManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.instance.IsSwitchingScene())
                return;

            if (!mActiveWithHitList)
            {
                if (mHitList.IsHit)
                    return;
            }

            if (!mActiveWithDestroyTime)
            {
                
                if (mDestroyObjectWithTime != null)
                {
                    if (mDestroyObjectWithTime.TimesUp)
                        return;
                }
            }

            // Add Destroy Sound
            GameObject gm = new GameObject();
            JCS_DestroySoundEndEvent dse = gm.AddComponent<JCS_DestroySoundEndEvent>();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroySoundEffect";
#endif
            AudioClip ac = this.mRandomSoundAction.GetRandomSound();
            if (ac != null)
                dse.SetAudioClipAndPlayOneShot(ac, mRandomSoundAction.SoundType);
        }
    }
}
