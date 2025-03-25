/**
 * $File: JCS_SplitSpawnEvent.cs $
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
    /// Spawn multiple objects in a row.
    /// </summary>
    [RequireComponent(typeof(JCS_ObjectList))]
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_DestroyObjectWithTime))]
    public class JCS_SplitSpawnEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_ObjectList mObjectList = null;

        private JCS_HitListEvent mHitList = null;

        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        [Separator("Runtime Variables (JCS_SplitSpawnEvent)")]

        [Tooltip("How many object to spawn?")]
        [SerializeField]
        [Range(1, 50)]
        private int mObjectsToSpawn = 1;

        [Header("- Trigger Conditions")]

        [Tooltip(@"Do the event while on destroy function 
built-in Unity Engine.")]
        [SerializeField]
        private bool mSpawnWhileDestroy = true;

        [Tooltip("Spawn once after delay time.")]
        [SerializeField]
        private bool mSpawnWhileDelay = false;

        [Tooltip("Delay time.")]
        [SerializeField]
        [Range(0.1f, 10.0f)]
        private float mDelayTime = 2.0f;

        [Tooltip("Spawn periodically be delay time.")]
        [SerializeField]
        private bool mDelayContinuous = false;

        // use to check the event active once?
        private bool mSpawned = false;

        // timer to time the delay time.
        private float mTimer = 0;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Random Spawn Offset")]

        [Tooltip("Enable random spawn position offset effect on x axis?")]
        [SerializeField]
        private bool mRandPosX = false;

        [Tooltip("Randomize position on x axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeX = 1f;

        [Tooltip("Enable random spawn position offset effect on y axis?")]
        [SerializeField]
        private bool mRandPosY = false;

        [Tooltip("Randomize position on y axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeY = 1f;

        [Tooltip("Enable random spawn position offset effect on z axis?")]
        [SerializeField]
        private bool mRandPosZ = false;

        [Tooltip("Randomize position on z axis.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandPosRangeZ = 1f;

        [Header("- Random Degree Effects")]

        [Tooltip("Enable random degree effect on x axis?")]
        [SerializeField]
        private bool mRandDegreeX = false;

        [Tooltip("Randomize rotation on x axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mDegreeValueX = 0;

        [Tooltip("Enable random degree effect on y axis?")]
        [SerializeField]
        private bool mRandDegreeY = false;

        [Tooltip("Randomize rotation on y axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mDegreeValueY = 0;

        [Tooltip("Enable random degree effect on z axis?")]
        [SerializeField]
        private bool mRandDegreeZ = false;

        [Tooltip("Randomize rotation on z axis.")]
        [SerializeField]
        [Range(0, 360)]
        private float mDegreeValueZ = 0;

        /* Setter & Getter */

        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            this.mObjectList = this.GetComponent<JCS_ObjectList>();
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
        }

        private void Update()
        {
            DoDelayEvent();
        }

        private void OnDestroy()
        {
            // if is quitting the application don't spawn object,
            // or else will cause memory leak!
            if (JCS_AppManager.APP_QUITTING)
                return;

            // if switching the scene, don't spawn new gameObject.
            if (JCS_SceneManager.instance.IsSwitchingScene())
                return;

            // cause if we dont have the hit list, 
            // the event will keep on going forever.
            // It will cause big problem with memory, 
            // so stop this event while we hit something 
            // in the hit list.
            if (mHitList.IsHit)
                return;

            // same reason above.
            if (mDestroyObjectWithTime.TimesUp)
                return;

            // spawn the object
            if (mSpawnWhileDestroy)
                SpawnObjects();
        }

        /// <summary>
        /// Do the randomize the transform position 
        /// offset algorithm.
        /// </summary>
        /// <param name="spawnPos"> position we want to apply. </param>
        /// <returns> position after applied the random offset value. </returns>
        public Vector3 RandTransform(Vector3 spawnPos)
        {
            Vector3 newPos = spawnPos;

            if (mRandPosX)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeX, mRandPosRangeX);
                newPos.x += effectRange;
            }

            if (mRandPosY)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeY, mRandPosRangeY);
                newPos.y += effectRange;
            }

            if (mRandPosZ)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeZ, mRandPosRangeZ);
                newPos.z += effectRange;
            }

            return newPos;
        }

        /// <summary>
        /// Return the randomize engular angles.
        /// </summary>
        /// <param name="angle"> engular angles we want to randomize. </param>
        /// <returns> randomize angles. </returns>
        public Vector3 RandDegree(Vector3 angle)
        {
            Vector3 newRotation = angle;

            if (mRandDegreeX)
            {
                float randDegree = JCS_Random.Range(-mDegreeValueX, mDegreeValueX);
                newRotation.x += randDegree;
            }

            if (mRandDegreeY)
            {
                float randDegree = JCS_Random.Range(-mDegreeValueY, mDegreeValueY);
                newRotation.y += randDegree;
            }

            if (mRandDegreeZ)
            {
                float randDegree = JCS_Random.Range(-mDegreeValueZ, mDegreeValueZ);
                newRotation.z += randDegree;
            }

            return newRotation;
        }

        /// <summary>
        /// Algorithm to do the main action from this event.
        /// </summary>
        public void SpawnObjects()
        {
            Transform spawnTrans = null;

            for (int counter = 0; counter < mObjectsToSpawn; ++counter)
            {
                // get the random object from the list.
                spawnTrans = mObjectList.GetRandomObjectFromList();

                // check if null.
                if (spawnTrans == null)
                {
                    JCS_Debug.LogError(
                        "Spawning object detect that are null references... Could not spawn the object!");

                    break;
                }


                // spawn the object by transform.
                // and get the object which we just spawned.
                spawnTrans = (Transform)JCS_Util.Instantiate(spawnTrans, this.transform.position);

                // apply random position
                spawnTrans.transform.position = RandTransform(spawnTrans.position);

                // apply random rotation
                spawnTrans.transform.eulerAngles = RandDegree(spawnTrans.eulerAngles);

                var adtaThis = this.GetComponent<JCS_ApplyDamageTextToLiveObjectAction>();
                var adtaSpawned = spawnTrans.GetComponent<JCS_ApplyDamageTextToLiveObjectAction>();
                if (adtaThis != null && adtaSpawned != null)
                {
                    // copy the apply damage text information to spawned object!
                    adtaSpawned.CopyToThis(adtaThis);
                }

            }
        }

        /// <summary>
        /// Algorithm base on time.
        /// </summary>
        private void DoDelayEvent()
        {
            if (!mSpawnWhileDelay)
                return;

            // if not continuous
            if (!mDelayContinuous)
            {
                // check if the action do once already.
                if (mSpawned)
                    return;
            }

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mTimer < mDelayTime)
                return;

            SpawnObjects();

            // reset timer.
            mTimer = 0;

            // do once!
            mSpawned = true;
        }
    }
}
