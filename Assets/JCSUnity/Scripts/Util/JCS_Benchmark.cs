/**
 * $File: JCS_Benchmark.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Benchmark operations.
    /// </summary>
    public static class JCS_Benchmark
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Time execution of action.
        /// </summary>
        /// <param name="times"> Times to execute. </param>
        /// <param name="action"> Action to execute. </param>
        /// <returns> Return time spent in milliseconds. </returns>
        public static int Run(int times, Action action)
        {
            if (action == null)
                return 0;

            DateTime before = DateTime.Now;
            JCS_Loop.Times(times, (count) =>
            {
                action.Invoke();
            });

            DateTime after = DateTime.Now;
            TimeSpan duration = after.Subtract(before);

            return duration.Milliseconds;
        }
    }
}
