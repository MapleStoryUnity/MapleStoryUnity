/**
 * $File: JCS_CollisionManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Take care of all the collision that could 
    /// happen in the game.
    /// </summary>
    public class JCS_CollisionManager : JCS_Managers<JCS_CollisionManager>
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetCollisionMode();
        }

        /// <summary>
        /// Call this everytime u spawn a object 
        /// with character controller components on it.
        /// </summary>
        public void SetCollisionMode()
        {
            if (!JCS_GameSettings.instance.IGNORE_EACH_OTHER_CHARACTER_CONTROLLER)
                return;

            CharacterController[] controllers = Resources.FindObjectsOfTypeAll<CharacterController>();

            // Make all the character controller ignore each other
            for (int index = 0;
                index < controllers.Length;
                ++index)
            {
                for (int pairIndex = index + 1;
                    pairIndex < controllers.Length;
                    ++pairIndex)
                {

                    Physics.IgnoreCollision(
                            controllers[index],
                            controllers[pairIndex], true);
                }
            }
        }
    }
}
