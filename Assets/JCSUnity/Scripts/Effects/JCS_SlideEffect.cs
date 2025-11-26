/**
 * $File: JCS_SlideEffect.cs $
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
    /// Make object to make the smooth slide effect.
    ///     - Could be compose with JCS_TransformTweener class.
    /// </summary>
    public class JCS_SlideEffect : JCS_UnityObject
    {
        /* Variables */

        private Vector3 mTargetPosition = Vector3.zero;

        private Vector3 mRecordPosition = Vector3.zero;
        private Vector3 mTowardPosition = Vector3.zero;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_SlideEffect)")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to do the active slide effect.")]
        [SerializeField]
        private KeyCode mActiveKey = KeyCode.A;

        [Tooltip("Key to do the deactive slide effect.")]
        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.S;
#endif

        [Separator("Check Variables (JCS_SlideEffect)")]

        [Tooltip("Is this effect active?")]
        [SerializeField]
        [ReadOnly]
        private bool mIsActive = false;

        [Tooltip("The panel root object.")]
        [SerializeField]
        [ReadOnly]
        private JCS_PanelRoot mPanelRoot = null;

        [Tooltip("Event trigger system.")]
        [SerializeField]
        [ReadOnly]
        private EventTrigger mEventTrigger = null;

        [Separator("Initialize Variables (JCS_SlideEffect)")]

        [Tooltip("Direction object slides.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Tooltip("How far the object slides.")]
        [SerializeField]
        [Range(-30000, 30000)]
        private float mDistance = -50;

        [Tooltip("How fast the object slides.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFriction = 0.2f;

        [Separator("Runtime Variables (JCS_SlideEffect)")]

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("UI")]

        [Tooltip("Add event to event trigger system!")]
        [SerializeField]
        private bool mAutoAddEvent = true;

        [Tooltip("Event trigger type to active the the slide effect.")]
        [SerializeField]
        private EventTriggerType[] mActiveEventTriggerType = null;

        [Tooltip("Event trigger type to deactive the the slide effect.")]
        [SerializeField]
        private EventTriggerType[] mDeactiveEventTriggerType = null;

        [Header("Sound")]

        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("If slide out, do the sound.")]
        [SerializeField]
        private AudioClip mActiveClip = null;

        [Tooltip("If slide back the original position, do the sound.")]
        [SerializeField]
        private AudioClip mDeactiveClip = null;

        [Tooltip("Don't track on x-axis?")]
        [SerializeField]
        private bool mIgnoreX = false;

        [Tooltip("Don't track on y-axis?")]
        [SerializeField]
        private bool mIgnoreY = false;

        [Tooltip("Don't track on z-axis?")]
        [SerializeField]
        private bool mIgnoreZ = false;

        [Header("Optional")]

        [Tooltip(@"If you want to active this effect by button, plz set the button here.")]
        [SerializeField]
        private JCS_Button mActiveButton = null;

        /* Setter & Getter */

        public bool isActive { get { return mIsActive; } }
        public JCS_Axis axis { get { return mAxis; } set { mAxis = value; } }
        public float friction { get { return mFriction; } set { mFriction = value; } }
        public float distance
        {
            get { return mDistance; }
            set
            {
                mDistance = value;

                Vector3 newPos = transform.localPosition;
                // record the original position
                mRecordPosition = newPos;
                mTargetPosition = newPos;

                switch (mAxis)
                {
                    case JCS_Axis.AXIS_X:
                        newPos.x += mDistance;
                        break;
                    case JCS_Axis.AXIS_Y:
                        newPos.y += mDistance;
                        break;
                    case JCS_Axis.AXIS_Z:
                        newPos.z += mDistance;
                        break;
                }

                mTowardPosition = newPos;
            }
        }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool autoAddEvent { get { return mAutoAddEvent; } set { mAutoAddEvent = value; } }

        public AudioClip activeClip { get { return mActiveClip; } set { mActiveClip = value; } }
        public AudioClip deactiveClip { get { return mDeactiveClip; } set { mDeactiveClip = value; } }

        public bool ignoreX { get { return mIgnoreX; } set { mIgnoreX = value; } }
        public bool ignoreY { get { return mIgnoreY; } set { mIgnoreY = value; } }
        public bool ignoreZ { get { return mIgnoreZ; } set { mIgnoreZ = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // sound player will be optional.
            if (mSoundPlayer == null)
                mSoundPlayer = GetComponent<JCS_SoundPlayer>();

            // set the call back function if there is button assigned.
            if (mActiveButton != null)
                mActiveButton.SetSystemCallback(JCS_OnMouseOver);
        }

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
                    mEventTrigger = gameObject.GetOrAddComponent<EventTrigger>();

                    foreach (EventTriggerType evt in mActiveEventTriggerType)
                    {
                        JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, evt, JCS_OnMouseOver);
                    }

                    foreach (EventTriggerType evt in mDeactiveEventTriggerType)
                    {
                        JCS_UIUtil.AddEventTriggerEvent(mEventTrigger, evt, JCS_OnMouseExit);
                    }
                }
            }

            Vector3 newPos = transform.localPosition;
            // record the original position
            mRecordPosition = newPos;
            mTargetPosition = newPos;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    {
                        // mPanelRoot will be null is the object isn't UI
                        // game object.
                        if (mPanelRoot != null)
                            mDistance *= mPanelRoot.panelDeltaWidthRatio;

                        newPos.x += mDistance;
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        // mPanelRoot will be null is the object isn't UI
                        // game object.
                        if (mPanelRoot != null)
                            mDistance *= mPanelRoot.panelDeltaHeightRatio;

                        newPos.y += mDistance;
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        newPos.z += mDistance;
                    }
                    break;
            }

            mTowardPosition = newPos;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            SlideEffect();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();

            if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

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
        /// Call it when is on mouse exit.
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
        /// Active the effect. (Script)
        /// </summary>
        public void Active()
        {
            mIsActive = true;
            mTargetPosition = mTowardPosition;

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mActiveClip);
        }

        /// <summary>
        /// Deactive the effect. (Script)
        /// </summary>
        public void Deactive()
        {
            mIsActive = false;
            mTargetPosition = mRecordPosition;

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mDeactiveClip);
        }


        /// <summary>
        /// Check the object in at the position.
        /// </summary>
        /// <param name="accept"> acceptable range </param>
        /// <returns> 
        /// true: at the position, 
        /// false: not at the position 
        /// </returns>
        public bool IsIdle(float accept = 0)
        {
            int distance = (int)Vector3.Distance(mTargetPosition, transform.localPosition);

            if (accept == 0)
                return (distance == 0);

            return (distance < accept);
        }

        /// <summary>
        /// Check the mouse if over the panel.
        /// </summary>
        /// <returns> true: is over, false: not over </returns>
        public bool IsOnThere()
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_UIUtil.MouseOverGUI(mRectTransform))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Check the mouse if over the panel.
        /// </summary>
        /// <param name="rootPanel"> if there are root child plz use this to get the correct calculation </param>
        /// <returns> true: is over, false: not over </returns>
        public bool IsOnThere(RectTransform rootPanel)
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_UIUtil.MouseOverGUI(mRectTransform, rootPanel))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Main algorithm to do the slide effect.
        /// </summary>
        private void SlideEffect()
        {
            Vector3 tempTargetPost = mTargetPosition;

            if (mIgnoreX)
                tempTargetPost.x = localPosition.x;
            if (mIgnoreY)
                tempTargetPost.y = localPosition.y;
            if (mIgnoreZ)
                tempTargetPost.z = localPosition.z;

            localPosition += (tempTargetPost - localPosition) / mFriction * JCS_Time.ItTime(mTimeType);
        }
    }
}
