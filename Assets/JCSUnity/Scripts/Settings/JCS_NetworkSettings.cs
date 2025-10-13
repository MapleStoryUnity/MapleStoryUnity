/**
 * $File: JCS_NetworkSettings.cs $
 * $Date: 2017-08-20 07:08:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Store all the network settings.
    /// </summary>
    [RequireComponent(typeof(JCS_ServerRequestProcessor))]
    [RequireComponent(typeof(JCS_PacketLostPreventer))]
    public class JCS_NetworkSettings : JCS_Settings<JCS_NetworkSettings>
    {
        /* Variables */

        private static JCS_Socket SOCKET = null;
        private static JCS_ClientHandler PRESET_CLIENT_HANDLER = null;

        [Separator("Check Variables (JCS_NetworkSettings)")]

        [Tooltip(@"Current mode this client in, should be update by the server!")]
        [ReadOnly]
        public JCS_ClientMode clientMode = JCS_ClientMode.LOGIN_SERVER;

        [Tooltip("On switching the server?")]
        [ReadOnly]
        public bool switchingServer = false;

        [Tooltip("Flag to check if is force switching the server.")]
        [ReadOnly]
        public bool forceSwitchServer = false;

        [Separator("Runtime Variables (JCS_NetworkSettings)")]

        [Tooltip("Is the current game with online mode active?")]
        public bool onlineMode = false;

        [Tooltip("Type of the client protocal.")]
        public JCS_ProtocalType protocolType = JCS_ProtocalType.TCP;

        [Tooltip("Client hostname.")]
        public string host = "127.0.0.1";

        [Tooltip("Client port.")]
        public int port = 5454;

        [Tooltip("Channel count in this game.")]
        public int channelCount = 1;

        private JCS_ServerRequestProcessor mServerRequestProcessor = null;
        private JCS_PacketLostPreventer mPacketLostPreventer = null;

        /* Setter & Getter */

        public static void PresetClientHandler(JCS_ClientHandler handler) { PRESET_CLIENT_HANDLER = handler; }
        public static JCS_ClientHandler GetPresetClientHandler() { return PRESET_CLIENT_HANDLER; }
        public JCS_ServerRequestProcessor GetServerRequestProcessor() { return mServerRequestProcessor; }
        public JCS_PacketLostPreventer GetPacketLostPreventer() { return mPacketLostPreventer; }

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            mServerRequestProcessor = GetComponent<JCS_ServerRequestProcessor>();
            mPacketLostPreventer = GetComponent<JCS_PacketLostPreventer>();
        }

        private void OnApplicationQuit()
        {
            if (onlineMode)
                CloseSocket();
        }

        private void Update()
        {
            OnSwitchServer();
        }

        /// <summary>
        /// Create the socket and connect to the host and 
        /// port provided.
        /// </summary>
        /// <param name="hostname"> host name </param>
        /// <param name="port"> port number </param>
        /// <returns> Sucess or vice versa. </returns>
        public static bool CreateNetwork(string hostname, int port)
        {
            return CreateNetwork(hostname, port, GetPresetClientHandler());
        }

        /// <summary>
        /// Create the socket and connect to the host and 
        /// port provided.
        /// </summary>
        /// <param name="hostname"> host name </param>
        /// <param name="port"> port number </param>
        /// <param name="handler"></param>
        /// <returns> Sucess or vice versa. </returns>
        public static bool CreateNetwork(string hostname, int port, JCS_ClientHandler handler)
        {
            if (SOCKET != null)
                return false;

            switch (FirstInstance().protocolType)
            {
                case JCS_ProtocalType.TCP:
                    {
                        SOCKET = new JCS_TCPSocket(handler);
                        SOCKET.Connect(hostname, port);
                    }
                    break;
                case JCS_ProtocalType.UDP:
                    {
                        SOCKET = new JCS_UDPSocket(handler);
                        SOCKET.Connect(hostname, port);
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Close the current socket the safe way.
        /// </summary>
        public static void CloseSocket()
        {
            if (SOCKET == null)
                return;

            SOCKET.Close();
            SOCKET = null;
        }

        /// <summary>
        /// Return the socket we are using.
        /// </summary>
        /// <returns> socket. </returns>
        public static JCS_Socket GetSocket()
        {
            return SOCKET;
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        public void SwitchServer()
        {
            SwitchServer(host, port);
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        public void SwitchServer(
            string hostname,
            int port)
        {
            SwitchServer(
                hostname,
                port,
                false,
                GetPresetClientHandler());
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        /// <param name="force"> force to switch server? (Default : false) </param>
        public void SwitchServer(
            string hostname,
            int port,
            bool force)
        {
            SwitchServer(
                hostname,
                port,
                force,
                GetPresetClientHandler());
        }

        /// <summary>
        /// Do this when transfering to different server.
        /// 
        /// For instance:
        /// Login Server -> Channel Server
        /// </summary>
        /// <param name="hostname"> Host name </param>
        /// <param name="port"> Port Number </param>
        /// <param name="force"> force to switch server? (Default : false) </param>
        /// <param name="handler"> handler handle packet income. </param>
        public void SwitchServer(
            string hostname,
            int port,
            bool force,
            JCS_ClientHandler handler)
        {
            if (host == hostname &&
                this.port == port &&
                handler == null)
            {
                Debug.LogError(
                    "Not need to switch server, we already in....");
                return;
            }

            // update hostname, port, and handler.
            host = hostname;
            this.port = port;

            if (handler != null)
                PresetClientHandler(handler);

            // start switching server.
            switchingServer = true;

            forceSwitchServer = force;
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_NetworkSettings _old, JCS_NetworkSettings _new)
        {
            _new.onlineMode = _old.onlineMode;
            _new.host = _old.host;
            _new.port = _old.port;
            _new.protocolType = _old.protocolType;
            _new.channelCount = _old.channelCount;

            _new.switchingServer = _old.switchingServer;
            _new.forceSwitchServer = _old.forceSwitchServer;

            _new.clientMode = _old.clientMode;
        }

        /// <summary>
        /// On switching the server.
        /// </summary>
        private void OnSwitchServer()
        {
            if (!switchingServer)
                return;

            var plp = JCS_PacketLostPreventer.FirstInstance();

            // If not force, then we do need to check if we meet the 
            // requirement to swtich server.
            if (!forceSwitchServer)
            {
                // Cannot switch server if we are still waiting for packet 
                // to process.
                if (plp.IsPreventing())
                    return;
            }
            else
            {
                // If force to switch the server/service. Meaning we 
                // will like to terminate the server request!
                // 
                // ATTENTION(jenchieh): Use this carefully.
                plp.ClearTracking();
            }

            // close the previous one.
            CloseSocket();

            // open the new one for next server.
            CreateNetwork(FirstInstance().host, FirstInstance().port);

            switchingServer = false;
            forceSwitchServer = false;
        }
    }
}
