/**
 * $File: JCS_3DCursorShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Shoot bullet toward the cursor position in 3D space.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_3DCursorShootAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Separator("Check Variables (JCS_3DCursorShootAction)")]

        [SerializeField]
        [ReadOnly]
        private JCS_ShootAction mShootAction = null;

        [Separator("Runtime Variables (JCS_3DCursorShootAction)")]

        [Tooltip("Key code to active this action.")]
        [SerializeField]
        private KeyCode mShootKeyCode = KeyCode.None;

        [Tooltip("Vector to look at.")]
        [SerializeField]
        private JCS_Vector3Direction mDirection = JCS_Vector3Direction.UP;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public KeyCode shooKeyCode { get { return mShootKeyCode; } }
        public JCS_Vector3Direction direction { get { return mDirection; } set { mDirection = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mShootAction = GetComponent<JCS_ShootAction>();

            // override the shoot action.
            //mShootAction.OverrideShoot = true;
        }

        private void FixedUpdate()
        {
            LookAtMouse();
        }

        /// <summary>
        /// Source: http://wiki.unity3d.com/index.php?title=LookAtMouse
        /// </summary>
        private void LookAtMouse()
        {
            float speed = 10;

            Vector3 direction = JCS_Vector.Direction(mDirection);

            // Generate a plane that intersects the transform's position with an upwards normal.
            Plane playerPlane = new Plane(direction, mShootAction.spawnPoint.position);

            // Generate a ray from the cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.
            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            //   then find the point along that ray that meets that distance.  This will be the point
            //   to look at.
            float hitdist = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - mShootAction.spawnPoint.position);

                // Smoothly rotate towards the target point.
                mShootAction.spawnPoint.rotation = Quaternion.Slerp(mShootAction.spawnPoint.rotation, targetRotation, speed * JCS_Time.ItTime(mTimeType));
            }
        }
    }
}
