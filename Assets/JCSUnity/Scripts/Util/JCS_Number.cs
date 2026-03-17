using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Number extensions.
    /// </summary>
    public static class JCS_Number
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return the percentage.
        /// </summary>
        public static int GetP(this int data, float p)
        {
            return (int)(data * p / 100.0);
        }
        public static float GetP(this float data, float p)
        {
            return data * p / 100.0f;
        }
        public static long GetP(this long data, double p)
        {
            return (long)(data * p / 100.0);
        }
        public static double GetP(this double data, double p)
        {
            return data * p / 100.0;
        }

        /// <summary>
        /// Delta the `num` with `val` and clamp the result
        /// with `min` and `max`.
        /// </summary>
        public static int Delta(this int data, int val, int max)
        {
            return data.Delta(val, 0, max);
        }
        public static int Delta(this int data, int val, int min, int max)
        {
            return Mathf.Clamp(data.Delta(val), min, max);
        }
        public static int Delta(this int data, int val)
        {
            return data + val;
        }

        public static long Delta(this long data, long val, long max)
        {
            return data.Delta(val, 0, max);
        }
        public static long Delta(this long data, long val, long min, long max)
        {
            return System.Math.Clamp(data.Delta(val), min, max);
        }
        public static long Delta(this long data, long val)
        {
            return data + val;
        }

        public static float Delta(this float data, float val, float max)
        {
            return data.Delta(val, 0.0f, max);
        }
        public static float Delta(this float data, float val, float min, float max)
        {
            return Mathf.Clamp(data.Delta(val), min, max);
        }
        public static float Delta(this float data, float val)
        {
            return data + val;
        }

        public static double Delta(this double data, double val, double max)
        {
            return data.Delta(val, 0.0f, max);
        }
        public static double Delta(this double data, double val, double min, double max)
        {
            return System.Math.Clamp(data.Delta(val), min, max);
        }
        public static double Delta(this double data, double val)
        {
            return data + val;
        }

        /// <summary>
        /// Delta the `num` with `val` by percentage and 
        /// clamp the result with `min` and `max`.
        /// </summary>
        public static int DeltaP(this int data, int p, int max)
        {
            return data.DeltaP(p, 0, max);
        }
        public static int DeltaP(this int data, int p, int min, int max)
        {
            return Mathf.Clamp(data.DeltaP(p), min, max);
        }
        public static int DeltaP(this int data, int p)
        {
            int val = data.GetP(p);

            return data + val;
        }

        public static long DeltaP(this long data, long p, long max)
        {
            return data.DeltaP(p, 0, max);
        }
        public static long DeltaP(this long data, long p, long min, long max)
        {
            return System.Math.Clamp(data.DeltaP(p), min, max);
        }
        public static long DeltaP(this long data, long p)
        {
            long val = data.GetP(p);

            return data + val;
        }

        public static float DeltaP(this float data, float p, float max)
        {
            return data.DeltaP(p, 0.0f, max);
        }
        public static float DeltaP(this float data, float p, float min, float max)
        {
            return Mathf.Clamp(data.DeltaP(p), min, max);
        }
        public static float DeltaP(this float data, float p)
        {
            float val = data.GetP(p);

            return data + val;
        }

        public static double DeltaP(this double data, double p, double max)
        {
            return data.DeltaP(p, 0.0, max);
        }
        public static double DeltaP(this double data, double p, double min, double max)
        {
            return System.Math.Clamp(data.DeltaP(p), min, max);
        }
        public static double DeltaP(this double data, double p)
        {
            double val = data.GetP(p);

            return data + val;
        }
    }
}
