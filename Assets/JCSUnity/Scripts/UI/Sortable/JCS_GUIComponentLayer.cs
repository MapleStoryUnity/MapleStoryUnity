/**
 * $File: JCS_GUIComponentLayer.cs $
 * $Date: 2016-11-20 21:58:21 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Sort the GUI components so they will have correct 
    /// render order.
    /// 
    /// NOTE(jenchieh): This will only do under the panel layer.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_GUIComponentLayer : JCS_SortingObject
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            // if already sorted, return.
            if (sorted)
                return;

            // get the parent.
            Transform parentObj = transform.parent;

            if (parentObj != null)
            {
                // get all the panel layers in the scene
                JCS_GUIComponentLayer[] compLayers = parentObj.GetComponentsInChildren<JCS_GUIComponentLayer>();

                var sortedCompLayers = new JCS_Sort<JCS_GUIComponentLayer>();

                sortedCompLayers.AddAll(compLayers);

                compLayers = sortedCompLayers.InsertionSort();

                OriganizeChildOrder(compLayers);
            }
        }

        /// <summary>
        /// Re-order all the object.
        /// </summary>
        /// <param name="arr"> sorted object. </param>
        private void OriganizeChildOrder(JCS_GUIComponentLayer[] arr)
        {
            for (int index = 0; index < arr.Length; ++index)
            {
                // this will make gui ontop of each other.
                JCS_Util.MoveToTheLastChild(arr[index].transform);

                // make sure is sorted already.
                arr[index].sorted = true;
            }
        }
    }
}
