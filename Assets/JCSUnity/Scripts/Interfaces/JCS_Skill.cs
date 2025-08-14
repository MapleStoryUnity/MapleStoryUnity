/**
 * $File: JCS_Skill.cs $
 * $Date: 2016-12-23 00:08:32 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all the skill. The base function will have to
    /// code it yourself.
    /// </summary>
    public class JCS_Skill : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Skill)")]

        [Tooltip("Sound when the skill active.")]
        [SerializeField]
        protected AudioClip mSkillSound = null;

        /* Setter & Getter */

        public AudioClip skillSound { get { return mSkillSound; } set { mSkillSound = value; } }

        /* Functions */

    }
}
