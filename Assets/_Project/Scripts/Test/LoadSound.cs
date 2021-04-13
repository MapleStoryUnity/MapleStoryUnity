/**
 * $File: LoadSound.cs $
 * $Date: 2021-04-13 20:46:02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using System.IO;
using NAudio.Wave;

namespace MSU.Test
{
    /// <summary>
    /// Test to load file `Sound.wz`.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class LoadSound
        : MonoBehaviour
    {
        /* Variables */

        public AudioSource audioSource = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            WzFileManager wzfm = WzFileManager.instance;

            WzDirectory wzd = wzfm.GetWzDir("Sound");

            var s = wzd["Bgm00.img"] as WzImage;
            s.ParseImage();
            var sound = s["FloralLife"] as WzSoundProperty;

            AudioClip clip = UWL.NAudioPlayer.FromMp3Data(sound.name, sound.GetBytes(false));
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
