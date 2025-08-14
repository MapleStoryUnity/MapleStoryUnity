/**
 * $File: JCS_DestroySpawnEffect.cs $
 * $Date: 2017-04-17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Effect that spawn a game object after this game object is destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformPool))]
    public class JCS_DestroySpawnEffect : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformPool mTransformPool = null;
        private JCS_HitListEvent mHitList = null;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        [Separator("Runtime Variables (JCS_TransformPool)")]

        [Tooltip("How many transform spawn?")]
        [SerializeField]
        private int mSpawnCount = 0;

        //-- Hit List
        [Tooltip("Active this effect by what ever this object is destoryed.")]
        [SerializeField]
        private bool mActiveWhatever = false;

        [Tooltip("Active the effect by hitting the certain object.")]
        [SerializeField]
        private bool mActiveWithHitList = true;

        //-- Time
        [Tooltip("Active the effect by the destroy time.")]
        [SerializeField]
        private bool mActiveWithDestroyTime = false;

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

        [Header("- Random Effect")]

        [Tooltip("Randomize the position when spawn the game object.")]
        [SerializeField]
        private bool mRandPos = false;

        [Tooltip("Random position value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRange = 0.0f;

        [Tooltip("Randomize the rotation when spawn the game object.")]
        [SerializeField]
        private bool mRandRot = false;

        [Tooltip("Random rotation value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandRotRange = 0.0f;

        [Tooltip("Randomize the scale when spawn the game object.")]
        [SerializeField]
        private bool mRandScale = false;

        [Tooltip("Random scale value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRange = 0.0f;

        /* Setter & Getter */

        public bool activeWhatever { get { return mActiveWhatever; } set { mActiveWhatever = value; } }
        public bool activeWithHitList { get { return mActiveWithHitList; } set { mActiveWithHitList = value; } }
        public bool activeWithDestroyTime { get { return mActiveWithDestroyTime; } set { mActiveWithDestroyTime = value; } }

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
            mHitList = GetComponent<JCS_HitListEvent>();
            mTransformPool = GetComponent<JCS_TransformPool>();

            mDestroyObjectWithTime = GetComponent<JCS_DestroyObjectWithTime>();
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


            // spawn the game object.
            DoSpawn();
        }

        /// <summary>
        /// Do the spawning.
        /// </summary>
        private void DoSpawn()
        {
            for (int index = 0; index < mSpawnCount; ++index)
            {
                Transform newTrans = (Transform)JCS_Util.Instantiate(mTransformPool.GetRandomObject());

                if (mSamePosition)
                    newTrans.position = transform.position;
                if (mSameRotation)
                    newTrans.rotation = transform.rotation;
                if (mSameScale)
                    newTrans.localScale = transform.localScale;

                // Random Effect
                if (mRandPos)
                    AddRandomPosition(newTrans);
                if (mRandRot)
                    AddRandomRotation(newTrans);
                if (mRandScale)
                    AddRandomScale(newTrans);
            }
        }

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(Transform trans)
        {
            float addPos;
            Vector3 newPos = trans.position;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            // Not sure we have to include z direction?
            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.z += addPos;

            trans.position = newPos;
        }

        /// <summary>
        /// Add random value to the effect's transform's rotation.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomRotation(Transform trans)
        {
            float addRot;
            Vector3 newRot = trans.localEulerAngles;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.x += addRot;

            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.y += addRot;

            // Not sure we have to include z direction?
            addRot = Random.Range(-mRandRotRange, mRandRotRange);
            newRot.z += addRot;

            trans.localEulerAngles = newRot;
        }

        /// <summary>
        /// Add random value to the effect's transform's scale.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomScale(Transform trans)
        {
            float addScale;
            Vector3 newScale = trans.localScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.x += addScale;

            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.y += addScale;

            // Not sure we have to include z direction?
            addScale = Random.Range(-mRandScaleRange, mRandScaleRange);
            newScale.z += addScale;

            trans.localScale = newScale;
        }
    }
}
