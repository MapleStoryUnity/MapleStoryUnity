/**
 * $File: JCS_Physics.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Collider Information data structure.
    /// </summary>
    [Serializable]
    public struct JCS_ColliderInfo
    {
        public float width;
        public float height;

        // center position.
        public Vector3 centerPosition;

        // bounding point.
        public Vector3 topBound;
        public Vector3 bottomBound;
        public Vector3 leftBound;
        public Vector3 rightBound;

        /// <summary>
        /// Find out all four bounding point.
        /// </summary>
        public void MeasureBounding()
        {
            /* All apply to center position. */
            Vector3 topBound = centerPosition;
            Vector3 bottomBound = centerPosition;
            Vector3 leftBound = centerPosition;
            Vector3 rightBound = centerPosition;

            /* then start adding bounding value. */
            topBound.y += height / 2;
            bottomBound.y -= height / 2;

            leftBound.x += width / 2;
            rightBound.x -= width / 2;
        }
    };

    /// <summary>
    /// All physics util design here...
    /// </summary>
    public static class JCS_Physics
    {
        /// <summary>
        /// Return Character controller's collider's width and height.
        /// </summary>
        /// <param name="cap"> Character Controller component want to check. </param>
        /// <returns> x : width, y : height </returns>
        public static Vector2 GetColliderWidthHeight(CharacterController cap)
        {
            // holder
            Vector2 capWH = Vector2.zero;

            Vector3 capScale = cap.transform.localScale;

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            float cR = cap.radius * capScale.x;
            float cH = cap.height * capScale.y;

            if (cH < 0)
                cH = 0;

            // apply to holder
            capWH.x = cR * 2;
            capWH.y = cH;

            return capWH;
        }

        /// <summary>
        /// Return Box Collider's width and height.
        /// </summary>
        /// <param name="cap"> object we want to take from </param>
        /// <returns> x : width, y : height </returns>
        public static Vector2 GetColliderWidthHeight(BoxCollider rect)
        {
            // holder
            Vector2 widthHeight = Vector2.zero;

            Vector3 rectScale = rect.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;

            // apply to holder
            widthHeight.x = rWidth;
            widthHeight.y = rHeight;

            return widthHeight;
        }

        /// <summary>
        /// Return CharacterController's collider's center as 
        /// a global position.
        /// </summary>
        /// <returns> CharacterController's collider's center as a 
        /// global position. </returns>
        public static Vector3 GetColliderCenterPosition(CharacterController cap)
        {
            Vector3 capScale = cap.transform.localScale;

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            Vector3 capCenterPos = cap.transform.position + cCenter;

            return capCenterPos;
        }

        /// <summary>
        /// Return BoxCollider's center as a global position.
        /// </summary>
        /// <returns> box collider's center as a global position. </returns>
        public static Vector3 GetColliderCenterPosition(BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            // 取得Collider的中心"相對位置".
            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            return rectCenter + rect.transform.position;
        }

        /// <summary>
        /// Return Character Controller's info
        /// </summary>
        /// <param name="cap"> object we want to take from </param>
        /// <returns> width and height </returns>
        public static JCS_ColliderInfo GetColliderInfo(CharacterController cap)
        {
            // holder
            JCS_ColliderInfo capInfo = new JCS_ColliderInfo();

            Vector2 cWH = GetColliderWidthHeight(cap);
            capInfo.width = cWH.x;
            capInfo.height = cWH.y;

            Vector3 centerPos = GetColliderCenterPosition(cap);
            capInfo.centerPosition = centerPos;

            return capInfo;
        }

        /// <summary>
        /// Returns character controller's infomration.
        /// </summary>
        /// <param name="cap"></param>
        /// <returns></returns>
        public static JCS_ColliderInfo GetColliderInfo(BoxCollider rect)
        {
            JCS_ColliderInfo rectInfo = new JCS_ColliderInfo();

            Vector2 rWH = GetColliderWidthHeight(rect);

            // get width and height.
            rectInfo.width = rWH.x;
            rectInfo.height = rWH.y;

            Vector3 centerPos = GetColliderCenterPosition(rect);
            rectInfo.centerPosition = centerPos;

            rectInfo.MeasureBounding();

            return rectInfo;
        }

        /// <summary>
        /// Returns character controller's infomration.
        /// </summary>
        /// <param name="rect2d"></param>
        /// <returns> width and height </returns>
        public static Vector2 GetColliderInfo(BoxCollider2D rect2d)
        {
            Vector2 widthHeight = Vector2.zero;

            Vector3 rectScale = rect2d.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            float rWidth = rect2d.size.x * rectScale.x;
            float rHeight = rect2d.size.y * rectScale.y;

            // apply to holder
            widthHeight.x = rWidth;
            widthHeight.y = rHeight;

#if (UNITY_EDITOR)
            Vector3 right = rect2d.transform.position;
            right.x += widthHeight.x / 2;

            Debug.DrawLine(rect2d.transform.position, right, Color.gray);
#endif

            return widthHeight;
        }

        /// <summary>
        /// Set on top of the box above the ray.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="maxDistance"></param>
        public static bool SetOnTopOfClosestBoxWithRay(
            CharacterController cap, 
            float maxDistance, 
            JCS_Vector3Direction inDirection)
        {
            Vector3 direction = cap.transform.TransformDirection(JCS_Utility.VectorDirection(inDirection));

            RaycastHit[] hits = Physics.RaycastAll(cap.transform.position, direction, maxDistance);

            /* Nothing detected. */
            if (hits.Length == 0)
                return false;

            foreach (RaycastHit hit in hits)
            {
                /* Check if there are box collider on hit. */
                BoxCollider boxCollider = hit.transform.GetComponent<BoxCollider>();
                if (boxCollider == null)
                    continue;

                /* Check ray ignore attached. */
                JCS_RayIgnore ri = hit.transform.GetComponent<JCS_RayIgnore>();
                if (ri != null)
                    continue;

                // set on top of that box
                SetOnTopOfBox(cap, boxCollider);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Set on top of the box above the ray.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="maxDistance"></param>
        public static bool SetOnTopOfClosestBoxWithRay(BoxCollider box, float maxDistance)
        {
            Vector3 down = box.transform.TransformDirection(Vector3.down);

            RaycastHit[] hits = Physics.RaycastAll(box.transform.position, down, maxDistance);

            /* Nothing detected. */
            if (hits.Length == 0)
                return false;

            foreach (RaycastHit hit in hits)
            {
                /* Check if there are box collider on hit. */
                BoxCollider boxCollider = hit.transform.GetComponent<BoxCollider>();
                if (boxCollider == null)
                    continue;

                /* Check ray ignore attached. */
                JCS_RayIgnore ri = hit.transform.GetComponent<JCS_RayIgnore>();
                if (ri != null)
                    continue;

                // set on top of that box
                SetOnTopOfBox(box, boxCollider);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Set a collider on top of a box collider.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Vector3 SetOnTopOfBox(CharacterController cap, BoxCollider rect)
        {
            return SetOnTopOfBox(cap, rect, 0);
        }

        /// <summary>
        /// Set a collider on top of a box collider.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="rect"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector3 SetOnTopOfBox(CharacterController cap, BoxCollider rect, float offset)
        {
            Vector2 rWH = GetColliderWidthHeight(rect);
            Vector2 cWH = GetColliderWidthHeight(cap);

            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            // 取得Collider的中心"相對位置".
            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector3 newPos = cCenter + cap.transform.position;
            newPos.y = rectCenter.y + rect.transform.position.y + (rWH.y / 2.0f) + (cWH.y / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            cap.transform.position = newPos - cCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Set a collider on top of a box collider including measuring 
        /// the slope from the box collider.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="rect"></param>
        /// <param name="offset"></param>
        /// <returns>  
        /// Success : new position will fit ontop of box.
        /// Faild : Vector.zero
        /// </returns>
        public static Vector3 SetOnTopOfBoxWithSlope(
            CharacterController cap, 
            BoxCollider rect, 
            float offset = 0)
        {
            float innerAngle = rect.transform.localEulerAngles.z;
            // if no angle interact, just use the normal one.
            if (innerAngle == 0)
                return SetOnTopOfBox(cap, rect, offset);

            /* Capsule */
            Vector3 capScale = cap.transform.localScale;

            Vector2 cWH = GetColliderWidthHeight(cap);

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            Vector3 capCenterPos = cap.transform.position + cCenter;

            /* Box */
            Vector3 rectScale = rect.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            float rectWidth = rect.size.x * rectScale.x;
            float rectHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 rectCenterPos = rect.transform.position + rectCenter;

            float radianInnerAngle = JCS_Mathf.DegreeToRadian(innerAngle);

            // at here 'rectHeight' are the same as 'adjacent'.
            // hyp = adj / cos(Θ)
            /* 斜角度的高度. */
            float hypHeight = (rectHeight / Mathf.Cos(radianInnerAngle)) / 2;

            // 求'水平寬'.
            // adj = cos(Θ) * hyp
            float rectLeftToRight = Mathf.Cos(radianInnerAngle) * rectWidth;

            // 求'垂直高'.
            float rectTopAndBot = Mathf.Sin(radianInnerAngle) * rectWidth;

            float distRectLeftToCapCenter = JCS_Mathf.DistanceOfUnitVector(
                capCenterPos.x,
                rectCenterPos.x - (rectLeftToRight / 2));

            /* Check if the capsule inside the the rect on x-axis. */
            {
                float distCapToBox = JCS_Mathf.DistanceOfUnitVector(
                    capCenterPos.x,
                    rectCenterPos.x);

                if (distCapToBox > rectLeftToRight / 2)
                    return Vector3.zero;
            }

            float heightToCapBot = JCS_Mathf.CrossMultiply(
                rectLeftToRight,
                distRectLeftToCapCenter,
                rectTopAndBot);

#if (UNITY_EDITOR)
            Debug.DrawLine(
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y - (rectTopAndBot / 2),
                    rectCenterPos.z),
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2),
                    rectCenterPos.z),
                Color.cyan);
#endif

            // 在平台上的點.
            float pointOnPlatform = rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2);

            Vector3 newPos = capCenterPos;
            newPos.y = pointOnPlatform + (cWH.y / 2);
            cap.transform.position = newPos - cCenter;

            return newPos;
        }

        /// <summary>
        /// Set the box collider ontop of the other box collider
        /// </summary>
        /// <param name="setBox"> box we are setting on top of the other box </param>
        /// <param name="beSetBox"> box be under the setting box </param>
        /// <returns></returns>
        public static Vector3 SetOnTopOfBox(BoxCollider topBox, BoxCollider botBox, float offset = 0)
        {
            // WH stand for Width and Height
            Vector2 topBoxWH = GetColliderWidthHeight(topBox);
            Vector2 botBoxWH = GetColliderWidthHeight(botBox);

            Vector3 topBoxScale = topBox.transform.localScale;
            Vector3 botBoxScale = botBox.transform.localScale;

            topBoxScale = JCS_Mathf.AbsoluteValue(topBoxScale);
            botBoxScale = JCS_Mathf.AbsoluteValue(botBoxScale);

            // 取得Collider的中心"相對位置".
            Vector3 topBoxCenter = new Vector3(
                topBox.center.x * topBoxScale.x,
                topBox.center.y * topBoxScale.y,
                topBox.center.z * topBoxScale.z);

            Vector3 botBoxCenter = new Vector3(
                botBox.center.x * botBoxScale.x,
                botBox.center.y * botBoxScale.y,
                botBox.center.z * botBoxScale.z);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector3 newPos = topBoxCenter + topBox.transform.position;
            newPos.y = botBoxCenter.y + botBox.transform.position.y + (botBoxWH.y / 2.0f) + (topBoxWH.y / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            topBox.transform.position = newPos - topBoxCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Set the box collider ontop of the other box collider 
        /// with slop as a variable.
        /// </summary>
        /// <param name="setBox"> box we are setting on top of the other box </param>
        /// <param name="beSetBox"> box be under the setting box </param>
        /// <returns></returns>
        public static Vector3 SetOnTopOfBoxWithSlope(BoxCollider topBox, BoxCollider botBox, float offset = 0)
        {
            float innerAngle = botBox.transform.localEulerAngles.z;
            // if no angle interact, just use the normal one.
            if (innerAngle == 0)
                return SetOnTopOfBox(topBox, botBox);


            /* Top Box */
            Vector3 topBoxScale = topBox.transform.localScale;
            topBoxScale = JCS_Mathf.AbsoluteValue(topBoxScale);

            Vector2 topBoxWH = GetColliderWidthHeight(topBox);

            Vector3 topBoxCenter = new Vector3(
               topBox.center.x * topBoxScale.x,
               topBox.center.y * topBoxScale.y,
               topBox.center.z * topBoxScale.z);

            Vector3 topBoxCenterPos = topBox.transform.position + topBoxCenter;

            /* Bot Box */
            Vector3 rectScale = botBox.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            float rectWidth = botBox.size.x * rectScale.x;
            float rectHeight = botBox.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                botBox.center.x * rectScale.x,
                botBox.center.y * rectScale.y,
                botBox.center.z * rectScale.z);

            Vector3 rectCenterPos = botBox.transform.position + rectCenter;

            float radianInnerAngle = JCS_Mathf.DegreeToRadian(innerAngle);

            // at here 'rectHeight' are the same as 'adjacent'.
            // hyp = adj / cos(Θ)
            /* 斜角度的高度. */
            float hypHeight = (rectHeight / Mathf.Cos(radianInnerAngle)) / 2;

            // 求'水平寬'.
            // adj = cos(Θ) * hyp
            float rectLeftToRight = Mathf.Cos(radianInnerAngle) * rectWidth;

            // 求'垂直高'.
            float rectTopAndBot = Mathf.Sin(radianInnerAngle) * rectWidth;

            float distRectLeftToCapCenter = JCS_Mathf.DistanceOfUnitVector(
                topBoxCenterPos.x,
                rectCenterPos.x - (rectLeftToRight / 2));

            /* Check if the capsule inside the the rect on x-axis. */
            {
                float distCapToBox = JCS_Mathf.DistanceOfUnitVector(
                    topBoxCenterPos.x,
                    rectCenterPos.x);

                if (distCapToBox > rectLeftToRight / 2)
                    return Vector3.zero;
            }

            float heightToCapBot = JCS_Mathf.CrossMultiply(
                rectLeftToRight,
                distRectLeftToCapCenter,
                rectTopAndBot);

#if (UNITY_EDITOR)
            Debug.DrawLine(
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y - (rectTopAndBot / 2),
                    rectCenterPos.z),
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2),
                    rectCenterPos.z),
                Color.cyan);
#endif

            // 在平台上的點.
            float pointOnPlatform = rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2);

            /** ------------------------------------------- **/

            Vector3 newPos = topBoxCenterPos;
            newPos.y = pointOnPlatform + (topBoxWH.y / 2);
            topBox.transform.position = newPos - topBoxCenter;

            return newPos;
        }

        /// <summary>
        /// Set the box collider ontop of the other box collider
        /// </summary>
        /// <param name="setBox"> box we are setting on top of the other box </param>
        /// <param name="beSetBox"> box be under the setting box </param>
        /// <returns></returns>
        public static Vector2 SetOnTopOfBox(BoxCollider2D topBox, BoxCollider2D botBox, float offset = 0)
        {
            // WH stand for Width and Height
            Vector2 topBoxWH = GetColliderInfo(topBox);
            Vector2 botBoxWH = GetColliderInfo(botBox);

            Vector3 topBoxScale = topBox.transform.localScale;
            Vector3 botBoxScale = botBox.transform.localScale;

            topBoxScale = JCS_Mathf.AbsoluteValue(topBoxScale);
            botBoxScale = JCS_Mathf.AbsoluteValue(botBoxScale);

            // 取得Collider的中心"相對位置".
            Vector2 topBoxCenter = new Vector2(
                topBox.offset.x * topBoxScale.x,
                topBox.offset.y * topBoxScale.y);

            Vector2 botBoxCenter = new Vector2(
                botBox.offset.x * botBoxScale.x,
                botBox.offset.y * botBoxScale.y);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector2 newPos = new Vector2(
                topBoxCenter.x + topBox.transform.position.x,
                topBoxCenter.y + topBox.transform.position.y);

            newPos.y = botBoxCenter.y + botBox.transform.position.y + (botBoxWH.y / 2.0f) + (topBoxWH.y / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            topBox.transform.position = newPos - topBoxCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Set a collider at the bottom of the box collider.
        /// </summary>
        /// <param name="botBox"></param>
        /// <param name="topBox"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 SetOnBottomOfBox(BoxCollider2D botBox, BoxCollider2D topBox, float offset = 0)
        {
            // WH stand for Width and Height
            Vector2 botBoxWH = GetColliderInfo(botBox);
            Vector2 topBoxWH = GetColliderInfo(topBox);

            Vector3 botBoxScale = botBox.transform.localScale;
            Vector3 topBoxScale = topBox.transform.localScale;

            botBoxScale = JCS_Mathf.AbsoluteValue(botBoxScale);
            topBoxScale = JCS_Mathf.AbsoluteValue(topBoxScale);

            // 取得Collider的中心"相對位置".
            Vector2 botBoxCenter = new Vector2(
                botBox.offset.x * botBoxScale.x,
                botBox.offset.y * botBoxScale.y);

            Vector2 topBoxCenter = new Vector2(
                topBox.offset.x * topBoxScale.x,
                topBox.offset.y * topBoxScale.y);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector2 newPos = new Vector2(
                botBoxCenter.x + botBox.transform.position.x,
                botBoxCenter.y + botBox.transform.position.y);

            newPos.y = topBoxCenter.y + topBox.transform.position.y - (topBoxWH.y / 2.0f) - (botBoxWH.y / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            botBox.transform.position = newPos - botBoxCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Set a collider at the right of the box collider.
        /// </summary>
        /// <param name="leftBox"></param>
        /// <param name="rightBox"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 SetOnRightOfBox(BoxCollider2D leftBox, BoxCollider2D rightBox, float offset = 0)
        {
            // WH stand for Width and Height
            Vector2 leftBoxWH = GetColliderInfo(leftBox);
            Vector2 rightBoxWH = GetColliderInfo(rightBox);

            Vector3 leftBoxScale = leftBox.transform.localScale;
            Vector3 rightBoxScale = rightBox.transform.localScale;

            leftBoxScale = JCS_Mathf.AbsoluteValue(leftBoxScale);
            rightBoxScale = JCS_Mathf.AbsoluteValue(rightBoxScale);

            // 取得Collider的中心"相對位置".
            Vector2 leftBoxCenter = new Vector2(
                leftBox.offset.x * leftBoxScale.x,
                leftBox.offset.y * leftBoxScale.y);

            Vector2 rightBoxCenter = new Vector2(
                rightBox.offset.x * rightBoxScale.x,
                rightBox.offset.y * rightBoxScale.y);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector2 newPos = new Vector2(
                leftBoxCenter.x + leftBox.transform.position.x,
                leftBoxCenter.y + leftBox.transform.position.y);

            newPos.x = rightBoxCenter.x + rightBox.transform.position.x + (rightBoxWH.x / 2.0f) + (leftBoxWH.x / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            leftBox.transform.position = newPos - leftBoxCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Set a collider at the left of the box collider.
        /// </summary>
        /// <param name="rightBox"></param>
        /// <param name="leftBox"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 SetOnLeftOfBox(BoxCollider2D rightBox, BoxCollider2D leftBox, float offset = 0)
        {
            // WH stand for Width and Height
            Vector2 rightBoxWH = GetColliderInfo(rightBox);
            Vector2 leftBoxWH = GetColliderInfo(leftBox);

            Vector3 rightBoxScale = rightBox.transform.localScale;
            Vector3 leftBoxScale = leftBox.transform.localScale;

            rightBoxScale = JCS_Mathf.AbsoluteValue(rightBoxScale);
            leftBoxScale = JCS_Mathf.AbsoluteValue(leftBoxScale);

            // 取得Collider的中心"相對位置".
            Vector2 rightBoxCenter = new Vector2(
                rightBox.offset.x * rightBoxScale.x,
                rightBox.offset.y * rightBoxScale.y);

            Vector2 leftBoxCenter = new Vector2(
                leftBox.offset.x * leftBoxScale.x,
                leftBox.offset.y * leftBoxScale.y);

            // * rectCenter 跟 rect.transfrom.position 是"相對位置', 
            // 所以要相加才能得到正確的"世界位置"!
            // * cCenter + cap.transform.position 同理.
            Vector2 newPos = new Vector2(
                rightBoxCenter.x + rightBox.transform.position.x,
                rightBoxCenter.y + rightBox.transform.position.y);

            newPos.x = leftBoxCenter.x + leftBox.transform.position.x - (leftBoxWH.x / 2.0f) - (rightBoxWH.x / 2.0f) + offset;

            // 這裡要扣掉原本的中心位置
            rightBox.transform.position = newPos - rightBoxCenter;

            // optional
            return newPos;
        }

        /// <summary>
        /// Check if the character controller top of the box without 
        /// slope calculation.
        /// </summary>
        /// <param name="cap"> character controller to test. </param>
        /// <param name="rect"> box to test. </param>
        /// <returns>
        /// true : is top of box.
        /// false : vice versa.
        /// </returns>
        public static bool TopOfBox(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            //float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            if (cH < 0)
                cH = 0;

            float rTopBound = rect.transform.position.y + rectCenter.y + (rHeight / 2.0f);
            float cBotBound = cap.transform.position.y + cCenter.y - (cH / 2.0f) - cR;

#if (UNITY_EDITOR)
            Debug.DrawLine(rect.transform.position + rectCenter, 
                new Vector3(
                    rect.transform.position.x,
                    rTopBound,
                    rect.transform.position.z)
                , Color.cyan);

            Debug.DrawLine(cap.transform.position + cCenter,
                new Vector3(
                    cap.transform.position.x,
                    cBotBound,
                    cap.transform.position.z)
                , Color.blue);
#endif

            if (rTopBound <= cBotBound)
                return true;

            return false;
        }

        /// <summary>
        /// Check if the character controller top of the box with the 
        /// slop calculation.
        /// </summary>
        /// <param name="cap"> character controller to test. </param>
        /// <param name="rect"> box to test. </param>
        /// <returns>
        /// true : is top of box.
        /// false : vice versa.
        /// </returns>
        public static bool TopOfBoxWithSlope(CharacterController cap, BoxCollider rect)
        {
            float innerAngle = rect.transform.localEulerAngles.z;
            // if no angle interact, just use the normal one.
            if (innerAngle == 0)
                return TopOfBox(cap, rect);

            /* Capsule */
            Vector3 capScale = cap.transform.localScale;

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            Vector3 capCenterPos = cap.transform.position + cCenter;

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            // capsule botton point position.
            float cBotBound = cap.transform.position.y + cCenter.y - (cH / 2.0f) - cR;

            /* Box */
            Vector3 rectScale = rect.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);

            float rectWidth = rect.size.x * rectScale.x;
            float rectHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 rectCenterPos = rect.transform.position + rectCenter;

            float radianInnerAngle = JCS_Mathf.DegreeToRadian(innerAngle);

            // at here 'rectHeight' are the same as 'adjacent'.
            // hyp = adj / cos(Θ)
            /* 斜角度的高度. */
            float hypHeight = (rectHeight / Mathf.Cos(radianInnerAngle)) / 2;

            // 求'水平寬'.
            // adj = cos(Θ) * hyp
            float rectLeftToRight = Mathf.Cos(radianInnerAngle) * rectWidth;

            // 求'垂直高'.
            float rectTopAndBot = Mathf.Sin(radianInnerAngle) * rectWidth;

            float distRectLeftToCapCenter = JCS_Mathf.DistanceOfUnitVector(
                capCenterPos.x,
                rectCenterPos.x - (rectLeftToRight / 2));

            /* Check if the capsule inside the the rect on x-axis. */
            {
                float distCapToBox = JCS_Mathf.DistanceOfUnitVector(
                    capCenterPos.x,
                    rectCenterPos.x);

                if (distCapToBox > rectLeftToRight / 2)
                    return false;
            }

            float heightToCapBot = JCS_Mathf.CrossMultiply(
                rectLeftToRight, 
                distRectLeftToCapCenter, 
                rectTopAndBot);

#if (UNITY_EDITOR)
            Debug.DrawLine(
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y - (rectTopAndBot / 2),
                    rectCenterPos.z),
                new Vector3(
                    rectCenterPos.x - (rectLeftToRight / 2) + distRectLeftToCapCenter,
                    rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2),
                    rectCenterPos.z),
                Color.cyan);
#endif

            // 在平台上的點.
            float pointOnPlatform = rectCenterPos.y + heightToCapBot + hypHeight - (rectTopAndBot / 2);

            if (pointOnPlatform <= cBotBound)
                return true;

            return false;
        }

        /// <summary>
        /// Check if a collider under the bottom of the box collider.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool BottomOfBox(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            //float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            if (cH < 0)
                cH = 0;

            float cTopBound = cap.transform.position.y + cCenter.y + (cH / 2.0f) + cR;
            float rBotBound = rect.transform.position.y + rectCenter.y - (rHeight / 2.0f);

            if (cTopBound <= rBotBound)
                return true;

            return false;
        }

        /// <summary>
        /// Check if two collider collapse.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool JcsOnTriggerCheck(CharacterController cap, BoxCollider rect)
        {
            Vector3 rectScale = rect.transform.localScale;
            Vector3 capScale = cap.transform.localScale;

            rectScale = JCS_Mathf.AbsoluteValue(rectScale);
            capScale = JCS_Mathf.AbsoluteValue(capScale);

            float rWidth = rect.size.x * rectScale.x;
            float rHeight = rect.size.y * rectScale.y;
            //float widthSqr = JCS_Mathf.Sqr(rWidth);
            //float heightSqr = JCS_Mathf.Sqr(rHeight);
            //float diagonal = Mathf.Sqrt(widthSqr + heightSqr);

            rWidth = JCS_Mathf.AbsoluteValue(rWidth);
            rHeight = JCS_Mathf.AbsoluteValue(rHeight);

            Vector3 rectCenter = new Vector3(
                rect.center.x * rectScale.x,
                rect.center.y * rectScale.y,
                rect.center.z * rectScale.z);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            float offsetY = cCenter.y;
            float cHalfHeight = cH / 2.0f;

            if (cH < 0)
                cH = 0;

            Vector3 topCirclePoint = new Vector3(
                cap.transform.position.x + cCenter.x,
                cap.transform.position.y + cHalfHeight + offsetY,
                cap.transform.position.z + cCenter.z);
            Vector3 botCirclePoint = new Vector3(
                cap.transform.position.x + cCenter.x,
                cap.transform.position.y - cHalfHeight + offsetY,
                cap.transform.position.z + cCenter.z);


            float maxDistX = cR + (rWidth / 2.0f);
            float maxDistY = ((cH / 2.0f) + cR) + (rHeight / 2.0f);

            float realDistX = JCS_Mathf.DistanceOfUnitVector(
                cap.transform.position.x + cCenter.x,
                rect.transform.position.x + rectCenter.x);
            float realDistY = JCS_Mathf.DistanceOfUnitVector(
                cap.transform.position.y + cCenter.y,
                rect.transform.position.y + rectCenter.y);

            if (realDistX <= maxDistX &&
                realDistY <= maxDistY)
            {
                Vector2 rectPosOffset = new Vector2(
                    rect.transform.position.x + rectCenter.x,
                    rect.transform.position.y + rectCenter.y
                    );

                Rect r = new Rect();
                r.x = rectPosOffset.x - (rWidth / 2.0f);
                r.y = rectPosOffset.y + (rHeight / 2.0f);
                r.width = rectPosOffset.x + (rWidth / 2.0f);
                r.height = rectPosOffset.y - (rHeight / 2.0f);

                Vector2 topLeft = new Vector2(r.x, r.y);
                Vector2 topRight = new Vector2(r.width, r.y);
                Vector2 botRight = new Vector2(r.width, r.height);
                Vector2 botLeft = new Vector2(r.x, r.height);

#if (UNITY_EDITOR)
                //Debug.DrawLine(topLeft, topCirclePoint);
                //Debug.DrawLine(topRight, topCirclePoint);
                //Debug.DrawLine(botRight, topCirclePoint);
                //Debug.DrawLine(botLeft, topCirclePoint);

                //Debug.DrawLine(topLeft, botCirclePoint);
                //Debug.DrawLine(topRight, botCirclePoint);
                //Debug.DrawLine(botRight, botCirclePoint);
                //Debug.DrawLine(botLeft, botCirclePoint);
#endif

                // check if point intersect with each other
                float distance = 0;

                // top circle
                distance = Vector2.Distance(topLeft, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(topRight, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botRight, topCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botLeft, topCirclePoint);
                if (distance <= cR)
                    return true;

                // bot circle
                distance = Vector2.Distance(topLeft, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(topRight, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botRight, botCirclePoint);
                if (distance <= cR)
                    return true;

                distance = Vector2.Distance(botLeft, botCirclePoint);
                if (distance <= cR)
                    return true;
            }


            return false;
        }

        /// <summary>
        /// Returns the gameobject that the closest to the current 
        /// pass in transform.
        /// </summary>
        /// <returns></returns>
        public static RaycastHit2D FindClosestHit(RaycastHit2D[] list, Transform trans)
        {
            float distance = 0;
            RaycastHit2D closestHit = default(RaycastHit2D);

            bool first = false;

            foreach (RaycastHit2D hit in list)
            {
                // check null
                if (!hit)
                    continue;

                if (!first)
                {
                    // first assign
                    closestHit = hit;
                    distance = Vector2.Distance(hit.transform.position, trans.position);

                    // complete first assign.
                    first = false;

                    continue;
                }

                float tempDistance = Vector2.Distance(hit.transform.position, trans.position);

                // update the object.
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    closestHit = hit;
                }
            }

            return closestHit;
        }

        /// <summary>
        /// Get the collider position. 2D
        /// </summary>
        /// <param name="collider"> Collider u want to find. </param>
        /// <returns> Position of this collider. </returns>
        public static Vector2 GetColliderPosition(BoxCollider2D collider)
        {
            Vector2 newPos = collider.offset;

            newPos.x *= collider.transform.localScale.x;
            newPos.y *= collider.transform.localScale.y;

            return newPos + (Vector2)collider.transform.position;
        }

        /// <summary>
        /// Get the collider position.
        /// </summary>
        /// <param name="collider"> Collider u want to find. </param>
        /// <returns> Position of this collider. </returns>
        public static Vector3 GetColliderPosition(BoxCollider collider)
        {
            Vector3 newPos = collider.center;

            newPos.x *= collider.transform.localScale.x;
            newPos.y *= collider.transform.localScale.y;
            newPos.z *= collider.transform.localScale.z;

            return newPos + collider.transform.position;
        }

    }
}
