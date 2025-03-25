/**
 * $File: JCS_DelayLoadSceneEvent.cs $
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
    /// Delay seconds before loading a scene.
    /// </summary>
    public class JCS_DelayLoadSceneEvent : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DelayLoadSceneEvent)")]

        [Tooltip("Next scene to load; if empy, load the next scene instead.")]
        [SerializeField]
        private string mNextLevel = "";

        [Tooltip("Reload the current scene, and ignore the target scene name.")]
        [SerializeField]
        public bool mReloadScene = false;

        [Tooltip("Second to show logo and load to the next scene.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mDelayTime = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        private float mDelayTimer = 0.0f;

        /* Setter & Getter */

        public string NextLevel { get { return this.mNextLevel; } set { this.mNextLevel = value; } }
        public float DelayTime { get { return this.mDelayTime; } set { this.mDelayTime = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

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
            mDelayTimer += JCS_Time.ItTime(mTimeType);

            if (mDelayTime < mDelayTimer)
            {
                string sceneName = JCS_SceneManager.GetSceneNameByOption(mNextLevel, mReloadScene);

                JCS_SceneManager.instance.LoadScene(sceneName);
            }
        }
    }
}
