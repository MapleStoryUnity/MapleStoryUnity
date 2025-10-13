/**
 * $File: JCS_SlidePanel.cs $
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
    /// Slide Panel works with 'JCS_SlideScreenPanelHolder'.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_SlidePanel : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        [Separator("Check Variables (JCS_SlidePanel)")]

        [SerializeField]
        [ReadOnly]
        private Vector3 mTargetPosition = Vector3.zero;

        [Separator("Runtime Variables (JCS_SlidePanel) ")]

        [Tooltip("How fast the this slide panel slide in x axis.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mSlideFrictionX = 0.2f;

        [Tooltip("How fast the this slide panel slide in y axis.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mSlideFrictionY = 0.2f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public float slideFrictionX { get { return mSlideFrictionX; } set { mSlideFrictionX = value; } }
        public float slideFrictionY { get { return mSlideFrictionY; } set { mSlideFrictionY = value; } }
        public void SetTargetPosition(Vector3 pos) { mTargetPosition = pos; }
        public Vector3 GetTargetPosition() { return mTargetPosition; }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Start()
        {
            mRectTransform = GetComponent<RectTransform>();

            mTargetPosition = mRectTransform.localPosition;
        }

        private void Update()
        {
            Vector3 newPosition = mRectTransform.localPosition;

            float dt = JCS_Time.ItTime(mTimeType);

            newPosition.x += (mTargetPosition.x - newPosition.x) / mSlideFrictionX * dt;
            newPosition.y += (mTargetPosition.y - newPosition.y) / mSlideFrictionY * dt;

            mRectTransform.localPosition = newPosition;
        }
    }
}
