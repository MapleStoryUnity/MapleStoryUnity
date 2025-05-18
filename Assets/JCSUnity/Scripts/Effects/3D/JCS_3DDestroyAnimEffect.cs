/**
 * $File: JCS_3DDestroyAnimEffect.cs $
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
    /// Play the animation when the game object is destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    [RequireComponent(typeof(JCS_AnimPool))]
    public class JCS_3DDestroyAnimEffect : MonoBehaviour
    {
        /* Variables */

        private JCS_AnimPool mDestroyAnim = null;
        private JCS_HitListEvent mHitList = null;
        private JCS_DestroyObjectWithTime mDestroyObjectWithTime = null;

        [Separator("Runtime Variables (JCS_3DDestroyAnimEffect)")]

        [Tooltip("Sorting layer this effect going to render.")]
        [SerializeField]
        private int mOrderLayer = 1;

        [Tooltip("How many times to animate then destroy.")]
        [SerializeField]
        private int mLoopTimes = 1;

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

        public int OrderLayer { get { return this.mOrderLayer; } set { this.mOrderLayer = value; } }
        public int LoopTimes { get { return this.mLoopTimes; } set { this.mLoopTimes = value; } }

        public bool ActiveWhatever { get { return this.mActiveWhatever; } set { this.mActiveWhatever = value; } }
        public bool ActiveWithHitList { get { return this.mActiveWithHitList; } set { this.mActiveWithHitList = value; } }
        public bool ActiveWithDestroyTime { get { return this.mActiveWithDestroyTime; } set { this.mActiveWithDestroyTime = value; } }

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
            this.mHitList = this.GetComponent<JCS_HitListEvent>();
            this.mDestroyAnim = this.GetComponent<JCS_AnimPool>();

            this.mDestroyObjectWithTime = this.GetComponent<JCS_DestroyObjectWithTime>();
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
                        Debug.LogError("You can't active destroy time without `JCS_DestroyObjectWithTime` component");
                    }
                }
            }

            // do not trigger this effect.
            if (!onTrigger)
                return;


            GameObject gm = new GameObject();
#if UNITY_EDITOR
            gm.name = "JCS_3DDestroyAnimEffect";
#endif

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = mDestroyAnim.GetRandomAnim();

            // add this event, so the when animation done play it will get destroy.
            JCS_DestroyAnimEndEvent dae = gm.AddComponent<JCS_DestroyAnimEndEvent>();
            dae.LoopTimes = LoopTimes;

            if (mSamePosition)
                dae.transform.position = this.transform.position;
            if (mSameRotation)
                dae.transform.rotation = this.transform.rotation;
            if (mSameScale)
                dae.transform.localScale = this.transform.localScale;

            // Random Effect
            if (mRandPos)
                AddRandomPosition(dae);
            if (mRandRot)
                AddRandomRotation(dae);
            if (mRandScale)
                AddRandomScale(dae);
        }

        /// <summary>
        /// Add random value to the effect's transform's position.
        /// </summary>
        /// <param name="dae"> effect transform </param>
        private void AddRandomPosition(JCS_DestroyAnimEndEvent dae)
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
        private void AddRandomRotation(JCS_DestroyAnimEndEvent dae)
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
        private void AddRandomScale(JCS_DestroyAnimEndEvent dae)
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
