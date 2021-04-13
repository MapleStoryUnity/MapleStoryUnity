/**
 * $File: LoadMap.cs $
 * $Date: 2021-04-11 16:35:55 $
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
    public class LoadMap
        : MonoBehaviour
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
            WzFileManager wzfm = WzFileManager.instance;

            WzDirectory wzd = wzfm.GetWzDir("Map");

            //foreach (WzImage img in wzd.WzImages) print(img.name);
            //foreach (WzDirectory img in wzd.WzDirectories) print(img.name);

            var s = wzd["Worldmap"]["WorldMap00.img"] as WzImage;
            var sub = s["BaseImg"] as WzSubProperty;
            var bitmap = sub["0"].GetBitmap();

            var bts = Util.ImageToByteArray(bitmap);
            var sprite = JCS_ImageLoader.Create(bts);

            sp.sprite = sprite;

            print(bitmap.Width);
            print(bitmap.Height);
            //print(s["BaseImg"]["0"]["origin"]);
            //print(s["BaseImg"]["0"]["z"]);
        }

        
    }
}
