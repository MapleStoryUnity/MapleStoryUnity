/**
 * $File: JCS_2DInitLookByTypeAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Look at the gameobject depends on the find method.
    /// </summary>
    [RequireComponent(typeof(JCS_AttackerInfo))]
    public class JCS_2DInitLookByTypeAction : MonoBehaviour
    {
        /* Variables */

        public enum FindMethod
        {
            CLOSEST,

            CLOSEST_RIGHT,
            CLOSEST_LEFT,
            CLOSEST_TOP,
            CLOSEST_BOTTOM
        };

        public enum State
        {
            NONE,

            POSITIVE,
            NEGATIVE
        };

        private JCS_AttackerInfo mAttackerInfo = null;

        [Separator("Runtime Variables (JCS_2DInitLookByTypeAction)")]

        [Tooltip("Direction the target looking at.")]
        [SerializeField]
        private FindMethod mMethod = FindMethod.CLOSEST_RIGHT;

        [Tooltip("Active this action.")]
        [SerializeField]
        private bool mActiveEffect = true;

        [Tooltip(@"The object will recognize the object
which to target which not to.")]
        [SerializeField]
        private bool mUseAttacker = true;

        [Tooltip("Rotate back to original point by negative 90 degrees.")]
        [SerializeField]
        private bool mRotateBack90 = false;

        // record down the first check.
        private State mDirection = State.NONE;

        /* Setter & Getter */

        public FindMethod Method { get { return this.mMethod; } set { this.mMethod = value; } }
        public bool ActiveEffect { get { return this.mActiveEffect; } set { this.mActiveEffect = value; } }
        public bool UseAttacker { get { return this.mUseAttacker; } set { this.mUseAttacker = value; } }

        /* Functions */

        private void Awake()
        {
            mAttackerInfo = this.GetComponent<JCS_AttackerInfo>();
        }

        private void Start()
        {
            if (mActiveEffect)
                LockOnInit();
        }

        /// <summary>
        /// Lock a gameobject, and look at it.
        /// So the object will seems like it
        /// "approach/further away" to the object.
        /// </summary>
        public void LockOnInit()
        {
            LockOnInit(mMethod);
        }
        /// <summary>
        /// Lock a gameobject, and look at it. So the object will seems like it
        /// "approach/further away" to the object.
        /// </summary>
        /// <param name="method"> method to find. </param>
        public void LockOnInit(FindMethod method)
        {
            JCS_2DLiveObject closestliveObj = null;

            switch (method)
            {
                case FindMethod.CLOSEST:
                    closestliveObj = FindClosest();
                    break;
                case FindMethod.CLOSEST_RIGHT:
                    closestliveObj = FindClosestRight();
                    break;
                case FindMethod.CLOSEST_LEFT:
                    closestliveObj = FindClosestLeft();
                    break;
                case FindMethod.CLOSEST_TOP:
                    closestliveObj = FindClosestTop();
                    break;
                case FindMethod.CLOSEST_BOTTOM:
                    closestliveObj = FindClosestBottom();
                    break;
            }

            // no object found!
            if (closestliveObj == null)
                return;

            Vector3 newLookPoint = closestliveObj.transform.position;

            // look at the target object
            this.transform.LookAt(newLookPoint);

            // rotate back to original point.
            if (mRotateBack90)
                this.transform.Rotate(0, -90, 0);

            if (mAttackerInfo.Attacker != null)
            {
                if (mDirection == State.NONE)
                {
                    if (JCS_Mathf.IsNegative(mAttackerInfo.Attacker.localScale.x))
                        mDirection = State.NEGATIVE;
                    else
                        mDirection = State.POSITIVE;
                }

                // 我們規定所有的圖往一邊, 所以只檢查一邊.
                if (mDirection == State.NEGATIVE)
                    this.transform.Rotate(0, 0, 180);
            }

#if UNITY_EDITOR
            // print out the name.
            if (JCS_GameSettings.instance.DEBUG_MODE)
                JCS_Debug.PrintName(closestliveObj.transform);
#endif
        }

        /// <summary>
        /// Check if able to target.
        /// </summary>
        /// <param name="liveObj"> object to check </param>
        /// <returns>
        /// true: able to target, no worry.
        /// false: not able to target by following reason.
        ///             1) Same Tribe
        ///             2) not able in the scene/hierarchy.
        /// </returns>
        private bool AbleTarget(JCS_2DLiveObject liveObj)
        {
            if (mAttackerInfo.Attacker != null)
            {
                // cannot target it-self.
                if (liveObj.transform == mAttackerInfo.Attacker)
                    return false;

                JCS_2DLiveObject owenerLiveObject = mAttackerInfo.Attacker.GetComponent<JCS_2DLiveObject>();

                if (!JCS_GameSettings.instance.TRIBE_DAMAGE_EACH_OTHER)
                {
                    // check same tribe.
                    if (JCS_Util.IsSameTribe(liveObj, owenerLiveObject))
                        return false;
                }
            }

            // don't target the are disabled.
            if (liveObj.gameObject.activeInHierarchy == false)
                return false;

            // don't target the dead live object
            if (liveObj.IsDead())
                return false;

            // make sure the object can be target/damage.
            if (!liveObj.CanDamage)
                return false;

            return true;
        }

        /// <summary>
        /// Find the closest object.
        /// </summary>
        /// <returns> oject found </returns>
        private JCS_2DLiveObject FindClosest()
        {
            // find all living object in the scene
            JCS_2DLiveObject[] objs = JCS_2DLiveObjectManager.instance.LIVE_OBJECT_LIST;

            float distance = 0;
            bool firstAssign = false;

            JCS_2DLiveObject targetFound = null;

            foreach (JCS_2DLiveObject obj in objs)
            {
                if (obj == null)
                    continue;

                if (mUseAttacker)
                {
                    if (!AbleTarget(obj))
                        continue;
                }

                // first assign.
                if (!firstAssign)
                {
                    targetFound = obj;
                    distance = Vector3.Distance(obj.transform.position, this.transform.position);

                    // no more first assign
                    firstAssign = true;

                    continue;
                }

                // current distance we are checking.
                float checkingDistance = Vector3.Distance(obj.transform.position, this.transform.position);

                // check if the distance are closer.
                if (checkingDistance < distance)
                {
                    // found another object that are closer than the last object
                    // we found,
                    //
                    // Update the distance and object.
                    distance = checkingDistance;
                    targetFound = obj;
                }

            }


            return targetFound;
        }

        /// <summary>
        /// Find the closest object and is at the right of this object.
        /// </summary>
        /// <returns> oject found </returns>
        private JCS_2DLiveObject FindClosestRight()
        {
            // find all living object in the scene
            JCS_2DLiveObject[] objs = JCS_2DLiveObjectManager.instance.LIVE_OBJECT_LIST;

            float distance = 0;
            bool firstAssign = false;

            JCS_2DLiveObject targetFound = null;

            foreach (JCS_2DLiveObject obj in objs)
            {
                if (obj == null)
                    continue;

                if (mUseAttacker)
                {
                    if (!AbleTarget(obj))
                        continue;
                }

                // first assign.
                if (!firstAssign)
                {
                    // if object is at the right!
                    if (CheckIsRight(obj.transform))
                    {
                        targetFound = obj;
                        distance = Vector3.Distance(obj.transform.position, this.transform.position);

                        // no more first assign
                        firstAssign = true;
                    }

                    continue;
                }

                // current distance we are checking.
                float checkingDistance = Vector3.Distance(obj.transform.position, this.transform.position);

                // check if the distance are closer.
                if (checkingDistance < distance)
                {
                    // if object is at the right!
                    if (CheckIsRight(obj.transform))
                    {
                        // found another object that are closer than the last
                        // object we found,
                        //
                        // Update the distance and object.
                        distance = checkingDistance;
                        targetFound = obj;
                    }
                }

            }


            return targetFound;
        }

        /// <summary>
        /// Check the object is at the right side of this object.
        /// </summary>
        /// <param name="trans"> object to check </param>
        /// <returns>
        /// true: object is at the right,
        /// false: object is at the left.
        /// </returns>
        private bool CheckIsRight(Transform trans)
        {
            if (trans.position.x < this.transform.position.x)
                return false;       // is at the left

            return true;        // is at the right
        }

        /// <summary>
        /// Check the object is at the top side of this object.
        /// </summary>
        /// <param name="trans"> object to check </param>
        /// <returns>
        /// true: object is at the top,
        /// false: object is at the right.
        /// </returns>
        private bool CheckIsTop(Transform trans)
        {
            if (trans.position.y < this.transform.position.y)
                return false;       // is at the bot

            return true;        // is at the top
        }

        /// <summary>
        /// Find the closest object and is at the left of this object.
        /// </summary>
        /// <returns> oject found </returns>
        private JCS_2DLiveObject FindClosestLeft()
        {
            // find all living object in the scene
            JCS_2DLiveObject[] objs = JCS_2DLiveObjectManager.instance.LIVE_OBJECT_LIST;

            float distance = 0;
            bool firstAssign = false;

            JCS_2DLiveObject targetFound = null;

            foreach (JCS_2DLiveObject obj in objs)
            {
                if (obj == null)
                    continue;

                if (mUseAttacker)
                {
                    if (!AbleTarget(obj))
                        continue;
                }

                // first assign.
                if (!firstAssign)
                {
                    // if object is at the right!
                    if (!CheckIsRight(obj.transform))
                    {
                        targetFound = obj;
                        distance = Vector3.Distance(obj.transform.position, this.transform.position);

                        // no more first assign
                        firstAssign = true;
                    }

                    continue;
                }

                // current distance we are checking.
                float checkingDistance = Vector3.Distance(obj.transform.position, this.transform.position);

                // check if the distance are closer.
                if (checkingDistance < distance)
                {
                    // if object is at the right!
                    if (!CheckIsRight(obj.transform))
                    {
                        // found another object that are
                        // closer than the last object we found,
                        // Update the distance and object.
                        distance = checkingDistance;
                        targetFound = obj;
                    }
                }

            }


            return targetFound;
        }

        /// <summary>
        /// Find the closest object and is at the top of this object.
        /// </summary>
        /// <returns> oject found </returns>
        private JCS_2DLiveObject FindClosestTop()
        {
            // find all living object in the scene
            JCS_2DLiveObject[] objs = JCS_2DLiveObjectManager.instance.LIVE_OBJECT_LIST;

            float distance = 0;
            bool firstAssign = false;

            JCS_2DLiveObject targetFound = null;

            foreach (JCS_2DLiveObject obj in objs)
            {
                if (obj == null)
                    continue;

                if (mUseAttacker)
                {
                    if (!AbleTarget(obj))
                        continue;
                }

                // first assign.
                if (!firstAssign)
                {
                    // if object is at the right!
                    if (CheckIsTop(obj.transform))
                    {
                        targetFound = obj;
                        distance = Vector3.Distance(obj.transform.position, this.transform.position);

                        // no more first assign
                        firstAssign = true;
                    }

                    continue;
                }

                // current distance we are checking.
                float checkingDistance = Vector3.Distance(obj.transform.position, this.transform.position);

                // check if the distance are closer.
                if (checkingDistance < distance)
                {
                    // if object is at the right!
                    if (CheckIsTop(obj.transform))
                    {
                        // found another object that are
                        // closer than the last object we found,
                        // Update the distance and object.
                        distance = checkingDistance;
                        targetFound = obj;
                    }
                }

            }


            return targetFound;
        }

        /// <summary>
        /// Find the closest object and is at the bottom of this object.
        /// </summary>
        /// <returns> oject found </returns>
        private JCS_2DLiveObject FindClosestBottom()
        {
            // find all living object in the scene
            JCS_2DLiveObject[] objs = JCS_2DLiveObjectManager.instance.LIVE_OBJECT_LIST;

            float distance = 0;
            bool firstAssign = false;

            JCS_2DLiveObject targetFound = null;

            foreach (JCS_2DLiveObject obj in objs)
            {
                if (obj == null)
                    continue;

                if (mUseAttacker)
                {
                    if (!AbleTarget(obj))
                        continue;
                }

                // first assign.
                if (!firstAssign)
                {
                    // if object is at the right!
                    if (!CheckIsTop(obj.transform))
                    {
                        targetFound = obj;
                        distance = Vector3.Distance(obj.transform.position, this.transform.position);

                        // no more first assign
                        firstAssign = true;
                    }

                    continue;
                }

                // current distance we are checking.
                float checkingDistance = Vector3.Distance(obj.transform.position, this.transform.position);

                // check if the distance are closer.
                if (checkingDistance < distance)
                {
                    // if object is at the right!
                    if (!CheckIsTop(obj.transform))
                    {
                        // found another object that are closer than the last
                        // object we found,
                        //
                        // Update the distance and object.
                        distance = checkingDistance;
                        targetFound = obj;
                    }
                }

            }

            return targetFound;
        }
    }
}
