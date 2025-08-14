/**
 * $File: JCS_WebcamButton.cs $
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
    /// Base of the webcam related feature handle.
    /// </summary>
    public class JCS_WebcamButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables*/

        [Separator("Check Variables (JCS_WebcamButton)")]

        [Tooltip("Current webcam image path.")]
        [SerializeField]
        [ReadOnly]
        private string mCurrentWebcamImagePath = "";

        [Separator("Initialize Variables (JCS_WebcamButton)")]

        [Tooltip("Webcam to use.")]
        [SerializeField]
        private JCS_Webcam mWebcam = null;

        /* Setter & Getter */

        public JCS_Webcam webcam { get { return this.mWebcam; } set { this.mWebcam = value; } }
        public string currentWebcamImagePath { get { return this.mCurrentWebcamImagePath; } }

        /* Functions */

        public override void OnClick()
        {
            // Current only one function applied, so just do take snap shot 
            // as defualt action!
            TakeSnapshotWebcam();
        }

        /// <summary>
        /// Do take snap shot api call.
        /// </summary>
        private void TakeSnapshotWebcam()
        {
            if (mWebcam == null)
            {
                Debug.Log("Assign the button but no webcam attached or in the scene");
                return;
            }

            mCurrentWebcamImagePath = mWebcam.TakeSnapshotWebcam();
        }
    }
}
