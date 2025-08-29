/**
 * $File: JCS_ParticleSystem.cs $
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
    /// Particle System thats spawns `JCS_Particle` object.
    /// </summary>
    [RequireComponent(typeof(JCS_EnvSoundPlayer))]
    public class JCS_ParticleSystem : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Variables Variables (JCS_ParticleSystem)")]

        [Tooltip("Flag for component testing.")]
        public bool testWithKey = false;

        [Tooltip("Key that starts this particle system.")]
        public KeyCode startKey = KeyCode.C;

        [Tooltip("Key that stops this particle system.")]
        public KeyCode stopKey = KeyCode.V;

        [Tooltip("Key that plays one shot of particles.")]
        public KeyCode playOneShotKey = KeyCode.B;

        [Tooltip("Particle count for testing one shot.")]
        [Range(1, 300)]
        public int oneShotParticleCount = 10;
#endif

        [Separator("Initialize Variables (JCS_ParticleSystem)")]

        [Tooltip("Number of particle this particle system hold.")]
        [SerializeField]
        [Range(50, 5000)]
        private int mNumOfParticle = 50;

        private JCS_Vec<JCS_Particle> mParticles = null;
        private int mLastAvaliableIndex = 0;

        [Separator("Runtime Variables (JCS_ParticleSystem)")]

        [Tooltip("Particle you want to spawn.")]
        [SerializeField]
        private JCS_Particle mParticle = null;

        [Tooltip("Is the particle engine active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Active thread pool?")]
        [SerializeField]
        private bool mActiveThread = true;

        [Tooltip("What layer should this be render?")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("How much do it range?")]
        [SerializeField]
        [Range(0, 1000)]
        private int mDensity = 5;

        [Tooltip("How much to tilt the particle?")]
        [SerializeField]
        [Range(-180, 179)]
        private float mWindSpeed = 0;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Position")]

        [Tooltip("Randomize the X position. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandPosX = 0.0f;

        [Tooltip("Randomize the Y position. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandPosY = 0.0f;

        [Tooltip("Randomize the Z position. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandPosZ = 0;

        [Header("Rotation")]

        [Tooltip("Randomize the X rotation.")]
        [SerializeField]
        [Range(0.0f, 359.999f)]
        private float mRandAngleX = 0.0f;

        [Tooltip("Randomize the Y rotation.")]
        [SerializeField]
        [Range(0.0f, 359.999f)]
        private float mRandAngleY = 0.0f;

        [Tooltip("Randomize the Z rotation.")]
        [SerializeField]
        [Range(0.0f, 359.999f)]
        private float mRandAngleZ = 0.0f;

        [Header("Scale")]

        [Tooltip(@"Apply the scale always the same. This will only take the and 
mRandScaleX as a standard and ignore mRandScaleY and mRandScaleZ variables.")]
        [SerializeField]
        private bool mAlwaysTheSameScale = true;

        [Tooltip("Randomize the X scale. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandScaleX = 0.0f;

        [Tooltip("Randomize the Y scale. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandScaleY = 0.0f;

        [Tooltip("Randomize the Z scale. (Default : 0)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandScaleZ = 0.0f;

        private float mSequenceTimer = 0.0f;
        private float mTimeAParticle = 0.5f;

        [Header("Freeze Effect")]

        [Tooltip("Freeze the x axis.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze the y axis.")]
        [SerializeField]
        private bool mFreezeY = true;

        [Tooltip("Freeze the z axis.")]
        [SerializeField]
        private bool mFreezeZ = true;

        private Vector3 mFreezePos = Vector3.zero;

        [Header("One shot")]

        [Tooltip("Do not process the particle by thread, by main thread.")]
        [SerializeField]
        private bool mDoShotImmediately = false;

        // check if the particle spawned.
        private bool mParticleSpawned = false;

        [Header("Other")]

        [Tooltip("Set the particles as child?")]
        [SerializeField]
        private bool mSetChild = true;

        [Tooltip("Everytime active the particle set the position to this transform position.")]
        [SerializeField]
        private bool mSetToSamePositionWhenActive = true;

        //-- Thread
        private JCS_Vec<int> mThread = null;
        private JCS_Vec<float> mTimers = null;           // timer per thread
        private JCS_Vec<int> mParticleCount = null;         // how many shoot should process per thread
        private JCS_Vec<int> mParticleCounter = null;         // counter per thread

        /* Setter & Getter */

        public JCS_Vec<JCS_Particle> GetParticles() { return mParticles; }

        // Binds.
        public bool active { get { return mActive; } set { mActive = value; } }
        public bool activeThread { get { return mActiveThread; } set { mActiveThread = value; } }
        public int orderLayer { get { return mOrderLayer; } set { mOrderLayer = value; } }
        public int numOfParticle { get { return mNumOfParticle; } set { mNumOfParticle = value; } }
        public JCS_Particle particle { get { return mParticle; } set { mParticle = value; } }
        public int density { get { return mDensity; } set { mDensity = value; } }
        public float windSpeed { get { return mWindSpeed; } set { mWindSpeed = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool alwaysTheSameScale { get { return mAlwaysTheSameScale; } set { mAlwaysTheSameScale = value; } }
        public bool setChild { get { return mSetChild; } set { mSetChild = value; } }
        public bool setToSamePositionWhenActive { get { return mSetToSamePositionWhenActive; } set { mSetToSamePositionWhenActive = value; } }
        public bool freezeX { get { return mFreezeX; } set { mFreezeX = value; } }
        public bool freezeY { get { return mFreezeY; } set { mFreezeY = value; } }
        public bool freezeZ { get { return mFreezeZ; } set { mFreezeZ = value; } }
        public float randPosX { get { return mRandPosX; } set { mRandPosX = value; } }
        public float randPosY { get { return mRandPosY; } set { mRandPosY = value; } }
        public float randPosZ { get { return mRandPosZ; } set { mRandPosZ = value; } }
        public float randAngleX { get { return mRandAngleX; } set { mRandAngleX = value; } }
        public float randAngleY { get { return mRandAngleY; } set { mRandAngleY = value; } }
        public float randAngleZ { get { return mRandAngleZ; } set { mRandAngleZ = value; } }
        public float randScaleX { get { return mRandScaleX; } set { mRandScaleX = value; } }
        public float randScaleY { get { return mRandScaleY; } set { mRandScaleY = value; } }
        public float randScaleZ { get { return mRandScaleZ; } set { mRandScaleZ = value; } }
        public bool doShotImmediately { get { return mDoShotImmediately; } set { mDoShotImmediately = value; } }

        /* Functions */

        private void Awake()
        {
            mParticles = new JCS_Vec<JCS_Particle>();

            mThread = new JCS_Vec<int>();
            mTimers = new JCS_Vec<float>();
            mParticleCount = new JCS_Vec<int>();
            mParticleCounter = new JCS_Vec<int>();

            mFreezePos = transform.position;
        }

        private void Start()
        {
            SpawnParticles();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            if (mActiveThread)
                ProccessSequences();

            // check if this particle engine active?
            if (!mActive)
                return;

            DoParticle();

            Freeze();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(startKey))
                StartActive();

            if (JCS_Input.GetKeyDown(stopKey))
                StopActive();

            if (JCS_Input.GetKeyDown(playOneShotKey))
                PlayOneShot(oneShotParticleCount);
        }
#endif

        /// <summary>
        /// Start the paritcle engine.
        /// </summary>
        public void StartActive()
        {
            mActive = true;
        }

        /// <summary>
        /// Stop the paritcle engine.
        /// </summary>
        public void StopActive()
        {
            mActive = false;
        }

        /// <summary>
        /// Active some particle in certain number current frame.
        /// </summary>
        /// <param name="num"> number to spawn the particle. </param>
        public void PlayOneShot()
        {
            PlayOneShot(mDensity);
        }

        /// <summary>
        /// Active some particle in certain number current frame.
        /// </summary>
        /// <param name="num"> number to spawn the particle. </param>
        public void PlayOneShot(int num)
        {
            if (mParticle == null)
                return;

            if (mDoShotImmediately)
            {
                for (int count = 0; count < num; ++count)
                {
                    JCS_Particle particle = SearchAvaliableParticles();

                    if (particle == null)
                        continue;

                    SetParticleBySetting(particle);
                }
            }
            else
            {
                for (int count = 0; count < num; ++count)
                {
                    DoSequenceParticle(mDensity);
                }
            }
        }

        /// <summary>
        /// Check if no particle running?
        /// </summary>
        /// <returns>
        /// true : no thread running.
        /// false : vice versa.
        /// </returns>
        public bool IsParticleEnd()
        {
            return (mThread.length == 0);
        }

        /// <summary>
        /// Spawn all particle base on the count.
        /// </summary>
        public void SpawnParticles()
        {
            // check if particles already spawned?
            if (mParticleSpawned)
                return;

            if (mParticle == null)
            {
                Debug.Log(
                    "No particle assign!");
                return;
            }

            for (int index = 0; index < mNumOfParticle; ++index)
            {
                var trans = JCS_Util.Instantiate(mParticle) as JCS_Particle;
                mParticles.push(trans);

                // disable the object
                trans.gameObject.SetActive(false);

                if (mSetChild)
                {
                    // set parent
                    trans.transform.SetParent(transform);
                }
            }

            mParticleSpawned = true;
        }

        /// <summary>
        /// Freee the paritcle engine position?
        /// </summary>
        private void Freeze()
        {
            Vector3 newPos = transform.position;

            if (mFreezeX)
                newPos.x = mFreezePos.x;
            if (mFreezeY)
                newPos.y = mFreezePos.y;
            if (mFreezeZ)
                newPos.z = mFreezePos.z;

            transform.position = newPos;
        }

        /// <summary>
        /// Do the particle algorithm here.
        /// </summary>
        private void DoParticle()
        {
            if (mParticle == null)
                return;

            mSequenceTimer += JCS_Time.ItTime(mTimeType);

            // NOTE(jenchieh): magic number...
            float time = JCS_Random.Range(0.5f, 1.0f);

            if (time < mSequenceTimer)
            {
                // reset timer
                mSequenceTimer = 0;

                // do a sequence
                DoSequenceParticle(mDensity);
            }
        }

        /// <summary>
        /// Spawn a thread in order to get process.
        /// </summary>
        /// <param name="density"> density for the particle effect. </param>
        private void DoSequenceParticle(int density)
        {
            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);
            mParticleCount.push(density);
            mParticleCounter.push(0);
        }

        /// <summary>
        /// Find the avaliable particle in the list.
        /// </summary>
        /// <returns> non-active paritcle. </returns>
        private JCS_Particle SearchAvaliableParticles(bool sec = false)
        {
            if (mNumOfParticle <= 0)
            {
                Debug.LogError(
                    "Number of particle cannot lower or equal to zero...");
                return null;
            }

            for (int index = mLastAvaliableIndex; index < mParticles.length; ++index)
            {
                JCS_Particle particle = mParticles.at(index);
                bool isActive = particle.gameObject.activeInHierarchy;
                if (isActive == false)
                {
                    particle.gameObject.SetActive(true);
                    mLastAvaliableIndex = index;
                    return particle;
                }
            }

            // if we get here mean we cycle once but we
            // did not spawn a text!
            // so reset the spawn pos and 
            // try to search agian until we find one!
            mLastAvaliableIndex = 0;

            // if second time still does not find, return null.
            // prevent stack overflow.
            if (sec)
                return null;

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            return SearchAvaliableParticles(true);
        }

        /// <summary>
        /// Loop through thread list and process it.
        /// </summary>
        private void ProccessSequences()
        {
            for (int processIndex = 0; processIndex < mThread.length; ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }

        /// <summary>
        /// Process the thread.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += JCS_Time.ItTime(mTimeType);

            // check if we can do the particle or not
            if (mTimeAParticle < newTimer)
            {
                int totalParticleCount = mParticleCount.at(processIndex);
                int currentParticleCount = mParticleCounter.at(processIndex);
                if (currentParticleCount == totalParticleCount)
                {
                    // Remove Thread.
                    EndProcessSequence(processIndex);
                    return;
                }

                // find a particle
                JCS_Particle particle = SearchAvaliableParticles();

                if (particle == null)
                {
                    // exit function.
                    return;
                }

                SetParticleBySetting(particle);

                ++currentParticleCount;

                // update new count, in order 
                // to spawn next bullet
                mParticleCounter.set(processIndex, currentParticleCount);
                newTimer = 0;
            }

            // update timer
            mTimers.set(processIndex, newTimer);
        }

        /// <summary>
        /// Delete the thead when done processing.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mParticleCount.slice(processIndex);
            mParticleCounter.slice(processIndex);
        }

        /// <summary>
        /// Make a particle fit the setting.
        /// </summary>
        /// <param name="particle"> particle to change and fit the setting. </param>
        private void SetParticleBySetting(JCS_Particle particle)
        {
            // get this particle info.
            Vector3 newPos = particle.transform.position;

            if (mSetToSamePositionWhenActive)
            {
                // set position to the same position as particle 
                // system's position.
                particle.transform.position = transform.position;
            }

            // after we have set the position to current 
            // particle system's position. Next step is to 
            // apply the random value to the newer position.
            newPos.x += JCS_Random.Range(-mRandPosX, mRandPosX);
            newPos.y += JCS_Random.Range(-mRandPosY, mRandPosY);
            newPos.z += JCS_Random.Range(-mRandPosZ, mRandPosZ);
            particle.transform.position = newPos;

            // set rotation depend on wind speed
            /**
             * NOTE(jenchieh): 
             *                      ------- Set the scale from 
             *                      ↓      the original particle, 
             *                      ↓      so the euler angles won't 
             *                  ↓-------↓  get stack up.              */
            Vector3 newRot = mParticle.transform.localEulerAngles;
            newRot.z = mWindSpeed;

            // apply wind speed.
            newRot.z += JCS_Random.Range(-mRandAngleX, mRandAngleX);

            // apply random rotation.
            newRot.x += JCS_Random.Range(-mRandAngleX, mRandAngleX);
            newRot.y += JCS_Random.Range(-mRandAngleY, mRandAngleY);
            newRot.z += JCS_Random.Range(-mRandAngleZ, mRandAngleZ);
            particle.transform.localEulerAngles = newRot;

            /**
             * NOTE(jenchieh): 
             *                      ------- Set the scale from 
             *                      ↓      the original particle, 
             *                      ↓      so the scale won't 
             *                  ↓-------↓  get stack up.              */
            Vector3 newScale = mParticle.transform.localScale;
            newScale.x += JCS_Random.Range(-mRandScaleX, mRandScaleX);
            newScale.y += JCS_Random.Range(-mRandScaleY, mRandScaleY);
            newScale.z += JCS_Random.Range(-mRandScaleZ, mRandScaleZ);
            if (mAlwaysTheSameScale)
            {
                // set the scale the same.
                newScale.y = newScale.x;
                newScale.z = newScale.x;
            }
            particle.transform.localScale = newScale;
        }
    }
}
