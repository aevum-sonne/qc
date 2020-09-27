using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace BrokenLinks
{
    public class UrlFinder
    {
        private static readonly List<string> Excepted = new List<string>()
        {
            "mailto:",
            "tel:",
            "sms:",
            "https:",
            "http:"
        };

        private List<string> _links = new List<string>();

        private static bool IsSitePage(string str)
        {
            // Condition { Not in excepted links or anchor }
            return !(Excepted.Any(element => str.Contains(element)) || str.StartsWith("#"));
        }

        private static HashSet<string> GetUniqueLinksFromPage(string url)
        {
            // Load DOM
            var web = new HtmlWeb();
            var doc = web.Load(url);
            
            // Get correct unique links from page
            var links = doc.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u) && IsSitePage(u))
                .ToHashSet();

            return links;
        }

        // DFS traversal of site pages
        public static IEnumerable<string> GetUniqueLinksFromSite(string url)
        {
            // Get all unique links (hashset) from root url
            var links = GetUniqueLinksFromPage(url);
            
            var stack = new Stack<string>();    // Stores unvisited links to visit
            var visited = new Stack<string>();    // Result array with all unique links
            
            // Start search from first link that stores root url
            stack.Push(links.First());

            while (stack.Count != 0)
            {
                var link = stack.Pop();

                if (!visited.Contains(link))
                {
                    // Append new links from current link to hashset
                    links.UnionWith(GetUniqueLinksFromPage(url + "/" + link));

                    visited.Push(link);
                }

                // If unvisited push to stack all links
                foreach (var curr in links.Where(curr => !visited.Contains(curr)))
                {
                    stack.Push(curr);
                }
            }
            
            return visited.OrderBy(s => s).ToList();
        }
    }
}