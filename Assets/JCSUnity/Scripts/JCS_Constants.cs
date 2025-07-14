/**
 * $File: JCS_Constants.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Hold all the game constant here.
    /// </summary>
    public static class JCS_Constants
    {
        /* Variables */

        #region System

        // The time delay to be invoked for the first frame.
        //
        // First frame invocation can't be set to 0; therefore, we set
        // to somthing really small. Generally has to be smaller than
        // the 1/60 frame time.
        //
        // See https://docs.unity3d.com/6000.1/Documentation/ScriptReference/MonoBehaviour.Invoke.html
        public const float FIRST_FRAME_INVOKE_TIME = 0.001f;

        #endregion

        #region Matrix

        // Define the 8 corners of a unit cube centered at `Vector3.zero`.
        public static readonly Vector3[] CORNERS_CUBE = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
        };

        #endregion

        #region Effect

        // The minimum friction value.
        public const float FRICTION_MIN = 0.01f;

        // The default effect friction value.
        public const float FRICTION = 0.2f;

        // Below this threshold is consider close enough.
        public const float NEAR_THRESHOLD = 0.01f;

        #endregion

        /* Setter & Getter */

        /* Functions */

    }
}
