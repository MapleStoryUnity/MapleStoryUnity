/**
* $File: JCS_TCPGameSocket.cs $
* $Date: 2017-08-21 16:38:46 $
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
    /// Socket Descriptor holder.
    /// </summary>
    public class JCS_TCPGameSocket : JCS_GameSocket
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

        public JCS_TCPGameSocket(JCS_ClientHandler handler = null)
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
                if (mSocket != null && mSocket.Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    mSocket.Close();
                }

                // Create the socket object
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
        /// Get the new data and send it out to all other connections. 
        /// Note: If not data was recieved the connection has probably 
        /// died.
        /// </summary>
        /// <param name="ar"></param>
        public void OnReceiveData(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            // Check if we got any data
            try
            {
                int nBytesRec = sock.EndReceive(ar);

                if (nBytesRec > 0)
                {
                    // Set decode Charset!
                    byte[] buffer = GetBytesFromInputBuffer(0, nBytesRec);

                    // send buffer to decoder and get the decrypted packet
                    byte[] decryptedBuffer = (byte[])JCS_CodecFactory.GetInstance().GetDecoder().Decode(buffer);

                    // invoke data to handler.
                    if (mClientHandler != null)
                        mClientHandler.MessageReceived(decryptedBuffer);

                    // WARNING : The following line is NOT thread safe. Invoke is
                    //m_lbRecievedData.Items.Add( sRecieved );
                    //Invoke(m_AddMessage, new string[] { sRecieved });

                    // If the connection is still usable restablish the callback
                    SetupRecieveCallback(sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    Console.WriteLine("Client {0}, disconnected", sock.RemoteEndPoint);
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Unusual error druing Recieve!: " + ex.Message);

                if (!SocketConnected(mSocket))
                {
                    mSocket.Close();
                    JCS_NetworkManager.SERVER_CLOSE = true;
                }
            }
        }

        /// <summary>
        /// Callback when the data has sent.
        /// </summary>
        /// <param name="ar"></param>
        public void OnSendData(IAsyncResult ar)
        {
            // Socket was the passed in object
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
                    Console.WriteLine("Client {0}, disconnected", sock.RemoteEndPoint);
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
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        public void SetupRecieveCallback(Socket sock)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnReceiveData);
                sock.BeginReceive(mInputBuff, 0, mInputBuff.Length, SocketFlags.None, recieveData, sock);
            }
            catch (Exception ex)
            {
                Debug.LogError("Setup Recieve Callback failed!: " + ex.Message);
            }
        }

        /// <summary>
        /// Close the socekt the safe way.
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
        /// Check is connected to the server or not.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return mSocket.Connected;
        }

        /// <summary>
        /// Send a sequence of byte to server.
        /// </summary>
        /// <param name="buffer"> byte array to send. </param>
        /// <returns> boolean to check if success. </returns>
        public bool SendPacket(byte[] buffer)
        {
            if (mSocket == null || !mSocket.Connected)
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
    }
}
