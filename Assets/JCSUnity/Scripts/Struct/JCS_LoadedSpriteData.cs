/**
 * $File: JCS_LoadedSpriteData.cs $
 * $Date: 2020-07-07 00:46:43 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Sprite data for loaded image.
    /// </summary>
    [System.Serializable]
    public class JCS_LoadedSpriteData
    {
        private Sprite mSprite;  /* Image sprite data. */
        private int mIndex;      /* Image file index. */

        public Sprite sprite { get { return mSprite; } }
        public int index { get { return mIndex; } }

        public JCS_LoadedSpriteData(Sprite sp, int index)
        {
            mSprite = sp;
            mIndex = index;
        }
    };
}
