/**
 * $File: JCS_3DPortalType.cs $
 * $Date: 2016-11-06 17:40:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Type of the 3D portal.
    /// </summary>
    public enum JCS_3DPortalType
    {
        // Portal that jump into another scene.
        SCENE_PORTAL,   // common

        // Portal that transfer object to another position. (portal)
        TRANSFER_PORTAL,
    }
}
