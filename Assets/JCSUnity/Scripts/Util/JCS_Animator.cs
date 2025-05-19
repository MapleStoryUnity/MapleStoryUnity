/**
 * $File: JCS_Animator.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Animator utilities.
    /// </summary>
    public static class JCS_Animator
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Sets the weight of the layer at the given name.
        /// 
        /// Similar to `Animationr.SetLayerWeight` but accept string name.
        /// </summary>
        public static void SetLayerWeight(Animator ator, string name, float val)
        {
            int index = ator.GetLayerIndex(name);

            SetLayerWeight(ator, index, val);
        }
        // For compatibility.
        public static void SetLayerWeight(Animator ator, int index, float val)
        {
            ator.SetLayerWeight(index, val);
        }

        /// <summary>
        /// Returns the weight of the layer at the specified name.
        /// 
        /// Similar to `Animationr.SetLayerWeight` but accept string name.
        /// </summary>
        public static float GetLayerWeight(Animator ator, string name)
        {
            int index = ator.GetLayerIndex(name);

            return GetLayerWeight(ator, index);
        }
        // For compatibility.
        public static float GetLayerWeight(Animator ator, int index)
        {
            return ator.GetLayerWeight(index);
        }

        /// <summary>
        /// Like `Animator.HasState` but make second paramter 
        /// accepts the string.
        /// </summary>
        public static bool HasState(Animator animator, int layer, string name)
        {
            int stateHash = Animator.StringToHash(name);
            return animator.HasState(layer, stateHash);
        }
    }
}
