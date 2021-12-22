/**
 * $File: JCS_TransformLinkedObjectController.cs $
 * $Date: 2020-08-08 21:49:13 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Controller that controls `JCS_TransformLinkedObject` component.
    /// </summary>
    public class JCS_TransformLinkedObjectController : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_TransformLinkedObjectController) **")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to add linked node.")]
        [SerializeField]
        private KeyCode mKeyAdd = KeyCode.A;

        [Tooltip("Starting index to create.")]
        [SerializeField]
        private int mCreateStartIndex = 0;

        [Tooltip("Count to create the new linked node")]
        [SerializeField]
        [Range(1, 10)]
        private int mCreateCount = 1;

        [Tooltip("Key to remove linked node.")]
        [SerializeField]
        private KeyCode mKeyRemove = KeyCode.S;

        [Tooltip("Starting index to remove.")]
        [SerializeField]
        [Range(0, 30)]
        private int mRemoveStartIndex = 0;

        [Tooltip("Remvoe count from starting index.")]
        [SerializeField]
        [Range(1, 10)]
        private int mRemoveCount = 1;
#endif

        [Header("** Check Variables (JCS_TransformLinkedObjectController) **")]

        [Tooltip("List of all managed transform linked object.")]
        [SerializeField]
        private List<JCS_TransformLinkedObject> mManagedList = null;

        [Header("** Initialize Variables (JCS_TransformLinkedObjectController) **")]

        [Tooltip("Main clone linked object.")]
        [SerializeField]
        private JCS_TransformLinkedObject mClone = null;

        [Header("** Runtime Variables (JCS_TransformLinkedObjectController) **")]

        [Tooltip("Transform vector offset for each linked object.")]
        [SerializeField]
        private Vector3 mIndexOffset = new Vector3(0.0f, 1.0f, 0.0f);

        /* Setter & Getter */

        public List<JCS_TransformLinkedObject> ManagedList { get { return this.mManagedList; } }
        public JCS_TransformLinkedObject Clone { get { return this.mClone; } }
        public Vector3 IndexOffset { get { return this.mIndexOffset; } set { this.mIndexOffset = value; } }

        /* Functions */

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mKeyAdd))
                NewLinked(mCreateCount, mCreateStartIndex);
            if (JCS_Input.GetKeyDown(mKeyRemove))
                RemoveLinked(mRemoveCount, mRemoveStartIndex);
        }
#endif

        /// <summary>
        /// Create new linked node object by N count.
        /// </summary>
        /// <param name="n"> Number of linked object to create. </param>
        /// <param name="startIndex"> Starting index to create. </param>
        /// <returns> List of created linked object. </returns>
        public List<JCS_TransformLinkedObject> NewLinked(int n = 1, int startIndex = 0)
        {
            if (mClone == null)
            {
                JCS_Debug.LogReminder("Can't create new linked node without the clone");
                return null;
            }

            if (n <= 0)
            {
                JCS_Debug.LogReminder("Can't create new linked node N lower than 1");
                return null;
            }

            var lst = new List<JCS_TransformLinkedObject>();

            int maxIndex = startIndex + n;

            for (int index = startIndex; index < maxIndex; ++index)
            {
                var newNode = JCS_Util.SpawnGameObject(mClone) as JCS_TransformLinkedObject;

                // Set to it centers.
                {
                    newNode.transform.SetParent(this.transform);
                    newNode.transform.localPosition = Vector3.zero;
                    newNode.transform.localScale = Vector3.one;
                }

                lst.Add(newNode);
                mManagedList.Insert(index, newNode);
            }

            OrganizedLinked();

            return lst;
        }

        /// <summary>
        /// Remove linked node from the managed list.
        /// </summary>
        /// <param name="n"> Number of linked object to remove. </param>
        /// <param name="startIndex"> Starting index to remove. </param>
        /// <returns> List of removed linked object. </returns>
        public List<JCS_TransformLinkedObject> RemoveLinked(int n = 1, int startIndex = 0)
        {
            if (!JCS_Util.WithInArrayRange(startIndex, mManagedList))
            {
                JCS_Debug.LogReminder("Can't remove linked node with index lower than 0");
                return null;
            }

            var lst = new List<JCS_TransformLinkedObject>();

            int maxIndex = startIndex + n;
            List<int> ids = new List<int>();

            for (int index = startIndex; index < mManagedList.Count && index < maxIndex; ++index)
            {
                JCS_TransformLinkedObject node = mManagedList[index];

                ids.Add(index);  // First record it down.
                lst.Add(node);  // Added to the remove list, and ready to return.
            }

            for (int index = 0; index < ids.Count; ++index)
            {
                int id = ids[index];

                JCS_TransformLinkedObject node = mManagedList[id];
                Destroy(node.gameObject);

                mManagedList.RemoveAt(id);
            }

            OrganizedLinked();

            return lst;
        }

        private void OrganizedLinked()
        {
            mManagedList = JCS_Util.RemoveEmptySlotIncludeMissing(mManagedList);

            for (int index = 0; index < mManagedList.Count; ++index)
            {
                JCS_TransformLinkedObject node = mManagedList[index];

                Vector3 newOffset = mIndexOffset * index;

                node.TransformTweener.DoTween(newOffset);
            }
        }
    }
}
