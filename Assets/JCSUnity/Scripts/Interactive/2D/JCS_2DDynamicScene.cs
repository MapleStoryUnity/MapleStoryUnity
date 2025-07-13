/**
 * $File: JCS_2DDynamicScene.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Tag been handle by "JCS_SceneManager"
    /// </summary>
    public class JCS_2DDynamicScene : JCS_DynamicScene
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            // get handle!
            JCS_SceneManager.instance.SetDynamicScene(this);
        }
    }
}
