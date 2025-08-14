/**
 * $File: JCS_SortingObject.cs $
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
    /// Interface of all the sorting object.
    /// </summary>
    public abstract class JCS_SortingObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_SortingObject)")]

        [Tooltip(@"With this higher the panel have higher sorthing order. 
(Higher will be in the front.)")]
        [SerializeField]
        protected int mOrderIndex = 0;

        // check if this is sorted?
        protected bool mSorted = false;

        /* Setter & Getter */

        public int orderIndex { get { return mOrderIndex; } }
        public bool sorted { get { return mSorted; } set { mSorted = value; } }

        /* Functions */

    }
}
