#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Text;
using JCSUnity;

public class CpuTest : MonoBehaviour
{
    /* Variables */

    private int m_fontSize = 4;     // Change the Font Size

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
        int w = JCS_Screen.width;
        int h = JCS_Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * m_fontSize / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = "\nCPU 使用率：" + CpuInfo.cpuUsageRate.ToString("0.0") + " %";
        GUI.Label(rect, text, style);
    }
}

#endif
