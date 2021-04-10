/**
 * $File: JCS_PatchManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JCSUnity
{
    /// <summary>
    /// Handle the patch for updating the game version, etc.
    /// 
    /// Using the version number provided in the game data system could 
    /// update the game patch by just checking the VERSION number.
    /// </summary>
    public class JCS_PatchManager 
        : JCS_Managers<JCS_PatchManager>
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_PatchManager) **")]

        [Tooltip("Next level load after the patch checked.")]
        [SerializeField]
        private string mNextLevel = "JCS_LogoScene";

        [Tooltip("What do the server time out?")]
        [SerializeField] [Range(0, 600)]        // 0 sec ~ 600 sec
        private float mConnectTimeOut = 30;

        // timer for connection.
        private float mConnectTimer = 0;

        /* Setter & Getter */

        public string NextLevel() { return this.mNextLevel; }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (!JCS_NetworkSettings.instance.ONLINE_MODE)
            {
                if (JCS_NetworkManager.FIRST_LOGIN)
                {
                    JCS_NetworkManager.FIRST_LOGIN = false;
                    LoadNextLevel();
                }

                return;
            }
        }

        private void Update()
        {
            // if is online game
            if (JCS_Startup.InitializeApplication())
            {
                Debug.Log("Server Connected!");

                LoadNextLevel();
            }

            DoTimeOut();
        }

        /// <summary>
        /// Load the next level.
        /// </summary>
        public void LoadNextLevel()
        {
            JCS_SceneManager.instance.LoadScene(mNextLevel);
        }

        /// <summary>
        /// Do the connection time out.
        /// </summary>
        private void DoTimeOut()
        {
            // add up timer.
            mConnectTimer += Time.deltaTime;

            // check if connection time out?
            if (mConnectTimer < mConnectTimeOut)
                return;

            // reset timer.
            mConnectTimer = 0;

            Debug.Log("Server Connection Time Out, Quit Application.");

            // quit the application smoothly.
            JCS_ApplicationManager.instance.Quit(true);
        }
    }
}
