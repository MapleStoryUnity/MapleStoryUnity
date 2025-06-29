/**
 * $File: JCS_Util.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using PeterVuorela.Tweener;

namespace JCSUnity
{
    // Function pointer
    public delegate float TweenDelegate(float t, float b, float c, float d);

    public delegate int JCS_Range(int min, int max);

    /// <summary>
    /// All the utility function put here.
    /// </summary>
    public static class JCS_Util
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        #region Number

        /// <summary>
        /// Delta the `num` with `val` and clamp the result with `min` 
        /// and `max`.
        /// </summary>
        public static int Delta(this int data, int val, int max)
        {
            return data.Delta(val, 0, max);
        }
        public static int Delta(this int data, int val, int min, int max)
        {
            return Mathf.Clamp(data.Delta(val), min, max);
        }
        public static int Delta(this int data, int val)
        {
            return data + val;
        }

        public static float Delta(this float data, float val, float max)
        {
            return data.Delta(val, 0.0f, max);
        }
        public static float Delta(this float data, float val, float min, float max)
        {
            return Mathf.Clamp(data.Delta(val), min, max);
        }
        public static float Delta(this float data, float val)
        {
            return data + val;
        }

        /// <summary>
        /// Delta the `num` with `val` by percentage and clamp the 
        /// result with `min` and `max`.
        /// </summary>
        public static int DeltaP(this int data, int p, int max)
        {
            return data.DeltaP(p, 0, max);
        }
        public static int DeltaP(this int data, int p, int min, int max)
        {
            return Mathf.Clamp(data.DeltaP(p), min, max);
        }
        public static int DeltaP(this int data, int p)
        {
            int val = (int)(data * p / 100.0f);

            return data + val;
        }

        public static float DeltaP(this float data, float p, float max)
        {
            return data.DeltaP(p, 0.0f, max);
        }
        public static float DeltaP(this float data, float p, float min, float max)
        {
            return Mathf.Clamp(data.DeltaP(p), min, max);
        }
        public static float DeltaP(this float data, float p)
        {
            float val = (data * p / 100.0f);

            return data + val;
        }

        #endregion

        #region String

        /// <summary>
        /// Convert byte array to string by charset type.
        /// </summary>
        /// <param name="data"> Byte array data to convert to string data. </param>
        /// <param name="charset"> Target charset type. </param>
        /// <returns> String data that had been converted. </returns>
        public static string BytesToString(byte[] data, JCS_CharsetType charset)
        {
            switch (charset)
            {
                case JCS_CharsetType.DEFAULT: return Encoding.Default.GetString(data);
                case JCS_CharsetType.ASCII: return Encoding.ASCII.GetString(data);
                case JCS_CharsetType.UTF7: return Encoding.UTF7.GetString(data);
                case JCS_CharsetType.UTF8: return Encoding.UTF8.GetString(data);
                case JCS_CharsetType.UTF32: return Encoding.UTF32.GetString(data);
                case JCS_CharsetType.Unicode: return Encoding.Unicode.GetString(data);
                case JCS_CharsetType.BigEndianUnicode: return Encoding.BigEndianUnicode.GetString(data);
            }
            Debug.LogError("This shouldn't happens, charset `bytes to string`");
            return null;
        }

        /// <summary>
        /// Convert string to byte array by charset type.
        /// </summary>
        /// <param name="data"> String data to convert to byte array. </param>
        /// <param name="charset"> Target charset type. </param>
        /// <returns> Byte array that had been converted. </returns>
        public static byte[] StringToBytes(string data, JCS_CharsetType charset)
        {
            switch (charset)
            {
                case JCS_CharsetType.DEFAULT: return Encoding.Default.GetBytes(data);
                case JCS_CharsetType.ASCII: return Encoding.ASCII.GetBytes(data);
                case JCS_CharsetType.UTF7: return Encoding.UTF7.GetBytes(data);
                case JCS_CharsetType.UTF8: return Encoding.UTF8.GetBytes(data);
                case JCS_CharsetType.UTF32: return Encoding.UTF32.GetBytes(data);
                case JCS_CharsetType.Unicode: return Encoding.Unicode.GetBytes(data);
                case JCS_CharsetType.BigEndianUnicode: return Encoding.BigEndianUnicode.GetBytes(data);
            }
            Debug.LogError("This shouldn't happens, charset `string to bytes`");
            return null;
        }

        /// <summary>
        /// Simple version of escape url.
        /// </summary>
        /// <param name="url"> Url you want to escape. </param>
        /// <returns> Return the escaped url. </returns>
        public static string EscapeURL(string url)
        {
            url = url.Replace(" ", "%20");
            return url;
        }

        #endregion

        #region Enum

        /// <summary>
        /// Enum typed version casting.
        /// Source: http://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values
        /// </summary>
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Return the length of an enumerator.
        /// </summary>
        /// <typeparam name="T"> Enum type. </typeparam>
        /// <returns> Size of the enum listed. </returns>
        public static int EnumSize<T>()
        {
            return System.Enum.GetNames(typeof(T)).Length;
        }

        #endregion

        #region Range

        /// <summary>
        /// Check the value within the range plus acceptable range.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="acceptRange"></param>
        /// <param name="currentVal"></param>
        /// <returns></returns>
        public static bool WithInAcceptRange(float range, float acceptRange, float currentVal)
        {
            return WithInRange(range - acceptRange, range + acceptRange, currentVal);
        }

        /// <summary>
        /// Check the value within the range.
        /// </summary>
        /// <param name="minRange"></param>
        /// <param name="maxRange"></param>
        /// <param name="currentVal"></param>
        /// <returns></returns>
        public static bool WithInRange(float minRange, float maxRange, float currentVal)
        {
            return (currentVal >= minRange && currentVal <= maxRange);
        }

        /// <summary>
        /// With in collection range.
        /// </summary>
        /// <returns></returns>
        public static bool WithInRange(int index, ICollection arr)
        {
            return index >= 0 && index < arr.Count;
        }

        #endregion

        #region Stream

        /// <summary>
        /// Copy byte array to another byte array memory space.
        /// </summary>
        /// <param name="inBuf"> byte array to copy. </param>
        /// <param name="start"> Starting index to copy. </param>
        /// <param name="len"> Length to copy. </param>
        /// <returns> byte array that are created in new memroy space. </returns>
        public static byte[] CopyByteArray(byte[] inBuf, int start, int len)
        {
            byte[] bytes = new byte[len];

            for (int count = 0; count < len; ++count)
            {
                bytes[count] = inBuf[count];
            }

            return bytes;
        }

        #endregion

        #region Parse

        /// <summary>
        /// Is the string the valid number to parse.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberString(string str)
        {
            double test;
            return (double.TryParse(str, out test));
        }

        /// <summary>
        /// Parse `str` to integer, return `defaultValue` if failed.
        /// </summary>
        public static int Parse(string str, int defaultValue)
        {
            int result;

            if (int.TryParse(str, out result))
                return int.Parse(str);

            return defaultValue;
        }

        /// <summary>
        /// Parse `str` to float, return `defaultValue` if failed.
        /// </summary>
        public static float Parse(string str, float defaultValue)
        {
            float result;

            if (float.TryParse(str, out result))
                return float.Parse(str);

            return defaultValue;
        }

        /// <summary>
        /// Parse `str` to boolean, return `defaultValue` if failed.
        /// </summary>
        public static bool Parse(string str, bool defaultValue)
        {
            bool result;

            if (bool.TryParse(str, out result))
                return bool.Parse(str);

            return defaultValue;
        }

        #endregion

        #region JSON

        /// <summary>
        /// Return JSON by passing serializable object.
        /// </summary>
        /// <param name="obj"> Object that are serializable. </param>
        /// <returns> JSON string. </returns>
        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        #endregion

        #region Component

        /// <summary>
        /// Do enable/distance component.
        /// </summary>
        /// <param name="comp"> Component reference you want to act. </param>
        /// <param name="act"> Boolean to assign to enabled variable. </param>
        public static void EnableComponent(Component comp, bool act)
        {
            /* Behaviour */
            {
                var behaviour = comp as Behaviour;
                if (behaviour != null)
                {
                    behaviour.enabled = act;
                    return;
                }
            }

            /* Collider */
            {
                var collider = comp as Collider;
                if (collider != null)
                {
                    collider.enabled = act;
                    return;
                }
            }
        }

        /// <summary>
        /// Force to get a component, if not found we will add one then.
        /// </summary>
        /// <typeparam name="T"> Got or Added component. </typeparam>
        /// <param name="mb"> Any MonoBehaviour. </param>
        /// <returns>
        /// Got or new added component.
        /// </returns>
        public static T ForceGetComponent<T>(Component component)
            where T : Component
        {
            T target = component.GetComponent<T>();

            // Did found! great just returns it.
            if (target != null)
                return target;

            // Sadly, we have to add it ourselves.
            target = component.gameObject.AddComponent<T>();

            // Returns the new added component.
            return target;
        }

        /// <summary>
        /// Set eabled/disabled to all component in a transform.
        /// </summary>
        /// <param name="trans"> transform to apply the effect. </param>
        /// <param name="act"> enable or disable? </param>
        public static void SetEnableAllComponents(Transform trans, bool act)
        {
            foreach (var component in trans.GetComponents<MonoBehaviour>())
            {
                component.enabled = act;
            }
        }

        #endregion

        #region Transform & Children

        /// <summary>
        /// Set active according to it's type.
        /// </summary>
        public static void SetActive(List<Transform> transforms, bool act)
        {
            SetActive(transforms.ToArray(), act);
        }
        public static void SetActive(Transform[] transforms, bool act)
        {
            foreach (Transform trans in transforms)
            {
                if (trans == null)
                    continue;

                trans.gameObject.SetActive(act);
            }
        }

        /// <summary>
        /// Set active to all children.
        /// </summary>
        public static void SetActiveChildren(Transform transform, bool act)
        {
            for (int index = 0; index < transform.childCount; ++index)
            {
                transform.GetChild(index).gameObject.SetActive(act);
            }
        }

        /// <summary>
        /// Active all the child in a transform.
        /// </summary>
        /// <param name="trans"> transform to do the effect. </param>
        /// <param name="act"> action to the effect </param>
        public static void SetActiveToAllChildren(Transform trans, bool act)
        {
            Transform child = null;

            for (int index = 0; index < trans.childCount; ++index)
            {
                child = trans.GetChild(index);
                child.gameObject.SetActive(act);
            }
        }

        /// <summary>
        /// Move the object to the last child of the Unty's
        /// tree system(Hierarchy) transform.
        /// </summary>
        /// <param name="trans"></param>
        public static void MoveToTheLastChild(Transform trans)
        {
            Transform parent = trans.parent;

            Vector3 recordPos = trans.localPosition;
            Vector3 recordScale = trans.localScale;
            Quaternion recordRot = trans.localRotation;

            // This part will mess up the transform so we record all we need and 
            // set it back.
            {
                trans.SetParent(null);
                trans.SetParent(parent);
            }

            // here we set it back!
            trans.localPosition = recordPos;
            trans.localScale = recordScale;
            trans.localRotation = recordRot;
        }

        /// <summary>
        /// Set the transform to onther transform in the
        /// hierarchy and without losing the info.
        /// Info: 
        ///     position, rotation, scale, etc.
        /// </summary>
        /// <param name="trans"> transform to set to parent transform </param>
        /// <param name="parent"> parent transform </param>
        public static void SetParentWithoutLosingInfo(Transform trans, Transform parent)
        {
            Vector3 recordPos = trans.localPosition;
            Vector3 recordScale = trans.localScale;
            Quaternion recordRot = trans.localRotation;

            {
                trans.SetParent(parent);
            }

            // here we set it back!
            trans.localPosition = recordPos;
            trans.localScale = recordScale;
            trans.localRotation = recordRot;
        }

        /// <summary>
        /// Detttach all the child from one transform.
        /// </summary>
        /// <param name="trans"> transform you want to remove all 
        /// the children under. </param>
        /// <returns>
        /// All the children under as a list.
        /// </returns>
        public static List<Transform> DetachChildren(Transform trans)
        {
            var childs = new List<Transform>();

            for (int index = 0; index < trans.childCount; ++index)
            {
                Transform child = trans.GetChild(index);

                childs.Add(child);

                // Remove from parent.
                child.SetParent(null);
            }

            return childs;
        }
        public static List<RectTransform> DetachChildren(RectTransform trans)
        {
            var childs = new List<RectTransform>();

            var canvas = JCS_Canvas.GuessCanvas();

            for (int index = 0; index < trans.childCount; ++index)
            {
                Transform child = trans.GetChild(index);
                var rect = child.GetComponent<RectTransform>();

                if (rect == null)
                    continue;

                childs.Add(rect);

                // Remove from parent.
                child.SetParent(canvas.AppRect);
            }

            return childs;
        }

        /// <summary>
        /// Return true, if transform has at least one children.
        /// </summary>
        public static bool HasChild(Transform trans)
        {
            if (trans == null)
                return false;

            return trans.childCount != 0;
        }
        public static bool HasChild(RectTransform trans)
        {
            if (trans == null)
                return false;

            for (int index = 0; index < trans.childCount; ++index)
            {
                Transform child = trans.GetChild(index);

                var rect = child.GetComponent<RectTransform>();

                if (rect != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Force to clean all the children, this will make sure the 
        /// transform have 0 children transform.
        /// </summary>
        /// <param name="trans"> transform you want to remove all 
        /// the children under. </param>
        /// <returns>
        /// All the children under as a list.
        /// </returns>
        public static List<Transform> ForceDetachChildren(Transform trans)
        {
            var childs = new List<Transform>();

            while (HasChild(trans))
            {
                List<Transform> tmpChilds = DetachChildren(trans);

                childs.AddRange(tmpChilds);
            }

            return childs;
        }
        public static List<RectTransform> ForceDetachChildren(RectTransform trans)
        {
            var childs = new List<RectTransform>();

            while (HasChild(trans))
            {
                List<RectTransform> tmpChilds = DetachChildren(trans);

                childs.AddRange(tmpChilds);
            }

            return childs;
        }

        /// <summary>
        /// Attach all childs to this transform.
        /// </summary>
        /// <param name="trans"> transform we want to add the childs to. </param>
        /// <param name="childs"> childs we want to add to transform. </param>
        public static void AttachChildren(Transform trans, List<Transform> childs)
        {
            if (trans == null || childs == null)
                return;

            for (int index = 0; index < childs.Count; ++index)
            {
                Transform child = childs[index];

                // Add to parent.
                child.SetParent(trans);
            }
        }
        public static void AttachChildren(RectTransform trans, List<RectTransform> childs)
        {
            if (trans == null || childs == null)
                return;

            for (int index = 0; index < childs.Count; ++index)
            {
                RectTransform child = childs[index];

                // Add to parent.
                child.SetParent(trans);
            }
        }

        /// <summary>
        /// Execution callback after detach and reattach.
        /// </summary>
        /// <param name="trans"> Transform you want to detach and reattach after callback. </param>
        /// <param name="callback"> Callback after detach and before reattach. </param>
        public static void ReattachSelf(Transform trans, System.Action<Transform> callback)
        {
            if (trans == null || callback == null)
                return;

            var parent = trans.parent;
            trans.SetParent(null);

            callback?.Invoke(parent);

            trans.SetParent(parent);
        }
        public static void ReattachSelf(RectTransform trans, System.Action<Transform> callback)
        {
            if (trans == null || callback == null)
                return;

            var canvas = JCS_Canvas.GuessCanvas();

            var parent = trans.parent;
            trans.SetParent(canvas.AppRect);

            callback?.Invoke(parent);

            trans.SetParent(parent);
        }

        #endregion

        #region Spawning

        /// <summary>
        /// Spawn a gmae object.
        /// </summary>
        /// <param name="objectPath"> path of the game object </param>
        /// <param name="position"> position of the game object spawn </param>
        /// <param name="rotation"> rotation of the game object spawn </param>
        /// <returns></returns>
        public static GameObject Instantiate(string objectPath, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            var original = Resources.Load<GameObject>(objectPath);
            return MonoBehaviour.Instantiate(original, position, rotation);
        }

        /// <summary>
        /// Spawn a gmae object.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Object Instantiate(Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (trans == null) return null;
            return MonoBehaviour.Instantiate(trans, position, rotation);
        }

        /// <summary>
        /// Spwan a game object to another scene.
        /// </summary>
        /// <param name="original"> The original game object. </param>
        /// <param name="scene"> The target scene </param>
        /// <returns> Return the newly spawned game object. </returns>
        public static Object InstantiateToScene(Object original, Scene scene, Vector3 position, Quaternion rotation)
        {
            Object obj = InstantiateToScene(original, scene);

            var trans = obj.GetComponent<Transform>();

            trans.position = position;
            trans.rotation = rotation;

            return obj;
        }
        public static Object InstantiateToScene(Object original, Scene scene)
        {
            return MonoBehaviour.Instantiate(original, scene);
        }

        /// <summary>
        /// Spawn an animate object.
        /// </summary>
        /// <param name="anim"> anim assign </param>
        /// <param name="orderLayer"> sorting order </param>
        /// <returns> object spawned. </returns>
        public static GameObject SpawnAnimateObject(RuntimeAnimatorController anim, int orderLayer = 15)
        {
            GameObject gm = new GameObject();

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = orderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = anim;

            return gm;
        }

        /// <summary>
        /// Spawn a animate object with the death event, 
        /// so after the animate was played serval loop times
        /// the object will be destroyed.
        /// </summary>
        /// <param name="anim"> animation u want to play </param>
        /// <param name="orderLayer"> sorting order  </param>
        /// <param name="loopTimes"> loop times </param>
        /// <returns> object spawned. </returns>
        public static GameObject SpawnAnimateObjectDeathEvent(RuntimeAnimatorController anim, int orderLayer = 15, int loopTimes = 1)
        {
            GameObject gm = SpawnAnimateObject(anim, orderLayer);

            JCS_DestroyAnimEndEvent daee = gm.AddComponent<JCS_DestroyAnimEndEvent>();

            daee.LoopTimes = loopTimes;

            return gm;
        }

        #endregion

        #region Destroy

        /// <summary>
        /// Destroy all the 'TYPE' object in the scene.
        /// </summary>
        public static void DestroyAllTypeObjectInScene<T>()
            where T : MonoBehaviour
        {
            // Destroy all the live object in the scene.
            T[] rrEnemy = Resources.FindObjectsOfTypeAll<T>();

            foreach (T e in rrEnemy)
            {
                // NOTE(JenChieh): kill the object that are clone!
                // or else it will effect the prefab object...
                if (IsClone(e.gameObject))
                    MonoBehaviour.Destroy(e.gameObject);
            }
        }

        /// <summary>
        /// Destroy all the 'TYPE' object in the scene.
        /// </summary>
        public static void DestroyImmediateAllTypeObjectInScene<T>()
            where T : MonoBehaviour
        {
            // Destroy all the live object in the scene.
            T[] rrEnemy = Resources.FindObjectsOfTypeAll<T>();

            foreach (T e in rrEnemy)
            {
                // NOTE(JenChieh): kill the object that are clone!
                // or else it will effect the prefab object...
                if (IsClone(e.gameObject))
                    MonoBehaviour.DestroyImmediate(e.gameObject);
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Return true if the object is a clone.
        /// </summary>
        public static bool IsClone(Object obj)
        {
            return obj.name.Contains("(Clone)");
        }

        /// <summary>
        /// Remove the text "(Clone)" from the object's name,
        /// and return the new name string.
        /// </summary>
        public static string RemoveCloneString(Object obj)
        {
            obj.name = obj.name.Replace("(Clone)", "");

            return obj.name;
        }

        #endregion

        #region Finding

        /// <summary>
        /// Retrieves the first active loaded object of Type type.
        /// </summary>
        public static Object FindObjectByType(System.Type type)
        {
#if UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindFirstObjectByType(type);
#else
            return UnityEngine.Object.FindObjectOfType(type);
#endif
        }

        /// <summary>
        /// Retrieves a list of all loaded objects of Type type.
        /// </summary>
        public static UnityEngine.Object[] FindObjectsByType(System.Type type)
        {
#if UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindObjectsByType(type, FindObjectsSortMode.None);
#else
            return UnityEngine.Object.FindObjectsOfType(type);
#endif
        }

        /// <summary>
        /// Return a list of game object with the tag name.
        /// 
        /// This is CG intensive, please try not to use it too often.
        /// </summary>
        public static List<GameObject> FindGameObjectsWithTag(string tag)
        {
            List<GameObject> result = new();

            JCS_Loop.Times(SceneManager.sceneCount, (count) =>
            {
                Scene scene = SceneManager.GetSceneAt(count);

                List<GameObject> objs = FindGameObjectsWithTag(tag, scene);

                result.AddRange(objs);
            });

            return result;
        }
        // Deal with specific scene.
        public static List<GameObject> FindGameObjectsWithTag(string tag, Scene scene)
        {
            List<GameObject> result = new();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                // Add root self.
                if (root.tag == tag)
                    result.Add(root);

                List<GameObject> objs = FindGameObjectsWithTag(tag, root);

                result.AddRange(objs);
            }

            return result;
        }
        // Deal with specific GameObject.
        public static List<GameObject> FindGameObjectsWithTag(string tag, GameObject go)
        {
            List<GameObject> result = new();

            // Add children.
            foreach (Transform child in go.transform)
            {
                if (child.tag == tag)
                    result.Add(child.gameObject);

                List<GameObject> nested = FindGameObjectsWithTag(tag, child.gameObject);

                result.AddRange(nested);
            }

            return result;
        }

        /// <summary>
        /// Find all the objects that are clone in the scene by type.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> Type array. </returns>
        public static T[] FindCloneObjectsOfTypeAll<T>()
            where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (IsClone(obj.gameObject))
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        /// <summary>
        /// Find all the objects that are not clone in the scene by type.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> Type array. </returns>
        public static T[] FindNotCloneObjectsOfTypeAll<T>()
            where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (!IsClone(obj.gameObject))
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        /// <summary>
        /// Find all objects that only ACTIVE in hierarchy.
        /// </summary>
        /// <typeparam name="T"> Type to find. </typeparam>
        /// <returns> type array, object only in Hierarcrchy. </returns>
        public static T[] FindObjectsOfTypeAllInHierarchy<T>()
             where T : MonoBehaviour
        {
            T[] typeArr = Resources.FindObjectsOfTypeAll<T>();

            T[] cleanArr = new T[typeArr.Length];
            int length = 0;

            foreach (T obj in typeArr)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    cleanArr[length] = obj;
                    ++length;
                }
            }

            return cleanArr;
        }

        #endregion

        #region Effect

        /// <summary>
        /// Return the easing function pointer base on tweener type.
        /// </summary>
        /// <param name="type"> type of the tween formula </param>
        /// <returns> function pointer </returns>
        public static TweenDelegate GetEasing(JCS_TweenType type)
        {
            TweenDelegate easing = null;

            switch (type)
            {
                // default to linear
                case JCS_TweenType.LINEAR:
                    easing = Easing.Linear;
                    break;

                case JCS_TweenType.EASE_IN_SINE:
                    easing = Easing.SineEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CUBIC:
                    easing = Easing.CubicEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUINT:
                    easing = Easing.QuintEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CIRC:
                    easing = Easing.CircEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BACK:
                    easing = Easing.BackEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CIRC:
                    easing = Easing.CircEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BACK:
                    easing = Easing.BackEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CIRC:
                    easing = Easing.CircEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BACK:
                    easing = Easing.BackEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_QUAD:
                    easing = Easing.QuadEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUART:
                    easing = Easing.QuartEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_EXPO:
                    easing = Easing.ExpoEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_ELASTIC:
                    easing = Easing.ElasticEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BOUNCE:
                    easing = Easing.BounceEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUART:
                    easing = Easing.QuartEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_ELASTIC:
                    easing = Easing.ElasticEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BOUNCE:
                    easing = Easing.BounceEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUART:
                    easing = Easing.QuartEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_ELASTIC:
                    easing = Easing.ElasticEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BOUNCE:
                    easing = Easing.BounceEaseInOut;
                    break;
            }

            return easing;
        }

        #endregion

        #region Scene

        /// <summary>
        /// Check current scene's with NAME.
        /// </summary>
        /// <param name="name"> Name of the scene. </param>
        /// <returns>
        /// Return true, if the current scene name the same as NAME.
        /// Return false, if the current scene name NOT the same as NAME.
        /// </returns>
        public static bool IsScene(string name)
        {
            return SceneManager.GetActiveScene().name == name;
        }

        /// <summary>
        /// Returns true if the scene 'name' exists and is in your Build settings, 
        /// false otherwise.
        /// </summary>
        public static bool IsSceneExists(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            for (int index = 0; index < SceneManager.sceneCountInBuildSettings; ++index)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(index);
                var lastSlash = scenePath.LastIndexOf("/");
                var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

                if (string.Compare(name, sceneName, true) == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Execute within the active scene without losing the
        /// current scene.
        /// </summary>
        /// <param name="scene"> Target scene we want to execute. </param>
        /// <param name="action"> The execution body. </param>
        public static void WithActiveScene(Scene scene, System.Action action)
        {
            Scene oldScene = SceneManager.GetActiveScene();

            // If the same scene, just execute and leave.
            if (oldScene == scene)
            {
                if (action != null)
                    action.Invoke();

                return;
            }

            // Switch to new scene.
            SceneManager.SetActiveScene(scene);

            if (action != null)
                action.Invoke();

            // Revert back to old scene.
            SceneManager.SetActiveScene(oldScene);
        }

        #endregion

        #region Particle

        /// <summary>
        /// Play the particle at point.
        /// </summary>
        public static ParticleSystem PlayParticleAtPoint(ParticleSystem ps, Vector3 position)
        {
            return PlayParticleAtPoint(ps, position, ps.main.duration);
        }
        public static ParticleSystem PlayParticleAtPoint(ParticleSystem ps, Vector3 position, float duration)
        {
            if (ps == null)
                return null;

            ParticleSystem newPS = MonoBehaviour.Instantiate(ps);
            newPS.gameObject.name = "One shot particle";
            newPS.transform.position = position;
            newPS.Play();

            DestroyParticle(newPS, duration);

            return newPS;
        }

        /// <summary>
        /// Destroy the particle by its duration.
        /// </summary>
        public static void DestroyParticle(ParticleSystem ps)
        {
            DestroyParticle(ps, ps.main.duration);
        }
        public static void DestroyParticle(ParticleSystem ps, float duration)
        {
            if (ps.main.loop)
                return;

            Object.Destroy(ps.gameObject, duration);
        }

        #endregion

        #region Gameplay

        /// <summary>
        /// Check if the object are the same tribe.
        /// </summary>
        /// <param name="liveObj1"> obj one </param>
        /// <param name="liveObj2"> obj two </param>
        /// <returns>
        /// true: same tribe
        /// false: not the same tribe
        /// </returns>
        public static bool IsSameTribe(JCS_2DLiveObject liveObj1, JCS_2DLiveObject liveObj2)
        {
            if (liveObj1 == null || liveObj2 == null)
                return false;

            // if both player does not need to add in to list.
            // or if both enemy does not need to add in to list.
            return (liveObj1.IsPlayer == liveObj2.IsPlayer);
        }

        #endregion
    }
}
