/**
 * $File: JCS_Random.cs $
 * $Date: 2017-04-10 21:48:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Store of random algorithm.
    /// </summary>
    public static class JCS_Random
    {
        /// <summary>
        /// Return normal random range.
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }
        public static uint Range(uint min, uint max)
        {
            return (uint)Random.Range(min, max);
        }
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Return normal random range and cover the max value.
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int RangeInclude(int min, int max)
        {
            return Range(min, max + 1);
        }
        public static uint RangeInclude(uint min, uint max)
        {
            return Range(min, max + 1);
        }
        public static float RangeInclude(float min, float max)
        {
            float include = FindDecimalInclude(max);
            return Range(min, max + include);
        }

        /// <summary>
        /// Return a random color.
        /// </summary>
        /// <returns> random color object. </returns>
        public static Color PickColor(float a = 1.0f)
        {
            return new Color(Range(0.0f, 1.0f), Range(0.0f, 1.0f), Range(0.0f, 1.0f), a);
        }

        /// <summary>
        /// Find the decimal include.
        /// </summary>
        /// <param name="num"> Number use to calculate. </param>
        /// <returns> Return the include decimal number. </returns>
        private static float FindDecimalInclude(float num)
        {
            string str = num.ToString();

            if (str.Contains("."))
                str = str.Split('.')[1];

            return 1.0f / Mathf.Pow(10.0f, str.Length);
        }

        /// <summary>
        /// Choose one object from the list.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="inArray"> The list or array to choose from. </param>
        /// <returns> The chosen object from the list or array. </returns>
        public static T ChooseOne<T>(T[] inArray)
        {
            if (inArray.Length == 0) return default(T);
            int index = Range(0, inArray.Length);
            return inArray[index];
        }
        public static T ChooseOne<T>(List<T> inList)
        {
            if (inList.Count == 0) return default(T);
            int index = Range(0, inList.Count);
            return inList[index];
        }
        public static T ChooseOneE<T>(params T[] args)  // Ellipsis
        {
            if (args.Length == 0) return default(T);
            int index = Range(0, args.Length);
            return args[index];
        }

        /// <summary>
        /// Return a random enum value.
        /// </summary>
        /// <typeparam name="T"> The enum type. </typeparam>
        /// <returns> The enum value that has been randomly chosen. </returns>
        public static T EnumValue<T>()
        {
            var r = new System.Random();
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(r.Next(v.Length));
        }
    }
}
