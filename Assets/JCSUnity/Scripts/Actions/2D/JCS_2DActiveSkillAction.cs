/**
 * $File: JCS_2DActiveSkillAction.cs $
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
    /// Spawn a skill and destroy itself after
    /// done displaying the skill effect.
    /// </summary>
    public class JCS_2DActiveSkillAction
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_2DActiveSkillAction) **")]

        [Tooltip("Key to active this skill.")]
        [SerializeField]
        private KeyCode mKeyCode = KeyCode.None;

        [Tooltip("Animation displayed order layer.")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("Animation controller to use to display animation.")]
        [SerializeField]
        private RuntimeAnimatorController mSkillAnim = null;

        [Tooltip("Stick to the parent gameobject.")]
        [SerializeField]
        private bool mStayWithActiveTarget = true;

        [Header("** Spawn Settings (JCS_2DActiveSkillAction) **")]

        [Tooltip("Spawns in the same position.")]
        [SerializeField]
        private bool mSamePosition = true;

        [Tooltip("Spawns in the same rotation.")]
        [SerializeField]
        private bool mSameRotation = true;

        /* Setter & Getter */

        public KeyCode KeyCode { get { return this.mKeyCode; } set { this.mKeyCode = value; } }
        public int OrderLayer { get { return this.mOrderLayer; } set { this.mOrderLayer = value; } }
        public RuntimeAnimatorController SkillAnim { get { return this.mSkillAnim; } set { this.mSkillAnim = value; } }
        public bool StayWithActiveTarget { get { return this.mSameRotation; } set { this.mStayWithActiveTarget = value; } }
        public bool SamePosition { get { return this.mSamePosition; } set { this.mSamePosition = value; } }
        public bool SameRotation { get { return this.mSameRotation; } set { this.mSameRotation = value; } }

        /* Functions */

        private void Update()
        {
            if (JCS_Input.GetKeyDown(mKeyCode))
                ActiveSkill();
        }

        /// <summary>
        /// Active the skill.
        /// </summary>
        public void ActiveSkill()
        {
            if (mSkillAnim == null)
            {
                JCS_Debug.LogReminder(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Utility.SpawnAnimateObject(mSkillAnim, mOrderLayer);

            if (mSamePosition)
                obj.transform.position = this.transform.position;
            if (mSameRotation)
                obj.transform.rotation = this.transform.rotation;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(this.transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }
    }
}
