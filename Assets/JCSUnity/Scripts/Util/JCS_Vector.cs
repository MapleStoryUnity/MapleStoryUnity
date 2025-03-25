/**
 * $File: JCS_Vector.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Vector utilities.
    /// </summary>
    public static class JCS_Vector
    {
        /* Variables */

        // Distance to look in direction to get the point.
        public const float LOOK_DISTANCE = 10.0f;

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Set the vector value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 SetVecX(Vector3 val, float x)
        {
            return SetVec3(val, x, val.y, val.z);
        }
        public static Vector3 SetVecY(Vector3 val, float y)
        {
            return SetVec3(val, val.x, y, val.z);
        }
        public static Vector3 SetVecZ(Vector3 val, float z)
        {
            return SetVec3(val, val.x, val.y, z);
        }
        public static Vector3 SetVec3(Vector3 val, float x, float y, float z)
        {
            Vector3 newVec = val;

            newVec.x = x;
            newVec.y = y;
            newVec.z = z;

            val = newVec;

            return newVec;
        }

        /// <summary>
        /// Add the vector value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 IncVecX(Vector3 val, float x)
        {
            return IncVec3(val, x, 0, 0);
        }
        public static Vector3 IncVecY(Vector3 val, float y)
        {
            return IncVec3(val, 0, y, 0);
        }
        public static Vector3 IncVecZ(Vector3 val, float z)
        {
            return IncVec3(val, 0, 0, z);
        }
        public static Vector3 IncVec3(Vector3 val, float x, float y = 0, float z = 0)
        {
            Vector3 newVec = val;

            newVec.x += x;
            newVec.y += y;
            newVec.z += z;

            val = newVec;

            return newVec;
        }

        /// <summary>
        /// Return direction of Unity's Vector system.
        /// </summary>
        /// <param name="direction"> Target direction. </param>
        /// <returns> Direction vector. </returns>
        public static Vector3 Direction(JCS_Vector3Direction direction)
        {
            switch (direction)
            {
                case JCS_Vector3Direction.CENTER: return Vector3.zero;
                case JCS_Vector3Direction.UP: return Vector3.up;
                case JCS_Vector3Direction.DOWN: return Vector3.down;
                case JCS_Vector3Direction.FORWARD: return Vector3.forward;
                case JCS_Vector3Direction.BACK: return Vector3.back;
                case JCS_Vector3Direction.RIGHT: return Vector3.right;
                case JCS_Vector3Direction.LEFT: return Vector3.left;

                case JCS_Vector3Direction.FORWARD_LEFT:
                    return new Vector3(-1, 0, 1);
                case JCS_Vector3Direction.FORWARD_RIGHT:
                    return new Vector3(1, 0, 1);

                case JCS_Vector3Direction.BACK_LEFT:
                    return new Vector3(-1, 0, -1);
                case JCS_Vector3Direction.BACK_RIGHT:
                    return new Vector3(1, 0, -1);

                case JCS_Vector3Direction.UP_LEFT:
                    return new Vector3(-1, 1, 0);
                case JCS_Vector3Direction.UP_RIGHT:
                    return new Vector3(1, 1, 0);

                case JCS_Vector3Direction.FORWARD_UP:
                    return new Vector3(0, 1, 1);
                case JCS_Vector3Direction.FORWARD_DOWN:
                    return new Vector3(0, -1, 1);

                case JCS_Vector3Direction.BACK_UP:
                    return new Vector3(0, 1, -1);
                case JCS_Vector3Direction.BACK_DOWN:
                    return new Vector3(0, -1, -1);

                case JCS_Vector3Direction.DOWN_LEFT:
                    return new Vector3(-1, -1, 0);
                case JCS_Vector3Direction.DOWN_RIGHT:
                    return new Vector3(1, -1, 0);

                case JCS_Vector3Direction.FORWARD_UP_LEFT:
                    return new Vector3(-1, 1, 1);
                case JCS_Vector3Direction.FORWARD_UP_RIGHT:
                    return new Vector3(1, 1, 1);

                case JCS_Vector3Direction.FORWARD_DOWN_LEFT:
                    return new Vector3(-1, -1, 1);
                case JCS_Vector3Direction.FORWARD_DOWN_RIGHT:
                    return new Vector3(1, -1, 1);

                case JCS_Vector3Direction.BACK_UP_LEFT:
                    return new Vector3(-1, 1, -1);
                case JCS_Vector3Direction.BACK_UP_RIGHT:
                    return new Vector3(1, 1, -1);

                case JCS_Vector3Direction.BACK_DOWN_LEFT:
                    return new Vector3(-1, -1, -1);
                case JCS_Vector3Direction.BACK_DOWN_RIGHT:
                    return new Vector3(1, -1, -1);
            }

            // this cannot happens
            return Vector3.zero;
        }

        /// <summary>
        /// Get the vector base on the transform's rotation.
        /// </summary>
        /// <param name="direction"> Target direction. </param>
        /// <param name="trans"> 
        /// Target transform use to calculate the vector, and where we get the 
        /// rotation from. 
        /// </param>
        /// <returns> Direction vector. </returns>
        public static Vector3 Direction(JCS_Vector3Direction direction, Transform trans)
        {
            switch (direction)
            {
                case JCS_Vector3Direction.CENTER: return Vector3.zero;

                case JCS_Vector3Direction.UP: return trans.up;
                case JCS_Vector3Direction.DOWN: return -trans.up;

                case JCS_Vector3Direction.FORWARD: return trans.forward;
                case JCS_Vector3Direction.BACK: return -trans.forward;

                case JCS_Vector3Direction.RIGHT: return trans.right;
                case JCS_Vector3Direction.LEFT: return -trans.right;

                case JCS_Vector3Direction.FORWARD_LEFT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.FORWARD_RIGHT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.BACK_LEFT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.BACK_RIGHT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.UP_LEFT:
                    return Direction(JCS_Vector3Direction.UP, trans)
                        + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.UP_RIGHT:
                    return Direction(JCS_Vector3Direction.UP, trans)
                        + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.FORWARD_UP:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.UP, trans);
                case JCS_Vector3Direction.FORWARD_DOWN:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.DOWN, trans);

                case JCS_Vector3Direction.BACK_UP:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                       + Direction(JCS_Vector3Direction.UP, trans);
                case JCS_Vector3Direction.BACK_DOWN:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                       + Direction(JCS_Vector3Direction.DOWN, trans);

                case JCS_Vector3Direction.DOWN_LEFT:
                    return Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.DOWN_RIGHT:
                    return Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.FORWARD_UP_LEFT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.UP, trans)
                       + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.FORWARD_UP_RIGHT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.UP, trans)
                       + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.FORWARD_DOWN_LEFT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.FORWARD_DOWN_RIGHT:
                    return Direction(JCS_Vector3Direction.FORWARD, trans)
                        + Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.BACK_UP_LEFT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.UP, trans)
                       + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.BACK_UP_RIGHT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.UP, trans)
                       + Direction(JCS_Vector3Direction.RIGHT, trans);

                case JCS_Vector3Direction.BACK_DOWN_LEFT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.LEFT, trans);
                case JCS_Vector3Direction.BACK_DOWN_RIGHT:
                    return Direction(JCS_Vector3Direction.BACK, trans)
                        + Direction(JCS_Vector3Direction.DOWN, trans)
                       + Direction(JCS_Vector3Direction.RIGHT, trans);
            }

            // this cannot happens
            return Vector3.zero;
        }

        /// <summary>
        /// Add random value to Vector3
        /// </summary>
        /// <param name="trans"> transfrorm u want to apply. </param>
        /// <param name="randVec"> value for each axis. </param>
        /// <param name="checks"> check for eaxh axis. </param>
        /// <returns> transform result. </returns>
        public static Vector3 ApplyRandVector3(Vector3 trans, Vector3 randVec, JCS_Bool3 checks)
        {
            Vector3 tempVec = trans;

            if (checks.check1)
            {
                float val = JCS_Random.Range(-randVec.x, randVec.x);
                tempVec = IncVecX(tempVec, val);
            }

            if (checks.check2)
            {
                float val = JCS_Random.Range(-randVec.y, randVec.y);
                tempVec = IncVecY(tempVec, val);
            }

            if (checks.check3)
            {
                float val = JCS_Random.Range(-randVec.z, randVec.z);
                tempVec = IncVecZ(tempVec, val);
            }

            trans = tempVec;

            return tempVec;
        }

        /// <summary>
        /// Multiply all the parent localEulerAngles to get the correct 
        /// description of the transform information. 
        /// 
        /// ATTENTION(jenchieh): This will cause some performance, use 
        /// it wisely.
        /// </summary>
        /// <param name="trans"> transform we want to get from and use 
        /// it for parent. </param>
        /// <param name="inEulerAngles"> use to store the result. </param>
        /// <returns> Accumilate the result. </returns>
        public static Vector3 GetFinalLocalEulerAngles(Transform trans, ref Vector3 inEulerAngles)
        {
            if (trans.parent == null)
                return inEulerAngles;

            Vector3 parentVal = trans.parent.transform.localEulerAngles;

            inEulerAngles = SetVec3(
                inEulerAngles,
                parentVal.x * inEulerAngles.x,
                parentVal.y * inEulerAngles.y,
                parentVal.z * inEulerAngles.z);

            return GetFinalLocalEulerAngles(trans.parent, ref inEulerAngles);
        }

        /// <summary>
        /// Make the vector to a ground vector.
        /// </summary>
        public static Vector3 GroundVector(Vector3 vec, float y = 0.0f)
        {
            return new Vector3(vec.x, y, vec.z);
        }
        public static Vector2 GroundVector(Vector2 vec, float y = 0.0f)
        {
            return new Vector3(vec.x, y);
        }

        /// <summary>
        /// How close player looks toward the target block.
        /// 
        /// Copied from https://docs.unity3d.com/ScriptReference/Vector3.Angle.html.
        /// </summary>
        public static float LookDegree(Transform self, Transform target, JCS_Vector3Direction direct = JCS_Vector3Direction.FORWARD)
        {
            return LookDegree(self, target.position, direct);
        }
        public static float LookDegree(Transform self, Vector3 target, JCS_Vector3Direction direct = JCS_Vector3Direction.FORWARD)
        {
            Vector3 pos = self.position;
            pos.y = target.y;
            Vector3 targetDir = target - pos;
            Vector3 direction = Direction(direct, self);
            float angle = Vector3.Angle(targetDir, direction);
            return angle;
        }
        public static float SignedLookDegree(Transform self, Transform target, JCS_Vector3Direction direct = JCS_Vector3Direction.FORWARD)
        {
            return SignedLookDegree(self, target.position, direct);
        }
        public static float SignedLookDegree(Transform self, Vector3 target, JCS_Vector3Direction direct = JCS_Vector3Direction.FORWARD)
        {
            Vector3 pos = self.position;
            pos.y = target.y;
            Vector3 targetDir = target - pos;
            Vector3 direction = Direction(direct, self);
            float angle = Vector3.SignedAngle(targetDir, direction, Vector3.up);
            return angle;
        }

        /// <summary>
        /// Check if distance between pos1 and pos2 is under the threshold.
        /// </summary>
        public static bool InDistance(Vector3 pos1, Vector3 pos2, float threshold)
        {
            float distance = Vector3.Distance(pos1, pos2);
            return (distance <= threshold);
        }

        /// <summary>
        /// Return the point in specific direction.
        /// </summary>
        public static Vector3 PointInDirection(JCS_Vector3Direction direction, Transform trans)
        {
            return PointInDirection(direction, trans, LOOK_DISTANCE);
        }
        public static Vector3 PointInDirection(JCS_Vector3Direction direction, Transform trans, float distance)
        {
            Vector3 dir = Direction(direction, trans);

            return trans.position + dir * distance;
        }
    }
}
