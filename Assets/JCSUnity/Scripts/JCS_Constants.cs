/**
 * $File: JCS_Constants.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Hold all the game constant here.
    /// </summary>
    public static class JCS_Constants
    {
        /* Variables */

        // The time delay to be invoked for the first frame.
        //
        // First frame invocation can't be set to 0; therefore, we set
        // to somthing really small. Generally has to be smaller than
        // the 1/60 frame time.
        //
        // See https://docs.unity3d.com/6000.1/Documentation/ScriptReference/MonoBehaviour.Invoke.html
        public const float FIRST_FRAME_INVOKE_TIME = 0.001f;

        // The minimum friction value.
        public const float FRICTION_MIN = 0.01f;

        // The default effect friction value.
        public const float FRICTION = 0.2f;

        // Below this threshold is consider close enough.
        public const float NEAR_THRESHOLD = 0.01f;

        /* Setter & Getter */

        /* Functions */

    }
}
