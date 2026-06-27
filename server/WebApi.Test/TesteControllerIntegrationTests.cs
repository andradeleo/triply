using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test
{
    public class TesteControllerIntegrationTests : IClassFixture<WebApplicationFactory<Api.Program>>
    {
        private readonly HttpClient _client;
        public TesteControllerIntegrationTests(WebApplicationFactory<Api.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Publico_WithoutToken_Returns200()
        {
            var response = await _client.GetAsync("/api/teste/publico");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal("Qualquer um acessa.", body);
        }

        [Fact]
        public async Task Seguro_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Seguro_WithValidToken_Returns200()
        {
            var token = await GetTokenAsync();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Olá", body);
        }

        [Fact]
        public async Task Admin_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/teste/admin");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Admin_WithValidAdminToken_Returns200()
        {
            var token = await GetTokenAsync();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/admin");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal("Área restrita a Admin.", body);
        }

        private async Task<string> GetTokenAsync()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            return body.GetProperty("token").GetString()!;
        }
    }
}
