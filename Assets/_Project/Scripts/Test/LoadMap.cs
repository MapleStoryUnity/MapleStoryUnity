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
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace MSU.Test
{
    /// <summary>
    /// Test to load file `Map.wz`.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class LoadMap : MonoBehaviour
    {
        /* Variables */

        public SpriteRenderer sp = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.sp = this.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            var wzfm = WzFileManager.FirstInstance();

            WzDirectory wzd = wzfm.GetWzDir("Map");

            //foreach (WzImage img in wzd.WzImages) print(img.name);
            //foreach (WzDirectory img in wzd.WzDirectories) print(img.name);

            var s = wzd["Worldmap"]["WorldMap00.img"] as WzImage;
            var sub = s["BaseImg"] as WzSubProperty;
            var bitmap = sub["0"].GetBitmap();

            var bts = Util.ImageToByteArray(bitmap);
            var sprite = JCS_ImageLoader.Create(bts);

            sp.sprite = sprite;

            //print(bitmap.Width);
            //print(bitmap.Height);
            //print(s["BaseImg"]["0"]["origin"]);
            //print(s["BaseImg"]["0"]["z"]);
        }
    }
}
