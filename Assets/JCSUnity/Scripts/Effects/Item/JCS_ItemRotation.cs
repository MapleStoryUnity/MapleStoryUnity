/**
 * $File: JCS_ItemRotation.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Item rotation effect.
    /// </summary>
    [RequireComponent(typeof(JCS_OneJump))]
    public class JCS_ItemRotation : JCS_Rotation
    {
        /* Variables */

        // gravity object
        private JCS_OneJump mOneJump = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            mOneJump = this.GetComponent<JCS_OneJump>();
            Effect = true;
        }

        protected override void Update()
        {
            if (!Effect)
                return;

            base.Update();

            if (mOneJump.GetVelocity().y == 0)
                Stop();
        }
    }
}
