/**
 * $File: JCS_2DReflectBulletAction.cs $
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
    /// Object that reflect the bullet.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPoolAction))]
    public class JCS_2DReflectBulletAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DReflectBulletAction)")]

        // TODO(jenchieh): not done yet.
        [Tooltip("Force add on after reflect.")]
        [SerializeField] private float mReflectForce = 0;

        [Tooltip("Collider detection to reflect.")]
        [SerializeField] private Collider mCollider = null;

        // Sound
        private JCS_SoundPoolAction mSoundPoolAction = null;

        [Header("- Reflect")]

        [Tooltip(@"After reflecting the bullet, add the random
degree in to it. In order to get this effect work, better set
degree lower than 90.")]
        [SerializeField]
        [Range(0, 180)]
        private float mRandomReflectDegree = 0;

        [Header("- Random Teleport Effect")]

        [Tooltip("Position offset.")]
        [SerializeField] private Vector3 mPosOffset = Vector3.zero;

        [Tooltip("after bullet get reflected, teleport the bullet to somewhere else.")]
        [SerializeField]
        private bool mRandPosX = false;

        [SerializeField]
        [Range(0.1f, 1000)]
        private float mRandPosRangeX = 1f;

        [SerializeField]
        private bool mRandPosY = false;

        [SerializeField]
        [Range(0.1f, 1000)]
        private float mRandPosRangeY = 1f;

        [SerializeField]
        private bool mRandPosZ = false;

        [SerializeField]
        [Range(0.1f, 1000)]
        private float mRandPosRangeZ = 1f;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mSoundPoolAction = this.GetComponent<JCS_SoundPoolAction>();

            // try to get the collider by itself.
            if (mCollider == null)
                mCollider = this.GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            JCS_Bullet bullet = other.GetComponent<JCS_Bullet>();
            if (bullet != null)
                ReflectBullet(bullet);
        }

        /// <summary>
        /// Reflect the bullet by turning the degree.
        /// </summary>
        /// <param name="bullet"> bullet reflecting. </param>
        private void ReflectBullet(JCS_Bullet bullet)
        {
            // if object cannot be reflected, end function call
            if (!bullet.Reflectable)
                return;

            float randDegree = JCS_Random.Range(-mRandomReflectDegree, mRandomReflectDegree);

            Vector3 rotation = bullet.transform.eulerAngles;
            rotation.z += (180 + randDegree);      // plus 180 so it goes other direction.
            bullet.transform.eulerAngles = rotation;

            // apply the force.
            bullet.MoveSpeed += mReflectForce;

            // play sound from sound pool
            mSoundPoolAction.PlayRandom();

            // do random teleport effect
            RandomPosEffect(bullet);

            // apply position offset
            bullet.transform.position += mPosOffset;
        }

        private void RandomPosEffect(JCS_Bullet bullet)
        {
            Vector3 newPos = bullet.transform.position;

            if (mRandPosX)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeX, mRandPosRangeX);
                newPos.x += effectRange;
            }

            if (mRandPosY)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeY, mRandPosRangeY);
                newPos.y += effectRange;
            }

            if (mRandPosZ)
            {
                float effectRange = JCS_Random.Range(-mRandPosRangeZ, mRandPosRangeZ);
                newPos.z += effectRange;
            }

            // apply result
            bullet.transform.position = newPos;
        }
    }
}
