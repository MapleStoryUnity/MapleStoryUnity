/**
 * $File: NetworkConstant.cs $
 * $Date: 2021-04-10 17:19:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */

namespace MSU
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetworkConstant
    {
        /* Variables */

        public const int HEADER_LENGTH = 4;
        public const int OPCODE_LENGTH = 2;
        public const int MIN_PACKET_LENGTH = HEADER_LENGTH + OPCODE_LENGTH;
        public const int MAX_PACKET_LENGTH = 131072;

        /* Setter & Getter */

        /* Functions */

    }
}
