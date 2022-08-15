/**
 * $File: JCS_2DCursorShootAction.cs $
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
    /// Shoot a bullet toward the cursor position in 2D space.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_2DCursorShootAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Header("** Check Variables (JCS_2DCursorShootAction) **")]

        [SerializeField]
        private JCS_ShootAction mShootAction = null;

        // if this is true, meaning there are other shoot action going on.
        private bool mOverrideShoot = false;


        /* Setter & Getter */

        public bool OverrideShoot { get { return this.mOverrideShoot; } set { this.mOverrideShoot = value; } }

        /// <summary>
        /// Call back during shooting a bullet.
        /// </summary>
        /// <param name="func"> function to set. </param>
        public void SetShootCallback(EmptyFunction func)
        {
            this.mShootAction.SetShootCallback(func);
        }

        /// <summary>
        /// Function to check if able to do this action.
        /// </summary>
        /// <param name="func"></param>
        public void SetCheckAbleToShootFunction(CheckAbleToShoot func)
        {
            this.mShootAction.SetCheckAbleToShootFunction(func);
        }


        /* Functions */

        private void Awake()
        {
            mShootAction = this.GetComponent<JCS_ShootAction>();

            mShootAction.OverrideShoot = true;
        }

        private void Update()
        {
            LookAtMouse();

            if (!OverrideShoot)
                ProcessInput();
        }

        /// <summary>
        /// Algrothims to do look at mouse in 2d space.
        /// </summary>
        private void LookAtMouse()
        {
            Camera cam = JCS_Camera.main.GetCamera();

            // Set Mouse Position in world
            Vector3 Mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Input.mousePosition.z - cam.transform.position.z));

            // line connect with curosr pos and this pos.
            Vector3 lockPos;

            // handle the scaling.
            // the scale will change the direction of the shooting
            // angle, fix the angle to the correct angle base
            // on the scale.
            if (JCS_Mathf.IsPositive(transform.localScale.x))
            {
                lockPos = new Vector3(0, 0,
                    Mathf.Atan2((Mouse.y - mShootAction.SpawnPoint.position.y),
                    (Mouse.x - mShootAction.SpawnPoint.position.x)) * Mathf.Rad2Deg);
            }
            else
            {
                lockPos = new Vector3(0, 0,
                    Mathf.Atan2((-Mouse.y + mShootAction.SpawnPoint.position.y),
                    (-Mouse.x + mShootAction.SpawnPoint.position.x)) * Mathf.Rad2Deg);
            }

            // Have the Gun Object look at the Mouse Var
            mShootAction.SpawnPoint.eulerAngles = lockPos;
        }

        /// <summary>
        /// Process/Handle the inputs from the user.
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetMouseByAction(mShootAction.KeyAct, mShootAction.MouseButton) ||
                JCS_Input.GetKeyByAction(mShootAction.KeyAct, mShootAction.ShootKeyCode))
            {
                if (mShootAction.GetCheckAbleToShootFunction().Invoke())
                {
                    // do call back
                    mShootAction.GetShootCallback().Invoke();

                    // shoot a bullet.
                    for (int count = 0; count < mShootAction.ShootCount; ++count)
                    {
                        mShootAction.Shoot();
                    }
                }
            }
        }
    }
}
