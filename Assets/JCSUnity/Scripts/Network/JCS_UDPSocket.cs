/**
 * $File: JCS_UDPSocket.cs $
 * $Date: 2017-08-30 18:20:10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.Net;                                     // Endpoint
using System.Net.Sockets;                             // Socket namespace
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// UDP Socket.
    /// </summary>
    public class JCS_UDPSocket : JCS_Socket
    {
        /* Variables */

        private Socket mSocket = null;  // Server connection

        private byte[] mInputBuff = new byte[JCS_NetworkConstant.INBUFSIZE];    // Recieved data buffer
        private byte[] mOutputBuff = new byte[JCS_NetworkConstant.OUTBUFSIZE];

        private JCS_ClientHandler mClientHandler = null;

        /* Setter & Getter */

        public byte[] GetInputBuffer() { return this.mInputBuff; }
        public byte[] GetOutPutBuffer() { return this.mOutputBuff; }

        /// <summary>
        /// Set the client handler.
        /// </summary>
        /// <param name="handler"> handler. </param>
        public void SetHandler(JCS_ClientHandler handler)
        {
            this.mClientHandler = handler;
        }

        /* Functions */

        public JCS_UDPSocket(JCS_ClientHandler handler = null)
        {
            if (handler != null)
                SetHandler(handler);
            else
                SetHandler(new JCS_DefaultClientHandler());
        }

        /// <summary>
        /// Try to connect once to the setting server.
        /// </summary>
        /// <param name="hostname"> Internet Protocal </param>
        /// <param name="port"> Port Number </param>
        public void Connect(string hostname, int port)
        {
            try
            {
                // Close the socket if it is still open
                if (mSocket != null)
                {
                    System.Threading.Thread.Sleep(10);
                    Close();
                }

                // Create the socket object
                mSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                // Define the Server address and port
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(hostname), port);

                // Connect to server non-Blocking method
                mSocket.Blocking = false;
                AsyncCallback onconnect = new AsyncCallback(OnConnect);

                mSocket.BeginConnect(epServer, onconnect, mSocket);

                // set receive callback
                SetupRecieveCallback(mSocket);
            }
            catch (Exception ex)
            {
                Debug.LogError("Server Connect failed: " + ex.Message);
                JCS_UtilFunctions.PopIsConnectDialogue();
            }
        }

        /// <summary>
        /// Call back when on connect.
        /// </summary>
        /// <param name="ar"> async state. </param>
        public void OnConnect(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we were sucessfull
            try
            {
                // Complete the connection. 
                sock.EndConnect(ar);

                if (sock.Connected)
                    SetupRecieveCallback(sock);
                else
                    Debug.LogError("Unable to connect to remote machine, Connected Failed");
            }
            catch (Exception ex)
            {
                Debug.LogError("Unusual error during Connect!: " + ex.Message);
                JCS_NetworkManager.SERVER_CLOSE = true;
            }
        }

        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        public void SetupRecieveCallback(Socket sock)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnReceiveData);
                // Begin receiving the data from the remote device.
                sock.BeginReceive(
                    mInputBuff,
                    0,
                    mInputBuff.Length,
                    SocketFlags.None,
                    recieveData,
                    sock);
            }
            catch (Exception ex)
            {
                Debug.LogError("Setup Recieve Callback failed!: " + ex.Message);
            }
        }

        /// <summary>
        /// When receive the data.
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceiveData(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                // Read data from the remote device.
                int bytesRead = sock.EndReceive(ar);

                if (bytesRead > 0)
                {
                    byte[] buffer = GetBytesFromInputBuffer(0, bytesRead);

                    // send buffer to decoder and get the decrypted packet
                    byte[] decryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetDecoder().Decode(buffer);

                    // invoke data to handler.
                    if (mClientHandler != null)
                        mClientHandler.MessageReceived(decryptedBuffer);
                }
                else
                {
                    // ..
                    Debug.Log("Receive buffer that length is lower than 0.");
                }
            }
            catch (Exception ex)
            {
                // If no data was recieved then the connection is probably dead
                Debug.LogError("Server OnReceive failed: " + ex.Message);
                Close();
            }

            SetupRecieveCallback(sock);
        }

        /// <summary>
        /// Close the socket the safe way.
        /// </summary>
        public void Close()
        {
            if (mSocket != null && mSocket.Connected)
            {
                mSocket.Shutdown(SocketShutdown.Both);
                mSocket.Close();
            }
        }

        /// <summary>
        /// Callback when the data has sent.
        /// </summary>
        /// <param name="ar"></param>
        public void OnSendData(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = sock.EndSend(ar);

                if (bytesSent > 0)
                {
                    // Set decode Charset!
                    byte[] buffer = GetBytesFromOutputBuffer(0, bytesSent);

                    // send buffer to decoder and get the decrypted packet
                    byte[] encryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetEncoder().Encode(buffer);

                    // invoke data to handler.
                    if (mClientHandler != null)
                        mClientHandler.MessageSent(encryptedBuffer);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    Console.WriteLine("Client {0}, disconnected");
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Unusual error druing Send!: " + ex.Message);

                if (!SocketConnected(mSocket))
                {
                    mSocket.Close();
                    JCS_NetworkManager.SERVER_CLOSE = true;
                }
            }
        }

        /// <summary>
        /// Send the packet to the other end.
        /// </summary>
        /// <param name="buffer"> buffer stream to send. </param>
        public bool SendPacket(byte[] buffer)
        {
            if (mSocket == null)
            {
                Debug.Log("Must be connected to Send a message");
                return false;
            }

            if (buffer == null)
                return false;

            byte[] encryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetEncoder().Encode(buffer);

            try
            {
                mSocket.BeginSend(
                    encryptedBuffer,
                    0,
                    encryptedBuffer.Length,
                    0,
                    new AsyncCallback(OnSendData),
                    mSocket);
            }
            catch (Exception ex)
            {
                Debug.Log("Send Message Failed!: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get byte array from the current input buffer memory space.
        /// </summary>
        /// <param name="start"> start index. </param>
        /// <param name="len"> len of the byte to read. </param>
        /// <returns></returns>
        public byte[] GetBytesFromInputBuffer(int start, int len)
        {
            return JCS_Util.CopyByteArray(mInputBuff, start, len);
        }

        /// <summary>
        /// Get byte array from the current output buffer memory space.
        /// </summary>
        /// <param name="start"> start index. </param>
        /// <param name="len"> len of the byte to read. </param>
        /// <returns></returns>
        public byte[] GetBytesFromOutputBuffer(int start, int len)
        {
            return JCS_Util.CopyByteArray(mOutputBuff, start, len);
        }

        /// <summary>
        /// Check if the socket already connected?
        /// </summary>
        /// <param name="s"> socket descriptor </param>
        /// <returns> boolean to check is connected? </returns>
        public bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check is connected to the server or not.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return mSocket.Connected;
        }
    }
}
