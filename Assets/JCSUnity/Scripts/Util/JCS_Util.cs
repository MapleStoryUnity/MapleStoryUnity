/**
 * $File: JCS_Util.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using PeterVuorela.Tweener;

namespace JCSUnity
{
    // Function pointer
    public delegate void EmptyFunction();
    public delegate bool EmptyBoolFunction();

    public delegate float TweenDelegate(float t, float b, float c, float d);

    /// <summary>
    /// Callback when time is up.
    /// </summary>
    public delegate void TimeIsUpFunc();

    public delegate int JCS_Range(int min, int max);
    public delegate void ReattachCallback(Transform parent);

    /// <summary>
    /// All the utility function put here.
    /// </summary>
    public static class JCS_Util
    {
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

        #region List

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
            if (currentVal >= minRange && currentVal <= maxRange)
                return true;
            return false;
        }

        /// <summary>
        /// With in array range. (Array)
        /// </summary>
        /// <returns></returns>
        public static bool WithInRange<T>(int index, T[] arr)
        {
            return index >= 0 && index < arr.Length;
        }

        /// <summary>
        /// With in array range. (List)
        /// </summary>
        /// <returns></returns>
        public static bool WithInRange<T>(int index, List<T> arr)
        {
            return index >= 0 && index < arr.Count;
        }

        /// <summary>
        /// Loop in an array. (Array)
        /// </summary>
        /// <typeparam name="T"> Type. </typeparam>
        /// <param name="index"> Index </param>
        /// <param name="arr"> Array. </param>
        /// <returns> index that looped. </returns>
        public static int LoopIn<T>(int index, T[] arr)
        {
            // loop through the array, if at the tail of the array set it to head.
            if (index < 0)
                index = arr.Length - 1;
            // loop through the array, if at head of the array we set it to the tail.
            else if (index >= arr.Length)
                index = 0;
            return index;
        }

        /// <summary>
        /// Loop in an array. (List)
        /// </summary>
        /// <typeparam name="T"> Type. </typeparam>
        /// <param name="index"> Index </param>
        /// <param name="arr"> List. </param>
        /// <returns> index that looped. </returns>
        public static int LoopIn<T>(int index, List<T> arr)
        {
            // loop through the array, if at the tail of the array set it to head.
            if (index < 0)
                index = arr.Count - 1;
            // loop through the array, if at head of the array we set it to the tail.
            else if (index >= arr.Count)
                index = 0;

            return index;
        }

        /// <summary>
        /// Merge multiple arrays into one array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] MergeArrays<T>(params T[][] arrList)
        {
            if (arrList.Length <= 1)
            {
                JCS_Debug.Log("You trying to merge the array less then two array");
            }

            int arrLen = 0;
            foreach (var arr in arrList)
                arrLen += arr.Length;

            // first combine the first two array.
            T[] data = MergeArrays2<T>(arrList[0], arrList[1]);

            // combine the rest.
            for (int index = 2; index < arrList.Length; ++index)
            {
                data = MergeArrays2<T>(data, arrList[index]);
            }
            return data;
        }

        /// <summary>
        /// Merging two array and return the new array.
        /// </summary>
        /// <typeparam name="T"> Type of the array. </typeparam>
        /// <param name="arr1"> First array. </param>
        /// <param name="arr2"> Second array. </param>
        /// <returns> Merged array. </returns>
        public static T[] MergeArrays2<T>(T[] arr1, T[] arr2)
        {
            T[] data = new T[arr1.Length + arr2.Length];

            System.Array.Copy(arr1, data, arr1.Length);
            System.Array.Copy(arr2, 0, data, arr1.Length, arr2.Length);

            return data;
        }

        /// <summary>
        /// Merging two list and return the new list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static List<T> MergeList<T>(params List<T>[] lists)
        {
            if (lists.Length <= 1)
            {
                JCS_Debug.Log("You trying to merge the List less then two array");
            }

            var newList = new List<T>();

            for (int index = 0; index < lists.Length; ++index)
            {
                // Loop through all list.
                List<T> list = lists[index];

                if (list == null)
                    continue;

                for (int listIndex = 0; listIndex < list.Count; ++listIndex)
                {
                    // Loop through item.
                    T item = list[listIndex];

                    newList.Add(item);
                }
            }

            return newList;
        }

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

        /// <summary>
        /// Check if the list empty.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsArrayEmpty(string[] list)
        {
            for (int index = 0; index < list.Length; ++index)
            {
                if (list[index] != "")
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Pop the last value from the list.
        /// </summary>
        public static T ListPopFront<T>(List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            T data = list[0];

            list.RemoveAt(0);

            return data;
        }

        /// <summary>
        /// Pop the last value from the list.
        /// </summary>
        public static T ListPopBack<T>(List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            int lastIndex = list.Count - 1;

            T data = list[lastIndex];

            list.RemoveAt(lastIndex);

            return data;
        }

        /// <summary>
        /// Fill slots with initialize value type by length.
        /// </summary>
        /// <typeparam name="T"> Type from `inArray`. </typeparam>
        /// <param name="inArray"> Array you would like to fill out. </param>
        /// <param name="len"> Target length to initialize. </param>
        /// <param name="with"> Initialize object type. </param>
        /// <returns> Return filled array. </returns>
        public static T[] FillSlot<T>(T[] inArray, int len, T with)
        {
            return FillSlot(inArray.ToList(), len, with).ToArray();
        }

        public static List<T> FillSlot<T>(List<T> inList, int len, T with)
        {
            for (int index = inList.Count; index < len; ++index)
                inList.Add(with);
            return inList;
        }

        /// <summary>
        /// Remove the empty slot in the array.
        /// </summary>
        /// <typeparam name="T"> Type of the List. </typeparam>
        /// <param name="inArray"> Array list. </param>
        /// <returns> Cleaned up Array object. </returns>
        public static T[] RemoveEmptySlot<T>(T[] inArray)
        {
            return RemoveEmptySlot(inArray.ToList()).ToArray();
        }
        public static List<T> RemoveEmptySlot<T>(List<T> inList)
        {
            List<T> newArray = new List<T>(inList.Count);

            for (int index = 0; index < inList.Count; ++index)
            {
                // Add itself if exists.
                if (inList[index] != null)
                    newArray.Add(inList[index]);
            }

            return newArray;
        }

        /// <summary>
        /// Remove the empty slot in the list including remove 
        /// the missing gameobject too. 
        /// 
        /// I guess Unity do the CG collection later a while when 
        /// you call 'Destory()' function. Before scripting layer 
        /// acknowledge this game object is destory might be too 
        /// late in some situation. This will avoid this type of 
        /// issue/circumstance.
        /// </summary>
        /// <typeparam name="T"> Type of the List. </typeparam>
        /// <param name="inList"> List object. </param>
        /// <returns> Cleaned up List object. </returns>
        public static T[] RemoveEmptySlotIncludeMissing<T>(T[] inArray)
            where T : UnityEngine.Object
        {
            return RemoveEmptySlotIncludeMissing(inArray.ToList()).ToArray();
        }
        public static List<T> RemoveEmptySlotIncludeMissing<T>(List<T> inList)
            where T : UnityEngine.Object
        {
            List<T> newArray = new List<T>(inList.Count);

            for (int index = 0; index < inList.Count; ++index)
            {
                // Add itself if exists.
                // 
                // SOURCE(jenchieh): https://answers.unity.com/questions/131158/how-can-i-check-if-an-object-is-null.html
                // INFORMATION(jenchieh): https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
                if (inList[index] ?? false)
                    newArray.Add(inList[index]);
            }

            return newArray;
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
            JCS_Debug.LogError("This shouldn't happens, charset `bytes to string`");
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
            JCS_Debug.LogError("This shouldn't happens, charset `string to bytes`");
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
            List<Transform> childs = null;

            while (HasChild(trans))
            {
                List<Transform> tmpChilds = DetachChildren(trans);

                childs = MergeList(tmpChilds, childs);
            }

            return childs;
        }
        public static List<RectTransform> ForceDetachChildren(RectTransform trans)
        {
            List<RectTransform> childs = null;

            while (HasChild(trans))
            {
                List<RectTransform> tmpChilds = DetachChildren(trans);

                childs = MergeList(tmpChilds, childs);
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
        public static void ReattachSelf(Transform trans, ReattachCallback callback)
        {
            if (trans == null || callback == null)
                return;

            var parent = trans.parent;
            trans.SetParent(null);

            if (callback != null)
                callback.Invoke(parent);

            trans.SetParent(parent);
        }
        public static void ReattachSelf(RectTransform trans, ReattachCallback callback)
        {
            if (trans == null || callback == null)
                return;

            var canvas = JCS_Canvas.GuessCanvas();

            var parent = trans.parent;
            trans.SetParent(canvas.AppRect);

            if (callback != null)
                callback.Invoke(parent);

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
            return MonoBehaviour.Instantiate(Resources.Load<GameObject>(objectPath), position, rotation) as GameObject;
        }

        /// <summary>
        /// Spawn a gmae object.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static UnityEngine.Object Instantiate(UnityEngine.Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (trans == null) return null;
            return MonoBehaviour.Instantiate(trans, position, rotation);
        }

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
        public static Object[] FindObjectsByType(System.Type type)
        {
#if UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindObjectsByType(type, FindObjectsSortMode.None);
#else
            return UnityEngine.Object.FindObjectsOfType(type);
#endif
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
                if (e.gameObject.name.Contains("(Clone)"))
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
                if (e.gameObject.name.Contains("(Clone)"))
                    MonoBehaviour.DestroyImmediate(e.gameObject);
            }
        }

        #endregion

        #region Finding

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
                if (obj.gameObject.name.Contains("(Clone)"))
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
                if (!obj.gameObject.name.Contains("(Clone)"))
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

        #endregion

        #region Animation

        /// <summary>
        /// Sets the weight of the layer at the given name.
        /// 
        /// Similar to `Animationr.SetLayerWeight` but accept string name.
        /// </summary>
        public static void SetLayerWeight(Animator ator, string name, float val)
        {
            int index = ator.GetLayerIndex(name);

            SetLayerWeight(ator, index, val);
        }
        // For compatibility.
        public static void SetLayerWeight(Animator ator, int index, float val)
        {
            ator.SetLayerWeight(index, val);
        }

        /// <summary>
        /// Returns the weight of the layer at the specified name.
        /// 
        /// Similar to `Animationr.SetLayerWeight` but accept string name.
        /// </summary>
        public static float GetLayerWeight(Animator ator, string name)
        {
            int index = ator.GetLayerIndex(name);

            return GetLayerWeight(ator, index);
        }
        // For compatibility.
        public static float GetLayerWeight(Animator ator, int index)
        {
            return ator.GetLayerWeight(index);
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
