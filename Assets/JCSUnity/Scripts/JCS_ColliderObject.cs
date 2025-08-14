/**
 * $File: JCS_ColliderObject.cs $
 * $Date: 2020-05-08 14:19:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright ?2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Contain all possible collider component and information.
    /// </summary>
    public class JCS_ColliderObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_ColliderObject)")]

        [Tooltip("Type of the current collider.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ColliderType mColliderType = JCS_ColliderType.NONE;

        private CharacterController mCharacterController = null;
        private BoxCollider mBoxCollider = null;
        private SphereCollider mSphereCollider = null;
        private CapsuleCollider mCapsuleCollider = null;
        private BoxCollider2D mBoxCollider2D = null;
        private CircleCollider2D mCircleCollider2D = null;
        private CapsuleCollider2D mCapsuleCollider2D = null;

        /* Setter & Getter */

        public JCS_ColliderType colliderType { get { return this.mColliderType; } }

        public CharacterController characterController { get { return this.mCharacterController; } }
        public BoxCollider boxCollider { get { return this.mBoxCollider; } }
        public SphereCollider sphereCollider { get { return this.mSphereCollider; } }
        public CapsuleCollider capsuleCollider { get { return this.mCapsuleCollider; } }
        public BoxCollider2D boxCollider2D { get { return this.mBoxCollider2D; } }
        public CircleCollider2D circleCollider2D { get { return this.mCircleCollider2D; } }
        public CapsuleCollider2D capsuleCollider2D { get { return this.mCapsuleCollider2D; } }

        /* Functions */

        protected virtual void Awake()
        {
            DetectColliderOnce();
        }

        /// <summary>
        /// Identify the current collider type once.
        /// </summary>
        /// <returns>
        /// Type of the current collider.
        /// </returns>
        public JCS_ColliderType DetectColliderOnce()
        {
            this.mCharacterController = this.GetComponent<CharacterController>();
            this.mBoxCollider = this.GetComponent<BoxCollider>();
            this.mSphereCollider = this.GetComponent<SphereCollider>();
            this.mCapsuleCollider = this.GetComponent<CapsuleCollider>();
            this.mBoxCollider2D = this.GetComponent<BoxCollider2D>();
            this.mCircleCollider2D = this.GetComponent<CircleCollider2D>();
            this.mCapsuleCollider2D = this.GetComponent<CapsuleCollider2D>();

            if (mCharacterController) this.mColliderType = JCS_ColliderType.CHARACTER_CONTROLLER;
            if (mBoxCollider) this.mColliderType = JCS_ColliderType.BOX;
            if (mSphereCollider) this.mColliderType = JCS_ColliderType.SPHERE;
            if (mCapsuleCollider) this.mColliderType = JCS_ColliderType.CAPSULE;
            if (mBoxCollider2D) this.mColliderType = JCS_ColliderType.BOX_2D;
            if (mCircleCollider2D) this.mColliderType = JCS_ColliderType.CIRCLE_2D;
            if (mCapsuleCollider2D) this.mColliderType = JCS_ColliderType.CAPSULE_2D;

            return this.mColliderType;
        }

        /// <summary>
        /// Check if TYPE current collider type.
        /// </summary>
        /// <param name="type"> Collider type you want to confirm. </param>
        /// <returns>
        /// Return true, if TYPE is this collider type.
        /// Return false, if TYPE isn't this collider type.
        /// </returns>
        public bool IsColliderType(JCS_ColliderType type)
        {
            return this.mColliderType == type;
        }

        public Vector3 center
        {
            get
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        return mCharacterController.center;
                    case JCS_ColliderType.BOX:
                        return mBoxCollider.center;
                    case JCS_ColliderType.SPHERE:
                        return mSphereCollider.center;
                    case JCS_ColliderType.CAPSULE:
                        return mCapsuleCollider.center;
                    default:
                        Debug.LogWarning("No collider found");
                        return Vector3.zero;
                }
            }
            set
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        this.mCharacterController.center = value;
                        break;
                    case JCS_ColliderType.BOX:
                        this.mBoxCollider.center = value;
                        break;
                    case JCS_ColliderType.SPHERE:
                        this.mSphereCollider.center = value;
                        break;
                    case JCS_ColliderType.CAPSULE:
                        this.mCapsuleCollider.center = value;
                        break;
                    default:
                        Debug.LogWarning("No collider found");
                        break;
                }
            }
        }

        public Vector3 offset
        {
            get
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.BOX_2D:
                        return mBoxCollider2D.offset;
                    case JCS_ColliderType.CIRCLE_2D:
                        return mCircleCollider2D.offset;
                    case JCS_ColliderType.CAPSULE_2D:
                        return mCapsuleCollider2D.offset;
                    default:
                        Debug.LogWarning("No collider found");
                        return Vector3.zero;
                }
            }
            set
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.BOX_2D:
                        this.mBoxCollider2D.offset = value;
                        break;
                    case JCS_ColliderType.CIRCLE_2D:
                        this.mCircleCollider2D.offset = value;
                        break;
                    case JCS_ColliderType.CAPSULE_2D:
                        this.mCapsuleCollider2D.offset = value;
                        break;
                    default:
                        Debug.LogWarning("No collider found");
                        break;
                }
            }
        }

        public Vector3 size
        {
            get
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.BOX:
                        return mBoxCollider.size;
                    case JCS_ColliderType.BOX_2D:
                        return mBoxCollider2D.size;
                    case JCS_ColliderType.CAPSULE_2D:
                        return mCapsuleCollider2D.size;
                    default:
                        Debug.LogWarning("No collider found");
                        return Vector3.zero;
                }
            }
            set
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.BOX:
                        this.mBoxCollider.size = value;
                        break;
                    case JCS_ColliderType.BOX_2D:
                        this.mBoxCollider2D.size = value;
                        break;
                    case JCS_ColliderType.CAPSULE_2D:
                        this.mCapsuleCollider2D.size = value;
                        break;
                    default:
                        Debug.LogWarning("No collider found");
                        break;
                }
            }
        }

        public float radius
        {
            get
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        return mCharacterController.radius;
                    case JCS_ColliderType.SPHERE:
                        return mSphereCollider.radius;
                    case JCS_ColliderType.CAPSULE:
                        return mCapsuleCollider.radius;
                    case JCS_ColliderType.CIRCLE_2D:
                        return mCircleCollider2D.radius;
                    default:
                        Debug.LogWarning("No collider found");
                        return 0.0f;
                }
            }
            set
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        mCharacterController.radius = value;
                        break;
                    case JCS_ColliderType.SPHERE:
                        this.mSphereCollider.radius = value;
                        break;
                    case JCS_ColliderType.CAPSULE:
                        this.mCapsuleCollider.radius = value;
                        break;
                    case JCS_ColliderType.CIRCLE_2D:
                        this.mCircleCollider2D.radius = value;
                        break;
                    default:
                        Debug.LogWarning("No collider found");
                        break;
                }
            }
        }

        public float height
        {
            get
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        return mCharacterController.height;
                    case JCS_ColliderType.CAPSULE:
                        return mCapsuleCollider.height;
                }
                Debug.LogWarning("No collider found");
                return 0.0f;
            }
            set
            {
                switch (mColliderType)
                {
                    case JCS_ColliderType.CHARACTER_CONTROLLER:
                        this.mCharacterController.height = value;
                        break;
                    case JCS_ColliderType.CAPSULE:
                        this.mCapsuleCollider.height = value;
                        break;
                    default:
                        Debug.LogWarning("No collider found");
                        break;
                }
            }
        }
    }
}
