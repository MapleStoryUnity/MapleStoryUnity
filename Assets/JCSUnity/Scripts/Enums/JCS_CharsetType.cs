/**
 * $File: JCS_CharsetType.cs $
 * $Date: 2020-08-04 15:03:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Charset type list.
    /// </summary>
    public enum JCS_CharsetType
    {
        DEFAULT,

        ASCII,

        UTF7,
        UTF8,
        UTF32,

        Unicode,
        BigEndianUnicode,
    }
}
