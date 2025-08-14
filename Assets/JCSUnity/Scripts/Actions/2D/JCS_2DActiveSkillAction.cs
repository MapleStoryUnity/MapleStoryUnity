/**
 * $File: JCS_2DActiveSkillAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Spawn a skill and destroy itself after
    /// done displaying the skill effect.
    /// </summary>
    public class JCS_2DActiveSkillAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DActiveSkillAction)")]

        [Tooltip("Key to active this skill.")]
        [SerializeField]
        private KeyCode mKeyCode = KeyCode.None;

        [Tooltip("Animation displayed order layer.")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("Animation controller to use to display animation.")]
        [SerializeField]
        private RuntimeAnimatorController mSkillAnim = null;

        [Tooltip("Stick to the parent game object.")]
        [SerializeField]
        private bool mStayWithActiveTarget = true;

        [Header("- Spawn")]

        [Tooltip("Spawns in the same position.")]
        [SerializeField]
        private bool mSamePosition = true;

        [Tooltip("Spawns in the same rotation.")]
        [SerializeField]
        private bool mSameRotation = true;

        /* Setter & Getter */

        public KeyCode keyCode { get { return mKeyCode; } set { mKeyCode = value; } }
        public int orderLayer { get { return mOrderLayer; } set { mOrderLayer = value; } }
        public RuntimeAnimatorController skillAnim { get { return mSkillAnim; } set { mSkillAnim = value; } }
        public bool stayWithActiveTarget { get { return mSameRotation; } set { mStayWithActiveTarget = value; } }
        public bool samePosition { get { return mSamePosition; } set { mSamePosition = value; } }
        public bool sameRotation { get { return mSameRotation; } set { mSameRotation = value; } }

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
                Debug.Log(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Util.SpawnAnimateObject(mSkillAnim, mOrderLayer);

            if (mSamePosition)
                obj.transform.position = transform.position;
            if (mSameRotation)
                obj.transform.rotation = transform.rotation;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(transform);

            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }
    }
}
