using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
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
        [InlineData("admin@admin.com", "wrong")] 
        [InlineData("wrong@wrong.com", "123456")]      
        public async Task Login_InvalidCredentials_Returns401(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "wrong")]
        [InlineData("wrong@wrong.com", "        ")]
        [InlineData("wrong#wrong", "123456")]
        public async Task Login_WrongCredentials_Returns400(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
