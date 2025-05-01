/**
 * $File: JCS_SlideScreenPanelHolder.cs $
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
    /// Slide panel holder.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_SlideScreenPanelHolder : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        [Separator("Initialize Variables (JCS_SlideScreenPanelHolder)")]

        [Tooltip("How fast the this slide panel slide in x axis.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mSlideFrictionX = 0.2f;

        [Tooltip("How fast the this slide panel slide in y axis.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mSlideFrictionY = 0.2f;

        // Panel rect transform holder.
        // since the panel does not need to contain "JCS_SlidePanel" class, 
        // we will help them add the class during init time.
        public RectTransform[] slidePanels = null;

        private JCS_SlidePanel[] mSlidePanelsComponents = null;

        /* Setter & Getter */

        public RectTransform rectTransform { get { return this.mRectTransform; } }
        public float SlideFrictionX { get { return this.mSlideFrictionX; } set { this.mSlideFrictionX = value; } }
        public float SlideFrictionY { get { return this.mSlideFrictionY; } set { this.mSlideFrictionY = value; } }

        /* Functions */

        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            UpdateSlidePanels();
        }

        /// <summary>
        /// Update all the slide panels once.
        /// </summary>
        /// <param name="force"> If true, force update the existing panels </param>
        public void UpdateSlidePanels(bool force = false)
        {
            if (mSlidePanelsComponents != null && !force)
                return;

            mSlidePanelsComponents = new JCS_SlidePanel[slidePanels.Length];

            for (int index = 0; index < slidePanels.Length; ++index)
            {
                // add the component to the slide panel
                mSlidePanelsComponents[index] =
                    slidePanels[index].gameObject.AddComponent<JCS_SlidePanel>();

                // set friction
                mSlidePanelsComponents[index].SlideFrictionX = mSlideFrictionX;
                mSlidePanelsComponents[index].SlideFrictionY = mSlideFrictionY;
            }
        }

        /// <summary>
        /// Enable/Disable slide panels by ACT.
        /// </summary>
        /// <param name="act"></param>
        public void EnableSlidePanels(bool act)
        {
            UpdateSlidePanels();

            foreach (JCS_SlidePanel sp in mSlidePanelsComponents)
                sp.enabled = act;
        }

        /// <summary>
        /// Move the position by delta position.
        /// </summary>
        /// <param name="deltaPos"></param>
        public void DeltaMove(Vector3 deltaPos)
        {
            UpdateSlidePanels();

            foreach (JCS_SlidePanel sp in mSlidePanelsComponents)
                sp.transform.localPosition += deltaPos;
        }

        /// <summary>
        /// Add Force to the panel.
        /// </summary>
        /// <param name="pos"></param>
        public void AddForce(Vector3 pos)
        {
            UpdateSlidePanels();

            foreach (JCS_SlidePanel sp in mSlidePanelsComponents)
            {
                Vector3 tempPos = sp.GetTargetPosition() - pos;
                sp.SetTargetPosition(tempPos);
            }
        }

        /// <summary>
        /// Add Force to  the panel.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="axis"></param>
        public void AddForce(float force, JCS_Axis axis)
        {
            if (force == 0.0f)
                return;

            Vector3 tempPos = Vector3.zero;

            switch (axis)
            {
                case JCS_Axis.AXIS_X:
                    tempPos.x = force;
                    break;
                case JCS_Axis.AXIS_Y:
                    tempPos.y = force;
                    break;
                case JCS_Axis.AXIS_Z:
                    tempPos.z = force;
                    break;
            }

            AddForce(tempPos);
        }
    }
}
