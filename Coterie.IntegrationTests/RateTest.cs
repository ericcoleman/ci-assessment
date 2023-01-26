using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Coterie.Api;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Coterie.IntegrationTests
{
    public class RateTest : IAsyncLifetime
    {
        private HttpClient _client;
        private TestServer _server;

        [Fact]
        public async Task CanCalculateRate()
        {
            const string request =
                "{\"business\": \"Plumber\", \"revenue\": 6000000, \"states\": [\"TX\", \"OH\", \"FLORIDA\"]}";

            var httpResult = await _client.PostAsync("rates",
                new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json));

            Assert.Equal(HttpStatusCode.OK, httpResult.StatusCode);

            var actual = JsonConvert.DeserializeObject<GetRateResponse>(await httpResult.Content.ReadAsStringAsync());

            Assert.Equal(BusinessTypes.Plumber, actual.Business);
            Assert.True(actual.IsSuccessful);
            Assert.Equal(6000000, actual.Revenue);

            Assert.Equal(3, actual.Premiums.Count);

            Assert.Equal(11316, actual.Premiums.First(x => x.State == States.TX).Premium);
            Assert.Equal(12000, actual.Premiums.First(x => x.State == States.OH).Premium);
            Assert.Equal(14400, actual.Premiums.First(x => x.State == States.FL).Premium);
        }

        [Fact]
        public async Task BadRateRequestReturns400()
        {
            const string request =
                "{\"business\": \"Plumber\", \"revenue\": 6000000, \"states\": [\"south carolina\", \"OH\", \"FLORIDA\"]}";
            
            var httpResult = await _client.PostAsync("rates",
                new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json));

            Assert.Equal(HttpStatusCode.BadRequest, httpResult.StatusCode);

            var actual = JsonConvert.DeserializeObject<BaseExceptionResponse>(await httpResult.Content.ReadAsStringAsync());

            Assert.Equal("Invalid State(s) provided: south carolina", actual.Message);
        }

        public async Task InitializeAsync()
        {
            var host = await Host.CreateDefaultBuilder()
                .ConfigureWebHost(x => x.UseTestServer().UseStartup<Startup>())
                .StartAsync();

            _server = host.GetTestServer();
            _client = host.GetTestClient();
        }

        public Task DisposeAsync()
        {
            _server?.Dispose();

            return Task.CompletedTask;
        }
    }
}