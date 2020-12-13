using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TriangleTests
{
    public class Tests
    {
        [Test]
        public void TriangleTestsFromFileWithOutputInFile()
        {
            const string tests = "../../../Tests.txt";
            const string logs = "../../../Logs.txt";

            using var writer = new StreamWriter(logs);
            using var reader = new StreamReader(tests);

            var counter = 0;
            
            string values; 
            while ((values = reader.ReadLine()) != null)
            {
                var testCase = Regex.Replace(values, @"\s+", " ").Split();
                var expected = reader.ReadLine() + '\n';
                        
                using var sw = new StringWriter();
                Console.SetOut(sw);
                
                Triangle.Program.Main(testCase);
                
                var result = sw.ToString();
                writer.WriteLine(++counter + " test " + (expected == result ? "success." : "failed."));
                
                Assert.AreEqual(expected, result);
            }
        }
    }
}