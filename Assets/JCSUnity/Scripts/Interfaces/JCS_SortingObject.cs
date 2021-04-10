/**
 * $File: JCS_SortingObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all the sorting object.
    /// </summary>
    public abstract class JCS_SortingObject
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_SortingObject) **")]

        [Tooltip(@"With this higher the panel have higher sorthing order. 
(Higher will be in the front.)")]
        [SerializeField]
        protected int mOrderIndex = 0;

        // check if this is sorted?
        protected bool mSorted = false;

        /* Setter & Getter */

        public int OrderIndex { get { return this.mOrderIndex; } }
        public bool Sorted { get { return this.mSorted; } set { this.mSorted = value; } }

        /* Functions */

    }
}
