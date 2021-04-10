/**
 * $File: JCS_SlidePanel.cs $
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
    /// Slide Panel works with 'JCS_SlideScreenPanelHolder'.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_SlidePanel
        : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        [Header("** Check Variables (JCS_SlidePanel) **")]

        [SerializeField]
        private Vector3 mTargetPosition = Vector3.zero;

        [Header("** Runtime Variables (JCS_SlidePanel) **")]

        [Tooltip("How fast the this slide panel slide in x axis.")]
        [SerializeField]
        [Range(0.01f, 5.0f)]
        private float mSlideFrictionX = 0.2f;

        [Tooltip("How fast the this slide panel slide in y axis.")]
        [SerializeField]
        [Range(0.01f, 5.0f)]
        private float mSlideFrictionY = 0.2f;

        /* Setter & Getter */

        public float SlideFrictionX { get { return this.mSlideFrictionX; } set { this.mSlideFrictionX = value; } }
        public float SlideFrictionY { get { return this.mSlideFrictionY; } set { this.mSlideFrictionY = value; } }
        public void SetTargetPosition(Vector3 pos) { this.mTargetPosition = pos; }
        public Vector3 GetTargetPosition() { return this.mTargetPosition; }

        /* Functions */

        private void Start()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            this.mTargetPosition = this.mRectTransform.localPosition;
        }

        private void Update()
        {
            Vector3 newPosition = this.mRectTransform.localPosition;

            newPosition.x += (mTargetPosition.x - newPosition.x) / mSlideFrictionX * Time.deltaTime;
            newPosition.y += (mTargetPosition.y - newPosition.y) / mSlideFrictionY * Time.deltaTime;

            this.mRectTransform.localPosition = newPosition;
        }
    }
}
