using System;

namespace Triangle
{
    public class TriangleTypeQualifier
    {
        private const double AcceptableDelta = 0.001;
        
        public static bool IsItPossibleToBuildTriangle(Triangle triangle)
        {
            return triangle.A + triangle.B - triangle.C > AcceptableDelta 
                   && triangle.B + triangle.C - triangle.A > AcceptableDelta 
                   && triangle.A + triangle.C - triangle.B > AcceptableDelta;
        }
        public static bool IsEquilateral(Triangle triangle)
        {
            return Math.Abs(triangle.A - triangle.B) < AcceptableDelta 
                   && Math.Abs(triangle.B - triangle.C) < AcceptableDelta;
        }
        
        public static bool IsIsosceles(Triangle triangle)
        {
            return Math.Abs(triangle.A - triangle.B) < AcceptableDelta 
                   || Math.Abs(triangle.B - triangle.C) < AcceptableDelta 
                   || Math.Abs(triangle.A - triangle.C) < AcceptableDelta;
        }

        public static string QualifyTriangle(Triangle triangle)
        {
            return IsEquilateral(triangle) ? "equilateral"
                : IsIsosceles(triangle) ? "isosceles"
                : "regular";
        }
    }
}