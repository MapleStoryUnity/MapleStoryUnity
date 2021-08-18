/**
 * $File: JCS_DestroyParticleEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Effect when the gameobject get destroyed, spawn a particle system.
    /// </summary>
    [RequireComponent(typeof(JCS_ParticleSystem))]
    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_DestroyParticleEffect : MonoBehaviour
    {
        /* Variables */

        private JCS_ParticleSystem mParticleSystem = null;

        [Header("** Initialize Variables (JCS_DestroyParticleEffect) **")]

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

        [Tooltip("Play the animation as the same position as the destroyed gameobject.")]
        [SerializeField]
        private bool mSamePosition = true;
        [Tooltip("Play the animation as the same rotation as the destroyed gameobject.")]
        [SerializeField]
        private bool mSameRotation = true;
        [Tooltip("Play the animation as the same scale as the destroyed gameobject.")]
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

        public bool ActiveWhatever { get { return this.mActiveWhatever; } set { this.mActiveWhatever = value; } }
        public bool ActiveWithHitList { get { return this.mActiveWithHitList; } set { this.mActiveWithHitList = value; } }
        public bool ActiveWithDestroyTime { get { return this.mActiveWithDestroyTime; } set { this.mActiveWithDestroyTime = value; } }

        public bool DestroyByTime { get { return this.mDestroyByTime; } set { this.mDestroyByTime = value; } }
        public float DestroyTime { get { return this.mDestroyTime; } set { this.mDestroyTime = value; } }

        public bool SamePosition { get { return this.mSamePosition; } set { this.mSamePosition = value; } }
        public bool SameRotation { get { return this.mSameRotation; } set { this.mSameRotation = value; } }
        public bool SameScale { get { return this.mSameScale; } set { this.mSameScale = value; } }

        public bool RandPos { get { return this.mRandPos; } set { this.mRandPos = value; } }
        public bool RandRot { get { return this.mRandRot; } set { this.mRandRot = value; } }
        public bool RandScale { get { return this.mRandScale; } set { this.mRandScale = value; } }
        public float RandPosRange { get { return this.mRandPosRange; } set { this.mRandPosRange = value; } }
        public float RandRotRange { get { return this.mRandRotRange; } set { this.mRandRotRange = value; } }
        public float RandScaleRange { get { return this.mRandScaleRange; } set { this.mRandScaleRange = value; } }

        /* Functions */

        private void Awake()
        {
            this.mParticleSystem = this.GetComponent<JCS_ParticleSystem>();
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
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
                    if (mHitList.IsHit)
                        onTrigger = true;
                }

                // if checking for destroy time.
                if (mActiveWithDestroyTime)
                {
                    if (mDestroyObjectWithTime != null)
                    {
                        if (mDestroyObjectWithTime.TimesUp)
                            onTrigger = true;
                    }
                    else
                    {
                        JCS_Debug.LogError("You can't active destroy time without `JCS_DestroyObjectWithTime` component");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;


            GameObject gm = new GameObject();
#if (UNITY_EDITOR)
            gm.name = "JCS_DestroyParticleEffect";
#endif

            JCS_ParticleSystem jcsps = gm.AddComponent<JCS_ParticleSystem>();

            if (mSamePosition)
                jcsps.transform.position = this.transform.position;
            if (mSameRotation)
                jcsps.transform.rotation = this.transform.rotation;
            if (mSameScale)
                jcsps.transform.localScale = this.transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(jcsps);
            if (mRandRot)
                AddRandomRotation(jcsps);
            if (mRandScale)
                AddRandomScale(jcsps);

            // copy and paste component to new one.
            {
                jcsps.Particle = mParticleSystem.Particle;
                jcsps.NumOfParticle = mParticleSystem.NumOfParticle;

                jcsps.Active = mParticleSystem.Active;
                jcsps.ActiveThread = mParticleSystem.ActiveThread;
                jcsps.OrderLayer = mParticleSystem.OrderLayer;
                jcsps.Density = mParticleSystem.Density;
                jcsps.WindSpeed = mParticleSystem.WindSpeed;

                jcsps.AlwaysTheSameScale = mParticleSystem.AlwaysTheSameScale;

                jcsps.FreezeX = mParticleSystem.FreezeX;
                jcsps.FreezeY = mParticleSystem.FreezeY;
                jcsps.FreezeZ = mParticleSystem.FreezeZ;

                jcsps.RandPosX = mParticleSystem.RandPosX;
                jcsps.RandPosY = mParticleSystem.RandPosY;
                jcsps.RandPosZ = mParticleSystem.RandPosZ;

                jcsps.RandAngleX = mParticleSystem.RandAngleX;
                jcsps.RandAngleY = mParticleSystem.RandAngleY;
                jcsps.RandAngleZ = mParticleSystem.RandAngleZ;

                jcsps.RandScaleX = mParticleSystem.RandScaleX;
                jcsps.RandScaleY = mParticleSystem.RandScaleY;
                jcsps.RandScaleZ = mParticleSystem.RandScaleZ;

                jcsps.DoShotImmediately = mParticleSystem.DoShotImmediately;

                jcsps.SetChild = mParticleSystem.SetChild;
                jcsps.SetToSamePositionWhenActive = mParticleSystem.SetToSamePositionWhenActive;
            }

            // spawn the particle immediately.
            jcsps.SpawnParticles();

            // spawn the particle.
            jcsps.PlayOneShot();


            if (mDestroyByTime)
            {
                JCS_DestroyObjectWithTime dowt = gm.AddComponent<JCS_DestroyObjectWithTime>();
                dowt.DestroyTime = mDestroyTime;
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
