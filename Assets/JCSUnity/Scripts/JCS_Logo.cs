/**
 * $File: JCS_Logo.cs $
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
    /// Logo object for next scene.
    /// </summary>
    public class JCS_Logo : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Logo)")]

        [Tooltip("Next scene to load.")]
        [SerializeField]
        private string mNextLevel = "JCS_AppCloseSimulate";

        [Tooltip("Second to show logo and load to the next scene.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mDelayTime = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        private float mDelayTimer = 0.0f;

        /* Setter & Getter */

        public string NextLevel { get { return this.mNextLevel; } set { this.mNextLevel = value; } }
        public float DelayTime { get { return this.mDelayTime; } set { this.mDelayTime = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

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
            mDelayTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            if (mDelayTime < mDelayTimer)
                JCS_SceneManager.instance.LoadScene(mNextLevel);
        }
    }
}
