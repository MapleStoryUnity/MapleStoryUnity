/**
 * $File: JCS_TweenInfo.cs $
 * $Date: 2020-04-06 14:37:33 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Tweener information in order to do the tweener effect.
    /// 
    /// Contain these informations.
    ///   -> startingValue
    ///   -> transformTweener
    ///   -> targetValue
    /// </summary>
    [System.Serializable]
    public class JCS_TweenInfo
    {
        [Separator("📋 Check Variabless (JCS_TweenInfo)")]

        [Tooltip("Record down the starting value, in order to go back.")]
        public Vector3 startingValue = Vector3.zero;

        [Separator("⚡️ Runtime Variables (JCS_TweenInfo)")]

        [Tooltip("Transform tweener we want to use.")]
        public JCS_TransformTweener transformTweener = null;

        [Tooltip("Do the tween effect to this value.")]
        public Vector3 targetValue = Vector3.zero;
    }
}
