/**
 * $File: JCS_PathActionType.cs $
 * $Date: 2020-04-11 17:15:07 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of path action type.
    /// </summary>
    public enum JCS_PathActionType
    {
        INCREMENT,  // Increment the path points.
        DECREMENT,  // Decrement the path points.

        INC_OR_DEC,  // Random the increment or decrement the path points.

        RANDOM_ALL,  // Random all the path points.
    }
}