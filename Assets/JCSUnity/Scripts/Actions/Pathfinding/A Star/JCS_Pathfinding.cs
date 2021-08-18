/**
 * $File: JCS_Pathfinding.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Path finding core behaviour.
    /// </summary>
    [RequireComponent(typeof(JCS_PfGrid))]
    public class JCS_Pathfinding : MonoBehaviour
    {
        /* Variables */

        public Transform seeker, target;

        private JCS_PfGrid mPfGrid = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mPfGrid = this.GetComponent<JCS_PfGrid>();
        }

        /// <summary>
        /// Start finding the path.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
        {
            StartCoroutine(FindPath(startPos, targetPos));
        }

        /// <summary>
        /// Find path algorithm.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            JCS_PfNode startNode = mPfGrid.NodeFromWorldPoint(startPos);
            JCS_PfNode targetNode = mPfGrid.NodeFromWorldPoint(targetPos);


            if (startNode.Walkable && targetNode.Walkable)
            {
                JCS_PfHeap<JCS_PfNode> openSet = new JCS_PfHeap<JCS_PfNode>(mPfGrid.MaxSize);
                HashSet<JCS_PfNode> closedSet = new HashSet<JCS_PfNode>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    JCS_PfNode currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (JCS_PfNode neighbour in mPfGrid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.Walkable || 
                            closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.Parent = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                        }
                    }
                }
            }
            yield return null;

            if (pathSuccess)
                waypoints = RetracePath(startNode, targetNode);

            JCS_PathRequestManager.instance.FinishedProcessingPath(waypoints, pathSuccess);
        }

        /// <summary>
        /// Retrace the path.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private Vector3[] RetracePath(JCS_PfNode startNode, JCS_PfNode endNode)
        {
            List<JCS_PfNode> path = new List<JCS_PfNode>();
            JCS_PfNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        /// <summary>
        /// Simplify the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Vector3[] SimplifyPath(List<JCS_PfNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int index = 1;
                index < path.Count; 
                ++index)
            {
                Vector2 directionNew = new Vector2(path[index - 1].GridX - path[index].GridX, path[index - 1].GridY - path[index].GridY);

                if (directionNew != directionOld)
                {
                    waypoints.Add(path[index].mWorldPosition);
                }

                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        /// <summary>
        /// Get the distance between the two nodes.
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        private int GetDistance(JCS_PfNode nodeA, JCS_PfNode nodeB)
        {
            // get the distance between node a and node b
            int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);

            return 14 * distX + 10 * (distY - distX);
        }
    }
}
