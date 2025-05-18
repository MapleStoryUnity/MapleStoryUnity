/**
 * $File: Debug.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Debugging class.
    /// </summary>
    public static class JCS_Debug
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Debug draw line.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void DrawLine(Vector2 from, Vector2 to)
        {
            var tempFrom = new Vector3(from.x, from.y, 0);
            var tempTo = new Vector3(to.x, to.y, 0);

            Debug.DrawLine(tempFrom, tempTo);
        }
        /// <summary>
        /// Debug draw line.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="col"></param>
        public static void DrawLine(Vector2 from, Vector2 to, Color col)
        {
            Vector3 tempFrom = new Vector3(from.x, from.y, 0);
            Vector3 tempTo = new Vector3(to.x, to.y, 0);

            Debug.DrawLine(tempFrom, tempTo, col);
        }
        /// <summary>
        /// Debug draw line.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Debug.DrawLine(from, to);
        }
        /// <summary>
        /// Debug draw line.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="col"></param>
        public static void DrawLine(Vector3 from, Vector3 to, Color col)
        {
            Debug.DrawLine(from, to, col);
        }

        /// <summary>
        /// Debug draw rectangle shape.
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="botRight"></param>
        /// <param name="botLeft"></param>
        /// <param name="col"></param>
        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft, Color col)
        {
            Debug.DrawLine(topLeft, topRight, col);
            Debug.DrawLine(topRight, botRight, col);
            Debug.DrawLine(botRight, botLeft, col);
            Debug.DrawLine(botLeft, topLeft, col);
        }
        /// <summary>
        /// Debug draw rectangle shape.
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="botRight"></param>
        /// <param name="botLeft"></param>
        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft)
        {
            // set default color as white
            DrawRect(topLeft, topRight, botRight, botLeft, Color.white);
        }

        /// <summary>
        /// Draw the collider
        /// </summary>
        /// <param name="collider"> Collider u want to draw. </param>
        /// <param name="col"> Color type. </param>
        public static void DrawCollider(BoxCollider2D collider, Color col)
        {
#if UNITY_EDITOR
            // get width and height information.
            Vector2 boxInfo = JCS_Physics.GetColliderInfo(collider);

            Vector3 pos = JCS_Physics.GetColliderPosition(collider);

            var topLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            var topRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            var botRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y - boxInfo.y / 2);
            var botLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y - boxInfo.y / 2);

            DrawRect(topLeft, topRight, botRight, botLeft, col);
#endif
        }

        /// <summary>
        /// Draw the collider, use for check last frame.
        /// </summary>
        /// <param name="collider"> Collider u want to draw. </param>
        /// <param name="col"> Color type. </param>
        /// <param name="origin">Provide the origin position. </param>
        public static void DrawCollider(BoxCollider2D collider, Color col, Vector3 origin)
        {
#if UNITY_EDITOR
            // get width and height information.
            Vector2 boxInfo = JCS_Physics.GetColliderInfo(collider);

            Vector3 pos = origin;

            var topLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            var topRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            var botRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y - boxInfo.y / 2);
            var botLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y - boxInfo.y / 2);

            DrawRect(topLeft, topRight, botRight, botLeft, col);
#endif
        }

    }
}
