/**
 * $File: JCS_Bool3.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Structure hold three boolean.
    /// </summary>
    [System.Serializable]
    public struct JCS_Bool3
    {
        public static JCS_Bool3 allTrue { get { return new JCS_Bool3(true, true, true); } }
        public static JCS_Bool3 allFalse { get { return new JCS_Bool3(false, false, false); } }
        
        public bool check1;
        public bool check2;
        public bool check3;

        // init specific value
        public JCS_Bool3(bool ch1 = false, bool ch2 = false, bool ch3 = false)
        {
            this.check1 = ch1;
            this.check2 = ch2;
            this.check3 = ch3;
        }

        // init all the same value
        public JCS_Bool3(bool val)
        {
            this.check1 = val;
            this.check2 = val;
            this.check3 = val;
        }
    }
}
