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

using System.Collections.Generic;
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

        [Separator("🌱 Initialize Variables (JCS_Unityobject)")]

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
        //-- Text Mesh 3D/UI
        protected TMP_Text mTextMesh = null;
#endif

        [Header("🔍 Material")]

        [Tooltip("Make material clone.")]
        [SerializeField]
        private bool mMakeUnique = false;

        [Tooltip("Shader's color properties.")]
        [SerializeField]
        private List<string> mColorProps = null;

        /* Setter & Getter */

        public void SetObjectType(JCS_UnityObjectType ob)
        {
            mObjectType = ob;
            UpdateUnityData();
        }
        public JCS_UnityObjectType GetObjectType() { return mObjectType; }
        public Image GetImage() { return mImage; }
        public Renderer GetRenderer() { return mRenderer; }
        public SpriteRenderer GetSpriteRenderer() { return mSpriteRenderer; }
        public RectTransform GetRectTransform() { return mRectTransform; }
        public Text GetText() { return mText; }
#if TMP_PRO
        public TMP_Text GetTextMesh() { return mTextMesh; }
#endif
        public bool makeUnique { get { return mMakeUnique; } set { mMakeUnique = value; } }
        public List<string> colorProps { get { return mColorProps; } set { mColorProps = value; } }

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
                    mRenderer = GetComponent<Renderer>();
                    break;
                case JCS_UnityObjectType.UI:
                    mImage = GetComponent<Image>();
                    mRectTransform = GetComponent<RectTransform>();
                    break;
                case JCS_UnityObjectType.SPRITE:
                    mSpriteRenderer = GetComponent<SpriteRenderer>();
                    break;
                case JCS_UnityObjectType.TEXT:
                    mText = GetComponent<Text>();
                    mRectTransform = GetComponent<RectTransform>();
                    break;
#if TMP_PRO
                case JCS_UnityObjectType.TMP:
                    mTextMesh = GetComponent<TMP_Text>();
                    break;
#endif
            }

            UpdateMaterial();
        }

        /// <summary>
        /// Make unique material.
        /// </summary>
        public virtual void UpdateMaterial(bool force = false)
        {
            if (!mMakeUnique && !force)
                return;

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    {
                        JCS_Loop.ForEach(mRenderer.materials, (index, mat) =>
                        {
                            mRenderer.materials[index] = Instantiate(mat);
                        });
                    }
                    break;
                case JCS_UnityObjectType.UI:
                    {
                        if (mImage != null)
                        {
                            mImage.material = Instantiate(mImage.material);
                        }
                    }
                    break;
                case JCS_UnityObjectType.SPRITE:
                    {
                        JCS_Loop.ForEach(mSpriteRenderer.materials, (index, mat) =>
                        {
                            mSpriteRenderer.materials[index] = Instantiate(mat);
                        });
                    }
                    break;
                case JCS_UnityObjectType.TEXT:
                    {
                        mText.material = Instantiate(mText.material);
                    }
                    break;
#if TMP_PRO
                case JCS_UnityObjectType.TMP:
                    {
                        mTextMesh.material = Instantiate(mTextMesh.material);
                    }
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
            return GetObjectType() == type;
        }

        /// <summary>
        /// Get/Set the local type.
        /// 
        /// GameObject -> Renderer
        /// Sprite -> Sprite Renderer
        /// UI -> Image
        /// Text -> Text
        /// </summary>
        public Component localType
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return mRenderer;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer;
                    case JCS_UnityObjectType.UI:
                        return mImage;
                    case JCS_UnityObjectType.TEXT:
                        return mText;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh;
#endif
                }

                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        mRenderer = (Renderer)value;
                        break;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer = (SpriteRenderer)value;
                        break;
                    case JCS_UnityObjectType.UI:
                        mImage = (Image)value;
                        break;
                    case JCS_UnityObjectType.TEXT:
                        mText = (Text)value;
                        break;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh = (TMP_Text)value;
                        break;
#endif
                }
            }
        }

        /// <summary>
        /// Get Current type's transform.
        /// </summary>
        public Transform localTransform
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform;
#endif
                }

                Debug.LogError("Return default local position (This should not happens)");
                return transform;
            }
        }

        /// <summary>
        /// Return the current type's material.
        /// </summary>
        public Material localMaterial
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return mRenderer.material;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.material;
                    case JCS_UnityObjectType.UI:
                        return mImage.material;
                    case JCS_UnityObjectType.TEXT:
                        return mText.material;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.material;
#endif
                }

                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        mRenderer.material = value;
                        break;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.material = value;
                        break;
                    case JCS_UnityObjectType.UI:
                        mImage.material = value;
                        break;
                    case JCS_UnityObjectType.TEXT:
                        mText.material = value;
                        break;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.material = value;
                        break;
#endif
                }
            }
        }

        /// <summary>
        /// Get Current type's position
        /// </summary>
        public Vector3 position
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform.position;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform.position;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform.position;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform.position;
#endif
                }

                Debug.LogError("Return default local position (This should not happens)");
                return transform.position;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        transform.position = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        mRectTransform.position = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.transform.position = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.transform.position = value;
                        return;
#endif
                }

                Debug.LogError("Set default local position (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's position
        /// </summary>
        public Vector3 localPosition
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform.localPosition;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform.localPosition;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform.localPosition;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform.localPosition;
#endif
                }
                Debug.LogError("Return default local position (This should not happens)");
                return transform.localPosition;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        transform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        mRectTransform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.transform.localPosition = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.transform.localPosition = value;
                        return;
#endif
                }

                Debug.LogError("Set default local position (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's rotation
        /// </summary>
        public Vector3 eulerAngles
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform.eulerAngles;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform.eulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform.eulerAngles;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform.eulerAngles;
#endif
                }

                Debug.LogError("Return default local rotation (This should not happens)");
                return transform.eulerAngles;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        transform.eulerAngles = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        mRectTransform.eulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.transform.eulerAngles = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.transform.eulerAngles = value;
                        return;
#endif
                }

                Debug.LogError("Set default local rotation (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's rotation
        /// </summary>
        public Vector3 localEulerAngles
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform.localEulerAngles;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform.localEulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform.localEulerAngles;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform.localEulerAngles;
#endif
                }

                Debug.LogError("Return default local rotation (This should not happens)");
                return transform.localEulerAngles;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        transform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        mRectTransform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.transform.localEulerAngles = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.transform.localEulerAngles = value;
                        return;
#endif
                }

                Debug.LogError("Set default local rotation (This should not happens)");
            }
        }

        /// <summary>
        /// Get Current type's scale
        /// </summary>
        public Vector3 localScale
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return transform.localScale;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return mRectTransform.localScale;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.transform.localScale;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.transform.localScale;
#endif
                }
                Debug.LogError("Return default local scale (This should not happens)");
                return transform.localScale;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        transform.localScale = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        mRectTransform.localScale = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.transform.localScale = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.transform.localScale = value;
                        return;
#endif
                }
                Debug.LogError("Set default local scale (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's Enable
        /// </summary>
        public bool localEnabled
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return mRenderer.enabled;
                    case JCS_UnityObjectType.UI:
                        return mImage.enabled;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.enabled;
                    case JCS_UnityObjectType.TEXT:
                        return mText.enabled;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.enabled;
#endif
                }
                Debug.LogError("Return default visible (This should not happens)");
                return false;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        mRenderer.enabled = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        mImage.enabled = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.enabled = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        mText.enabled = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.enabled = value;
                        return;
#endif
                }
            }
        }

        /// <summary>
        /// Return the possible material color.
        /// </summary>
        private Color GetColor()
        {
            foreach (string prop in mColorProps)
            {
                return mRenderer.material.GetColor(prop);
            }

            try
            {
                return mRenderer.material.color;
            }
            catch
            {
                // ignore
            }

            return Color.white;
        }

        /// <summary>
        /// Update the material color.
        /// </summary>
        private void SetColor(Color val)
        {
            foreach (string prop in mColorProps)
            {
                mRenderer.material.SetColor(prop, val);
            }

            try
            {
                mRenderer.material.color = val;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Get current type's color
        /// </summary>
        public Color localColor
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return GetColor();
                    case JCS_UnityObjectType.UI:
                        return mImage.color;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.color;
                    case JCS_UnityObjectType.TEXT:
                        return mText.color;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.color;
#endif
                }

                Debug.LogError("Return default Local Red (This should not happens)");
                return new Color(255, 128, 64, 32);
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        SetColor(value);
                        return;
                    case JCS_UnityObjectType.UI:
                        mImage.color = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.color = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        mText.color = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.color = value;
                        return;
#endif

                }
                Debug.LogError("Set default Local Red (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's alpha
        /// </summary>
        public float localAlpha
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return GetColor().a;
                    case JCS_UnityObjectType.UI:
                        return mImage.color.a;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.color.a;
                    case JCS_UnityObjectType.TEXT:
                        return mText.color.a;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.color.a;
#endif
                }
                Debug.LogError("Return default Local Alpha (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = mRenderer.material.color;
                            newColor.a = value;
                            SetColor(newColor);
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = mImage.color;
                            newColor.a = value;
                            mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = mSpriteRenderer.color;
                            newColor.a = value;
                            mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = mText.color;
                            newColor.a = value;
                            mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = mTextMesh.color;
                            newColor.a = value;
                            mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                Debug.LogError("Set default Local Alpha (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's red
        /// </summary>
        public float localRed
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return GetColor().r;
                    case JCS_UnityObjectType.UI:
                        return mImage.color.r;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.color.r;
                    case JCS_UnityObjectType.TEXT:
                        return mText.color.r;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.color.r;
#endif
                }
                Debug.LogError("Return default Local Red (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = mRenderer.material.color;
                            newColor.r = value;
                            SetColor(newColor);
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = mImage.color;
                            newColor.r = value;
                            mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = mSpriteRenderer.color;
                            newColor.r = value;
                            mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = mText.color;
                            newColor.r = value;
                            mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = mTextMesh.color;
                            newColor.r = value;
                            mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                Debug.LogError("Set default Local Red (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's green
        /// </summary>
        public float localGreen
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return GetColor().g;
                    case JCS_UnityObjectType.UI:
                        return mImage.color.g;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.color.g;
                    case JCS_UnityObjectType.TEXT:
                        return mText.color.g;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.color.g;
#endif
                }
                Debug.LogError("Return default Local Green (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = mRenderer.material.color;
                            newColor.g = value;
                            SetColor(newColor);
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = mImage.color;
                            newColor.g = value;
                            mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = mSpriteRenderer.color;
                            newColor.g = value;
                            mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = mText.color;
                            newColor.g = value;
                            mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = mTextMesh.color;
                            newColor.g = value;
                            mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's blue
        /// </summary>
        public float localBlue
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return GetColor().b;
                    case JCS_UnityObjectType.UI:
                        return mImage.color.b;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.color.b;
                    case JCS_UnityObjectType.TEXT:
                        return mText.color.b;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.color.b;
#endif
                }
                Debug.LogError("Return default Local Blue (This should not happens)");
                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = mRenderer.material.color;
                            newColor.b = value;
                            SetColor(newColor);
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = mImage.color;
                            newColor.b = value;
                            mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = mSpriteRenderer.color;
                            newColor.b = value;
                            mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = mText.color;
                            newColor.b = value;
                            mText.color = newColor;
                        }
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        {
                            newColor = mTextMesh.color;
                            newColor.b = value;
                            mTextMesh.color = newColor;
                        }
                        return;
#endif
                }
                Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get current type's main texture
        /// </summary>
        public Texture localMainTexture
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return mRenderer.material.mainTexture;
                    case JCS_UnityObjectType.UI:
                        return mImage.material.mainTexture;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.material.mainTexture;
                    case JCS_UnityObjectType.TEXT:
                        return mText.material.mainTexture;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        return mTextMesh.material.mainTexture;
#endif
                }
                Debug.LogError("Return default Local Blue (This should not happens)");
                return null;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        mRenderer.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        mImage.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.material.mainTexture = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                        mText.material.mainTexture = value;
                        return;
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
                        mTextMesh.material.mainTexture = value;
                        return;
#endif
                }
                Debug.LogError("Set default Local Blue (This should not happens)");
            }
        }

        /// <summary>
        /// Get the sprite.
        /// </summary>
        public Sprite localSprite
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.sprite;
                    case JCS_UnityObjectType.UI:
                        return mImage.sprite;
                }
                Debug.LogError("Failed to get sprite composite cuz current unity object setting does not have it.");
                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.sprite = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        mImage.sprite = value;
                        return;
                }
                Debug.LogError("Failed to set the sprite cuz the current unity object setting does not have sprite coposite.");
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
        public bool localIsVisible
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return mRenderer.isVisible;
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.isVisible;
                }
                Debug.LogError("Return default Local isVisible (This should not happens)");
                return false;
            }

        }

        /// <summary>
        /// Set the same flip x. If not SpriteRenderer 
        /// use negative scale instead.
        /// </summary>
        public bool localFlipX
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return JCS_Mathf.IsPositive(transform.localScale.x);
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.flipX;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        return JCS_Mathf.IsPositive(mRectTransform.localScale.x);
                }
                Debug.LogError("Return default Local FlipX (This should not happens)");
                return false;
            }

            set
            {
                Vector3 newScale;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newScale = transform.localScale;

                            if (value)
                                newScale.x = JCS_Mathf.ToPositive(newScale.x);
                            else
                                newScale.x = JCS_Mathf.ToNegative(newScale.x);

                            transform.localScale = newScale;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.flipX = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        {
                            newScale = mRectTransform.localScale;

                            if (value)
                                newScale.x = JCS_Mathf.ToPositive(newScale.x);
                            else
                                newScale.x = JCS_Mathf.ToNegative(newScale.x);

                            mRectTransform.localScale = newScale;
                        }
                        return;
                }
                Debug.LogError("Set default Local FlipX (This should not happens)");
            }
        }

        /// <summary>
        /// Set the same flip y. If not SpriteRenderer 
        /// use negative scale instead.
        /// </summary>
        public bool localFlipY
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return JCS_Mathf.IsPositive(transform.localScale.y);
                    case JCS_UnityObjectType.SPRITE:
                        return mSpriteRenderer.flipY;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        return JCS_Mathf.IsPositive(mRectTransform.localScale.y);
                }
                Debug.LogError("Return default Local FlipY (This should not happens)");
                return false;
            }

            set
            {
                Vector3 newScale;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newScale = transform.localScale;

                            if (value)
                                newScale.y = JCS_Mathf.ToPositive(newScale.y);
                            else
                                newScale.y = JCS_Mathf.ToNegative(newScale.y);

                            transform.localScale = newScale;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        mSpriteRenderer.flipY = value;
                        return;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
#if TMP_PRO
                    case JCS_UnityObjectType.TMP:
#endif
                        {
                            newScale = mRectTransform.localScale;

                            if (value)
                                newScale.y = JCS_Mathf.ToPositive(newScale.y);
                            else
                                newScale.y = JCS_Mathf.ToNegative(newScale.y);

                            mRectTransform.localScale = newScale;
                        }
                        return;
                }
                Debug.LogError("Set default Local FlipY (This should not happens)");
            }
        }
    }
}
