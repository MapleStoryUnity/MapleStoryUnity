/**
 * $File: JCS_NetworkManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Handle the networking of the game.
    /// 
    /// NOTE(jenchieh): To use this so the game could possibly become 
    /// the MO/MMO game.
    /// </summary>
    public class JCS_NetworkManager : JCS_Manager<JCS_NetworkManager>
    {
        /* Variables */

        // Server close flag.
        public static bool SERVER_CLOSE = false;

        // First login flag?
        public static bool FIRST_LOGIN = true;

        private float mConnectionCounter = 0.0f;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (!JCS_NetworkSettings.instance.ONLINE_MODE)
                return;

#if (UNITY_EDITOR)
            Test();
#endif

            if (JCS_ApplicationManager.APP_PAUSE)
                return;

            CheckConnectionWithTime();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.P))
            {
                byte[] bytesSend = new byte[5];
                bytesSend[0] = (byte)'H';
                bytesSend[1] = (byte)'E';
                bytesSend[2] = (byte)'l';
                bytesSend[3] = (byte)'L';
                bytesSend[4] = (byte)'o';
                JCS_NetworkSettings.GetGameSocket().SendPacket(bytesSend);
            }

            if (JCS_Input.GetKeyDown(KeyCode.O))
            {
                byte[] bytesSend = new byte[10];
                bytesSend[0] = (byte)'Z';
                bytesSend[1] = (byte)'X';
                bytesSend[2] = (byte)'C';
                bytesSend[3] = (byte)'V';
                bytesSend[4] = (byte)'B';
                bytesSend[5] = (byte)'N';
                bytesSend[6] = (byte)'M';
                bytesSend[7] = (byte)',';
                bytesSend[8] = (byte)'.';
                bytesSend[9] = (byte)'/';
                JCS_NetworkSettings.GetGameSocket().SendPacket(bytesSend);
            }

        }
#endif

        /// <summary>
        /// Spawn the security key for the packet.
        /// 
        /// NOTE(jenchieh): This is the most basic key, will recommand to 
        /// switch something more secure.
        /// </summary>
        /// <returns></returns>
        public static bool CreateKey()
        {
            // Create keys
            JCS_NetworkConstant.DECODE_BUFFER[0] = (byte)'J';
            JCS_NetworkConstant.DECODE_BUFFER[1] = (byte)'C';
            JCS_NetworkConstant.DECODE_BUFFER[2] = (byte)'S';
            JCS_NetworkConstant.DECODE_BUFFER[3] = (byte)'D';

            JCS_NetworkConstant.ENCODE_BUFFER[0] = (byte)'J';
            JCS_NetworkConstant.ENCODE_BUFFER[1] = (byte)'C';
            JCS_NetworkConstant.ENCODE_BUFFER[2] = (byte)'S';
            JCS_NetworkConstant.ENCODE_BUFFER[3] = (byte)'E';

            return true;
        }

        /// <summary>
        /// Do ping pong packet action.
        /// </summary>
        /// <returns>true : connect, false: disconnect</returns>
        public void CheckConnectionWithTime()
        {
            // do the following script only when is online mode
            if (!JCS_NetworkSettings.instance.ONLINE_MODE)
                return;

            mConnectionCounter += Time.deltaTime;

            if (mConnectionCounter < JCS_NetworkConstant.CONNECT_TIME)
                return;
            else
            {
                if (SERVER_CLOSE)
                {
                    Debug.Log("Server End!");
                    JCS_UtilityFunctions.PopIsConnectDialogue();
                }
                else if (FIRST_LOGIN)
                {
                    FIRST_LOGIN = false;
                    JCS_PatchManager.instance.LoadNextLevel();
                }
                mConnectionCounter = 0;
            }
        }
    }
}
