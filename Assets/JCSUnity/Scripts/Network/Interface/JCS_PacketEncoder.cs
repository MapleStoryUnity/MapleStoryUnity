/**
 * $File: JCS_PacketEncoder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all encoder
    /// </summary>
    public interface JCS_PacketEncoder
    {
        System.Object Encode(Object message);
    }
}
