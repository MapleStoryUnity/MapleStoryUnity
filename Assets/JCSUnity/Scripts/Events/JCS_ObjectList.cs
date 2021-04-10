/**
 * $File: JCS_ObjectList.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// List of gameobject holder, with some utility functions provided 
    /// messing with list of gameobject.
    /// </summary>
    public class JCS_ObjectList
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ObjectList) **")]

        [Tooltip("List of objects.")]
        [SerializeField]
        private List<Transform> mObjects = null;

        /* Setter & Getter */

        public List<Transform> Objects { get { return this.mObjects; } }

        /* Functions */

        /// <summary>
        /// Returns a random object from the list.
        /// </summary>
        /// <returns> object </returns>
        public Transform GetRandomObjectFromList()
        {
            if (mObjects.Count == 0)
                return null;

            int randIndex = JCS_Random.Range(0, mObjects.Count);

            Transform randObj = mObjects[randIndex];

            return randObj;
        }
    }
}
