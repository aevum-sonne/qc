using System;
using System.Collections.Generic;
using System.Linq;

namespace Triangle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid arguments count.");
                
                return;
            }

            var triangle = TriangleParser.ParseArgs(args);    // Pass array without first arg
            if (triangle.Equals(default(Triangle))) return;    // Invalid argument(s)

            if (TriangleTypeQualifier.IsItPossibleToBuildTriangle(triangle))
            {
                var type = TriangleTypeQualifier.QualifyTriangle(triangle);
                
                Console.WriteLine($"Triangle is {type}.");
            }

            else
            {
                Console.WriteLine("Triangle cannot be build.");
            }
        }
    }
}