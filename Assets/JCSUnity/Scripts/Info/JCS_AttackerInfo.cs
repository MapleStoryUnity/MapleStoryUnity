/**
 * $File: JCS_AttackerInfo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Object that need to know who is the ppl cast, use
    /// attack, spell the thing.
    /// 
    /// Ex: Star need to contain the attacker info.
    /// </summary>
    public class JCS_AttackerInfo : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_AttackerInfo)")]

        [Tooltip("Who is the attacker of this object(weapon)?")]
        [SerializeField]
        private Transform mAttacker = null;

        /* Setter & Getter */

        /// <summary>
        /// * If the attacker is throwing the star, 
        /// Attacker can be the one who throw the star.
        /// 
        /// * If the attacker using the blade. Weapon itself
        /// could be the attacker.
        /// 
        /// etc.
        /// </summary>
        public Transform Attacker { get { return this.mAttacker; } set { this.mAttacker = value; } }

        /* Functions */

    }
}
