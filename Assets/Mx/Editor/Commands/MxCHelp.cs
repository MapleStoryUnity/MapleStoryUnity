/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;

namespace Mx
{
    public class MxCHelp : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(
            icon: "_Help", 
            summary: "Navigate to Uniyt related sites")]
        public static void HelpSites()
        {
            CompletingRead("Unity site: ", new Dictionary<string, string>()
            {
                { "Homepage", "https://unity.com/" },
                { "Manual", "https://docs.unity3d.com/2023.1/Documentation/Manual/index.html" },
                { "Scripting Reference", "https://docs.unity3d.com/2023.1/Documentation/ScriptReference/index.html" },
                { "Learn", "https://learn.unity.com/" },
                { "Services", "https://unity.com/support-services" },
                { "Forum", "https://forum.unity.com/" },
                { "Discussions", "https://discussions.unity.com/" },
                { "Feedback", "https://unity.com/roadmap/unity-platform" },
                { "Assets Store", "https://assetstore.unity.com/" },
                { "Issue Tracker", "https://issuetracker.unity3d.com/" },
                { "OpenUPM", "https://openupm.com/" },
            },
            (name, item) =>
            {
                string url = item.summary;
                Application.OpenURL(url);
            });
        }
    }
}
