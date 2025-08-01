using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Singleton instance interface to keep the new instance.
    /// </summary>
    public abstract class JCS_InstanceNew<T> : JCS_Instance<T>
        where T : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Check singleton for keep the new one.
        /// </summary>
        /// <param name="_new"> new instance </param>
        /// <param name="destroyGO"> 
        /// If true, destory the entire game object instead of just the component.
        /// </param>
        protected virtual void CheckInstance(T _new, bool destroyGO = false)
        {
            T old = FirstInstance();

            if (old != null)
            {
                // only if needed
                TransferData(old, _new);

                // Destory the old one!
                if (destroyGO)
                    Destroy(old.gameObject);
                else
                    Destroy(old);
            }

            // Assign the new one!
            RegisterInstance(_new);
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected abstract void TransferData(T _old, T _new);
    }
}
