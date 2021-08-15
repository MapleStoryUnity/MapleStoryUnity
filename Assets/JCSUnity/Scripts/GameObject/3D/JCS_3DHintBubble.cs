/**
 * $File: JCS_3DHintBubble.cs $
 * $Date: 2020-04-06 14:25:49 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Hint bubble in 3D using Text Mesh Pro.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    [RequireComponent(typeof(JCS_TweenerHandler))]
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_3DHintBubble : MonoBehaviour
    {
        /* Variables */

        private TextMeshPro mTextMesh = null;

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_3DHintBubble) **")]

        [Tooltip("Test this component with these keys.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to active hint bubble.")]
        [SerializeField]
        private KeyCode mActiveKey = KeyCode.A;

        [Tooltip("Key to active hint bubble.")]
        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.S;

#endif

        [Header("** Check Variables (JCS_3DHintBubble) **")]

        [Tooltip("Flag to check if the hint bubble active.")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("Tweener handler to do tween to the hint bubble.")]
        [SerializeField]
        private JCS_TweenerHandler mTweenerHandler = null;

        [Tooltip("Fade the hint bubble.")]
        [SerializeField]
        private JCS_FadeObject mFadeObject = null;

        /* Setter & Getter */

        public bool active { get { return this.mActive; } }
#if TMP_PRO
        public TextMeshPro TextMesh { get { return this.mTextMesh; } }
#endif
        public JCS_TweenerHandler TweenerHandler { get { return this.mTweenerHandler; } }
        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }

        /* Functions */

        private void Awake()
        {
#if TMP_PRO
            this.mTextMesh = this.GetComponent<TextMeshPro>();
#endif
            this.mTweenerHandler = this.GetComponent<JCS_TweenerHandler>();
            this.mFadeObject = this.GetComponent<JCS_FadeObject>();
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

            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();
            else if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

        /// <summary>
        /// Set the text and active.
        /// </summary>
        /// <param name="msg"></param>
        public void SendText(string msg)
        {
#if TMP_PRO
            mTextMesh.text = msg;
#endif
            Active();
        }

        /// <summary>
        /// Active this hint bubble.
        /// </summary>
        public void Active()
        {
            TweenerHandler.DoAllTweenToTargetValue();
            mFadeObject.FadeIn();

            this.mActive = true;
        }

        /// <summary>
        /// Deactive this hint bubble.
        /// </summary>
        public void Deactive()
        {
            TweenerHandler.DoAllTweenToStartValue();
            mFadeObject.FadeOut();

            this.mActive = false;
        }
    }
}
