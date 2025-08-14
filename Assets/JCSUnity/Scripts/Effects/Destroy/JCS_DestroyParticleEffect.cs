/**
 * $File: JCS_DestroyParticleEffect.cs $
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
    /// Effect when the game object get destroyed, spawn a particle system.
    /// </summary>
    [RequireComponent(typeof(JCS_ParticleSystem))]
    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_DestroyParticleEffect : MonoBehaviour
    {
        /* Variables */

        private JCS_ParticleSystem mParticleSystem = null;

        [Separator("Initialize Variables (JCS_DestroyParticleEffect)")]

        //-- Hit List
        [Tooltip("Active this effect by what ever this object is destoryed.")]
        [SerializeField]
        private bool mActiveWhatever = false;

        [Tooltip("Active the effect by hitting the certain object.")]
        [SerializeField]
        private bool mActiveWithHitList = true;

        private JCS_HitListEvent mHitList = null;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;

        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        [Header("- Transform")]

        [Tooltip("Play the animation as the same position as the destroyed game object.")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("Play the animation as the same rotation as the destroyed game object.")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("Play the animation as the same scale as the destroyed game object.")]
        [SerializeField]
        private bool mSameScale = true;

        [Header("- Random")]

        [Tooltip("Randomize the position when spawn the particle system.")]
        [SerializeField]
        private bool mRandPos = false;

        [Tooltip("Random position value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRange = 0.0f;

        [Tooltip("Randomize the rotation when spawn the particle system.")]
        [SerializeField]
        private bool mRandRot = false;

        [Tooltip("Random rotation value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandRotRange = 0.0f;

        [Tooltip("Randomize the scale when spawn the particle system.")]
        [SerializeField]
        private bool mRandScale = false;

        [Tooltip("Random scale value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRange = 0.0f;

        [Header("- Peformance")]

        [Tooltip(@"Destroy the particle object by time, the 
default is be 'JCS_DestroyParticleEndEvent'.")]
        [SerializeField]
        private bool mDestroyByTime = false;

        [Tooltip("How long the object get destroy.")]
        [SerializeField] [Range(0.0f, 360.0f)]
        private float mDestroyTime = 10.0f;

        /* Setter & Getter */

        public bool activeWhatever { get { return mActiveWhatever; } set { mActiveWhatever = value; } }
        public bool activeWithHitList { get { return mActiveWithHitList; } set { mActiveWithHitList = value; } }
        public bool activeWithDestroyTime { get { return mActiveWithDestroyTime; } set { mActiveWithDestroyTime = value; } }

        public bool destroyByTime { get { return mDestroyByTime; } set { mDestroyByTime = value; } }
        public float destroyTime { get { return mDestroyTime; } set { mDestroyTime = value; } }

        public bool samePosition { get { return mSamePosition; } set { mSamePosition = value; } }
        public bool sameRotation { get { return mSameRotation; } set { mSameRotation = value; } }
        public bool sameScale { get { return mSameScale; } set { mSameScale = value; } }

        public bool randPos { get { return mRandPos; } set { mRandPos = value; } }
        public bool randRot { get { return mRandRot; } set { mRandRot = value; } }
        public bool randScale { get { return mRandScale; } set { mRandScale = value; } }
        public float randPosRange { get { return mRandPosRange; } set { mRandPosRange = value; } }
        public float randRotRange { get { return mRandRotRange; } set { mRandRotRange = value; } }
        public float randScaleRange { get { return mRandScaleRange; } set { mRandScaleRange = value; } }

        /* Functions */

        private void Awake()
        {
            mParticleSystem = GetComponent<JCS_ParticleSystem>();
            mHitList = GetComponent<JCS_HitListEvent>();
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

            // trigger this effect?
            bool onTrigger = false;

            if (mActiveWhatever)
            {
                onTrigger = true;
            }
            // no need to check the rest.
            else
            {
                // if checking for hit list
                if (mActiveWithHitList)
                {
                    if (mHitList.isHit)
                        onTrigger = true;
                }

                // if checking for destroy time.
                if (mActiveWithDestroyTime)
                {
                    if (mDestroyObjectWithTime != null)
                    {
                        if (mDestroyObjectWithTime.timesUp)
                            onTrigger = true;
                    }
                    else
                    {
                        Debug.LogError("You can't active destroy time without `JCS_DestroyObjectWithTime` component");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;

            var gm = new GameObject();
#if UNITY_EDITOR
            gm.name = "JCS_DestroyParticleEffect";
#endif

            var ps = gm.AddComponent<JCS_ParticleSystem>();

            if (mSamePosition)
                ps.transform.position = transform.position;
            if (mSameRotation)
                ps.transform.rotation = transform.rotation;
            if (mSameScale)
                ps.transform.localScale = transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(ps);
            if (mRandRot)
                AddRandomRotation(ps);
            if (mRandScale)
                AddRandomScale(ps);

            // copy and paste component to new one.
            {
                ps.particle = mParticleSystem.particle;
                ps.numOfParticle = mParticleSystem.numOfParticle;

                ps.active = mParticleSystem.active;
                ps.activeThread = mParticleSystem.activeThread;
                ps.orderLayer = mParticleSystem.orderLayer;
                ps.density = mParticleSystem.density;
                ps.windSpeed = mParticleSystem.windSpeed;

                ps.alwaysTheSameScale = mParticleSystem.alwaysTheSameScale;

                ps.freezeX = mParticleSystem.freezeX;
                ps.freezeY = mParticleSystem.freezeY;
                ps.freezeZ = mParticleSystem.freezeZ;

                ps.randPosX = mParticleSystem.randPosX;
                ps.randPosY = mParticleSystem.randPosY;
                ps.randPosZ = mParticleSystem.randPosZ;

                ps.randAngleX = mParticleSystem.randAngleX;
                ps.randAngleY = mParticleSystem.randAngleY;
                ps.randAngleZ = mParticleSystem.randAngleZ;

                ps.randScaleX = mParticleSystem.randScaleX;
                ps.randScaleY = mParticleSystem.randScaleY;
                ps.randScaleZ = mParticleSystem.randScaleZ;

                ps.doShotImmediately = mParticleSystem.doShotImmediately;

                ps.setChild = mParticleSystem.setChild;
                ps.setToSamePositionWhenActive = mParticleSystem.setToSamePositionWhenActive;
            }

            // spawn the particle immediately.
            ps.SpawnParticles();

            // spawn the particle.
            ps.PlayOneShot();


            if (mDestroyByTime)
            {
                var dowt = gm.AddComponent<JCS_DestroyObjectWithTime>();
                dowt.destroyTime = mDestroyTime;
            }
            else
            {
                // add destroy event.
                gm.AddComponent<JCS_DestroyParticleEndEvent>();
            }
        }

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(JCS_ParticleSystem dae)
        {
            float addPos;
            Vector3 newPos = dae.transform.position;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            // Not sure we have to include z direction?
            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.z += addPos;

            dae.transform.position = newPos;
        }

        /// <summary>
        /// Add random value to the effect's transform's rotation.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomRotation(JCS_ParticleSystem dae)
        {
            float addRot;
            Vector3 newRot = dae.transform.localEulerAngles;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.x += addRot;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.y += addRot;

            // Not sure we have to include z direction?
            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.z += addRot;

            dae.transform.localEulerAngles = newRot;
        }

        /// <summary>
        /// Add random value to the effect's transform's scale.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomScale(JCS_ParticleSystem dae)
        {
            float addScale;
            Vector3 newScale = dae.transform.localScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.x += addScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.y += addScale;

            // Not sure we have to include z direction?
            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.z += addScale;

            dae.transform.localScale = newScale;
        }
    }
}
