/**
 * $File: JCS_TimeRange.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of period of time.
    /// 
    /// Source: http://blog.sina.com.cn/s/blog_5b00159f01010jvf.html
    /// </summary>
    public enum JCS_TimeRange
    {
        UNKNOWN = 0,
        IN_THE_MORNING_5AM_8AM = 1,       // 5 - 8
        MORNING_8AM_12PM = 2,             // 8 - 12
        NOON_12PM_14PM = 3,               // 12 - 14
        AFTERNOON_14PM_17PM = 4,          // 14 - 17
        EVENING_17PM_19PM = 5,            // 17 - 19
        NIGHT_19PM_22PM = 6,              // 19 - 22
        LATE_AT_NIGHT_22PM_2AM = 7,       // 22 - 2
        BEFORE_DAWN_2AM_5AM = 8,          // 2 - 5
    }
}
