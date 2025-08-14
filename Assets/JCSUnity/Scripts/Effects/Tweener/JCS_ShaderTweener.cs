/**
 * $File: JCS_ShaderTweener.cs $
 * $Date: 2024-10-21 20:45:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2024 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Tweener that tween shader's properties.
    /// </summary>
    [RequireComponent(typeof(JCS_FloatTweener))]
    public class JCS_ShaderTweener : JCS_UnityObject
    {
        /* Variables */

        private JCS_FloatTweener mFloatTweener = null;

        [Separator("Runtime Variables (JCS_ShaderTweener)")]

        [Tooltip("Target shader's properties.")]
        [SerializeField]
        private List<string> mShaderProps = null;

        /* Setter & Getter */

        public JCS_FloatTweener floatTweener { get { return mFloatTweener; } }
        public List<string> shaderProps { get { return mShaderProps; } set { mShaderProps = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mFloatTweener = GetComponent<JCS_FloatTweener>();

            mFloatTweener.onValueChange = SetValue;
            mFloatTweener.onValueReturn = GetValue;
        }

        /// <summary>
        /// Update the target shader's properties.
        /// </summary>
        /// <param name="val"> The new value to assign with. </param>
        private void SetValue(float val)
        {
            foreach (string prop in mShaderProps)
            {
                localMaterial.SetFloat(prop, val);
            }
        }

        /// <summary>
        /// Return the shader's properties value.
        /// </summary>
        private float GetValue()
        {
            foreach (string prop in mShaderProps)
            {
                return localMaterial.GetFloat(prop);
            }

            return 0.0f;
        }
    }
}
