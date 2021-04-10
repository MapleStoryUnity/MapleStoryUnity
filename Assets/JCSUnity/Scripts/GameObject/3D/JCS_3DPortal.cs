/**
 * $File: JCS_3DPortal.cs $
 * $Date: 2016-11-06 17:25:43 $
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
    /// 3D portal object.
    /// </summary>
    public class JCS_3DPortal
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_3DPortal) **")]

        [Tooltip("Key to trigger this portal.")]
        [SerializeField]
        private KeyCode mKeyToTrigger = KeyCode.UpArrow;

        [Tooltip("Active portal when on trigger event.")]
        [SerializeField]
        private bool mAutoTrigger = false;

        [Tooltip("Type of the portal.")]
        [SerializeField]
        private JCS_3DPortalType mType = JCS_3DPortalType.SCENE_PORTAL;

        [Header("- Scene Portal")]

        [Tooltip("Scene name when portal load this scene.")]
        [SerializeField]
        private string mSceneName = "JCS_Demo";

        [Header("- Transfer Portal")]

        [Tooltip("Position when the portal moves the player's position.")]
        [SerializeField]
        private Transform mTargetPortal = null;

        /* Setter & Getter */

        public bool AutoTrigger { get { return this.mAutoTrigger; } set { this.mAutoTrigger = value; } }
        public JCS_3DPortalType Type { get { return this.mType; } set { this.mType = value; } }
        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public Transform TargetPortal { get { return this.mTargetPortal; } set { this.mTargetPortal = value; } }

        /* Functions */

        private void Awake()
        {
            if (mType == JCS_3DPortalType.TRANSFER_PORTAL && mTargetPortal == null)
                JCS_Debug.LogError("Transform portal does not exists.");
        }

        private void OnTriggerStay(Collider other)
        {
            JCS_Player p = other.GetComponent<JCS_Player>();
            if (p == null)
                return;

            // switch the scene.
            DoPortal(mType, p);
        }

        /// <summary>
        /// Do the portal.
        /// </summary>
        /// <param name="type"> Type of the portal. </param>
        /// <param name="player"> Player that do the portal event. </param>
        public void DoPortal(JCS_3DPortalType type, JCS_Player player)
        {
            switch (type)
            {
                case JCS_3DPortalType.SCENE_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKey(mKeyToTrigger))
                                JCS_SceneManager.instance.LoadScene(mSceneName);
                        }
                        else
                        {
                            // auto do the action
                            JCS_SceneManager.instance.LoadScene(mSceneName);
                        }
                    }
                    break;
                case JCS_3DPortalType.TRANSFER_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKeyDown(mKeyToTrigger))
                            {
                                if (mTargetPortal != null)
                                    player.transform.position = mTargetPortal.position;
                            }
                        }
                        else
                        {
                            // auto do the action
                            if (mTargetPortal != null)
                                player.transform.position = mTargetPortal.position;
                        }
                    }
                    break;
            }
        }
    }
}
