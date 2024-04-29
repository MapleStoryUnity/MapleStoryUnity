#if UNITY_EDITOR
using UnityEngine;
using JCSUnity;

// Dave Hampson 
// http://wiki.unity3d.com/index.php?title=FramesPerSecond
public class FPSDisplay : MonoBehaviour
{
    /* Variables */

    private float mFontSize = 4.0f;     // Change the Font Size

    private float deltaTime = 0.0f;

    /* Setter & Getter */

    /* Functions */

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float w = JCS_Screen.width;
        float h = JCS_Screen.height;

        var style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)(h * mFontSize / 100.0f);
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
#endif
