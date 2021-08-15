/**
 * $File: JCS_BinaryReader.cs $
 * $Date: 2017-08-25 16:55:53 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JCSUnity
{
    /// <summary>
    /// Binary Writer for JCSNetS framework use.
    /// </summary>
    public class JCS_BinaryReader
    {
        /* Variables */

        private BinaryReader mBuffer = null;

        private int mFloatPrecise = 1000;

        /* Setter & Getter */

        public BinaryReader GetBinaryReader() { return this.mBuffer; }

        /// <summary>
        /// How the float precise? (Default : 1000)
        /// </summary>
        /// <param name="fp"> Float precise value. </param>
        public void SetFloatPercise(int fp) { this.mFloatPrecise = fp; }

        /* Functions */

        public JCS_BinaryReader(BinaryReader br)
        {
            this.mBuffer = br;
        }


        /// <summary>
        /// Read boolean from buffer stream.
        /// </summary>
        /// <returns> boolean read from buffer stream. </returns>
        public bool ReadBoolean()
        {
            return mBuffer.ReadBoolean();
        }

        /// <summary>
        /// Read number of chars from buffer stream.
        /// </summary>
        /// <param name="count"> number of char read </param>
        /// <returns> number of chars from buffer stream </returns>
        public char[] ReadChars(int count)
        {
            return mBuffer.ReadChars(count);
        }

        /// <summary>
        /// Read char from buffer stream.
        /// </summary>
        /// <returns> char from buffer stream. </returns>
        public char ReadChar()
        {
            return mBuffer.ReadChar();
        }

        /// <summary>
        /// Read number of bytes from buffer stream.
        /// </summary>
        /// <param name="count"> number of byte </param>
        /// <returns> number of bytes from buffer stream </returns>
        public byte[] ReadBytes(int count)
        {
            return mBuffer.ReadBytes(count);
        }

        /// <summary>
        /// Read a byte from buffer stream.
        /// </summary>
        /// <returns> byte from buffer stream </returns>
        public byte ReadByte()
        {
            return mBuffer.ReadByte();
        }

        /// <summary>
        /// Read String from buffer stream.
        /// </summary>
        /// <returns> string from buffer stream. </returns>
        public string ReadString()
        {
            return mBuffer.ReadString();
        }

        /// <summary>
        /// Read single from buffer stream.
        /// </summary>
        /// <returns> single from buffer stream. </returns>
        public float ReadSingle()
        {
            return mBuffer.ReadSingle();
        }

        /// <summary>
        /// Read float from buffer stream.
        /// </summary>
        /// <returns> float from buffer stream. </returns>
        public float ReadFloat()
        {
            return mBuffer.ReadInt32() / (float)mFloatPrecise;
        }

        /// <summary>
        /// Read double from buffer stream.
        /// </summary>
        /// <returns> double from buffer stream. </returns>
        public double ReadDouble()
        {
            return mBuffer.ReadDouble();
        }

        /// <summary>
        /// Read Short from buffer stream.
        /// </summary>
        /// <returns> short from buffer stream. </returns>
        public short ReadShort()
        {
            return mBuffer.ReadInt16();
        }

        /// <summary>
        /// Read Unsigned short from buffer stream.
        /// </summary>
        /// <returns> unsigned short from buffer stream. </returns>
        public ushort ReadUShort()
        {
            return mBuffer.ReadUInt16();
        }

        /// <summary>
        /// Read Integer from buffer stream.
        /// </summary>
        /// <returns> integer read from buffer stream. </returns>
        public int ReadInt()
        {
            return mBuffer.ReadInt32();
        }

        /// <summary>
        /// Read Unsigned Integer from buffer stream.
        /// </summary>
        /// <returns> unsigned integer read from buffer stream. </returns>
        public uint ReadUInt()
        {
            return mBuffer.ReadUInt32();
        }

        /// <summary>
        /// Read Long from buffer stream.
        /// </summary>
        /// <returns> long read from buffer stream. </returns>
        public long ReadLong()
        {
            return mBuffer.ReadInt64();
        }

        /// <summary>
        /// Read Unsigned Long from buffer stream.
        /// </summary>
        /// <returns> unsigned long read from buffer stream. </returns>
        public ulong ReadULong()
        {
            return mBuffer.ReadUInt64();
        }

        /// <summary>
        /// Read ASCII until the null byte appear and put data 
        /// into a string data.
        /// </summary>
        /// <returns> string put together from the buffer stream. </returns>
        public string ReadNullTerminatedASCIIString()
        {
            List<byte> list = new List<byte>();

            byte b = mBuffer.ReadByte();
            while (b != 0)
            {
                //or whatever your condition is
                list.Add(b);
                b = mBuffer.ReadByte();
            }
            string output = Encoding.ASCII.GetString(list.ToArray());

            return output;
        }

        /// <summary>
        /// Read Unicode until the null byte appear and put 
        /// data into a string data.
        /// </summary>
        /// <returns> string put together from the buffer stream. </returns>
        public string ReadNullTerminatedUnicodeString()
        {
            List<byte> list = new List<byte>();

            byte b = mBuffer.ReadByte();
            while (b != 0)
            {
                //or whatever your condition is
                list.Add(b);
                b = mBuffer.ReadByte();
            }
            string output = Encoding.Unicode.GetString(list.ToArray());

            return output;
        }

        /// <summary>
        /// Read UTF-8 string until the null byte appear and 
        /// put data into a string data.
        /// </summary>
        /// <returns> string put together from the buffer stream. </returns>
        public string ReadNullTerminatedUTF8String()
        {
            List<byte> list = new List<byte>();

            byte b = mBuffer.ReadByte();
            while (b != 0)
            {
                //or whatever your condition is
                list.Add(b);
                b = mBuffer.ReadByte();
            }
            string output = Encoding.UTF8.GetString(list.ToArray());

            return output;
        }

        /// <summary>
        /// Read UTF-32 string until the null byte appear and put 
        /// data into a string data.
        /// </summary>
        /// <returns> string put together from the buffer stream. </returns>
        public string ReadNullTerminatedUTF32String()
        {
            List<byte> list = new List<byte>();

            byte b = mBuffer.ReadByte();
            while (b != 0)
            {
                //or whatever your condition is
                list.Add(b);
                b = mBuffer.ReadByte();
            }
            string output = Encoding.UTF32.GetString(list.ToArray());

            return output;
        }

        /// <summary>
        /// Available data left in this stream.
        /// </summary>
        /// <returns></returns>
        public long Available()
        {
            return mBuffer.BaseStream.Length - mBuffer.BaseStream.Position;
        }
    }
}
