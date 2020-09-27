using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BrokenLinks
{
    public class Scraper
    {
        public readonly HashSet<string> Valid = new HashSet<string>();
        public readonly HashSet<string> Invalid = new HashSet<string>();
        
        private readonly string _url;

        public Scraper(string url)
        {
            _url = url;
        }

        public void ScrapUrl()
        {
            var links = UrlFinder.GetUniqueLinksFromSite(_url).ToList();
            GetResponsesFromLinks(links);
        }

        private void GetResponsesFromLinks(IEnumerable<string> links)
        {
            using var client = new HttpClient();
            
            foreach (var link in links)
            {
                var url = _url + link;
                var response = client.GetAsync(url).Result;
                var log = $"{url} {response.StatusCode.GetHashCode()} {response.ReasonPhrase}";

                if (response.IsSuccessStatusCode)
                {
                    Valid.Add(log);
                }
                else
                {
                    Invalid.Add(log);
                }
            }
        }
    }
}