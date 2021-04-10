/**
 * $File: JCS_UniqueObject.cs $
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
    /// Singleton patter in Unity scripting layer.
    /// </summary>
    public class JCS_UniqueObject
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_UniqueObject) **")]

        [Tooltip("Type of the singleton object.")]
        [SerializeField]
        private JCS_SingletonObject mType = JCS_SingletonObject.NONE;

        /* Setter & Getter */

        public JCS_SingletonObject SingletonObjectType { get { return this.mType; } }

        /* Functions */

        private void Awake()
        {
            CheckByType();

            // only the root object can do this.
            if (this.transform.parent == null)
                DontDestroyOnLoad(this.gameObject);
            else
            {
                JCS_Debug.LogWarning("Only the root object can be use DontDestoryOnLoad");
            }
        }

        /// <summary>
        /// Check the signleton by type.
        ///
        /// NOTE(jenchieh): Non-use.
        /// </summary>
        private void CheckByType()
        {
            //switch (mType)
            //{
            //    case JCS_SingletonObject.JCS_MANAGERS:
            //        {
            //            JCS_Managers[] jc = GameObject.FindObjectsOfType<JCS_Managers>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.LogError("Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }
            //        }
            //        break;

            //    case JCS_SingletonObject.JCS_CANVAS:
            //        {
            //            JCS_Canvas[] jc = GameObject.FindObjectsOfType<JCS_Canvas>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.LogError("Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }

            //        }
            //        break;

            //    case JCS_SingletonObject.JCS_GAMESETTINGS:
            //        {
            //            JCS_GameSettings[] jc = GameObject.FindObjectsOfType<JCS_GameSettings>();

            //            if (jc.Length != 1)
            //            {
            //                JCS_Debug.LogError("Too many " + mType.ToString() + " in the scene...(Delete)");
            //                DestroyImmediate(this.gameObject);
            //                return;
            //            }

            //        } break;
            //}

        }

    }
}
