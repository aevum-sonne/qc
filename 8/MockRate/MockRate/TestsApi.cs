using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using NUnit.Framework;

using MbDotNet;
using MbDotNet.Enums;

using Newtonsoft.Json.Linq;

namespace MockRate
{
    public class Rate
    {
        public int Curr;
    }
    
    public class TestsApi
    {
        private const int Port = 8080;

        private MountebankClient _client;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _client = new MountebankClient();
            _client.DeleteImposterAsync(Port);
            
            _httpClient = new HttpClient();
        }
        
        [TestCase("usd", 69, HttpStatusCode.OK)]
        [TestCase("eur", 99, HttpStatusCode.OK)]
        [TestCase("gbp", 123, HttpStatusCode.NotFound)]

        [Test]
        
        public async Task GetRate(string expectedCurrency, int expectedRate, HttpStatusCode expectedStatusCode)
        {
            var imposter = _client.CreateHttpImposter(Port, "RateImposter");
            imposter.AddStub()
                .OnPathAndMethodEqual($"/rate/{expectedCurrency}", Method.Get)
                .ReturnsJson(expectedStatusCode, 
                    (expectedStatusCode == HttpStatusCode.OK) ? new Rate { Curr = expectedRate } 
                        : null
                    );

            await _client.SubmitAsync(imposter);
            
            var response = await _httpClient.GetAsync($"http://localhost:{Port}/rate/{expectedCurrency}");
            
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();

            // Asserts for OK status code
            if (content.Length != 0)
            {
                var jsonContent = JObject.Parse(content);
                var rate = jsonContent["Curr"].ToString();
            
                const int expectedCount = 1;
            
                Assert.AreEqual(expectedCount, jsonContent.Count);
                Assert.AreEqual(expectedRate.ToString(), rate);    
            }
        }
    }
}