/**
 * $File: JCS_BasicWaveSpawner.cs $
 * $Date: 2017-03-04 23:49:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action do spawns a wave repeatedly throughout the time is going.
    /// </summary>
    public class JCS_BasicWaveSpawner : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_BasicWaveSpawner) **")]

        [Tooltip("Active this component?")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("List of transform you want to spawn.")]
        [SerializeField]
        private List<Transform> mSpawnList = null;

        [Tooltip("Time to spawn a object.")]
        [SerializeField]
        [Range(0.0f, 60.0f)]
        private float mSpawnTime = 0.0f;

        [Tooltip("Randomize the spawn time a bit.")]
        [SerializeField]
        [Range(0, 10)]
        private float mRandomizeSpawnTime = 0.0f;

        private float mSpawnTimer = 0.0f;

        // real spawn time to compare with mSpawnTimer
        private float mRealSpawnTime = 0.0f;

        // check if do the spawn?
        private bool mSpawned = false;

        [Header("- Randomize Position")]

        [Tooltip("Spawn the item random position, in x-axis.")]
        [SerializeField]
        private bool mRandPosX = false;

        [Tooltip("Randomize value in x-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRangeX = 1.0f;

        [Tooltip("Spawn the item random position, in y-axis.")]
        [SerializeField]
        private bool mRandPosY = false;

        [Tooltip("Randomize value in y-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRangeY = 1.0f;

        [Tooltip("Spawn the item random position, in z-axis.")]
        [SerializeField]
        private bool mRandPosZ = false;

        [Tooltip("Randomize value in z-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRangeZ = 1.0f;

        [Header("- Randomize Rotation")]

        [Tooltip("Randomize the rotation in x-axis?")]
        [SerializeField]
        private bool mRandRotationX = false;

        [Tooltip("Random rotation in range in x-axis.")]
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float mRandRotRangeX = 0.0f;

        [Tooltip("Randomize the rotation in y-axis?")]
        [SerializeField]
        private bool mRandRotationY = false;

        [Tooltip("Random rotation in range in y-axis.")]
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float mRandRotRangeY = 0.0f;

        [Tooltip("Randomize the rotation in z-axis?")]
        [SerializeField]
        private bool mRandRotationZ = false;

        [Tooltip("Random rotation in range in z-axis.")]
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float mRandRotRangeZ = 0.0f;

        [Header("- Randomize Scale")]

        [Tooltip("Randomize the scale in x-axis?")]
        [SerializeField]
        private bool mRandScaleX = false;

        [Tooltip("Random scale in range in x-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRangeX = 0.0f;

        [Tooltip("Randomize the scale in y-axis?")]
        [SerializeField]
        private bool mRandScaleY = false;

        [Tooltip("Random scale in range in y-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRangeY = 0.0f;

        [Tooltip("Randomize the scale in z-axis?")]
        [SerializeField]
        private bool mRandScaleZ = false;

        [Tooltip("Random scale in range in z-axis.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRangeZ = 0.0f;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        public bool RandPosX { get { return this.mRandPosX; } set { this.mRandPosX = value; } }
        public float RandPosRangeX { get { return this.mRandPosRangeX; } set { this.mRandPosRangeX = value; } }
        public bool RandPosY { get { return this.mRandPosY; } set { this.mRandPosY = value; } }
        public float RandPosRangeY { get { return this.mRandPosRangeY; } set { this.mRandPosRangeY = value; } }
        public bool RandPosZ { get { return this.mRandPosZ; } set { this.mRandPosZ = value; } }
        public float RandPosRangeZ { get { return this.mRandPosRangeZ; } set { this.mRandPosRangeZ = value; } }

        public bool RandRotationX { get { return this.mRandRotationX; } set { this.mRandRotationX = value; } }
        public float RandRotRangeX { get { return this.mRandRotRangeX; } set { this.mRandRotRangeX = value; } }
        public bool RandRotationY { get { return this.mRandRotationY; } set { this.mRandRotationY = value; } }
        public float RandRotRangeY { get { return this.mRandRotRangeY; } set { this.mRandRotRangeY = value; } }
        public bool RandRotationZ { get { return this.mRandRotationZ; } set { this.mRandRotationZ = value; } }
        public float RandRotRangeZ { get { return this.mRandRotRangeZ; } set { this.mRandRotRangeZ = value; } }

        public bool RandScaleX { get { return this.mRandScaleX; } set { this.mRandScaleX = value; } }
        public float RandScaleRangeX { get { return this.mRandScaleRangeX; } set { this.mRandScaleRangeX = value; } }
        public bool RandScaleY { get { return this.mRandScaleY; } set { this.mRandScaleY = value; } }
        public float RandScaleRangeY { get { return this.mRandScaleRangeY; } set { this.mRandScaleRangeY = value; } }
        public bool RandScaleZ { get { return this.mRandScaleZ; } set { this.mRandScaleZ = value; } }
        public float RandScaleRangeZ { get { return this.mRandScaleRangeZ; } set { this.mRandScaleRangeZ = value; } }

        /* Functions */

        private void Awake()
        {
            // resize the spawn list.
            ResizeSpawnList();
        }

        private void Update()
        {
            // if not active, return.
            if (!mActive)
                return;

            // run the spawned algorithm
            DoSpawnRandomTransform();
        }

        /// <summary>
        /// Spawn a random transform.
        /// </summary>
        public void SpawnATransform()
        {
            int spawnIndex = JCS_Random.Range(0, mSpawnList.Count);

            // check null ref.
            if (mSpawnList[spawnIndex] == null)
            {
                JCS_Debug.Log("Can't spawn a null reference. Plz check the spawn list if there are transform attach or empty slot.");
                return;
            }

            // spawn a object
            Transform objSpawned = (Transform)JCS_Util.SpawnGameObject(
                mSpawnList[spawnIndex],
                this.transform.position);


            // randomize the position a bit.
            Vector3 randPos = JCS_Util.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.position,
                new Vector3(mRandPosRangeX, mRandPosRangeY, mRandPosRangeZ),
                new JCS_Bool3(mRandPosX, mRandPosY, mRandPosZ));

            // randomize the rotation a bit.
            Vector3 randRot = JCS_Util.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.eulerAngles,
                new Vector3(mRandRotRangeX, mRandRotRangeY, mRandRotRangeZ),
                new JCS_Bool3(mRandRotationX, mRandRotationY, mRandRotationZ));

            // randomize the rotation a bit.
            Vector3 randScale = JCS_Util.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.localScale,
                new Vector3(mRandScaleRangeX, mRandScaleRangeY, mRandScaleRangeZ),
                new JCS_Bool3(mRandScaleX, mRandScaleY, mRandScaleZ));

            objSpawned.transform.position = randPos;
            objSpawned.transform.eulerAngles = randRot;
            objSpawned.transform.localScale = randScale;
        }

        /// <summary>
        /// Spawn a random item.
        /// </summary>
        private void DoSpawnRandomTransform()
        {
            // reset time zone everytime if spawned
            if (mSpawned)
                ResetTimeZone();

            mSpawnTimer += Time.deltaTime;

            if (mSpawnTimer < mRealSpawnTime)
                return;

            // turn on the trigger so it will reset time zone.
            mSpawned = true;

            SpawnATransform();
        }

        /// <summary>
        /// Resize the spawn list.
        /// </summary>
        private void ResizeSpawnList()
        {
            for (int index = 0; index < mSpawnList.Count; ++index)
            {
                // remove everythin that are empty or null reference.
                if (mSpawnList[index] == null)
                    mSpawnList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Re-calculate the real time zone.
        /// </summary>
        private void ResetTimeZone()
        {
            float adjustTime = JCS_Random.Range(-mRandomizeSpawnTime, mRandomizeSpawnTime);
            mRealSpawnTime = mSpawnTime + adjustTime;

            mSpawned = false;
            mSpawnTimer = 0;
        }
    }
}
