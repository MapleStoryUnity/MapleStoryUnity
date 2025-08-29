/**
 * $File: JCS_ScaleEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.EventSystems;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Scale approach to a specific scale value.
    /// </summary>
    public class JCS_ScaleEffect : JCS_UnityObject
    {
        /* Variables */

        private Vector3 mTowardScale = Vector3.zero;
        private Vector3 mRecordScale = Vector3.zero;
        private Vector3 mTargetScale = Vector3.zero;

        [Separator("Check Variables (JCS_ScaleEffect)")]

        [Tooltip("Is the current component doing the effect now?")]
        [SerializeField]
        [ReadOnly]
        private bool mEffect = false;

        [Tooltip("")]
        [SerializeField]
        [ReadOnly]
        private JCS_PanelRoot mPanelRoot = null;

        [Tooltip("")]
        [SerializeField]
        [ReadOnly]
        private EventTrigger mEventTrigger = null;

        [Separator("Initialize Variables (JCS_ScaleEffect)")]

        [Tooltip("Do scale in x axis.")]
        [SerializeField]
        private bool mScaleX = true;

        [Tooltip("Do scale in y axis.")]
        [SerializeField]
        private bool mScaleY = true;

        [Tooltip("Do scale in z axis.")]
        [SerializeField]
        private bool mScaleZ = true;

        [Tooltip("How much it scales on each axis.")]
        [SerializeField]
        private Vector3 mScaleValue = Vector3.one;

        [Tooltip("How fast it scales on each axis.")]
        [SerializeField]
        private Vector3 mScaleFriction = new Vector3(0.2f, 0.2f, 0.2f);

        [Separator("Runtime Variables (JCS_ScaleEffect)")]

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("UI")]

        [Tooltip("Add event to event trigger system.")]
        [SerializeField]
        private bool mAutoAddEvent = true;

        [Tooltip("Event trigger type to active the the slide effect.")]
        [SerializeField]
        private EventTriggerType mActiveEventTriggerType = EventTriggerType.PointerEnter;

        [Tooltip("Event trigger type to deactive the the slide effect.")]
        [SerializeField]
        private EventTriggerType mDeactiveEventTriggerType = EventTriggerType.PointerExit;

        /* Setter & Getter */

        public Vector3 recordScale { get { return mRecordScale; } set { mRecordScale = value; } }
        public Vector3 towardScale { get { return mTowardScale; } set { mTowardScale = value; } }
        public Vector3 GetScaleValue() { return mScaleValue; }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Start()
        {
            // Only need it for the UI.
            if (GetObjectType() == JCS_UnityObjectType.UI ||
                GetObjectType() == JCS_UnityObjectType.TEXT)
            {
                // Get panel root, in order to calculate the correct distance 
                // base on the resolution.
                mPanelRoot = JCS_PanelRoot.GetFromParent(transform);

                if (mAutoAddEvent)
                {
                    // Event trigger is the must if we need to add the 
                    // event to event trigger system.
                    {
                        mEventTrigger = GetComponent<EventTrigger>();
                        if (mEventTrigger == null)
                            mEventTrigger = gameObject.AddComponent<EventTrigger>();
                    }

                    JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, mActiveEventTriggerType, JCS_OnMouseOver);
                    JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, mDeactiveEventTriggerType, JCS_OnMouseExit);
                }
            }

            Vector3 currentScale = transform.localScale;

            // record down the scale.
            mRecordScale = currentScale;
            mTargetScale = currentScale;

            SetTargetScale();
        }

        private void Update()
        {
            ScaleEffect();
        }

        /// <summary>
        /// Call it when is on mouse over.
        /// 
        /// Use in inspector for Event Trigger System. (Active)
        /// </summary>
        public void JCS_OnMouseOver(PointerEventData data)
        {
            JCS_OnMouseOver();
        }
        public void JCS_OnMouseOver()
        {
            Active();
        }

        /// <summary>
        /// Call it When is on mouse exit.
        /// 
        /// Use in inspector for Event Trigger System. (Deactive)
        /// </summary>
        /// <returns></returns>
        public void JCS_OnMouseExit(PointerEventData data)
        {
            JCS_OnMouseExit();
        }
        public void JCS_OnMouseExit()
        {
            Deactive();
        }

        /// <summary>
        /// Active the effect.
        /// </summary>
        public void Active()
        {
            mTargetScale = mTowardScale;

            mEffect = true;
        }

        /// <summary>
        /// Deactive the effect.
        /// </summary>
        public void Deactive()
        {
            if (!mEffect)
                return;

            mTargetScale = mRecordScale;

            mEffect = false;
        }

        /// <summary>
        /// Get the target scale.
        /// </summary>
        private void SetTargetScale()
        {
            // so it will scale base on the starting scale.
            Vector3 newTargetScale = mRecordScale;

            if (mScaleX)
            {
                // mPanelRoot will be null is the object isn't
                // UI game object.
                if (mPanelRoot != null)
                    mScaleValue.x *= mPanelRoot.panelDeltaWidthRatio;

                newTargetScale.x += mScaleValue.x;
            }

            if (mScaleY)
            {
                // mPanelRoot will be null is the object isn't
                // UI game object.
                if (mPanelRoot != null)
                    mScaleValue.y *= mPanelRoot.panelDeltaHeightRatio;

                newTargetScale.y += mScaleValue.y;
            }

            if (mScaleZ)
            {
                newTargetScale.z += mScaleValue.z;
            }

            // set to target scale.
            mTowardScale = newTargetScale;
        }

        /// <summary>
        /// Do the scale effect.
        /// </summary>
        private void ScaleEffect()
        {
            Vector3 newScale = this.transform.localScale;

            float dt = JCS_Time.ItTime(mTimeType);

            newScale.x += (mTargetScale.x - newScale.x) / mScaleFriction.x * dt;
            newScale.y += (mTargetScale.y - newScale.y) / mScaleFriction.y * dt;
            newScale.z += (mTargetScale.z - newScale.z) / mScaleFriction.y * dt;

            this.transform.localScale = newScale;
        }
    }
}
