/**
 * $File: JCS_PageLook.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Type of the dialogue system.
    /// </summary>
    public enum JCS_PageLook
    {
        FIRST_PAGE,         // page with only next button
        PREV_NEXT_PAGE,     // page with previous and next button
        YES_NO,             // page with yes and no button
        END_PAGE,           // page with only previous button
    }
}
