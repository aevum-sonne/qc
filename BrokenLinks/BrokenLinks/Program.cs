using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrokenLinks
{
    internal static class Program
    {
        private const string Url = "http://91.210.252.240/broken-links";

        static void Main(string[] args)
        {
            var scraper = new Scraper(Url);
            scraper.ScrapUrl();
            
            LogResponses(scraper.Valid, "../../../Valid.txt");
            LogResponses(scraper.Invalid, "../../../Invalid.txt");
        }
        
        private static void LogResponses(IReadOnlyCollection<string> responses, string path)
        {
            using var writer = new StreamWriter(path);
            foreach (var res in responses)
            {
                writer.WriteLine(res);
            }
            
            writer.WriteLine($"Responses count: {responses.Count()}");
            writer.WriteLine($"Time {DateTime.Now:h:mm:ss tt}");
        }
    }
}