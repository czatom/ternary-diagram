using System.Collections.Generic;

namespace TernaryDiagramLib
{
    /// <summary>
    /// Compares only A, B and C coordinates.
    /// </summary>
    public class TPointComparer : IEqualityComparer<PointT>
    {
        /// <summary>
        /// Check if 2 ternary points have the same ABC coordinates
        /// </summary>
        /// <param name="p1">Ternary point 1</param>
        /// <param name="p2">Ternary point 2</param>
        /// <returns>Returns true if ABC coordinates are the same</returns>
        public bool Equals(PointT p1, PointT p2)
        {
            if (p1.A == p2.A && p1.B == p2.B && p1.C == p2.C)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(PointT p)
        {
            return (int)(p.A * p.C + p.B * p.B + p.C * p.A);
        }
    }
}
