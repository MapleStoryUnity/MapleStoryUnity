/**
 * $File: JCS_RouteGuageSystem.cs $
 * $Date: 2017-05-01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Minimap of all the racers.
    /// 
    /// SOURCE: https://www.youtube.com/watch?v=yqMIfTWoA8A
    /// </summary>
    public class JCS_RouteGuageSystem : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public struct PlayerRegisterForm
        {
            [Tooltip("Player's transform.")]
            public Transform playerTransform;

            [Tooltip("Sprite transfrom represent this player.")]
            public Transform spriteTransform;
        };

        [Separator("📋 Check Variabless (JCS_RouteGuageSystem)")]

        [Tooltip("Distance between the starting line and goal.")]
        [SerializeField]
        [ReadOnly]
        private float mRealDistance = 0;

        [Tooltip("")]
        [SerializeField]
        [ReadOnly]
        private float mSpriteDistance = 0;

        [Tooltip("Ratio between real distance and sprite ditance.")]
        [SerializeField]
        [ReadOnly]
        private float mDistanceRatio = 0;

        [Separator("🌱 Initialize Variables (JCS_RouteGuageSystem)")]

        [Tooltip("Starting line position.")]
        [SerializeField]
        private Transform mRealStartTransform = null;

        [Tooltip("Goal position.")]
        [SerializeField]
        private Transform mRealGoalTransfrom = null;

        [Tooltip("Start transform for sprite.")]
        [SerializeField]
        private Transform mSpriteStartTransform = null;

        [Tooltip("Goal transform for sprite.")]
        [SerializeField]
        private Transform mSpriteGoalTransform = null;

        [Separator("⚡️ Runtime Variables (JCS_RouteGuageSystem)")]

        [Tooltip("All the player forms.")]
        [SerializeField]
        private List<PlayerRegisterForm> mPlayersForm = null;

        /* Setter & Getter */

        public List<PlayerRegisterForm> playersForm { get { return mPlayersForm; } }
        public Transform realStartTransform { get { return mRealStartTransform; } set { mRealStartTransform = value; } }
        public Transform realGoalTransfrom { get { return mRealGoalTransfrom; } set { mRealGoalTransfrom = value; } }
        public Transform spriteStartTransform { get { return mSpriteStartTransform; } set { mSpriteStartTransform = value; } }
        public Transform rpriteGoalTransform { get { return mSpriteGoalTransform; } set { mSpriteGoalTransform = value; } }

        /* Functions */

        private void Awake()
        {
            // get the position from two target transform.
            mRealDistance = Vector3.Distance(
                mRealGoalTransfrom.position, mRealStartTransform.position);

            mSpriteDistance = Vector3.Distance(
                mSpriteGoalTransform.position, mSpriteStartTransform.position);

            // get the ratio from real distance and sprite distance.
            mDistanceRatio = mSpriteDistance / mRealDistance;
        }

        private void Update()
        {
            DoRouteGuage();
        }

        /// <summary>
        /// Do the route guage algorithm here.
        /// </summary>
        private void DoRouteGuage()
        {
            // Cannot do anything with out these two components.
            if (mRealStartTransform == null ||
                mRealGoalTransfrom == null ||
                mSpriteStartTransform == null ||
                mSpriteGoalTransform == null)
                return;

            for (int index = 0; index < mPlayersForm.Count; ++index)
            {
                // 拿到選手
                PlayerRegisterForm prf = mPlayersForm[index];
                if (prf.playerTransform == null || prf.spriteTransform == null)
                    continue;

                PutPlayerSpriteInPositionBaseOnRealDistance(prf);

                // make sure player are in range.
                PutPlayerSpriteInRange(prf);
            }

        }

        /// <summary>
        /// Measure the real world thing, then determine the positon
        /// of the sprite going to be located.
        /// </summary>
        /// <param name="prf">
        /// player to set.
        /// </param>
        private void PutPlayerSpriteInPositionBaseOnRealDistance(PlayerRegisterForm prf)
        {
            float statusBarCenter = mSpriteStartTransform.position.x + (mSpriteDistance / 2);

            Vector3 newSpritePos = prf.spriteTransform.position;
            newSpritePos.x = statusBarCenter + (prf.playerTransform.position.x * mDistanceRatio);
            prf.spriteTransform.position = newSpritePos;
        }

        /// <summary>
        /// Make sure all the player sprite inside the sprite range.
        /// </summary>
        /// <param name="prf"></param>
        private void PutPlayerSpriteInRange(PlayerRegisterForm prf)
        {
            Vector3 playerSpritePos = prf.spriteTransform.position;

            Vector3 spriteGoalPos = mSpriteGoalTransform.position;
            Vector3 spriteStartPos = mSpriteStartTransform.position;


            if (playerSpritePos.x > spriteGoalPos.x)
                playerSpritePos.x = spriteGoalPos.x;
            else if (playerSpritePos.x < spriteStartPos.x)
                playerSpritePos.x = spriteStartPos.x;

            prf.spriteTransform.position = playerSpritePos;
        }
    }
}
