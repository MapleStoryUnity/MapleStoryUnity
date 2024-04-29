/**
 * $File: JCS_QuitAppOnLoadEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Event that skips the first scene for application load.
    /// </summary>
    public class JCS_SkipAppEvent : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        [Separator("Runtime Variables (JCS_SkipAppEvent)")]

        [Tooltip("The next starting scene to load.")]
        [SerializeField]
        private string mSceneName = "";

        /* Functions */

        private void Start()
        {
            if (mSceneName == "")
            {
                int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextIndex);
                return;
            }

            SceneManager.LoadScene(mSceneName);
        }
    }
}
