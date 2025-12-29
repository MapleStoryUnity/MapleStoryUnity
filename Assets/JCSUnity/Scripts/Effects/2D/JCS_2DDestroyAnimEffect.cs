/**
 * $File: JCS_2DDestroyAnimEffect.cs $
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
    /// Play the animation when the game object is destroyed.
    /// 
    /// Differet from normal 'JCS_DestroyAnimEffect' was this
    /// uses the JCS_2DAnimation to display the animation.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_2DAnimPool))]
    public class JCS_2DDestroyAnimEffect : MonoBehaviour
    {
        /* Variables */

        // pool to grab animation to play.
        private JCS_2DAnimPool m2DAnimPool = null;

        [Separator("⚡️ Runtime Variables (JCS_2DDestroyAnimEffect)")]

        [Tooltip("Sorting layer this effect going to render.")]
        [SerializeField]
        private int mOrderLayer = 1;

        [Tooltip("How many times the animation will played after destroyed.")]
        [SerializeField]
        private int mLoopTimes = 1;

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

        [Header("🔍 Transform")]

        [Tooltip("Play the animation as the same position as the destroyed game object.")]
        [SerializeField]
        private bool mSamePosition = true;

        [Tooltip("Play the animation as the same rotation as the destroyed game object.")]
        [SerializeField]
        private bool mSameRotation = true;

        [Tooltip("Play the animation as the same scale as the destroyed game object.")]
        [SerializeField]
        private bool mSameScale = true;

        [Header("🔍 Random Effect")]

        [Tooltip("Randomize the position when the animation is played.")]
        [SerializeField]
        private bool mRandPos = false;

        [Tooltip("Random position value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRange = 0.0f;

        [Tooltip("Randomize the rotation when the animation is played.")]
        [SerializeField]
        private bool mRandRot = false;

        [Tooltip("Random rotation value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandRotRange = 0.0f;

        [Tooltip("Randomize the scale when the animation is played.")]
        [SerializeField]
        private bool mRandScale = false;

        [Tooltip("Random scale value added.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandScaleRange = 0.0f;

        /* Setter & Getter */

        public int orderLayer { get { return mOrderLayer; } set { mOrderLayer = value; } }
        public int loopTimes { get { return mLoopTimes; } set { mLoopTimes = value; } }

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
            m2DAnimPool = GetComponent<JCS_2DAnimPool>();

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


            var gm = new GameObject();
#if UNITY_EDITOR
            gm.name = "JCS_2DDestroyAnimEffect";
#endif

            var sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;

            JCS_2DAnimation randAnim = m2DAnimPool.GetRandomAnim();
            var newAnim = gm.AddComponent<JCS_2DAnimation>();

            newAnim.active = randAnim.active;
            newAnim.loop = randAnim.loop;
            newAnim.playOnAwake = randAnim.playOnAwake;
            newAnim.secPerFrame = randAnim.secPerFrame;

            // set the animation to just spawn animation.
            newAnim.SetAnimationFrame(randAnim.animationFrames);
            newAnim.Play();

            // add this event, so the when animation done play it will get destroy.
            var das = gm.AddComponent<JCS_2DDestroyAnimEndEvent>();
            das.loopTimes = mLoopTimes;

            if (mSamePosition)
                sr.transform.position = transform.position;
            if (mSameRotation)
                sr.transform.rotation = transform.rotation;
            if (mSameScale)
                sr.transform.localScale = transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(sr);
            if (mRandRot)
                AddRandomRotation(sr);
            if (mRandScale)
                AddRandomScale(sr);
        }

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(SpriteRenderer dae)
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
        private void AddRandomRotation(SpriteRenderer dae)
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
        private void AddRandomScale(SpriteRenderer dae)
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
