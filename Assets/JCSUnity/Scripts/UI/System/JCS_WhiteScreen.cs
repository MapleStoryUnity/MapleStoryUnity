/**
 * $File: JCS_WhiteScreen.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// White screen for effect usage.
    /// 
    /// NOTE(jenchieh): For lightning/flash effect.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_WhiteScreen  : MonoBehaviour
    {
        /* Variables */

        private JCS_FadeObject mAO = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_WhiteScreen)")]

        [Tooltip("Test with the key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to test flash effect.")]
        [SerializeField]
        private KeyCode mTestKey = KeyCode.U;
#endif

        [Separator("Runtime Variables (JCS_WhiteScreen)")]

        [Tooltip("How long it fade out?")]
        [SerializeField]
        private float mFadeOutTime = 1.0f;

        [Tooltip("How long it fade in?")]
        [SerializeField]
        private float mFadeInTime = 0.2f;

        /* Setter & Getter */

        public float fadeOutTime { get { return mFadeOutTime; } set { mFadeOutTime = value; } }
        public float fadeInTime { get { return mFadeInTime; } set { mFadeInTime = value; } }

        /* Functions */

        private void Awake()
        {
            mAO = GetComponent<JCS_FadeObject>();

            mAO.localAlpha = 0;
        }

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mTestKey))
                FadeOut();
        }
#endif

        public void FadeIn()
        {
            FadeIn(mFadeInTime);
        }
        public void FadeOut()
        {
            FadeOut(mFadeOutTime);
        }
        public void FadeIn(float time)
        {
            mAO.FadeIn(time);
            JCS_Util.MoveToTheLastChild(transform);
        }
        public void FadeOut(float time)
        {
            mAO.FadeOut(time);
            JCS_Util.MoveToTheLastChild(transform);
        }
        public bool IsFadeIn()
        {
            return mAO.IsFadeIn();
        }
        public bool IsFadeOut()
        {
            return mAO.IsFadeOut();
        }
    }
}
