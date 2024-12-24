using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Component that requires the component `JCS_Canvas` without
    /// actually inherit the class `JCS_Canvas`.
    /// </summary>
    [RequireComponent(typeof(JCS_Canvas))]
    public class JCS_CanvasComp<T> : MonoBehaviour
    {
        /* Variables */

        // The canvas this component to control.
        protected JCS_Canvas mCanvas = null;

        /* Setter & Getter */

        /* Functions */

        protected virtual void Awake()
        {
            this.mCanvas = this.GetComponent<JCS_Canvas>();
        }

        public virtual void Show() => mCanvas.Show();
        public virtual void Hide() => mCanvas.Hide();
        public virtual void ToggleVisibility() => mCanvas.ToggleVisibility();
    }
}
