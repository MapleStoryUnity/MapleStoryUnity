/**
 * $File: JCS_UtilFunctions.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

// Starting Version 5.3.0 Unity use SceneManageement 
// instead of Application on load function call.
#if (UNITY_5_3 || UNITY_5_3_OR_NEWER)
using UnityEngine.SceneManagement;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Util Function put here, usually systematic.
    /// </summary>
    public class JCS_UtilFunctions : MonoBehaviour
    {
        private static string IS_CONNECT_DIALOGUE = "ForceDialogue/JCS_MessageBox_YesNo";
        private static string SETTING_PANEL = "InGameDialogue/Setting_Panel";
        private static string TALK_DIALOGUE = "InGameDialogue/Conversation_Dialogue";

        /// <summary>
        /// Pause the game.
        /// </summary>
        /// <param name="act"></param>
        public static void PauseGame(bool act = true)
        {
            JCS_GameManager.FirstInstance().gamePaused = act;
        }

        /// <summary>
        /// pause the app.
        /// </summary>
        /// <param name="act"></param>
        public static void PasueApplication(bool act = true)
        {
            JCS_AppManager.APP_PAUSE = act;
        }

        /// <summary>
        /// Close the application directly.
        /// </summary>
        public static void QuitApplication()
        {
#if UNITY_EDITOR
            SceneManager.LoadScene("JCS_AppCloseSimulate");
            JCS_AppManager.APP_PAUSE = true;
#endif

            Application.Quit();
        }

        /// <summary>
        /// This will switch the scene then close the application.
        /// </summary>
        public static void QuitApplicationWithSwithScene()
        {
            JCS_SceneManager.FirstInstance().LoadScene("JCS_AppCloseSimulate");
        }

        /// <summary>
        /// Destory the current selected dialogue.
        /// </summary>
        /// <param name="type"></param>
        public static void DestoryCurrentDialogue(JCS_DialogueType type)
        {
            if (JCS_GameManager.FirstInstance().gamePaused)
                return;

            JCS_UIManager.FirstInstance().HideTheLastOpenDialogue();

            switch (type)
            {
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                case JCS_DialogueType.NPC_DIALOGUE:
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    PauseGame(false);
                    break;
            }
        }

        /// <summary>
        /// Check if spawning the game ui is fine. If already exists 
        /// then is not fine.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckIfOkayToSpawnGameUI(JCS_DialogueType type)
        {
            if (JCS_UIManager.FirstInstance().GetJCSDialogue(type) != null)
            {
                Debug.LogError("(" + type.ToString() + ")No able to spawn Game UI cuz there are multiple GameUI in the scene...");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if enable to spawn the dialogue or not
        /// </summary>
        /// <returns></returns>
        private static bool CheckIfOkayToSpawnDialogue(JCS_DialogueType type)
        {
            // Force Diaglogue have higher priority, 
            // so it will block the lower priority dialogue type
            if (JCS_UIManager.FirstInstance().GetJCSDialogue(type) != null)
            {
                Debug.LogError("(" + type.ToString() + ")No able to spawn Dialogue cuz there are multiple dialogue in the scene...");
                return false;
            }

            if (JCS_Canvas.GuessCanvas() == null)
            {
                Debug.LogError("No able to spawn Dialogue cuz Canvas are null...");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Pop the JCS_BlackScreen object.
        /// </summary>
        public static void PopBlackScreen()
        {
            string path = JCS_UISettings.BLACK_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetBlackScreen(bs);
        }

        /// <summary>
        /// Pop the JCS_BlackSlideScreen object.
        /// </summary>
        public static void PopBlackSlideScreen()
        {
            string path = JCS_UISettings.BLACK_SLIDE_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackSlideScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetBlackSlideScreen(bs);
        }

        /// <summary>
        /// Spawn a white screen.
        /// </summary>
        public static void PopWhiteScreen()
        {
            string path = JCS_UISettings.WHITE_SCREEN_PATH;
            var ws = JCS_Util.Instantiate(path).GetComponent<JCS_WhiteScreen>();

            if (ws == null)
            {
                Debug.LogError("GameObject without `JCS_WhiteScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetWhiteScreen(ws);
        }

        //** Game UI (Game Layer)

        /// <summary>
        /// Spawn the game UI.
        /// </summary>
        public static void PopInGameUI()
        {
            if (!CheckIfOkayToSpawnGameUI(JCS_DialogueType.GAME_UI))
                return;

            //string path = JCS_GameSettings.FirstInstance().GAME_UI_PATH;
            //JCS_Util.SpawnGameObject(path);
        }

        //** (Application Layer)

        /// <summary>
        /// Spawn the connect dialgoue.
        /// </summary>
        public static void PopIsConnectDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.SYSTEM_DIALOGUE))
                return;

            JCS_Util.Instantiate(IS_CONNECT_DIALOGUE);

            PauseGame(true);
        }

        //** In Game Dialogue (Game Layer)

        /// <summary>
        /// Spawn the setting dialogue.
        /// </summary>
        public static void PopSettingDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_Util.Instantiate(SETTING_PANEL);

            //PauseGame(true);
        }

        /// <summary>
        /// Spawn the talke dialogue.
        /// </summary>
        public static void PopTalkDialogue()
        {
            if (!CheckIfOkayToSpawnDialogue(JCS_DialogueType.PLAYER_DIALOGUE))
                return;

            JCS_Util.Instantiate(TALK_DIALOGUE);

            PauseGame(true);
        }


    }
}
