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

        public float posX
        {
            get { return transform.position.x; }
            set
            {
                Vector3 tempPos = transform.position;
                tempPos.x = value;
                transform.position = tempPos;
            }
        }
        public float posY
        {
            get { return transform.position.y; }
            set
            {
                Vector3 tempPos = transform.position;
                tempPos.y = value;
                transform.position = tempPos;
            }
        }
        public float posZ
        {
            get { return transform.position.z; }
            set
            {
                Vector3 tempPos = transform.position;
                tempPos.z = value;
                transform.position = tempPos;
            }
        }
        public float LocalPosX
        {
            get { return transform.localPosition.x; }
            set
            {
                Vector3 tempPos = transform.localPosition;
                tempPos.x = value;
                transform.localPosition = tempPos;
            }
        }
        public float localPosY
        {
            get { return transform.localPosition.y; }
            set
            {
                Vector3 tempPos = transform.localPosition;
                tempPos.y = value;
                transform.localPosition = tempPos;
            }
        }
        public float localPosZ
        {
            get { return transform.localPosition.z; }
            set
            {
                Vector3 tempPos = transform.localPosition;
                tempPos.z = value;
                transform.localPosition = tempPos;
            }
        }

        /* Functions */

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public void SetLocalPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }
    }
}
