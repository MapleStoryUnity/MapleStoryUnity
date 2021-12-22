/**
 * $File: JCS_PfGrid.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Path finding grid gameobject.
    /// </summary>
    public class JCS_PfGrid : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_PfGrid) **")]

        [SerializeField]
        private bool mDisplayPathGizmos = false;
#endif

        [Header("** Runtime Variables (JCS_PfGrid) **")]

        [Tooltip("Mask to detect the unwalkable object.")]
        [SerializeField]
        private LayerMask mUnwalkableMask;

        [Tooltip("Size of the whole grid map.")]
        [SerializeField]
        private Vector2 mGridiWorldSize = new Vector2(30, 30);

        [Tooltip("Size of each grid.")]
        [SerializeField] [Range(0.1f, 5.0f)]
        private float mNodeRadius = 0.5f;

        [Tooltip("Direction the grid approach to.")]
        [SerializeField]
        private JCS_Vector3Direction mDirection = JCS_Vector3Direction.FORWARD;


        private JCS_PfNode[,] mGrid = null;

        private float mNodeDiameter;
        private int mGridSizeX, mGridSizeY;

        /* Setter & Getter */

        public LayerMask UnwalkableMask { get { return this.mUnwalkableMask; } set { this.mUnwalkableMask = value; } }
        public int MaxSize { get { return (mGridSizeX * mGridSizeY); } }

        /* Functions */

        private void Awake()
        {
            mNodeDiameter = mNodeRadius * 2;
            mGridSizeX = Mathf.RoundToInt(mGridiWorldSize.x / mNodeDiameter);
            mGridSizeY = Mathf.RoundToInt(mGridiWorldSize.y / mNodeDiameter);
            CreateGrid();
        }

#if (UNITY_EDITOR)

        public List<JCS_PfNode> path;
        private void OnDrawGizmos()
        {
            switch (mDirection)
            {
                case JCS_Vector3Direction.UP:
                    Gizmos.DrawWireCube(transform.position, new Vector3(mGridiWorldSize.x, mGridiWorldSize.y, 1));
                    break;
                case JCS_Vector3Direction.FORWARD:
                        Gizmos.DrawWireCube(transform.position, new Vector3(mGridiWorldSize.x, 1, mGridiWorldSize.y));
                    break;
            }

            if (mDisplayPathGizmos)
            {
                if (path != null)
                {
                    foreach (JCS_PfNode n in path)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(n.mWorldPosition, Vector3.one * (mNodeDiameter));
                    }
                }
            }
            else
            {
                if (mGrid != null)
                {
                    foreach (JCS_PfNode n in mGrid)
                    {
                        Gizmos.color = (n.Walkable) ? Color.white : Color.red;

                        if (path != null)
                            if (path.Contains(n))
                                Gizmos.color = Color.black;

                        Gizmos.DrawCube(n.mWorldPosition, Vector3.one * (mNodeDiameter));
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Get the nodes around.
        /// </summary>
        /// <param name="node"> node to use to find neighbours node. </param>
        /// <returns> list of node found are neighbours node. </returns>
        public List<JCS_PfNode> GetNeighbours(JCS_PfNode node)
        {
            List<JCS_PfNode> neighbours = new List<JCS_PfNode>();

            for (int x = -1;
                x <= 1;
                ++x)
            {
                for (int y = -1;
                    y <= 1;
                    ++y)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;

                    if (checkX >= 0 && checkX < mGridSizeX && checkY >= 0 && checkY < mGridSizeY)
                    {
                        neighbours.Add(mGrid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Find the node base on world position.
        /// </summary>
        /// <param name="worldPosition"> world position to use. </param>
        /// <returns> node found in the grid array base on the world position. </returns>
        public JCS_PfNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + mGridiWorldSize.x / 2) / mGridiWorldSize.x;
            float percentY = (worldPosition.z + mGridiWorldSize.y / 2) / mGridiWorldSize.y;

            // prevent out of grid map.
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((mGridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((mGridSizeY - 1) * percentY);

            return mGrid[x, y];
        }

        /// <summary>
        /// Create the grid.
        /// </summary>
        private void CreateGrid()
        {
            mGrid = new JCS_PfNode[mGridSizeX, mGridSizeY];

            Vector3 worldBottomLeft =
                transform.position -
                Vector3.right *
                mGridiWorldSize.x / 2 -
                JCS_Util.VectorDirection(mDirection) *
                mGridiWorldSize.y / 2;

            for (int x = 0;
                x < mGridSizeX;
                ++x)
            {
                for (int y = 0;
                y < mGridSizeY;
                ++y)
                {
                    Vector3 worldPoint =
                        worldBottomLeft +
                        Vector3.right *
                        (x * mNodeDiameter + mNodeRadius) +
                        JCS_Util.VectorDirection(mDirection) *
                        (y * mNodeDiameter + mNodeRadius);

                    bool walkable = !(Physics.CheckSphere(worldPoint, mNodeRadius, mUnwalkableMask));
                    mGrid[x, y] = new JCS_PfNode(walkable, worldPoint, x, y);
                }
            }
        }
    }
}
