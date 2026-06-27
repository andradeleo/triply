using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Api.Program>>
    {
        private readonly HttpClient _client;
        public AuthControllerIntegrationTests(WebApplicationFactory<Api.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ValidToken_Returns200()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(body);
            Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("token").GetString()));
        }

        [Theory]
        [InlineData("wrong", "123456")]
        [InlineData("admin@admin.com", "wrong")] 
        [InlineData("wrong", "wrong")] 
        [InlineData("", "123456")]     
        [InlineData("admin@admin.com", "")]      
        [InlineData("", "")]           
        public async Task Login_InvalidCredentials_Returns401(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
