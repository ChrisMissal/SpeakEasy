using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SpeakEasy.IntegrationTests.Controllers;
using SpeakEasy.Loggers;
using SpeakEasy.Serializers;

namespace SpeakEasy.IntegrationTests
{
    [TestFixture]
    public class BasicHttpMethodsWithXmlSerialization : WithApi
    {
        protected override IHttpClient CreateClient()
        {
            var settings = new HttpClientSettings();
            settings.Serializers.Clear();
            settings.Serializers.Add(new DotNetXmlSerializer());
            settings.Logger = new ConsoleLogger();

            return HttpClient.Create("http://localhost:1337/api", settings);
        }

        [Test]
        public void ShouldHaveCorrectDeserializerOnResponse()
        {
            var response = client.Get("products/1");

            Assert.That(response.Deserializer, Is.TypeOf<DotNetXmlSerializer>());
        }

        [Test]
        public void ShouldCreateNewProduct()
        {
            var product = new Product { Name = "Canoli", Category = "Italian Treats" };

            var isok = client.Post(product, "products").Is(HttpStatusCode.Created);

            Assert.That(isok);
        }

        [Test]
        public void ShouldGetCollection()
        {
            var products = client.Get("products").On(HttpStatusCode.OK).As<List<Product>>();

            Assert.That(products.Any(p => p.Name == "Chocolate Cake"));
        }
    }
}