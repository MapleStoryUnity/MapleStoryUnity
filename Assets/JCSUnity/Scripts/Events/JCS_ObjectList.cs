/**
 * $File: JCS_ObjectList.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// List of game object holder, with some utility functions provided 
    /// messing with list of game object.
    /// </summary>
    public class JCS_ObjectList : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_ObjectList)")]

        [Tooltip("List of objects.")]
        [SerializeField]
        private List<Transform> mObjects = null;

        /* Setter & Getter */

        public List<Transform> objects { get { return mObjects; } }

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
