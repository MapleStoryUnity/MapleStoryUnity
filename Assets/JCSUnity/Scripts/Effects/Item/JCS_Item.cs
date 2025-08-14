/**
 * $File: JCS_Item.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Base class for all the item subclasses.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class JCS_Item : JCS_UnityObject
    {
        /* Variables */

        // Execution after the item is picked.
        public Action<Collider> onPicked = null;

        protected bool mCanPick = true;

        protected BoxCollider mBoxCollider = null;

        [Separator("Check Variables (JCS_Item)")]

        [Tooltip(@"Collider detect can be picked.
You can set this directly in order to get the pick effect too. Once you set this, 
the object will do tween effect to this transform.")]
        [SerializeField]
        protected Collider mPickCollider = null;

        [Separator("Runtime Settings (JCS_Item)")]

        [Tooltip("Is the auto pick collider must be player?")]
        [SerializeField]
        protected bool mMustBeActivePlayer = true;

        [Tooltip("Key to active pick event.")]
        [SerializeField]
        protected KeyCode mPickKey = KeyCode.Z;

        [Header("- System")]

        [Tooltip("Pick item by click/mouse?")]
        [SerializeField]
        protected bool mPickByMouseDown = false;

        [Tooltip("When player hit this object pick it up automatically.")]
        [SerializeField]
        protected bool mAutoPickColliderTouched = false;

        [Tooltip(@"When the item are on the ground, pick it up when there is 
object that we target.")]
        [SerializeField]
        protected bool mAutoPickWhileCan = false;

        [Header("- Sound")]

        [Tooltip(@"Play one shot while not playing any other sound. (Pick Sound)")]
        [SerializeField]
        protected bool mPlayOneShotWhileNotPlayingForPickSound = true;

        [Tooltip("Sound played when you pick up this item.")]
        [SerializeField]
        protected AudioClip mPickSound = null;

        [Tooltip(@"Play one shot while not playing any other sound.")]
        [SerializeField]
        protected bool mPlayOneShotWhileNotPlayingForEffectSound = false;

        [Tooltip("Sound played when you pick up this item.")]
        [SerializeField]
        protected AudioClip mEffectSound = null;

        [Header("- Optional")]

        [Tooltip("Make item tween to the destination.")]
        [SerializeField]
        private JCS_TransformTweener mTweener = null;

        [Tooltip("Destroy when reach the destination.")]
        [SerializeField]
        private JCS_DestinationDestroy mDestinationDestroy = null;

        /* Setter & Getter */

        public bool autoPickColliderTouched { get { return mAutoPickColliderTouched; } set { mAutoPickColliderTouched = value; } }
        public bool pickByMouseDown { get { return mPickByMouseDown; } set { mPickByMouseDown = value; } }
        public bool autoPickWhileCan { get { return mAutoPickWhileCan; } set { mAutoPickWhileCan = value; } }
        public bool canPick { get { return mCanPick; } set { mCanPick = value; } }
        public BoxCollider GetBoxCollider() { return mBoxCollider; }
        public Collider pickCollider { get { return mPickCollider; } set { mPickCollider = value; } }

        /* Functions */

        protected override void Awake()
        {
            // Update the data once depends on what game object is.
            base.Awake();

            mBoxCollider = GetComponent<BoxCollider>();
        }

        protected virtual void Start()
        {
            if (mTweener == null)
                mTweener = GetComponent<JCS_TransformTweener>();
            if (mDestinationDestroy == null)
                mDestinationDestroy = GetComponent<JCS_DestinationDestroy>();
        }

        protected virtual void Update()
        {
            DoAutoPickWhileCan();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (mAutoPickColliderTouched)
            {
                // picked
                Pick(other);
                return;
            }

            if (JCS_Input.GetKey(mPickKey))
            {
                Pick(other);
            }
        }

        protected virtual void OnMouseDown()
        {
            if (!mPickByMouseDown)
                return;

            if (mPickCollider == null)
            {
                Debug.LogError("Cannot pick the item cuz there is no collider set");
                return;
            }

            Pick(mPickCollider);
        }

        /// <summary>
        /// Pick the item up.
        /// </summary>
        /// <param name="other"></param>
        public void Pick(Collider other)
        {
            if (!mCanPick)
                return;

            var joj = GetComponent<JCS_OneJump>();
            if (joj != null)
            {
                // Only when item is on the ground!
                if (joj.GetVelocity().y != 0)
                    return;
            }

            var p = other.GetComponent<JCS_Player>();

            if (mAutoPickColliderTouched && p != null)
            {
                DoPick(other);
                return;
            }

            if (mMustBeActivePlayer)
            {
                // Check the colliding object are is active player.
                if (JCS_PlayerManager.FirstInstance().IsActivePlayerTransform(other.transform))
                {
                    DoPick(other);
                }
            }
            else
            {
                DoPick(other);
            }
        }

        /// <summary>
        /// Do pick up item action.
        /// </summary>
        /// <param name="other"></param>
        private void DoPick(Collider other)
        {
            DropEffect(other);

            JCS_SoundPlayer sp = JCS_SoundManager.FirstInstance().GlobalSoundPlayer();

            /* Play Pick Sound */
            if (mPlayOneShotWhileNotPlayingForPickSound)
                sp.PlayOneShotWhileNotPlaying(mPickSound);
            else
                sp.PlayOneShot(mPickSound);

            // call item effect.
            onPicked?.Invoke(other);

            /* Play Effect Sound */
            if (mPlayOneShotWhileNotPlayingForEffectSound)
                sp.PlayOneShotWhileNotPlaying(mEffectSound);
            else
                sp.PlayOneShot(mEffectSound);

            mCanPick = false;
        }

        /// <summary>
        /// Do the drop effect.
        /// </summary>
        /// <param name="other"></param>
        private void DropEffect(Collider other)
        {
            // Throw Action Effect...
            {
                //var ta = gameObject.AddComponent<JCS_ThrowAction>();
                //ta.SetTargetTransform(other.transform);
                //ta.ActiveEffect();
            }

            // Tweener Effect...
            {
                if (mTweener == null)
                {
                    // default settings
                    mTweener = gameObject.AddComponent<JCS_TransformTweener>();
                    mTweener.SetObjectType(mObjectType);

                    mTweener.easingY = JCS_TweenType.EASE_OUT_BACK;
                    mTweener.durationX = 2.0f;
                    mTweener.durationY = 5.0f;
                    mTweener.durationZ = 0;
                    mTweener.stopTweenDistance = 0.2f;
                }
                mTweener.DoTweenContinue(other.GetComponent<JCS_UnityObject>());
            }


            if (mDestinationDestroy == null)
            {
                // default settings
                mDestinationDestroy = gameObject.AddComponent<JCS_DestinationDestroy>();
                mDestinationDestroy.SetObjectType(mObjectType);

                mDestinationDestroy.destroyDistance = 0.5f;
            }
            mDestinationDestroy.SetTargetTransform(other.transform);
        }

        /// <summary>
        /// If the Pick Collider is not null, then pick it up immediatly while 
        /// we can pick.
        /// </summary>
        private void DoAutoPickWhileCan()
        {
            if (!mAutoPickWhileCan)
                return;

            if (mPickCollider == null)
                return;

            Pick(mPickCollider);
        }
    }
}
