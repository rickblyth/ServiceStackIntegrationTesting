using System.Net;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;
using ServiceStackApp;

namespace IntegrationTests
{
    [TestFixture]
    class When_requesting_a_hello_for_Demis
    {
        readonly JsonServiceClient jsonServiceClient = new JsonServiceClient(SetUpFixture.BasePath);

        [Test]
        public void Then_result_should_be_Hello_Demis()
        {
            jsonServiceClient.Get<HelloResponse>("hello/Demis").Result.Should().Be("Hello, Demis");
        }

        [Test]
        public void Then_json_should_contain_Hello_Demis()
        {
            jsonServiceClient.Send<object>(new Hello { Name = "Demis" }).ToString().Should().Contain(@"{""Result"":""Hello, Demis""}");
        }
    }

    [TestFixture]
    class When_using_a_nullable_int_and_bad_json
    {
        [Test]
        public void Then_number_in_response_should_not_be_set()
        {
            var client = new WebClient { BaseAddress = SetUpFixture.BasePath };
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            var json = JsonSerializer.SerializeToString(new Hello { Name = "Demis", Number = 1 });
            json = json.Replace("1", "crap");
            var response = client.UploadString("hello?format=json", json);
            response.Should().NotContain("Number");
        }
    }

    [TestFixture]
    class When_using_a_nullable_int_and_valid_json
    {
        [Test]
        public void Then_number_in_response_should_be_set()
        {
            var client = new WebClient { BaseAddress = SetUpFixture.BasePath };
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            var json = JsonSerializer.SerializeToString(new Hello { Name = "Demis", Number = 1 });
            var response = client.UploadString("hello?format=json", json);
            response.Should().Contain(@"""Number"":1");
        }
    }
}