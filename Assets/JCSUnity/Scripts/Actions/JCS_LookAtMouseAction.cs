/**
 * $File: JCS_LookAtMouseAction.cs $
 * $Date: 2017-09-07 16:10:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Look at the mouse position.
    /// 
    /// SOURCE: http://wiki.unity3d.com/index.php?title=LookAtMouse
    /// </summary>
    public class JCS_LookAtMouseAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_LookAtMouseAction)")]

        [Tooltip("How fast to look at the mouse.")]
        [SerializeField]
        private float mSpeed = 10.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public float Speed { get { return this.mSpeed; } set { this.mSpeed = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void FixedUpdate()
        {
            // Generate a plane that intersects the transform's position with an upwards normal.
            var playerPlane = new Plane(Vector3.up, transform.position);

            // Generate a ray from the cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.

            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            // then find the point along that ray that meets that distance.  This will be the point
            // to look at.
            float hitdist = 0.0f;

            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, mSpeed * JCS_Time.ItTime(mTimeType));
            }
        }
    }
}
