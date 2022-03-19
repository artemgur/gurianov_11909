using System;
using System.Collections.Generic;
using static System.Math;
using static Sem1.Utilities;

namespace Sem1
{
    /// <summary>
    /// Contains various utility methods.
    /// </summary>
    static class Utilities
    {
        private const double E = 1e-6;
        internal const double PI6 = PI / 6;
        internal const double PI4 = PI / 4;
        internal const double PI2 = PI / 2;
        
        internal static double Pow2(double a) => a * a;

        /// <summary>
        /// Checks if doubles are equal.
        /// </summary>
        internal static bool AreDoublesEqual(double a, double b) => Abs(a - b) < E;
    }

    /// <summary>
    /// Comparer for sorting segments.
    /// </summary>
    class LengthComparer: IComparer<Segment>
    {
        public int Compare(Segment x, Segment y)
        {
            var d = x.Length - y.Length;
            if (AreDoublesEqual(d, 0))
                return 0;
            if (d > 0)
                return 1;
            return -1;
        }
    }
    
    /// <summary>
    /// Delegate, necessary for filtration of ElementList elements.
    /// </summary>
    public delegate bool SegmentCondition(Segment s, params double[] additionalData);

    /// <summary>
    /// Implementations of delegate SegmentCondition
    /// </summary>
    public static class SegmentConditions
    {
        public static bool PI3PI6Condition(Segment s, params double[] additionalData)
        {
            if (additionalData.Length > 0)
                throw new ArgumentException("Additional data is not necessary here.");
            return AreDoublesEqual(s.Angle, PI4) || AreDoublesEqual(s.Angle, PI6);
        }

        public static bool LengthRangeCondition(Segment s, params double[] additionalData)
        {
            if (additionalData.Length != 2)
                throw new ArgumentException("Additional data means length range here, length should be 2.");
            return additionalData[0] <= s.Length && s.Length <= additionalData[1];
        }
    }
}