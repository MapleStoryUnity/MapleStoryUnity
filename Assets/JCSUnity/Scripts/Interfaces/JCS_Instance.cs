/**
 * $File: JCS_Instance.cs $
 * $Date: 2021-12-26 01:31:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JCSUnity
{
    /// <summary>
    /// Singleton instance interface.
    /// </summary>
    public abstract class JCS_Instance<T> : MonoBehaviour
    {
        /* Variables */

        protected static Dictionary<Scene, T> mInstances = new();

        /* Setter & Getter */

        public Dictionary<Scene, T> instances { get { return mInstances; } }

        /* Functions */

        /// <summary>
        /// Register a scene instance.
        /// </summary>
        public void RegisterInstance(T _new)
        {
            var comp = _new as Component;

            if (comp != null)
                RegisterInstance(_new, comp.gameObject.scene);
            else
                RegisterInstance(_new, SceneManager.GetActiveScene());
        }
        public void RegisterInstance(T _new, Scene scene)
        {
            CleanInstances();

            if (!mInstances.ContainsKey(scene))
                mInstances.Add(scene, _new);

            mInstances[scene] = _new;
        }

        /// <summary>
        /// Deregister the instance.
        /// </summary>
        public void DeregisterInstance()
        {
            DeregisterInstance(SceneManager.GetActiveScene());
        }
        public void DeregisterInstance(Scene scene)
        {
            if (mInstances.ContainsKey(scene))
                mInstances.Remove(scene);
        }

        /// <summary>
        /// Clean up unused instances.
        /// </summary>
        public void CleanInstances()
        {
            List<Scene> scenes = new();

            foreach (Scene scene in mInstances.Keys)
            {
                if (!scene.isLoaded)
                {
                    scenes.Add(scene);
                }
            }

            foreach (Scene scene in scenes)
            {
                mInstances.Remove(scene);
            }
        }

        /// <summary>
        /// Return the first instance.
        /// </summary>
        public static T FirstInstance()
        {
            if (mInstances.Count == 0)
                return default;

            return mInstances.ElementAt(0).Value;
        }

        /// <summary>
        /// Return the instance.
        /// </summary>
        public static T GetInstance()
        {
            return GetInstance(SceneManager.GetActiveScene());
        }
        public static T GetInstance(uint id)
        {
            List<Scene> scenes = JCS_SceneManager.GetAllScenes();

            Scene scene = scenes.Where((scene) =>
            {
                string name = scene.name;

                if (!name.StartsWith("Arena"))
                    return false;

                string[] splits = name.Split(":");

                return splits.Last() == id.ToString();
            }).First();

            return GetInstance(scene);
        }
        public static T GetInstance(Component comp)
        {
            return GetInstance(comp.transform);
        }
        public static T GetInstance(Transform trans)
        {
            return GetInstance(trans.gameObject);
        }
        public static T GetInstance(GameObject go)
        {
            return GetInstance(go.scene);
        }
        public static T GetInstance(Scene scene)
        {
            // 如果有, 就返回.
            if (mInstances.ContainsKey(scene))
                return mInstances[scene];

            return default;
        }
    }
}
