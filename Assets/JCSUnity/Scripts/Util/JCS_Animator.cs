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
        /// Return true if the parameter exists.
        /// </summary>
        public static bool HasParameter(Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if the layer exists in the aniamtor.
        /// </summary>
        public static bool HasLayer(Animator animator, string name)
        {
            int index = animator.GetLayerIndex(name);

            return HasLayer(animator, index);
        }
        public static bool HasLayer(Animator animator, int index)
        {
            if (index < 0 || animator.layerCount <= index)
                return false;

            return true;
        }

        /// <summary>
        /// Like `Animator.HasState` but make second paramter 
        /// accepts the string.
        /// </summary>
        public static bool HasState(Animator animator, int layer, int name)
        {
            // First check if the layer exists.
            if (!HasLayer(animator, layer))
                return false;

            // Then do the check.
            return animator.HasState(layer, name);
        }
        public static bool HasState(Animator animator, int layer, string name)
        {
            // First check if the layer exists.
            if (!HasLayer(animator, layer))
                return false;

            // Convert name to hash.
            int stateHash = Animator.StringToHash(name);

            // Then do the check.
            return animator.HasState(layer, stateHash);
        }

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
    }
}
