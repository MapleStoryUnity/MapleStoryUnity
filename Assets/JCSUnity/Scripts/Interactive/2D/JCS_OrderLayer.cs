/**
 * $File: JCS_OrderLayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Scene layer.
    /// </summary>
    public class JCS_OrderLayer : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_OrderLayer) ")]

        [Tooltip("Rendering order.")]
        [SerializeField]
        private int mOrderLayer = 0;

        [Tooltip("How fast this layer moves.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mLayerFriction = 0;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public int orderLayer { get { return mOrderLayer; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void FixedUpdate()       /* Should use FixedUpdate if no jitter. */
        {
            if (mLayerFriction == 0)
                return;

            var cam = JCS_Camera.main;
            if (cam == null)
                return;

            float dt = JCS_Time.ItTime(mTimeType);

            Vector3 newPos = transform.position;
            newPos.x += cam.velocity.x / mLayerFriction * dt;
            newPos.y += cam.velocity.y / mLayerFriction * dt;
            transform.position = newPos;
        }
    }
}
