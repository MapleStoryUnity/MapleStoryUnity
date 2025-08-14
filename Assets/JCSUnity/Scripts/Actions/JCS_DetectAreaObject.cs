/**
 * $File: JCS_DetectAreaObject.cs $
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
    /// Tag for "JCS_DetectAreaAction".
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_DetectAreaObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_DetectAreaObject)")]

        [SerializeField]
        private JCS_2DLiveObject mLiveObject = null;

        /* Setter & Getter */

        public JCS_2DLiveObject GetLiveObject() { return mLiveObject; }

        /* Functions */

        private void Awake()
        {
            mLiveObject = GetComponent<JCS_2DLiveObject>();
        }
    }
}
