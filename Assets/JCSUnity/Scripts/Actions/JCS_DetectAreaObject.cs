/**
 * $File: JCS_DetectAreaObject.cs $
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
    /// Tag for "JCS_DetectAreaAction".
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_DetectAreaObject : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_DetectAreaObject) **")]

        [SerializeField]
        private JCS_2DLiveObject mLiveObject = null;

        /* Setter & Getter */

        public JCS_2DLiveObject GetLiveObject() { return this.mLiveObject; }

        /* Functions */

        private void Awake()
        {
            mLiveObject = this.GetComponent<JCS_2DLiveObject>();
        }
    }
}
