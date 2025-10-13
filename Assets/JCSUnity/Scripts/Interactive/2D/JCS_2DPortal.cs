/**
 * $File: JCS_2DPortal.cs $
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
    /// 2d portal object.
    /// </summary>
    public class JCS_2DPortal : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DPortal)")]

        [Tooltip("Is this portal enable to use?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Use a key to trigger.")]
        [SerializeField]
        private KeyCode mKeyToTrigger = KeyCode.UpArrow;

        [Tooltip("If on trigger enter will trigger immediately.")]
        [SerializeField]
        private bool mAutoTrigger = false;

        [Tooltip("Type of the portal.")]
        [SerializeField]
        private JCS_2DPortalType mPortalType = JCS_2DPortalType.SCENE_PORTAL;

        [Header("Scene Portal")]

        [Tooltip("Name of the scene.")]
        [SerializeField]
        [Scene]
        private string mSceneName = "";

        [Tooltip("Label of the portal.")]
        [SerializeField]
        private JCS_PortalLabel mPortalLabel = JCS_PortalLabel.NONE;

        [Header("Transfer Portal")]

        [Tooltip("Pair portal, target portal the player will transfer to.")]
        [SerializeField]
        private Transform mTargetPortal = null;

        /* Setter & Getter */

        public bool active { get { return mActive; } }
        public bool autoTrigger { get { return mAutoTrigger; } set { mAutoTrigger = value; } }
        public JCS_2DPortalType portalType { get { return mPortalType; } set { mPortalType = value; } }
        public string sceneName { get { return mSceneName; } set { mSceneName = value; } }
        public Transform targetPortal { get { return mTargetPortal; } set { mTargetPortal = value; } }
        public JCS_PortalLabel portalLabel { get { return mPortalLabel; } }

        /* Functions */

        private void Awake()
        {
            if (mPortalType == JCS_2DPortalType.TRANSFER_PORTAL && mTargetPortal == null)
                Debug.LogError("Transform portal does not exists.");
        }

        private void OnTriggerStay(Collider other)
        {
            JCS_Player p = other.GetComponent<JCS_Player>();
            if (p == null)
                return;

            DoPortal(mPortalType, p);
        }

        /// <summary>
        /// Do the portal by portal type.
        /// </summary>
        /// <param name="player"> Player take effect by the portal. </param>
        public void DoPortal(JCS_Player player)
        {
            DoPortal(mPortalType, player);
        }

        /// <summary>
        /// Do the portal by portal type.
        /// </summary>
        /// <param name="type"> Type of the portal. </param>
        /// <param name="player"> Player take effect by the portal. </param>
        public void DoPortal(JCS_2DPortalType type, JCS_Player player)
        {
            switch (type)
            {
                case JCS_2DPortalType.SCENE_PORTAL:
                    {
                        if (!mAutoTrigger)
                        {
                            if (JCS_Input.GetKey(mKeyToTrigger))
                                LoadScene();
                        }
                        else
                        {
                            LoadScene();
                        }
                    }
                    break;
                case JCS_2DPortalType.TRANSFER_PORTAL:
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

        private void LoadScene()
        {
            var ps = JCS_PortalSettings.FirstInstance();

            // set the portal label, in order to let the next scene load.
            if (ps != null)
            {
                ps.sceneLabel = mPortalLabel;
            }

            // auto do the action
            JCS_SceneManager.FirstInstance().LoadScene(mSceneName);
        }
    }
}
