/**
 * $File: JCS_Logger.cs $
 * $Date: 2017-08-20 12:34:33 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

#if (!NET_STANDARD_2_0 && !NET_4_6)
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerMemberNameAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerFilePathAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerLineNumberAttribute : Attribute { }
}
#endif

namespace JCSUnity
{
    /// <summary>
    /// Common Logger for JCSUnity.
    /// 
    /// This log more detail than just a log from 'JCS_Debug'.
    /// </summary>
    public static class JCS_Logger
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// JCSUnity custom log function.
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(
            string msg,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
#if UNITY_EDITOR
            Debug.Log("¶ [INFO] " + msg + " " + 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + 
                Path.GetFileName(file) + " " + 
                member + "(" + line + ")");
#endif
        }

    }
}
