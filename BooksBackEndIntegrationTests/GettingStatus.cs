using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace BooksBackEndIntegrationTests
{
    public class GettingStatus : IClassFixture<WebTestFixture>
    {
        private HttpClient _client;

        public GettingStatus(WebTestFixture fixture)
        {
            _client = fixture.CreateClient();
        }

        // Do we get a 200 status code?

        [Fact]
        public async void WeGetASuccessStatusCode()
        {
            var response = await _client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Do we get the status code encoded as app/json?

        [Fact]
        public async void FormattedAsJson()
        {
            var response = await _client.GetAsync("/status");
            var mediaType = response.Content.Headers.ContentType.MediaType;
            Assert.Equal("application/json", mediaType);
        }

        // Does the representation (model) that comes back look right?

        [Fact]
        public async void CheckRepresentation()
        {
            var response = await _client.GetAsync("/status");
            var content = await response.Content.ReadAsAsync<StatusResponse>();
            Assert.Equal("Looks Good", content.Message);
            Assert.Equal(new DateTime(1969, 04, 20, 23, 59, 00), content.LastChecked);
        }

        // etc. 
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public string CheckedBy { get; set; }
        public DateTime LastChecked { get; set; }
    }
}
