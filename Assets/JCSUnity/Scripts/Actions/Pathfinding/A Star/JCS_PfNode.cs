/**
 * $File: JCS_PfNode.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Path finding node.
    /// </summary>
    public class JCS_PfNode 
        : IHeapItem<JCS_PfNode>
    {
        /* Variables */

        public bool Walkable = false;
        public Vector3 mWorldPosition = Vector3.zero;
        public int GridX;
        public int GridY;

        public int gCost;
        public int hCost;

        public JCS_PfNode Parent;

        private int mHeapIndex;

        /* Setter & Getter */

        public int fCost { get { return gCost + hCost; } }
        public int HeapIndex { get { return this.mHeapIndex; } set { this.mHeapIndex = value; } }

        /* Functions */

        public JCS_PfNode(bool walkable, Vector3 worldPos, int gridX, int gridY)
        {
            this.Walkable = walkable;
            this.mWorldPosition = worldPos;
            this.GridX = gridX;
            this.GridY = gridY;
        }

        /// <summary>
        /// Compre the two nodes.
        /// </summary>
        /// <param name="nodeToCompare"></param>
        /// <returns></returns>
        public int CompareTo(JCS_PfNode nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);

            if (compare == 0)
                compare = hCost.CompareTo(nodeToCompare.hCost);

            return -compare;
        }
    }
}
