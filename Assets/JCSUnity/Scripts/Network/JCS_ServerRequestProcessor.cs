/**
 * $File: JCS_ServerRequestProcessor.cs $
 * $Date: 2017-08-24 17:08:45 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Process the all server request as callback.
    /// </summary>
    public class JCS_ServerRequestProcessor
        : JCS_Settings<JCS_ServerRequestProcessor>
    {
        public delegate void ServerRequest(JCS_BinaryReader br, JCS_Client client);

        /* Variables */

        private List<ServerRequest> mServerRequest = new List<ServerRequest>();
        private List<JCS_Client> mClient = new List<JCS_Client>();
        private List<JCS_BinaryReader> mBinaryReader = new List<JCS_BinaryReader>();

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
        }

        private void Update()
        {
            ProcessServerRequest();
        }

        /// <summary>
        /// Register request from server.
        /// </summary>
        public void RegisterRequest(ServerRequest request, JCS_BinaryReader br, JCS_Client client)
        {
            mServerRequest.Add(request);
            mBinaryReader.Add(br);
            mClient.Add(client);
        }

        /// <summary>
        /// Deregister the request from server.
        /// </summary>
        public void DeresgisterRequest(ServerRequest request, JCS_BinaryReader br, JCS_Client client)
        {
            mServerRequest.Remove(request);
            mBinaryReader.Remove(br);
            mClient.Remove(client);
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
        protected override void TransferData(JCS_ServerRequestProcessor _old, JCS_ServerRequestProcessor _new)
        {
            // before transfer, do all the packet request.
            ProcessServerRequest();
        }

        /// <summary>
        /// Process through the server request.
        /// </summary>
        private void ProcessServerRequest()
        {

            for (int index = 0; index < mServerRequest.Count; ++index)
            {
                if (index >= mServerRequest.Count ||
                    index >= mClient.Count ||
                    index >= mBinaryReader.Count)
                    continue;

                JCS_BinaryReader br = mBinaryReader[index];
                JCS_Client client = mClient[index];

                try
                {
                    // handle packet.
                    mServerRequest[index].Invoke(br, client);
                }
                catch (System.Exception e)
                {
                    JCS_Debug.LogError("Packet Handle Error : " + e.ToString());
                }
            }

            // done all request, wait for next frame's request.
            mServerRequest.Clear();
            mClient.Clear();
            mBinaryReader.Clear();
        }
    }
}
