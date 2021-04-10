/**
 * $File: JCS_2DDropItemAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Action makes the gameobject drops item.
    /// </summary>
    [RequireComponent(typeof(JCS_ItemDroppable))]
    public class JCS_2DDropItemAction
        : MonoBehaviour
    {
        /* Variables */

        private JCS_ItemDroppable mItemDroppable = null;


        [Header("** Runtime Variables (JCS_2DDropItemAction) **")]

        [Tooltip("Drop once when the object is dead.")]
        [SerializeField]
        private bool mDropWhenDies = true;

        [Tooltip("Drop by time.")]
        [SerializeField]
        private bool mDropByTime = false;

        [Tooltip("Time to drop one time.")]
        [SerializeField] [Range(0.0f, 10.0f)]
        private float mTimePerDrop = 0.0f;

        [Tooltip("Effect the time every time it drops.")]
        [SerializeField] [Range(0.0f, 5.0f)]
        private float mRandomTimeRange = 0.0f;

        // a time combine random + timer per drop value.
        private float mDropRealTime = 0.0f;

        // timer to calculate weather to do drop action or not
        private float mDropTimer = 0.0f;

        // trigger to re-calculate the drop time
        private bool mDroped = false;


        /* Setter & Getter */

        public bool DropWhenDies { get { return this.mDropWhenDies; } set {this.mDropWhenDies = value; } }
        public bool DropByTime { get { return this.mDropByTime; } set {this.mDropByTime = value; } }
        public float TimePerDrop { get { return this.mTimePerDrop; } set {this.mTimePerDrop = value; } }
        public float RandomTimeRange { get { return this.mRandomTimeRange; } set {this.mRandomTimeRange = value; } }


        /* Functions */

        private void Awake()
        {
            mItemDroppable = this.GetComponent<JCS_ItemDroppable>();
        }

        private void Update()
        {
            DoDropByTime();
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

            mDropTimer += Time.deltaTime;

            if (mDropRealTime < mDropTimer)
            {
                // Do drop action.
                mItemDroppable.DropItems();

                // re-calculate next drop
                mDroped = true;
            }

        }

        /// <summary>
        /// Once we do the drop item action,
        /// re-calculate the next drop time.
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
