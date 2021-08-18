/**
 * $File: JCS_AbilityFormat.cs $
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
    /// Ability format.
    /// </summary>
    public abstract class JCS_AbilityFormat : MonoBehaviour
    {
        /// <summary>
        /// return minimal damage form the ability
        /// </summary>
        /// <returns> actual value </returns>
        public abstract int GetMinDamage();
        /// <summary>
        /// return maximum damage form the ability
        /// </summary>
        /// <returns> actual value </returns>
        public abstract int GetMaxDamage();

        /// <summary>
        /// Will return damage itself!
        /// Normally the Min Damage ~ Max Damage.
        /// </summary>
        /// <returns>  </returns>
        public abstract int GetAbsoluteDamage();

        /// <summary>
        /// Possiblity apply critical effect.
        /// </summary>
        /// <returns></returns>
        public abstract int GetCriticalChance();

        /// <summary>
        /// Attack value.
        /// </summary>
        /// <returns></returns>
        public abstract int GetAttackValue();

        /// <summary>
        /// Defense value.
        /// </summary>
        /// <returns></returns>
        public abstract int GetDefenseValue();
    }
}
