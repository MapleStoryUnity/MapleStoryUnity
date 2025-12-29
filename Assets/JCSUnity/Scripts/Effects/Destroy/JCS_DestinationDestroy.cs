/**
 * $File: JCS_DestinationDestroy.cs $
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
    /// Effect when reach the destination destroy this game object.
    /// </summary>
    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_DestinationDestroy : JCS_UnityObject
    {
        /* Variables */

        public enum FadeType
        {
            IN,
            OUT
        }

        private JCS_AlphaObject mAlphaObject = null;

        [Separator("⚡️ Runtime Variables *(JCS_DestinationDestroy)")]

        [Tooltip("Do the action?")]
        [SerializeField]
        private bool mAction = true;

        [Tooltip("Target destination.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Accept range to destroy this object. (circle)")]
        [SerializeField]
        private float mDestroyDistance = 0.3f;

        [Header("🔍 Fade Effect")]

        [Tooltip("Fade when destroy.")]
        [SerializeField]
        private bool mFadeEffect = true;

        [Tooltip("What kind of fade?")]
        [SerializeField]
        private FadeType mFadeType = FadeType.IN;

        [Tooltip("How far start to fade?")]
        [SerializeField]
        private float mFadeDistance = 500;

        /* Setter & Getter */

        public bool action { get { return mAction; } set { mAction = value; } }
        public void SetTargetTransform(Transform pos) { mTargetTransform = pos; }
        public float fadeDistance { get { return mFadeDistance; } set { mFadeDistance = value; } }
        public float destroyDistance { get { return mDestroyDistance; } set { mDestroyDistance = value; } }

        /* Functions */

        private void Start()
        {
            mAlphaObject = GetComponent<JCS_AlphaObject>();

            mAlphaObject.SetObjectType(GetObjectType());
        }

        private void Update()
        {
            // check if action triggered?
            if (!mAction || mTargetTransform == null)
            {
#if UNITY_EDITOR
                if (JCS_GameSettings.FirstInstance().debugMode)
                    Debug.LogError("No target found");
#endif
                return;
            }

            float currentDistance = Vector3.Distance(transform.position, mTargetTransform.position);

            if (mFadeEffect)
            {
                if (currentDistance <= mFadeDistance)
                {
                    float alphaDeltaDistance = mFadeDistance - mDestroyDistance;
                    if (mFadeType == FadeType.IN)
                        mAlphaObject.targetAlpha = (currentDistance - mDestroyDistance) / alphaDeltaDistance;
                    else if (mFadeType == FadeType.OUT)
                        mAlphaObject.targetAlpha = 1 - (currentDistance - mDestroyDistance) / alphaDeltaDistance;
                }
            }

            if (currentDistance <= mDestroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
