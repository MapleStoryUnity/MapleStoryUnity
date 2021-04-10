/**
 * $File: JCS_Skill.cs $
 * $Date: 2016-12-23 00:08:32 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all the skill. The base function will have to
    /// code it yourself.
    /// </summary>
    public class JCS_Skill
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_Skill) **")]

        [Tooltip("Sound when the skill active.")]
        [SerializeField]
        protected AudioClip mSkillSound = null;

        /* Setter & Getter */

        public AudioClip SkillSound { get { return this.mSkillSound; } set { this.mSkillSound = value; } }

        /* Functions */

    }
}
