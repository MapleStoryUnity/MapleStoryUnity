/**
 * $File: JCS_OrderLayerObject.cs $
 * $Date: $
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
    /// Object in the in the scene layer.
    /// </summary>
    public class JCS_OrderLayerObject : MonoBehaviour
    {
        /* Variables */

        /* Down compatible. */
        private SpriteRenderer mSpriteRenderer = null;

        // whats on-top of this 'order layer object'.
        private JCS_OrderLayer mOrderLayer = null;

        [Separator("Initialize Variables (JCS_OrderLayerObject)")]

        [Tooltip("Extra sprite renederer you can set.")]
        [SerializeField]
        private List<SpriteRenderer> mSpriteRenderers = null;

        [Header("- Absolute Layer")]

        [Tooltip("Enable the Absolute Layer Effect.")]
        [SerializeField]
        private bool mAbsoluteLayerEffect = false;

        [Tooltip("This will set this object directly in the scene.")]
        [SerializeField] [Range(-30, 30)]
        private int mAbsotlueLayer = 0;

        /* Setter & Getter */

        public SpriteRenderer GetSpriteRenderer() { return mSpriteRenderer; }
        public JCS_OrderLayer orderLayer { get { return mOrderLayer; } }
        public List<SpriteRenderer> SpriteRenderers() { return mSpriteRenderers; }
        public SpriteRenderer SpriteRenderersAt(int index) { return mSpriteRenderers[index]; }
        public bool absoluteLayerEffect { get { return mAbsoluteLayerEffect; } }
        public int absotlueLayer { get { return mAbsotlueLayer; } }
        public int sortingOrder { get { return mOrderLayer.orderLayer; } }

        /* Functions */

        private void Awake()
        {
            /* Down compatible. */
            mSpriteRenderer = GetComponent<SpriteRenderer>();

            // clean up empty slot.
            mSpriteRenderers = JCS_Array.RemoveEmpty(mSpriteRenderers);
        }

        private void Start()
        {
            mOrderLayer = GetComponentInParent<JCS_OrderLayer>();

            foreach (SpriteRenderer spriteRenderer in mSpriteRenderers)
            {
                if (mOrderLayer == null)
                    continue;

                // override the current order layer.
                spriteRenderer.sortingOrder = mOrderLayer.orderLayer;
            }

            if (mSpriteRenderer != null && mOrderLayer != null)
                mSpriteRenderer.sortingOrder = mOrderLayer.orderLayer;

            //if set absolute effect are enable set it to that specific layer.
            if (mAbsoluteLayerEffect)
                SetObjectParentToOrderLayerByOrderLayerIndex(mAbsotlueLayer);
        }

        /// <summary>
        /// Set the sorting layer in absolute layer.
        /// </summary>
        /// <param name="orderLayer"> rendering order layer. </param>
        public void SetOrderLayer(int orderLayer)
        {
            /* Down compatible. */
            {
                // set the order layer by runtime. (shortcut)
                if (mSpriteRenderer != null)
                    mSpriteRenderer.sortingOrder = orderLayer;
            }

            foreach (SpriteRenderer spriteRenderer in mSpriteRenderers)
            {
                if (spriteRenderer == null)
                    continue;

                spriteRenderer.sortingOrder = orderLayer;
            }
        }

        /// <summary>
        /// Set the object into the scene layer in the scene.
        /// </summary>
        /// <param name="orderLayerIndex"> index of scene layer </param>
        public void SetObjectParentToOrderLayerByOrderLayerIndex(int orderLayerIndex)
        {
            JCS_2DDynamicSceneManager.FirstInstance().SetObjectParentToOrderLayerByOrderLayerIndex(
                this, 
                orderLayerIndex);
        }
    }
}
