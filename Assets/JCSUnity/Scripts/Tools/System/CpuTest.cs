#if UNITY_EDITOR
using UnityEngine;
using JCSUnity;

public class CpuTest : MonoBehaviour
{
    /* Variables */

    private float mFontSize = 4.0f;  // Change the Font Size

    /* Setter & Getter */

    /* Functions */

    private void Start()
    {
        InvokeRepeating("Refresh", 0, 1);
    }

    private void Refresh()
    {
        // NOTE(jenchieh): This is useless for now!
        //CpuInfo.Update();
    }

    private void OnGUI()
    {
        float w = JCS_Screen.width;
        float h = JCS_Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)(h * mFontSize / 100.0f);
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = "\nCPU 使用率：" + CpuInfo.cpuUsageRate.ToString("0.0") + " %";
        GUI.Label(rect, text, style);
    }
}

#endif
