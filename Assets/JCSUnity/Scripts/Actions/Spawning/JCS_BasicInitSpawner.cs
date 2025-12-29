/**
 * $File: JCS_BasicInitSpawner.cs $
 * $Date: 2017-03-06 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Spawn transforms at the initialize time.
    /// </summary>
    public class JCS_BasicInitSpawner : MonoBehaviour
    {
        /* Variables */

        [Separator("🌱 Initialize Variables (JCS_BasicInitSpawner)")]

        [Tooltip("List of transform you want to spawn.")]
        [SerializeField]
        private List<Transform> mSpawnList = null;

        [Tooltip("How many transform you want to spawn?")]
        [SerializeField]
        [Range(1, 300)]
        private int mSpawnCount = 1;

        [Header("🔍 Randomize Position")]

        [Tooltip("Spawn the item random position, in x-axis.")]
        [SerializeField]
        private bool mRandPosX = false;

        [Tooltip("Randomize value in x-axis.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRandPosRangeX = 1.0f;

        [Tooltip("Spawn the item random position, in y-axis.")]
        [SerializeField]
        private bool mRandPosY = false;

        [Tooltip("Randomize value in y-axis.")]
        [SerializeField]
        [Range(0, 300.0f)]
        private float mRandPosRangeY = 1.0f;

        [Tooltip("Spawn the item random position, in z-axis.")]
        [SerializeField]
        private bool mRandPosZ = false;

        [Tooltip("Randomize value in z-axis.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRandPosRangeZ = 1.0f;

        [Header("🔍 Randomize Rotation")]

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

        [Header("🔍 Randomize Scale")]

        [Tooltip("Randomize the scale in x-axis?")]
        [SerializeField]
        private bool mRandScaleX = false;

        [Tooltip("Random scale in range in x-axis.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRandScaleRangeX = 0.0f;

        [Tooltip("Randomize the scale in y-axis?")]
        [SerializeField]
        private bool mRandScaleY = false;

        [Tooltip("Random scale in range in y-axis.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRandScaleRangeY = 0.0f;

        [Tooltip("Randomize the scale in z-axis?")]
        [SerializeField]
        private bool mRandScaleZ = false;

        [Tooltip("Random scale in range in z-axis.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRandScaleRangeZ = 0.0f;

        /* Setter & Getter */

        public List<Transform> spawnList { get { return mSpawnList; } set { mSpawnList = value; } }
        public int spawnCount { get { return mSpawnCount; } set { mSpawnCount = value; } }

        public bool randPosX { get { return mRandPosX; } set { mRandPosX = value; } }
        public float randPosRangeX { get { return mRandPosRangeX; } set { mRandPosRangeX = value; } }
        public bool randPosY { get { return mRandPosY; } set { mRandPosY = value; } }
        public float randPosRangeY { get { return mRandPosRangeY; } set { mRandPosRangeY = value; } }
        public bool randPosZ { get { return mRandPosZ; } set { mRandPosZ = value; } }
        public float randPosRangeZ { get { return mRandPosRangeZ; } set { mRandPosRangeZ = value; } }

        public bool randRotationX { get { return mRandRotationX; } set { mRandRotationX = value; } }
        public float randRotRangeX { get { return mRandRotRangeX; } set { mRandRotRangeX = value; } }
        public bool randRotationY { get { return mRandRotationY; } set { mRandRotationY = value; } }
        public float randRotRangeY { get { return mRandRotRangeY; } set { mRandRotRangeY = value; } }
        public bool randRotationZ { get { return mRandRotationZ; } set { mRandRotationZ = value; } }
        public float randRotRangeZ { get { return mRandRotRangeZ; } set { mRandRotRangeZ = value; } }

        public bool randScaleX { get { return mRandScaleX; } set { mRandScaleX = value; } }
        public float randScaleRangeX { get { return mRandScaleRangeX; } set { mRandScaleRangeX = value; } }
        public bool randScaleY { get { return mRandScaleY; } set { mRandScaleY = value; } }
        public float randScaleRangeY { get { return mRandScaleRangeY; } set { mRandScaleRangeY = value; } }
        public bool randScaleZ { get { return mRandScaleZ; } set { mRandScaleZ = value; } }
        public float randScaleRangeZ { get { return mRandScaleRangeZ; } set { mRandScaleRangeZ = value; } }

        /* Functions */

        private void Awake()
        {
            // resize the spawn list.
            ResizeSpawnList();

            // do the spawn.
            InitSpawn();
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
                Debug.Log("Can't spawn a null reference. Plz check the spawn list if there are transform attach or empty slot.");
                return;
            }

            // spawn a object
            Transform objSpawned = (Transform)JCS_Util.Instantiate(
                mSpawnList[spawnIndex],
                transform.position);


            // randomize the position a bit.
            Vector3 randPos = JCS_Vector.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.position,
                new Vector3(mRandPosRangeX, mRandPosRangeY, mRandPosRangeZ),
                new JCS_Bool3(mRandPosX, mRandPosY, mRandPosZ));

            // randomize the rotation a bit.
            Vector3 randRot = JCS_Vector.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.eulerAngles,
                new Vector3(mRandRotRangeX, mRandRotRangeY, mRandRotRangeZ),
                new JCS_Bool3(mRandRotationX, mRandRotationY, mRandRotationZ));

            // randomize the rotation a bit.
            Vector3 randScale = JCS_Vector.ApplyRandVector3(
                // use the current spawner position.
                objSpawned.transform.localScale,
                new Vector3(mRandScaleRangeX, mRandScaleRangeY, mRandScaleRangeZ),
                new JCS_Bool3(mRandScaleX, mRandScaleY, mRandScaleZ));

            objSpawned.transform.position = randPos;
            objSpawned.transform.eulerAngles = randRot;
            objSpawned.transform.localScale = randScale;
        }

        /// <summary>
        /// Spawn the transforms once.
        /// </summary>
        public void InitSpawn()
        {
            for (int count = 0; count < mSpawnCount; ++count)
            {
                // spawn a transform.
                SpawnATransform();
            }
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
    }
}
