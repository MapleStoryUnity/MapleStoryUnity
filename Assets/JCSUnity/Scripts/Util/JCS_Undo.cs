#if UNITY_EDITOR
/**
 * $File: JCS_Undo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2026 by Shen, Jen-Chieh $
 */
using System;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace JCSUnity
{
    /// <summary>
    /// Undo and redo utilities.
    /// </summary>
    public static class JCS_Undo
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Like `Undo.RegisterCompleteObjectUndo` but a wrapper.
        /// </summary>
        public static void RegisterComplete(
            UnityEngine.Object obj, string name, Action action)
        {
            var objs = new UnityEngine.Object[1] { obj };

            RegisterComplete(objs, name, action);
        }
        public static void RegisterComplete(
            UnityEngine.Object[] objs, string name, Action action)
        {
            Undo.RegisterCompleteObjectUndo(objs, name);

            action?.Invoke();

            foreach (UnityEngine.Object obj in objs)
                EditorUtility.SetDirty(obj);
        }

        /// <summary>
        /// Like `RegisterComplete` but automatically assign the 
        /// undo operation name.
        /// </summary>
        public static void RegisterComplete(
            UnityEngine.Object obj,
            Action action,
            [CallerMemberName] string name = "")
        {
            var objs = new UnityEngine.Object[1] { obj };

            RegisterComplete(objs, action, name);
        }
        public static void RegisterComplete(
            UnityEngine.Object[] objs,
            Action action,
            [CallerMemberName] string name = "")
        {
            string id = objs.GetType().Name;

            RegisterComplete(objs, $"{id} ({name})", action);
        }
    }
}
#endif
