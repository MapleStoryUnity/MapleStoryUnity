/**
 * $File: JCS_2DDropItemAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action makes the game object drops item.
    /// </summary>
    [RequireComponent(typeof(JCS_ItemDroppable))]
    public class JCS_2DDropItemAction : MonoBehaviour
    {
        /* Variables */

        private JCS_ItemDroppable mItemDroppable = null;

        [Separator("⚡️ Runtime Variables (JCS_2DDropItemAction)")]

        [Tooltip("Drop once when the object is dead.")]
        [SerializeField]
        private bool mDropWhenDies = true;

        [Tooltip("Drop by time.")]
        [SerializeField]
        private bool mDropByTime = false;

        [Tooltip("Time to drop one time.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mTimePerDrop = 0.0f;

        [Tooltip("Effect the time every time it drops.")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mRandomTimeRange = 0.0f;

        // a time combine random + timer per drop value.
        private float mDropRealTime = 0.0f;

        // timer to calculate weather to do drop action or not
        private float mDropTimer = 0.0f;

        // trigger to re-calculate the drop time
        private bool mDroped = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool dropWhenDies { get { return mDropWhenDies; } set { mDropWhenDies = value; } }
        public bool dropByTime { get { return mDropByTime; } set { mDropByTime = value; } }
        public float timePerDrop { get { return mTimePerDrop; } set { mTimePerDrop = value; } }
        public float randomTimeRange { get { return mRandomTimeRange; } set { mRandomTimeRange = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mItemDroppable = GetComponent<JCS_ItemDroppable>();
        }

        private void Update()
        {
            DoDropByTime();
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_AppManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.FirstInstance().IsSwitchingScene())
                return;


            // check the event is true
            if (!mDropWhenDies)
                return;

            // do effects
            mItemDroppable.DropItems();
        }

        /// <summary>
        /// Main function runs drop by time.
        /// </summary>
        private void DoDropByTime()
        {
            if (!mDropByTime)
                return;

            ResetTime();

            mDropTimer += JCS_Time.ItTime(mTimeType);

            if (mDropRealTime < mDropTimer)
            {
                // Do drop action.
                mItemDroppable.DropItems();

                // re-calculate next drop
                mDroped = true;
            }

        }

        /// <summary>
        /// Once we do the drop item action, re-calculate the next drop time.
        /// so every time it drop is random.
        /// </summary>
        private void ResetTime()
        {
            if (!mDroped)
                return;

            // get the offset random time.
            float randTime = JCS_Random.Range(-mRandomTimeRange, mRandomTimeRange);

            // formula, on timing design here.
            mDropRealTime = randTime + mTimePerDrop;

            // reset timer
            mDropTimer = 0;

            mDroped = false;
        }
    }
}
