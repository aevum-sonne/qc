using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace LuxuryWatchesApiTests
{
    public class ApiTests
    {
        private const string ConfigPath = "../../../config.json";
        
        private readonly JObject _config;
        
        private readonly JToken _urls;
        
        public ApiTests()
        {
            using var reader = new StreamReader(ConfigPath);
            var configContent = reader.ReadToEnd();
            
            _config = JObject.Parse(configContent);
            
            _urls = _config["urls"].First();
        }
        
        private HttpClient _httpClient;
        
        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        [Test]
        public async Task GetProducts()
        {
            var products = await GetProductsList();

            Assert.IsNotEmpty(products);

            foreach (var product in products)
            {
                Assert.NotNull(product.id);
                Assert.NotNull(product.category_id);
                Assert.NotNull(product.title);
                Assert.NotNull(product.price);
                
                Assert.NotZero(product.id);
                Assert.NotZero(product.category_id);
            }
        }
        
        [Test]
        public async Task RemoveProduct()
        {
            var data = GetData("products");
            
            foreach (var productData in data)
            {
                var response = await AddData(productData);
                Assert.True(response.IsSuccessStatusCode);
            
                var product = await GetProductByResponse(response);
            
                var deleteResponse = await DeleteData(product.id);
                
                Assert.True(deleteResponse.IsSuccessStatusCode);
            }
        }

        [Test]
        public async Task AddProduct()
        {
            var data = GetData("products");
            
            foreach (var productData in data)
            {
                var expectedProduct = JsonConvert.DeserializeObject<Product>(productData);

                var response = await AddData(productData);
                
                Assert.True(response.IsSuccessStatusCode);

                var resultProduct = await GetProductByResponse(response);

                await DeleteData(resultProduct.id);

                Assert.AreEqual(expectedProduct, resultProduct);
            }
        }
        
        [Test]
        public async Task EditProduct()
        {
            var url = GetUrlFromConfig("edit_product");
            
            var data = GetData("products");
            var editedData = GetData("edited_products");

            for (var pos = 0; pos < data.Count; pos++)
            {
                var response = await AddData(data[pos]);
                
                var product = await GetProductByResponse(response);

                var editedProduct = JsonConvert.DeserializeObject<Product>(editedData[pos]);
                editedProduct.id = product.id;
                
                var editedProductAsJson = await JsonConvert.SerializeObjectAsync(editedProduct);
                var editedProductContent = new StringContent(editedProductAsJson, Encoding.UTF8, "application/json");

                var editedProductResponse = await _httpClient.PostAsync(url, editedProductContent);

                var editedProductFromServer = await GetProductById(product.id);
                
                await DeleteData(product.id);

                Assert.True(editedProductResponse.IsSuccessStatusCode);
                Assert.AreEqual(editedProduct, editedProductFromServer);
            }
        }
        
        private string GetUrlFromConfig(string configShortcut)
        {
            return _urls[configShortcut].ToString();
        }
        
        private async Task<List<Product>> GetProductsList()
        {
            var url = GetUrlFromConfig("get_all");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(content);
        }

        private List<string> GetData(string dataName)
        {
            return _config[dataName].Select(product => product["product"].ToString()).ToList();
        }

        private async Task<HttpResponseMessage> AddData(string data)
        {
            var url = GetUrlFromConfig("add_product");
            
            var productAsJson = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, productAsJson);

            return response;
        }
        
        private async Task<HttpResponseMessage> DeleteData(int id)
        {
            var url = GetUrlFromConfig("delete_product");

            return await _httpClient.GetAsync(url + id);
        }

        private async Task<Product> GetProductByResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);

            var id = int.Parse(jsonContent["id"].ToString());

            var product = await GetProductById(id);

            return product;
        }

        private async Task<Product> GetProductById(int id)
        {
            var products = await GetProductsList();

            return products.Find(p => p.id == id);
        }
    }
}