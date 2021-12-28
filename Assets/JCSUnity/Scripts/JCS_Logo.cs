﻿/**
 * $File: JCS_Logo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Logo object for next scene.
    /// </summary>
    public class JCS_Logo : MonoBehaviour
    {
        /* Variables */

        [Header("** Note: Fade Time can be set at JCS_SceneSettings. **")]

        [Tooltip("Next scene to load.")]
        [SerializeField]
        private string mNextLevel = "JCS_AppCloseSimulate";

        [Tooltip("Second to show logo and load to the next scene.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mDelayTime = 1.0f;

        private float mDelayTimer = 0.0f;

        private bool mCycleThrough = false;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            // Hide all the open dialogue
            JCS_UIManager.instance.HideAllOpenDialogue();

            // Plus the fade out time
            mDelayTime += JCS_SceneManager.instance.SceneFadeOutTime;
        }

        private void Update()
        {
            var gm = JCS_GameManager.instance;

            gm.GAME_PAUSE = true;

            mDelayTimer += Time.deltaTime;

            if (mDelayTime < mDelayTimer)
            {
                mCycleThrough = true;
            }

            if (mCycleThrough)
            {
                gm.GAME_PAUSE = false;
                JCS_SceneManager.instance.LoadScene(mNextLevel);
            }
        }
    }
}
