/**
 * $File: JCS_2DLiveObjectManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle all the 2d live object in the scene.
    /// </summary>
    public class JCS_2DLiveObjectManager : JCS_Manager<JCS_2DLiveObjectManager>
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DLiveObjectManager)")]

        [Tooltip("All the live object in the scene.")]
        public JCS_2DLiveObject[] LIVE_OBJECT_LIST = null;

        [Tooltip("Time to find all the live object in the scene periodically.")]
        public float TIME_TO_FIND_ALL_LIVE_OBJECT_IN_SCENE = 3;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            StartCoroutine(FindAllLiveObjectInScene(TIME_TO_FIND_ALL_LIVE_OBJECT_IN_SCENE));
        }

        /// <summary>
        /// Destroy all the live object in the scene.
        /// 全圖殺怪!
        /// </summary>
        public void DestroyAllLiveObject()
        {
            // Destroy all the live object in the scene.
            JCS_2DLiveObject[] los = Resources.FindObjectsOfTypeAll<JCS_2DLiveObject>();

            foreach (JCS_2DLiveObject lo in los)
            {
                // NOTE(JenChieh): kill the object that are clone!
                // or else it will effect the prefab object...
                if (JCS_Util.IsClone(lo.gameObject))
                    lo.Die();
            }
        }

        /// <summary>
        /// Time to find all live object in scene.
        /// </summary>
        /// <param name="time"> time to find. </param>
        /// <returns> wait. </returns>
        private IEnumerator FindAllLiveObjectInScene(float time)
        {
            while (true)
            {
                LIVE_OBJECT_LIST = JCS_Util.FindObjectsByType(typeof(JCS_2DLiveObject)) as JCS_2DLiveObject[];
                yield return new WaitForSeconds(time);
            }
        }
    }
}
