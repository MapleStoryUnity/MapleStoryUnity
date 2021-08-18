/**
 * $File: JCS_Particle.cs $
 * $Date: 2016-11-12 21:16:10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Particle base class for polynorphism.
    /// </summary>
    public class JCS_Particle : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        public float PosX
        {
            get { return this.transform.position.x; }
            set
            {
                Vector3 tempPos = this.transform.position;
                tempPos.x = value;
                this.transform.position = tempPos;
            }
        }
        public float PosY
        {
            get { return this.transform.position.y; }
            set
            {
                Vector3 tempPos = this.transform.position;
                tempPos.y = value;
                this.transform.position = tempPos;
            }
        }
        public float PosZ
        {
            get { return this.transform.position.z; }
            set
            {
                Vector3 tempPos = this.transform.position;
                tempPos.z = value;
                this.transform.position = tempPos;
            }
        }
        public float LocalPosX
        {
            get { return this.transform.localPosition.x; }
            set
            {
                Vector3 tempPos = this.transform.localPosition;
                tempPos.x = value;
                this.transform.localPosition = tempPos;
            }
        }
        public float LocalPosY
        {
            get { return this.transform.localPosition.y; }
            set
            {
                Vector3 tempPos = this.transform.localPosition;
                tempPos.y = value;
                this.transform.localPosition = tempPos;
            }
        }
        public float LocalPosZ
        {
            get { return this.transform.localPosition.z; }
            set
            {
                Vector3 tempPos = this.transform.localPosition;
                tempPos.z = value;
                this.transform.localPosition = tempPos;
            }
        }

        /* Functions */

        public void SetPosition(Vector3 pos)
        {
            this.transform.position = pos;
        }
        public void SetLocalPosition(Vector3 pos)
        {
            this.transform.localPosition = pos;
        }
    }
}
