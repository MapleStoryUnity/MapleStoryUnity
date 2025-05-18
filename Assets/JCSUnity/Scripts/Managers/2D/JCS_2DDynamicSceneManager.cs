/**
 * $File: JCS_2DDynamicSceneManager.cs $
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
    /// Manage all the 2d parallax layer.
    /// </summary>
    public class JCS_2DDynamicSceneManager : JCS_Manager<JCS_2DDynamicSceneManager>
    {
        /* Variables */

        [Separator("Check Variables (JCS_2DDynamicSceneManager)")]

        [Tooltip("")]
        [SerializeField]
        private JCS_OrderLayer[] mJCSOrderLayer = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            // get all the scene layer in the scene.
            // so it could be manage
            mJCSOrderLayer = JCS_Util.FindObjectsOfTypeAllInHierarchy<JCS_OrderLayer>();
        }

        /// <summary>
        /// Function provide to return the targeting order layer, 
        /// in the scene.
        /// </summary>
        /// <param name="orderLayerIndex"> index of the order layer u want to target. </param>
        /// <returns></returns>
        public JCS_OrderLayer GetOrderLayerByOrderLayerIndex(int orderLayerIndex)
        {
            foreach (JCS_OrderLayer ol in mJCSOrderLayer)
            {
                if (ol == null)
                    continue;

                // find the order layer with the index passed in!
                if (ol.OrderLayer == orderLayerIndex)
                    return ol;
            }

            return null;
        }

        /// <summary>
        /// Set the object into the scene layer in the scene.
        /// </summary>
        /// <param name="olo"> this keyword does not pass through, use this function will do. </param>
        /// <param name="orderLayerIndex"> index of scene layer </param>
        public void SetObjectParentToOrderLayerByOrderLayerIndex(JCS_OrderLayerObject olo, int orderLayerIndex)
        {
            SetObjectParentToOrderLayerByOrderLayerIndex(ref olo, orderLayerIndex);
        }
        /// <summary>
        /// Set the object into the scene layer in the scene.
        /// </summary>
        /// <param name="olo"> object u want to set to that specific scene layer </param>
        /// <param name="orderLayerIndex"> index of scene layer </param>
        public void SetObjectParentToOrderLayerByOrderLayerIndex(ref JCS_OrderLayerObject olo, int orderLayerIndex)
        {
            if (olo == null)
            {
                Debug.LogWarning(
                    "The 'JCS_OrderLayerObject' object you trying to set is null references...");
                return;
            }

            // get the order layer by order layer index!
            JCS_OrderLayer ol = GetOrderLayerByOrderLayerIndex(orderLayerIndex);

            if (ol == null)
            {
                Debug.LogWarning(
                    "Did not find the layer you willing to set to..., Layer Index: " + orderLayerIndex);
                return;
            }

            // set parent
            olo.transform.SetParent(ol.transform);

            // set order layer to the pass in object.
            olo.SetOrderLayer(orderLayerIndex);
        }
    }
}
