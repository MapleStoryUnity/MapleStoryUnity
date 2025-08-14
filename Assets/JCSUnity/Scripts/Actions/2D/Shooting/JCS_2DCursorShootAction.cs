/**
 * $File: JCS_2DCursorShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Shoot a bullet toward the cursor position in 2D space.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_2DCursorShootAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Separator("Check Variables (JCS_2DCursorShootAction)")]

        [SerializeField]
        private JCS_ShootAction mShootAction = null;

        // if this is true, meaning there are other shoot action going on.
        private bool mOverrideShoot = false;

        /* Setter & Getter */

        public bool overrideShoot { get { return mOverrideShoot; } set { mOverrideShoot = value; } }

        /// <summary>
        /// Call back during shooting a bullet.
        /// </summary>
        /// <param name="func"> function to set. </param>
        public void SetShootCallback(Action func)
        {
            mShootAction.SetShootCallback(func);
        }

        /// <summary>
        /// Function to check if able to do this action.
        /// </summary>
        /// <param name="func"></param>
        public void SetCheckAbleToShootFunction(Func<bool> func)
        {
            mShootAction.SetCheckAbleToShootFunction(func);
        }

        /* Functions */

        private void Awake()
        {
            mShootAction = GetComponent<JCS_ShootAction>();

            mShootAction.overrideShoot = true;
        }

        private void Update()
        {
            LookAtMouse();

            if (!mOverrideShoot)
                ProcessInput();
        }

        /// <summary>
        /// Algrothims to do look at mouse in 2d space.
        /// </summary>
        private void LookAtMouse()
        {
            Camera cam = JCS_Camera.main.GetCamera();

            Vector3 mousePosition = Input.mousePosition;

            // Set `mouse` Position in world
            var mouse = cam.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x, 
                    mousePosition.y,
                    mousePosition.z - cam.transform.position.z));

            // line connect with curosr pos and this pos.
            Vector3 lockPos;

            // handle the scaling.
            // the scale will change the direction of the shooting
            // angle, fix the angle to the correct angle base
            // on the scale.
            if (JCS_Mathf.IsPositive(transform.localScale.x))
            {
                lockPos = new Vector3(0, 0,
                    Mathf.Atan2((mouse.y - mShootAction.spawnPoint.position.y),
                    (mouse.x - mShootAction.spawnPoint.position.x)) * Mathf.Rad2Deg);
            }
            else
            {
                lockPos = new Vector3(0, 0,
                    Mathf.Atan2((-mouse.y + mShootAction.spawnPoint.position.y),
                    (-mouse.x + mShootAction.spawnPoint.position.x)) * Mathf.Rad2Deg);
            }

            // Have the Gun Object look at the `mouse` Var
            mShootAction.spawnPoint.eulerAngles = lockPos;
        }

        /// <summary>
        /// Process/Handle the inputs from the user.
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetMouseByAction(mShootAction.keyAct, mShootAction.mouseButton) ||
                JCS_Input.GetKeyByAction(mShootAction.keyAct, mShootAction.shootKeyCode))
            {
                if (mShootAction.GetCheckAbleToShootFunction().Invoke())
                {
                    // do call back
                    mShootAction.GetShootCallback().Invoke();

                    // shoot a bullet.
                    for (int count = 0; count < mShootAction.shootCount; ++count)
                    {
                        mShootAction.Shoot();
                    }
                }
            }
        }
    }
}
