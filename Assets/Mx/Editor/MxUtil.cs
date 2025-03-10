/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mx
{
    public static class MxUtil
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Construct a key to store `EditorPrefs` registry.
        /// </summary>
        public static string FormKey(string name)
        {
            return "Mx." + name;
        }

        /// <summary>
        /// Print a list in one console log.
        /// </summary>
        public static void Print(List<string> lst)
        {
            string result = "";

            foreach (string item in lst)
            {
                result += item + " ";
            }

            Debug.Log(result);
        }

        /// <summary>
        /// Get the texture from its source filename.
        /// </summary>
        public static Texture FindTexture(string name)
        {
            Texture tex = (name == "") ? null : EditorGUIUtility.FindTexture(name);
            return tex;
        }

        /// <summary>
        /// Get the texture by type.
        /// </summary>
        public static Texture FindTexture(UnityEngine.Object obj, Type t)
        {
            var image = EditorGUIUtility.ObjectContent(obj, t).image;
            return image;
        }
        public static Texture FindTexture(Type t)
        {
            return FindTexture(null, t);
        }
        public static Texture FindTexture(UnityEngine.Object obj)
        {
            if (obj == null) return null;
            return FindTexture(obj, obj.GetType());
        }

        /// <summary>
        /// Convert enum to a list with names.
        /// </summary>
        public static List<string> EnumList(Type e)
        {
            return Enum.GetNames(e).ToList();
        }

        /// <summary>
        /// Remove last item from list.
        /// </summary>
        public static void RemoveLast<T>(List<T> _list)
        {
            if (_list == null || _list.Count == 0)
                return;

            _list.RemoveAt(_list.Count - 1);
        }

        /// <summary>
        /// Move an item from the list to the end of the list.
        /// </summary>
        public static void MoveToLast<T>(List<T> lst, T obj)
        {
            lst.Remove(obj);
            lst.Add(obj);
        }

        /// <summary>
        /// Conver enum to a tuple contains two list.
        ///   - Item1 : is the name.
        ///   - Item2 : is the value.
        /// </summary>
        public static (List<string>, List<string>) EnumTuple(Type e)
        {
            List<string> names = EnumList(e);
            List<string> values = Enum.GetValues(e)
                .Cast<KeyCode>()
                .Select(i => ((int)i).ToString())
                .ToList();
            return (names, values);
        }

        /// <summary>
        /// Convert any list to list of string.
        /// </summary>
        public static List<string> ToListString<T>(List<T> lst)
        {
            return lst.Select(t => t.ToString()).ToList();
        }

        /// <summary>
        /// Cover GameObject to list of string.
        /// </summary>
        public static List<string> ToListString(List<GameObject> lst)
        {
            return lst.Select(i => i.name).ToList();
        }

        /// <summary>
        /// Convert list GameObjects to its instance ID.
        /// </summary>
        public static List<int> GetInstanceIDs(List<GameObject> objs)
        {
            return objs.Select(i => i.GetInstanceID()).ToList();
        }

        /// <summary>
        /// Convert list EditorWindow to its instance ID.
        /// </summary>
        public static List<int> GetInstanceIDs(List<EditorWindow> objs)
        {
            return objs.Select(i => i.GetInstanceID()).ToList();
        }
    }
}
