/**
 * $File: JCS_DestroyParticleEndEvent.cs $
 * $Date: 2017-04-10 20:56:24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Destroy when the particle system ends.
    /// </summary>
    [RequireComponent(typeof(JCS_ParticleSystem))]
    public class JCS_DestroyParticleEndEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_ParticleSystem mParticleSystem = null;


        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mParticleSystem = this.GetComponent<JCS_ParticleSystem>();
        }

        private void Update()
        {
            if (mParticleSystem.IsParticleEnd())
                Destroy(this.gameObject);
        }
    }
}
