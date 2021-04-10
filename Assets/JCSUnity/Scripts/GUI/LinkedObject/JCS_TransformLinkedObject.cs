/**
 * $File: JCS_TransformLinkedObject.cs $
 * $Date: 2020-08-08 21:44:32 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Object that linked to another linked object.
    /// 
    /// Mainly use for UI item.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    public class JCS_TransformLinkedObject
        : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;

        /* Setter & Getter */

        public JCS_TransformTweener TransformTweener { get { return this.mTransformTweener; } }

        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
        }
    }
}
