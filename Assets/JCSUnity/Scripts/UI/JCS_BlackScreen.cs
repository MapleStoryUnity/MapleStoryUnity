/**
 * $File: JCS_BlackScreen.cs $
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
    /// Black screen game object for JCSUnity usage.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_BlackScreen : MonoBehaviour
    {
        /* Variables */

        private JCS_FadeObject mFadeObject = null;

        /* Setter & Getter */

        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }
        public Color LocalColor { get { return this.mFadeObject.LocalColor; } set { this.mFadeObject.LocalColor = value; } }

        /* Functions */

        private void Awake()
        {
            this.mFadeObject = this.GetComponent<JCS_FadeObject>();
        }

        private void Start()
        {
            // everytime it reload the scene.
            // move to the last child make sure everything get cover by this.
            JCS_Util.MoveToTheLastChild(this.transform);
        }

        /// <summary>
        /// Fade in black screen.
        /// </summary>
        /// <param name="time"></param>
        public void FadeIn(float time)
        {
            mFadeObject.FadeIn(time);
        }

        /// <summary>
        /// Fade out black screen.
        /// </summary>
        /// <param name="time"></param>
        public void FadeOut(float time)
        {
            mFadeObject.FadeOut(time);
        }

        /// <summary>
        /// Is the black screen fade in?
        /// </summary>
        /// <returns></returns>
        public bool IsFadeIn()
        {
            return mFadeObject.IsFadeIn();
        }

        /// <summary>
        /// Is the black screen fade out?
        /// </summary>
        /// <returns></returns>
        public bool IsFadeOut()
        {
            return mFadeObject.IsFadeOut();
        }

        /// <summary>
        /// Move the panel to front
        /// </summary>
        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            Vector3 recordPos = this.transform.localPosition;
            Vector3 recordScale = this.transform.localScale;
            Quaternion recordRot = this.transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                this.transform.SetParent(null);
                this.transform.SetParent(parent);
            }

            // here we set it back!
            this.transform.localPosition = recordPos;
            this.transform.localScale = recordScale;
            this.transform.localRotation = recordRot;
        }
    }
}
