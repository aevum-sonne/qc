using System;
using System.Collections.Generic;

namespace Triangle
{
    public class TriangleParser
    {
        public static Triangle ParseArgs(IEnumerable<string> args)
        {
            var formattedArgs = new List<double>();
            foreach (var arg in args)
            {
                try
                {
                    formattedArgs.Add(double.Parse(arg));
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Argument format exception was thrown at '{arg}' argument.");
                    
                    return new Triangle();
                }
            }
            
            var triangle = new Triangle()
            {
                A = formattedArgs[0], 
                B = formattedArgs[1], 
                C = formattedArgs[2]
            };
            
            return triangle;
        }
    }
}