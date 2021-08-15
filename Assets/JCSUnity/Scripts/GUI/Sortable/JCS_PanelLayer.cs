/**
 * $File: JCS_PanelLayer.cs $
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
    /// Sort the panels so they will have correct render order.
    /// 
    /// NOTE(jenchieh): This will always be ontop of any other 
    /// GUI, so use this carefully!
    /// </summary>
    [RequireComponent(typeof(JCS_PanelRoot))]
    public class JCS_PanelLayer : JCS_SortingObject
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_PanelLayer) **")]

        [Tooltip("Test this component.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to active move to last child.")]
        [SerializeField]
        private KeyCode mMoveLastKey = KeyCode.L;
#endif

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            // if already sorted, return.
            if (this.Sorted)
                return;

            // get all the jcs_panellayer in the scene
            JCS_PanelLayer[] jcspls = (JCS_PanelLayer[])Resources.FindObjectsOfTypeAll(typeof(JCS_PanelLayer));

            var jcsS = new JCS_Sort<JCS_PanelLayer>();

            jcsS.AddAll(jcspls);

            jcspls = jcsS.InsertionSort();

            OriganizeChildOrder(jcspls);
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mMoveLastKey))
                JCS_Utility.MoveToTheLastChild(this.transform);
        }
#endif

        /// <summary>
        /// Re-order all the object.
        /// </summary>
        /// <param name="arr">sorted object. </param>
        private void OriganizeChildOrder(JCS_PanelLayer[] arr)
        {
            for (int index = 0; index < arr.Length; ++index)
            {
                var layer = arr[index];

                // this will make gui ontop of each other.
                JCS_Utility.MoveToTheLastChild(layer.transform);

                // make sure is sorted already.
                layer.Sorted = true;
            }
        }
    }
}
