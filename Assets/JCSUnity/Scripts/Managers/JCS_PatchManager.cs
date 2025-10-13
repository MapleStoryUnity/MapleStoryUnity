/**
 * $File: JCS_PatchManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle the patch for updating the game version, etc.
    /// 
    /// Using the version number provided in the game data system could 
    /// update the game patch by just checking the VERSION number.
    /// </summary>
    public class JCS_PatchManager : JCS_Manager<JCS_PatchManager>
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_PatchManager)")]

        [Tooltip("Next level load after the patch checked.")]
        [SerializeField]
        private string mNextScene = "JCS_LogoScene";

        [Tooltip("What do the server time out?")]
        [SerializeField]
        [Range(0, 600)]        // 0 sec ~ 600 sec
        private float mConnectTimeOut = 30;

        // timer for connection.
        private float mConnectTimer = 0;

        /* Setter & Getter */

        public string nextScene { get { return mNextScene; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);
        }

        private void Start()
        {
            if (!JCS_NetworkSettings.FirstInstance().onlineMode)
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
            if (JCS_Startup.InitApp())
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
            JCS_SceneManager.FirstInstance().LoadScene(mNextScene);
        }

        /// <summary>
        /// Do the connection time out.
        /// </summary>
        private void DoTimeOut()
        {
            // add up timer.
            mConnectTimer += Time.unscaledDeltaTime;

            // check if connection time out?
            if (mConnectTimer < mConnectTimeOut)
                return;

            // reset timer.
            mConnectTimer = 0;

            Debug.Log("Server Connection Time Out, Quit Application.");

            // quit the application smoothly.
            JCS_AppManager.FirstInstance().Quit(true);
        }
    }
}
