using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace BrokenLinks
{
    public static class UrlFinder
    {
        private static readonly List<string> Excepted = new List<string>()
        {
            "mailto:",
            "tel:",
            "sms:",
            "https:",
            "http:"
        };

        private static List<string> _links = new List<string>();

        private static bool IsSitePage(string str)
        {
            // Condition { Not in excepted links or anchor }
            return !(Excepted.Any(str.Contains) || str.StartsWith("#"));
        }

        private static List<string> GetUniqueLinksFromPage(string url)
        {
            // Load DOM
            var web = new HtmlWeb();
            var doc = web.Load(url);
            
            // Get correct unique links from page
            var links = doc.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u) && IsSitePage(u))
                .ToHashSet();

            return links.ToList();
        }

        // DFS traversal on site pages
        public static IEnumerable<string> GetUniqueLinksFromSite(string url)
        {
            var stack = new List<string>();    // Stores unvisited links to visit
            var visited = new List<string>();    // All links that called GetUniqueLinksFromPage()
            
            // Get all unique links from root url
            var links = GetUniqueLinksFromPage(url);
            // Mark root url visited
            visited.Add("");
            // Start traversal from first element
            stack.Add(links.First());
            
            while (stack.Any())
            {
                // Pop first element from stack
                var link = stack.First();
                stack.RemoveAt(0);

                if (!visited.Contains(link))
                {
                    visited.Add(link);
                }
                
                links = GetUniqueLinksFromPage(url + link);

                // If unvisited and not in stack push to stack
                foreach (var currLink in links.Where(currLink => !visited.Contains(currLink) && !stack.Contains(currLink)))
                {
                    stack.Insert(0, currLink);
                }
            }
            
            return visited;
        }
    }
}