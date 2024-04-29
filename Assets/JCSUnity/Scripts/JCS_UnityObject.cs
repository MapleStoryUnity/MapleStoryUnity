/**
 * $File: JCS_UnityObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using UnityEngine.UI;
using MyBox;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Cross Unity system object.
    /// </summary>
    public class JCS_UnityObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_Unityobject)")]

        [Tooltip("Type of the Unity game object.")]
        [SerializeField]
        protected JCS_UnityObjectType mObjectType = JCS_UnityObjectType.SPRITE;

        //-- Game Object
        protected Renderer mRenderer = null;
        //-- UI
        protected Image mImage = null;
        protected RectTransform mRectTransform = null;
        //-- Sprite
        protected SpriteRenderer mSpriteRenderer = null;
        //-- Text
        protected Text mText = null;
#if TMP_PRO
        //-- Text Mesh Pro
        protected TextMeshPro mTextMesh = null;
#endif

        /* Setter & Getter */

        public void SetObjectType(JCS_UnityObjectType ob)
        {
            this.mObjectType = ob;
            UpdateUnityData();
        }
        public JCS_UnityObjectType GetObjectType() { return this.mObjectType; }
        public Image GetImage() { return this.mImage; }
        public Renderer GetRenderer() { return this.mRenderer; }
        public SpriteRenderer GetSpriteRenderer() { return this.mSpriteRenderer; }
        public RectTransform GetRectTransform() { return this.mRectTransform; }
        public Text GetText() { return this.mText; }
#if TMP_PRO
        public TextMeshPro GetTextMesh() { return this.mTextMesh; }
#endif

        /* Functions */

        protected virtual void Awake()
        {
            UpdateUnityData();
        }

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
        public virtual void UpdateUnityData()
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer = this.GetComponent<Renderer>();
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    this.mRectTransform = this.GetComponent<RectTransform>();
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
                    break;
                case JCS_UnityObjectType.TEXT:
                    this.mText = this.GetComponent<Text>();
                    this.mRectTransform = this.GetComponent<RectTransform>();
                    break;
#if TMP_PRO
                case JCS_UnityObjectType.TMP:
                    this.mTextMesh = this.GetComponent<TextMeshPro>();
                    break;
#endif
            }
        }

        /// <summary>
        /// Check if TYPE is current object type.
        /// </summary>
        /// <param name="type"> Object type you want to confirm. </param>
        /// <returns>
        /// Return true, if TYPE is this object type.
        /// Return false, if TYPE isn't this object type.
        /// </returns>
        public bool IsObjectType(JCS_UnityObjectType type)
        {
            return this.GetObjectType() == type;
        }

        /// <summary>
        /// Get/Set the local type.
        /// 
        /// GameObject -> Renderer
        /// Sprite -> Sprite Renderer
        /// UI -> Image
        /// Text -> Text
        /// </summary>
        public Component LocalType
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer;
                    case JCS_UnityObjectType.UI:
                        return this.mImage;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh;
#endif
                }

                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer = (Renderer)value;
                        break;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer = (SpriteRenderer)value;
                        break;
                    case JCS_UnityObjectType.UI:
                        this.mImage = (Image)value;
                        break;
                    case JCS_UnityObjectType.TEXT:
                        this.mText = (Text)value;
                        break;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh = (TextMeshPro)value;
                        break;
#endif
                }
            }
        }

        /// <summary>
        /// Get Current type's transform
        /// </summary>
        public Transform LocalTransform
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform;
#endif
                }

                JCS_Debug.LogError("Return default local position (This should not happens)");
                return this.transform;
            }
        }

        /// <summary>
        /// Get Current type's position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.position;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.position;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.position;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform.position;
#endif
                }

                JCS_Debug.LogError("Return default local position (This should not happens)");
                return this.transform.position;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.position = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.position = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.position = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.transform.position = value;
                        return;
#endif
                }

                JCS_Debug.LogError("Set default local position (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's position
        /// </summary>
        public Vector3 LocalPosition
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localPosition;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localPosition;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localPosition;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform.localPosition;
#endif
                }
                JCS_Debug.LogError("Return default local position (This should not happens)");
                return this.transform.localPosition;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localPosition = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.transform.localPosition = value;
                        return;
#endif
                }

                JCS_Debug.LogError("Set default local position (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's rotation
        /// </summary>
        public Vector3 EulerAngles
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.eulerAngles;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.eulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.eulerAngles;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform.eulerAngles;
#endif
                }

                JCS_Debug.LogError("Return default local rotation (This should not happens)");
                return this.transform.eulerAngles;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.eulerAngles = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.eulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.eulerAngles = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.transform.eulerAngles = value;
                        return;
#endif
                }

                JCS_Debug.LogError("Set default local rotation (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's rotation
        /// </summary>
        public Vector3 LocalEulerAngles
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localEulerAngles;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localEulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localEulerAngles;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform.localEulerAngles;
#endif
                }

                JCS_Debug.LogError("Return default local rotation (This should not happens)");
                return this.transform.localEulerAngles;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localEulerAngles = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.transform.localEulerAngles = value;
                        return;
#endif
                }

                JCS_Debug.LogError("Set default local rotation (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's scale
        /// </summary>
        public Vector3 LocalScale
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localScale;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localScale;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localScale;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.transform.localScale;
#endif
                }
                JCS_Debug.LogError("Return default local scale (This should not happens)");
                return this.transform.localScale;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localScale = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localScale = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localScale = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.transform.localScale = value;
                        return;
#endif
                }
                JCS_Debug.LogError("Set default local scale (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's Enable
        /// </summary>
        public bool LocalEnabled
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.enabled;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.enabled;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.enabled;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.enabled;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.enabled;
#endif
                }
                JCS_Debug.LogError("Return default visible (This should not happens)");
                return false;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer.enabled = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.enabled = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.enabled = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        this.mText.enabled = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.enabled = value;
                        return;
#endif
                }
            }
        }

        /// <summary>
        /// Get current type's color
        /// </summary>
        public Color LocalColor
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.color;
#endif
                }

                JCS_Debug.LogError("Return default Local Red (This should not happens)");
                return new Color(255, 128, 64, 32);
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer.material.color = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.color = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.color = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        this.mText.color = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.color = value;
                        return;
#endif

                }
                JCS_Debug.LogError("Set default Local Red (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's alpha
        /// </summary>
        public float LocalAlpha
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.a;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.a;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.a;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.a;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.color.a;
#endif
                }
                JCS_Debug.LogError("Return default Local Alpha (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.a = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.a = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.a = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.a = value;
                            this.mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = this.mTextMesh.color;
                            newColor.a = value;
                            this.mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                JCS_Debug.LogError("Set default Local Alpha (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's red
        /// </summary>
        public float LocalRed
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.r;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.r;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.r;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.r;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.color.r;
#endif
                }
                JCS_Debug.LogError("Return default Local Red (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.r = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.r = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.r = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.r = value;
                            this.mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = this.mTextMesh.color;
                            newColor.r = value;
                            this.mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                JCS_Debug.LogError("Set default Local Red (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's green
        /// </summary>
        public float LocalGreen
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.g;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.g;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.g;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.g;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.color.g;
#endif
                }
                JCS_Debug.LogError("Return default Local Green (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.g = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.g = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.g = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.g = value;
                            this.mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = this.mTextMesh.color;
                            newColor.g = value;
                            this.mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                JCS_Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's blue
        /// </summary>
        public float LocalBlue
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.b;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.b;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.b;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.b;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.color.b;
#endif
                }
                JCS_Debug.LogError("Return default Local Blue (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.b = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.b = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.b = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.b = value;
                            this.mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = this.mTextMesh.color;
                            newColor.b = value;
                            this.mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                JCS_Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's main texture
        /// </summary>
        public Texture LocalMainTexture
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.mainTexture;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.material.mainTexture;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.material.mainTexture;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.material.mainTexture;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return this.mTextMesh.material.mainTexture;
#endif
                }
                JCS_Debug.LogError("Return default Local Blue (This should not happens)");
                return null;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        this.mText.material.mainTexture = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        this.mTextMesh.material.mainTexture = value;
                        return;
#endif
                }
                JCS_Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get the sprite.
        /// </summary>
        public Sprite LocalSprite
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.sprite;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.sprite;
                }
                JCS_Debug.LogError("Failed to get sprite composite cuz current unity object setting does not have it.");
                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.sprite = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.sprite = value;
                        return;
                }
                JCS_Debug.LogError("Failed to set the sprite cuz the current unity object setting does not have sprite coposite.");
            }
        }

        /// <summary>
        /// Check if the object is rendering on the 
        /// screen / render field.
        /// 
        /// NOTE(jenchieh): This will effect by the scene camera.
        /// ...
        /// Unity, seriously?
        /// </summary>
        public bool LocalIsVisible
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.isVisible;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.isVisible;
                }
                JCS_Debug.LogError("Return default Local isVisible (This should not happens)");
                return false;
            }

        }

        /// <summary>
        /// Set the same flip x. If not SpriteRenderer 
        /// use negative scale instead.
        /// </summary>
        public bool LocalFlipX
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return JCS_Mathf.IsPositive(this.transform.localScale.x);
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.flipX;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        return JCS_Mathf.IsPositive(this.mRectTransform.localScale.x);
                }
                JCS_Debug.LogError("Return default Local FlipX (This should not happens)");
                return false;
            }

            set
            {
                Vector3 newScale;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newScale = this.transform.localScale;

                            if (value)
                                newScale.x = JCS_Mathf.ToPositive(newScale.x);
                            else
                                newScale.x = JCS_Mathf.ToNegative(newScale.x);

                            this.transform.localScale = newScale;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.flipX = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        {
                            newScale = this.mRectTransform.localScale;

                            if (value)
                                newScale.x = JCS_Mathf.ToPositive(newScale.x);
                            else
                                newScale.x = JCS_Mathf.ToNegative(newScale.x);

                            this.mRectTransform.localScale = newScale;
                        }
                        return;
                }
                JCS_Debug.LogError("Set default Local FlipX (This should not happens)");
            }
        }

        /// <summary>
        /// Set the same flip y. If not SpriteRenderer 
        /// use negative scale instead.
        /// </summary>
        public bool LocalFlipY
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return JCS_Mathf.IsPositive(this.transform.localScale.y);
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.flipY;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        return JCS_Mathf.IsPositive(this.mRectTransform.localScale.y);
                }
                JCS_Debug.LogError("Return default Local FlipY (This should not happens)");
                return false;
            }

            set
            {
                Vector3 newScale;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newScale = this.transform.localScale;

                            if (value)
                                newScale.y = JCS_Mathf.ToPositive(newScale.y);
                            else
                                newScale.y = JCS_Mathf.ToNegative(newScale.y);

                            this.transform.localScale = newScale;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.flipY = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        {
                            newScale = this.mRectTransform.localScale;

                            if (value)
                                newScale.y = JCS_Mathf.ToPositive(newScale.y);
                            else
                                newScale.y = JCS_Mathf.ToNegative(newScale.y);

                            this.mRectTransform.localScale = newScale;
                        }
                        return;
                }
                JCS_Debug.LogError("Set default Local FlipY (This should not happens)");
            }
        }
    }
}
