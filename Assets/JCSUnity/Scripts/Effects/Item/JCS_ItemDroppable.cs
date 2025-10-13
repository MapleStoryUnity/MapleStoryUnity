/**
 * $File: JCS_ItemDroppable.cs $
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
    /// Effect make this object able to drop items.
    /// </summary>
    [RequireComponent(typeof(JCS_ItemIgnore))]
    public class JCS_ItemDroppable : MonoBehaviour
    {
        /* Variables */

        [Serializable]
        public struct ItemSet
        {
            [Tooltip("Possibilty droping this item, the higher the more possiblility.")]
            [Range(0.0f, 100.0f)]
            public float dropRate;

            [Tooltip("Item to drop.")]
            public JCS_Item item;
        };

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_ItemDroppable)")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to drop item once.")]
        [SerializeField]
        private KeyCode mDropItemKey = KeyCode.B;
#endif

        [Separator("Runtime Variables (JCS_ItemDroppable)")]

        [Tooltip(@"Weather or drop will depends on 
this variables first, before to do the actually 
compare algorithm")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mPossiblityDropAction = 100.0f;

        [Tooltip("Number of Item Drap possibilty will increase exponentially.")]
        [SerializeField]
        [Range(1, 1000)]
        private int mDropRate = 1;

        [Tooltip("How many item u want to drop?")]
        [SerializeField]
        [Range(0, 100)]
        private int mMinNumItemDrop = 0;

        [Tooltip("How many item u want to drop?")]
        [SerializeField]
        [Range(0, 100)]
        private int mMaxNumItemDrop = 0;

        [Tooltip("Item Set for droping thing")]
        [SerializeField]
        private ItemSet[] mItemSet = null;

        [Header("Gravity Effect (JCS_ItemDroppable)")]

        [Tooltip("Make item effect by gravity.")]
        [SerializeField]
        private bool mIsGravity = true;

        [Tooltip("How much the item push up to the air.")]
        [SerializeField]
        [Range(0.0f, 120.0f)]
        private float mJumpForce = 10.0f;

        [Tooltip("Randomize the jump force to each item?")]
        [SerializeField]
        private bool mRandomizeJumpForce = false;

        [Tooltip("Add this force to item.")]
        [SerializeField]
        [Range(0.01f, 50.0f)]
        private float mRandomizeJumpForceForce = 0.0f;

        [Header("Rotate Effect ")]

        [Tooltip("Does the item rotate while dropping?")]
        [SerializeField]
        private bool mRotateWhileDropping = true;

        [Tooltip("How fast the item rotates?")]
        [SerializeField]
        [Range(-5000.0f, 5000.0f)]
        private float mRotateSpeed = 1000.0f;

        [Header("Spread Effect (JCS_ItemDroppable)")]

        [Tooltip("Spread the item while dropping.")]
        [SerializeField]
        private bool mSpreadEffect = true;

        [Tooltip("How far between a item to the item next to this item.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mSpreadGap = 0.4f;

        [Tooltip("Did the effect include 3 dimensional.")]
        [SerializeField]
        private bool mIncludeDepth = false;

        [Header("Destroy Effect (JCS_ItemDroppable)")]

        [Tooltip("Does the item fade out when is destroyed.")]
        [SerializeField]
        private bool mDestroyFadeOutEffect = true;

        [Tooltip("When does the item get destroyed.")]
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float mDestroyTime = 30.0f;

        [Tooltip("How fast it fade out when get destroyed.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mFadeTime = 1.0f;

        [Header("Others (JCS_ItemDroppable)")]

        [Tooltip("Do constant wave effect to all the items dropped.")]
        [SerializeField]
        private bool mConstWaveEffect = true;

        [Tooltip(@"Do the item bounce back from the wall after hit the wall or 
just stop there.")]
        [SerializeField]
        private bool mBounceBackfromWall = true;

        [Header("Audio")]

        [Tooltip("Drop Sound.")]
        [SerializeField]
        private AudioClip mDropSound = null;

        /* Setter & Getter */

        public bool ConstWaveEffect { get { return mConstWaveEffect; } set { mConstWaveEffect = value; } }
        public bool BounceBackfromWall { get { return mBounceBackfromWall; } set { mBounceBackfromWall = value; } }

        /* Functions */

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mDropItemKey))
                DropItems();
        }
#endif

        /// <summary>
        /// Calculate possibility and drop the item.
        /// </summary>
        public void DropItems()
        {
            if (mMinNumItemDrop > mMaxNumItemDrop)
            {
                Debug.LogError("No item drop. min max.");
                return;
            }

            // calculate and see if we do the drop action.
            float doDrop = JCS_Random.Range(0, 100);
            if (doDrop > mPossiblityDropAction)
                return;

            // start doing the drop action.

            int itemDrop = JCS_Random.Range(mMinNumItemDrop, mMaxNumItemDrop + 1) * mDropRate;

            bool isEven = ((itemDrop % 2) == 0) ? true : false;

            int index = 0;

            for (index = 0; index < itemDrop; ++index)
            {
                JCS_Item item = ItemDropped();

                if (item == null)
                    continue;

                DropAnItem(item, index, isEven);
            }

            // make sure the object actually drop something.
            if (index > 0)
            {
                // play drop sound.
                PlayDropSound();
            }
        }

        /// <summary>
        /// Calculate possibility and drop the item.
        /// </summary>
        /// <param name="mustDropItem"> item must drop </param>
        /// <param name="specIndex"> specific drop item index </param>
        /// <param name="only"> only drop this item. </param>
        /// <param name="count"> how many of this item u want to drop. </param>
        public void DropItems(JCS_Item mustDropItem, int specIndex = -1, bool only = false, int count = 1)
        {
            if (mustDropItem == null)
            {
                Debug.LogError("Must drop item cannot be null references");
                return;
            }

            if (count <= 0)
            {
                Debug.LogError("Cannot drop item with count less or equal to zero");
                return;
            }

            int index = 0;

            if (only)
            {
                // just
                int itemDrop = count;

                bool isEven = ((itemDrop % 2) == 0) ? true : false;

                for (index = 0; index < count; ++index)
                {
                    // simple assign the item.
                    JCS_Item item = mustDropItem;

                    // do drop the item.
                    DropAnItem(item, index, isEven);
                }

            }
            else
            {
                if (mMinNumItemDrop > mMaxNumItemDrop)
                {
                    Debug.LogError("No item drop. min max.");
                    return;
                }

                // calculate and see if we do the drop action.
                float doDrop = JCS_Random.Range(0, 100);
                if (doDrop > mPossiblityDropAction)
                    return;

                // start doing the drop action.

                int itemDrop = JCS_Random.Range(mMinNumItemDrop, mMaxNumItemDrop + 1) * mDropRate + count;

                bool isEven = ((itemDrop % 2) == 0) ? true : false;

                int randDropIndex = specIndex;


                // check index out of range.
                if (specIndex < 0 || specIndex >= itemDrop)
                    randDropIndex = JCS_Random.Range(0, itemDrop);

                for (index = 0; index < itemDrop; ++index)
                {
                    JCS_Item item = null;

                    if (index == randDropIndex)
                    {
                        // assign must drop item.
                        item = mustDropItem;
                    }
                    else
                        item = ItemDropped();

                    if (item == null)
                        continue;

                    DropAnItem(item, index, isEven);
                }
            }

            // make sure the object actually drop something.
            if (index > 0)
            {
                // play drop sound.
                PlayDropSound();
            }
        }

        /// <summary>
        /// Drop an item.
        /// </summary>
        /// <param name="item"> item u want to spawn </param>
        /// <param name="index"> index to know the force this is pushing to. </param>
        /// <param name="isEven"> is the index even number? </param>
        private void DropAnItem(JCS_Item item, int index, bool isEven)
        {
            DropAnItem(
                item,
                index,
                isEven,
                mIsGravity,
                mSpreadEffect,
                mRotateWhileDropping,
                mConstWaveEffect,
                mDestroyFadeOutEffect);
        }

        /// <summary>
        /// Drop an item.
        /// </summary>
        /// <param name="item"> item u want to spawn </param>
        /// <param name="index"> index to know the force this is pushing to. </param>
        /// <param name="isEven"> is the index even number? </param>
        /// <param name="isGravity"> do gravity effect. </param>
        /// <param name="spreadEffect"> do spread effect. </param>
        /// <param name="rotateDrop"> rotate while dropping. </param>
        /// <param name="waveEffect"> do wave effect while on the ground. </param>
        /// <param name="destroyFade"> while picking it up will fade and destroy. </param>
        private void DropAnItem(
            JCS_Item item,
            int index,
            bool isEven,
            bool isGravity,
            bool spreadEffect,
            bool rotateDrop,
            bool waveEffect,
            bool destroyFade)
        {
            var newItem = JCS_Util.Instantiate(
               item,
               transform.position,
               transform.rotation) as JCS_Item;

            bool isEvenIndex = ((index % 2) == 0) ? true : false;

            if (isGravity)
            {
                var oj = newItem.gameObject.AddComponent<JCS_OneJump>();

                float gapDirection = mSpreadGap;
                if (isEvenIndex)
                    gapDirection = -mSpreadGap;

                oj.bounceBackfromWall = BounceBackfromWall;

                float gapForce = 0;

                if (spreadEffect)
                {
                    if (isEven)
                    {
                        if (!isEvenIndex)
                        {
                            gapForce = (gapDirection * (index - 1)) + gapDirection;
                        }
                        else
                        {
                            gapForce = (gapDirection * (index)) + gapDirection;
                        }
                    }
                    // if total is odd
                    else
                    {
                        if (isEvenIndex)
                        {
                            gapForce = (gapDirection * (index));
                        }
                        else
                        {
                            gapForce = (gapDirection * (index)) + gapDirection;
                        }
                    }
                }

                float jumpForce = mJumpForce;
                if (mRandomizeJumpForce)
                {
                    jumpForce += JCS_Random.Range(-mRandomizeJumpForceForce, mRandomizeJumpForceForce);
                }

                oj.DoForce(gapForce, jumpForce, mIncludeDepth);

                if (rotateDrop)
                {
                    var irx = newItem.gameObject.AddComponent<JCS_ItemRotation>();
                    irx.rotateSpeed = JCS_Random.Range(-mRotateSpeed, mRotateSpeed);
                    irx.effect = true;
                    irx.rotateDirection = JCS_Vector3Direction.FORWARD;

                    // if z axis interact in game
                    if (mIncludeDepth)
                    {
                        // add rotation on y axis.
                        var iry = newItem.gameObject.AddComponent<JCS_ItemRotation>();
                        iry.rotateSpeed = JCS_Random.Range(-mRotateSpeed, mRotateSpeed);
                        iry.effect = true;
                        iry.rotateDirection = JCS_Vector3Direction.UP;

                        // add rotation on z axis.
                        var irz = newItem.gameObject.AddComponent<JCS_ItemRotation>();
                        irz.rotateSpeed = JCS_Random.Range(-mRotateSpeed, mRotateSpeed);
                        irz.effect = true;
                        irz.rotateDirection = JCS_Vector3Direction.RIGHT;
                    }
                }
            }

            if (waveEffect)
            {
                var cwe = newItem.gameObject.AddComponent<JCS_3DConstWaveEffect>();
                cwe.SetObjectType(newItem.GetObjectType());
                cwe.effect = true;
            }

            if (destroyFade)
            {
                var dowt = newItem.gameObject.AddComponent<JCS_DestroyObjectWithTime>();
                dowt.fadeTime = mFadeTime;
                dowt.destroyTime = mDestroyTime;

                Renderer[] renderers = newItem.GetComponentsInChildren<Renderer>();

                foreach (Renderer rdr in renderers)
                {
                    var fo = rdr.gameObject.AddComponent<JCS_FadeObject>();

                    dowt.fadeObjects.Add(fo);

                    // set the object type the same.
                    fo.SetObjectType(item.GetObjectType());
                }
            }
        }

        /// <summary>
        /// Decide what item to drop base on
        /// the array list we have!
        /// </summary>
        /// <returns> item to drop. </returns>
        private JCS_Item ItemDropped()
        {
            JCS_Item item = null;

            float totalChance = 0;

            // add all possiblity chance together.
            for (int index = 0; index < mItemSet.Length; ++index)
            {
                totalChance += mItemSet[index].dropRate;
            }

            float dropIndex = JCS_Random.Range(0, totalChance + 1);

            float accumMaxDropRate = 0;
            float accumMinDropRate = 0;

            for (int index = 0; index < mItemSet.Length; ++index)
            {
                accumMaxDropRate += mItemSet[index].dropRate;

                if (index == 0)
                {
                    if (JCS_Util.WithInRange(0, mItemSet[0].dropRate, dropIndex))
                    {
                        item = mItemSet[0].item;
                        break;
                    }

                    continue;
                }

                // 比如: 10, 20, 30, 40

                // Loop 1: 0 ~ 10
                // Loop 2: 20(30-10) ~ 30
                // Loop 3: 30(60-30) ~ 60
                // Loop 4: 40(100-60) ~ 100     每個都減掉上一個的Drop Rate!
                if (JCS_Util.WithInRange(accumMinDropRate, accumMaxDropRate, dropIndex))
                {
                    item = mItemSet[index].item;
                    break;
                }

                accumMinDropRate += mItemSet[index].dropRate;
            }

            // meaning the last one.
            if (item == null &&
                mItemSet.Length != 0 &&
                mItemSet[mItemSet.Length - 1].dropRate != 0)
            {
                item = mItemSet[mItemSet.Length - 1].item;
            }

            return item;
        }

        /// <summary>
        /// Play one drop sound if drop 
        /// sound is assigned.
        /// </summary>
        private void PlayDropSound()
        {
            if (mDropSound == null)
                return;

            var sm = JCS_SoundManager.FirstInstance();

            sm.GlobalSoundPlayer().PlayOneShot(mDropSound);
        }
    }
}
