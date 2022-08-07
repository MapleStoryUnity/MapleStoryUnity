/**
 * $File: JCS_3DWalkAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.AI;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Simulate the walk action in 3D space.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_3DWalkAction : MonoBehaviour , JCS_Action
    {
        /* Variables */

        // All enemy should have the nav mesh agent for the path finding.
        private NavMeshAgent mNavMeshAgent = null;

        private NavMeshObstacle mNavMeshObstacle = null;

        private JCS_AdjustTimeTrigger mAdjustTimeTrigger = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_3DWalkAction) **")]

        [Tooltip("Found the path now.")]
        [SerializeField]
        private bool mFoundPath = false;

        [Tooltip("Remaining distance between destination position.")]
        [SerializeField]
        private float mRemainingDistance = 0.0f;
#endif

        [Header("** Check Variables (JCS_3DWalkAction) **")]

        [Tooltip("Current target destination.")]
        [SerializeField]
        private Vector3 mTargetDestination = Vector3.negativeInfinity;

        [Tooltip("Target transform that we are going to follow.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Record down the starting position.")]
        [SerializeField]
        private Vector3 mStartingPosition = Vector3.zero;

        // try to avoid stack overflow function call...
        [Tooltip("Counter for how many search per frame.")]
        [SerializeField]
        private int mSearchCounter = 0;

        [Header("** Runtime Variables (JCS_3DWalkAction) **")]

        [Tooltip("Check weather you want do this action.")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Type of the walk behaviour calculation.")]
        [SerializeField]
        private JCS_3DWalkType mWalkType = JCS_3DWalkType.SELF_IN_DISTANCE;

        // try to avoid stack overflow function call...
        [Tooltip("Count for how many search per frame.")]
        [SerializeField]
        [Range(1, 5)]
        private int mSearchCount = 2;

        [Tooltip("Allow each walk action having the same destination.")]
        [SerializeField]
        private bool mAllowOverlapDestination = false;

        [Tooltip("Distance that would count as overlap destination.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mOverlapDistance = 0.5f;

        [Tooltip("What value count as path complete action.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mAcceptRemainDistance = 0.2f;

        [Tooltip("Minimum randomly add vector with magnitude of distance at target position.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mMinOffDistance = 0.0f;

        [Tooltip("Maximum randomly add vector with magnitude of distance at target position.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mMaxOffDistance = 0.0f;

        [Header("- Self In Distance")]

        [Tooltip("Self distance without target transform interact.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mSelfDistance = 5.0f;

        [Header("- To Target")]

        [Tooltip("Range that enemy will try to get close to.")]
        [SerializeField]
        [Range(0.001f, 1000.0f)]
        private float mRangeDistance = 5.0f;

        [Tooltip("Randomly adjusts the range distance.")]
        [SerializeField]
        [Range(0.001f, 30.0f)]
        private float mAdjustRangeDistance = 0.0f;

        /* Setter & Getter */

        public Vector3 StartingPosition { get { return this.mStartingPosition; } set { this.mStartingPosition = value; } }
        public Vector3 TargetDestination { get { return this.mTargetDestination; } }

        public int SearchCount { get { return this.mSearchCount; } set { this.mSearchCount = value; } }

        public NavMeshAgent navMeshAgent { get { return this.mNavMeshAgent; } }
        public NavMeshObstacle navMeshObstacle { get { return this.mNavMeshObstacle; } }
        public JCS_AdjustTimeTrigger AdjustTimeTrigger { get { return this.mAdjustTimeTrigger; } }

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public JCS_3DWalkType WalkType { get { return this.mWalkType; } set { this.mWalkType = value; } }
        public float AcceptRemainDistance { get { return this.mAcceptRemainDistance; } set { this.mAcceptRemainDistance = value; } }

        public Transform TargetTransform { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }

        public float MinOffDistance { get { return this.mMinOffDistance; } set { this.mMinOffDistance = value; } }
        public float MaxOffDistance { get { return this.mMaxOffDistance; } set { this.mMaxOffDistance = value; } }

        public bool AllowOverlapDestination { get { return this.mAllowOverlapDestination; } set { this.mAllowOverlapDestination = value; } }
        public float OverlapDistance { get { return this.mOverlapDistance; } set { this.mOverlapDistance = value; } }

        public float RangeDistance { get { return this.mRangeDistance; } set { this.mRangeDistance = value; } }
        public float AdjustRangeDistance { get { return this.mAdjustRangeDistance; } set { this.mAdjustRangeDistance = value; } }

        public float SelfDistance { get { return this.mSelfDistance; } set { this.mSelfDistance = value; } }

        /* Functions */

        private void Awake()
        {
            this.mNavMeshAgent = this.GetComponent<NavMeshAgent>();
            this.mNavMeshObstacle = this.GetComponent<NavMeshObstacle>();
            ObstacleNow();

            this.mAdjustTimeTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            mAdjustTimeTrigger.actions = DoAI;

            this.mStartingPosition = this.transform.position;
        }

        private void Start()
        {
            JCS_3DWalkActionManager wam = JCS_3DWalkActionManager.instance;
            wam.AddWalkAction(this);
        }

        private void Update()
        {
            if (IsArrived())
                ObstacleNow();
        }

        /// <summary>
        /// Do AI action once.
        /// </summary>
        public void DoAI()
        {
            // Check function trigger.
            if (!mActive)
                return;

            AgentNow();

            TargetOne(mTargetTransform);
        }

        /// <summary>
        /// Turn the current nav mesh to agent.
        /// </summary>
        public void AgentNow()
        {
            navMeshObstacle.enabled = false;
            navMeshAgent.enabled = true;
        }

        /// <summary>
        /// Turn the current nav mesh to obstacle.
        /// </summary>
        public void ObstacleNow()
        {
            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;
        }

        /// <summary>
        /// Target one player and do in target action.
        /// </summary>
        /// <param name="target"> Target we are following. </param>
        public void TargetOne(Transform target)
        {
            if (!mNavMeshAgent.enabled)
                return;

            if (!IsTargetType())
                return;

            // if target is does not exist, end function call.
            if (target == null)
            {
                JCS_Debug.LogError("The transform you are targeting is null");
                return;
            }

            JCS_3DWalkActionManager wam = JCS_3DWalkActionManager.instance;

            if (mSearchCounter == mSearchCount)
            {
                // reset search count.
                mSearchCounter = 0;

                // exit out of recursive function call...
                return;
            }

            // reset the path every time it request.
            mNavMeshAgent.ResetPath();

            // calculate the distance and range relationship, and find out the 
            // position agent are approach to.
            Vector3 targetPos = GetPosByWalkType(target);

            JCS_3DWalkAction overlapped = null;
            if (!mAllowOverlapDestination)
                overlapped = wam.OverlapWithOthers(this, targetPos, mOverlapDistance);

            // set to the destination.
            bool found = false;
            if (!overlapped)
                found = mNavMeshAgent.SetDestination(targetPos);

            ++mSearchCounter;

#if (UNITY_EDITOR)
            this.mFoundPath = found;
#endif

            // if faild, try it again.
            if (!found)
            {
                if (overlapped)
                    TargetOne(overlapped.transform);
                else
                    TargetOne(target);
            }
            else
            {
                this.mTargetDestination = targetPos;

                // if succesed, reset search count.
                mSearchCounter = 0;
            }
        }

        /// <summary>
        /// Check if the nav mesh agent arrive the destination.
        /// </summary>
        public bool NavMeshArrive(NavMeshAgent agent)
        {
            float remDist = Vector3.Distance(agent.destination, agent.transform.position);

#if (UNITY_EDITOR)
            this.mRemainingDistance = remDist;
#endif

            if (!float.IsNaN(remDist) &&
                agent.pathStatus == NavMeshPathStatus.PathComplete &&
                remDist <= mAcceptRemainDistance)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if nav mesh agent path completed.
        /// </summary>
        /// <returns>
        /// Return true, if the path finding is complete.
        /// Return false, if the path finding is NOT complete.
        /// </returns>
        public bool IsArrived()
        {
            return NavMeshArrive(mNavMeshAgent);
        }

        /// <summary>
        /// Check if the transform in the range of the distance.
        /// </summary>
        /// <returns>
        /// Return true, if is in the range of distance.
        /// Return false, if is NOT in the range of distance.
        /// </returns>
        public bool InRangeDistance()
        {
            Vector3 targetPos = mTargetTransform.position;
            Vector3 selfPos = this.transform.position;
            float distance = Vector3.Distance(targetPos, selfPos);
            float maxDistance = 0.0f;

            switch (mWalkType)
            {
                case JCS_3DWalkType.SELF_IN_DISTANCE:
                    maxDistance = mSelfDistance + mMaxOffDistance + mAcceptRemainDistance;
                    break;
                case JCS_3DWalkType.TARGET_CLOSEST_POINT:
                case JCS_3DWalkType.TARGET_IN_RANGE:
                    maxDistance = mRangeDistance + mAdjustRangeDistance + mMaxOffDistance + mAcceptRemainDistance;
                    break;
            }

            return distance <= maxDistance;
        }

        /// <summary>
        /// Return the target position base on walk type.
        /// </summary>
        /// <param name="targetPos"> Target position. </param>
        /// <returns>
        /// Target position that calculated depends walk type.
        /// </returns>
        private Vector3 GetPosByWalkType(Transform target)
        {
            Vector3 targetPos = Vector3.zero;
            if (target) targetPos = target.transform.position;
            Vector3 newTargetPos = targetPos;

            switch (mWalkType)
            {
                case JCS_3DWalkType.SELF_IN_DISTANCE:
                    newTargetPos = CalculateRange(mStartingPosition, mSelfDistance);
                    break;
                case JCS_3DWalkType.TARGET_CLOSEST_POINT:
                    newTargetPos = CalculateClosest(targetPos, GetRangeDistance());
                    break;
                case JCS_3DWalkType.TARGET_IN_RANGE:
                    newTargetPos = CalculateRange(targetPos, GetRangeDistance());
                    break;
            }

            newTargetPos = AddOffDistance(newTargetPos);

            return newTargetPos;
        }

        /// <summary>
        /// Calculate the the closest point by range and targetPos.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private Vector3 CalculateClosest(Vector3 targetPos, float distance)
        {
            Vector3 newTargetPos = targetPos;

            Vector3 vec = this.transform.position - targetPos;

            vec = vec.normalized;

            float hyp = JCS_Mathf.PythagoreanTheorem(vec.x, vec.z, JCS_Mathf.TriSides.hyp);

            float ratio = distance / hyp;

            newTargetPos.x += vec.x * ratio;
            newTargetPos.z += vec.z * ratio;
            newTargetPos.y = this.transform.position.y;

            return newTargetPos;
        }

        /// <summary>
        /// Calculate the range and position relationship
        /// in order to find the best destination in the
        /// navigation map.
        ///
        /// IMPORTANT(JenChieh): if the vector does not in the range,
        /// enemy will stay at the place they are, which mean enemy
        /// will do nothing...
        /// </summary>
        /// <returns> result destination </returns>
        private Vector3 CalculateRange(Vector3 targetPos, float distance)
        {
            Vector3 newTargetPos = targetPos;

            Vector3 randVec = GetRandomVec();

            float magnitude = distance;

            randVec *= magnitude;

            return newTargetPos + randVec;
        }

        /// <summary>
        /// Add the off distance.
        /// </summary>
        /// <param name="targetPos"> Target position use to calculate. </param>
        /// <returns>
        /// Return new position with off distance added.
        /// </returns>
        private Vector3 AddOffDistance(Vector3 targetPos)
        {
            Vector3 randVec = GetRandomVec();

            float magnitude = JCS_Random.RangeInclude(mMinOffDistance, mMaxOffDistance);

            randVec *= magnitude;

            return targetPos + randVec;
        }

        /// <summary>
        /// Return the range distance.
        /// </summary>
        /// <returns> Value of the range distance. </returns>
        private float GetRangeDistance()
        {
            float hypOffset = JCS_Random.Range(-mAdjustRangeDistance, mAdjustRangeDistance);
            return mRangeDistance + hypOffset;
        }

        /// <summary>
        /// Get a random vector as unit vector. (direction)
        /// </summary>
        /// <returns>
        /// Random unit vector.
        /// </returns>
        private Vector3 GetRandomVec()
        {
            float xVec = JCS_Random.RangeInclude(-1.0f, 1.0f);
            float yVec = 0.0f;  // no direction on y axis.
            float zVec = JCS_Random.RangeInclude(-1.0f, 1.0f);

            return new Vector3(xVec, yVec, zVec).normalized;
        }

        /// <summary>
        /// Check if the current walk type target type.
        /// </summary>
        private bool IsTargetType()
        {
            switch (mWalkType)
            {
                case JCS_3DWalkType.TARGET_CLOSEST_POINT:
                case JCS_3DWalkType.TARGET_IN_RANGE:
                    return true;
            }
            return false;
        }
    }
}
