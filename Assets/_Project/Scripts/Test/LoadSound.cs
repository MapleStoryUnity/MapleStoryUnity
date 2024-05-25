/*
 This file is part of the MapleStory Unity

 Copyright (C) 2021-2024 Shen, Jen-Chieh <jcs090218@gmail.com> 

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License version 3
 as published by the Free Software Foundation. You may not use, modify
 or distribute this program under any other version of the
 GNU Affero General Public License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
